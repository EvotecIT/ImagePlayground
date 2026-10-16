using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using ChartForgeX.Typography;

namespace ImagePlayground.PowerShell;

/// <summary>One resolved table column: a source property name or calculated expression plus display settings.</summary>
internal sealed class TableColumnSelector {
    private static readonly string[] NameKeys = { "Name", "N", "Label", "L" };
    private static readonly string[] ExpressionKeys = { "Expression", "E" };
    private static readonly string[] FormatKeys = { "FormatString", "Format", "F" };
    private static readonly string[] AlignmentKeys = { "Alignment", "Align", "A" };
    private static readonly string[] WidthKeys = { "Width", "W" };

    private TableColumnSelector(string header, string? propertyName, ScriptBlock? expression) {
        Header = header;
        PropertyName = propertyName;
        Expression = expression;
    }

    /// <summary>Displayed column header.</summary>
    public string Header { get; set; }

    /// <summary>Source property or dictionary key, when the column reads a named value.</summary>
    public string? PropertyName { get; }

    /// <summary>Calculated expression evaluated with <c>$_</c> set to the row.</summary>
    public ScriptBlock? Expression { get; }

    /// <summary>Optional .NET format string applied to formattable values.</summary>
    public string? Format { get; private set; }

    /// <summary>Optional column alignment.</summary>
    public TextAlignment Alignment { get; set; } = TextAlignment.Left;

    /// <summary>Optional fixed column width.</summary>
    public double? Width { get; private set; }

    /// <summary>Creates a selector from a property name or a Select-Object/Format-Table style calculated property hashtable.</summary>
    public static TableColumnSelector Create(object? value) {
        value = Unwrap(value);
        if (value is IDictionary dictionary) {
            return FromDictionary(dictionary);
        }

        var name = value is string text ? text : LanguagePrimitives.ConvertTo<string>(value);
        if (string.IsNullOrWhiteSpace(name)) {
            throw new PSArgumentException("Property entries must be property names or calculated property hashtables.");
        }

        return new TableColumnSelector(name.Trim(), name.Trim(), null);
    }

    /// <summary>Creates selectors from the properties of the first row.</summary>
    public static List<TableColumnSelector> FromRow(PSObject row) {
        var selectors = new List<TableColumnSelector>();
        var value = Unwrap(row);
        if (value is IDictionary dictionary) {
            foreach (DictionaryEntry entry in dictionary) {
                var key = LanguagePrimitives.ConvertTo<string>(entry.Key);
                selectors.Add(new TableColumnSelector(key, key, null));
            }
            return selectors;
        }

        foreach (var name in DefaultPropertyNames(row)) {
            selectors.Add(new TableColumnSelector(name, name, null));
        }
        return selectors;
    }

    /// <summary>Reads the column value for a row. Missing properties produce an empty cell, matching Select-Object.</summary>
    public object? Read(PSObject row) {
        if (Expression != null) {
            var results = Expression.InvokeWithContext(null, new List<PSVariable> { new PSVariable("_", row) }, row);
            if (results.Count == 0) return null;
            if (results.Count == 1) return Unwrap(results[0]);
            var parts = new List<string>(results.Count);
            foreach (var result in results) parts.Add(Convert.ToString(Unwrap(result), CultureInfo.InvariantCulture) ?? string.Empty);
            return string.Join(", ", parts);
        }

        var value = Unwrap(row);
        if (value is IDictionary dictionary) {
            foreach (DictionaryEntry entry in dictionary) {
                if (string.Equals(LanguagePrimitives.ConvertTo<string>(entry.Key), PropertyName, StringComparison.OrdinalIgnoreCase)) {
                    return Unwrap(entry.Value);
                }
            }
            return null;
        }

        var property = row.Properties[PropertyName!];
        return property == null ? null : Unwrap(property.Value);
    }

    /// <summary>True when the row exposes the named property or key.</summary>
    public bool CanRead(PSObject row) {
        if (Expression != null) return true;
        var value = Unwrap(row);
        if (value is IDictionary dictionary) {
            foreach (DictionaryEntry entry in dictionary) {
                if (string.Equals(LanguagePrimitives.ConvertTo<string>(entry.Key), PropertyName, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }
        return row.Properties[PropertyName!] != null;
    }

    internal static object? Unwrap(object? value) {
        while (value is PSObject psObject) {
            value = psObject.BaseObject;
        }
        return value;
    }

    internal static TextAlignment ParseAlignment(object? value) {
        var text = LanguagePrimitives.ConvertTo<string>(Unwrap(value));
        switch ((text ?? string.Empty).Trim().ToLowerInvariant()) {
            case "left":
                return TextAlignment.Left;
            case "center":
            case "centre":
                return TextAlignment.Center;
            case "right":
                return TextAlignment.Right;
            default:
                throw new PSArgumentException("Alignment must be Left, Center, or Right.");
        }
    }

    private static TableColumnSelector FromDictionary(IDictionary dictionary) {
        object? expressionValue = Lookup(dictionary, ExpressionKeys);
        object? nameValue = Lookup(dictionary, NameKeys);
        string? name = nameValue == null ? null : LanguagePrimitives.ConvertTo<string>(Unwrap(nameValue));
        ScriptBlock? expression = Unwrap(expressionValue) as ScriptBlock;
        string? propertyName = expression == null && expressionValue != null ? LanguagePrimitives.ConvertTo<string>(Unwrap(expressionValue)) : null;

        if (expression == null && string.IsNullOrWhiteSpace(propertyName)) {
            propertyName = name;
        }
        if (string.IsNullOrWhiteSpace(name)) {
            name = propertyName ?? expression?.ToString().Trim();
        }
        if (string.IsNullOrWhiteSpace(name) || (expression == null && string.IsNullOrWhiteSpace(propertyName))) {
            throw new PSArgumentException("Calculated properties need a Name (or Label) and an Expression, for example @{ Name = 'Requests'; Expression = { [int] $_.Requests }; FormatString = 'N0' }.");
        }

        foreach (DictionaryEntry entry in dictionary) {
            var key = LanguagePrimitives.ConvertTo<string>(entry.Key);
            if (!IsKnownKey(key)) {
                throw new PSArgumentException("Calculated property key '" + key + "' is not supported. Use Name, Expression, FormatString, Alignment, or Width.");
            }
        }

        var selector = new TableColumnSelector(name!.Trim(), propertyName?.Trim(), expression);
        var format = Lookup(dictionary, FormatKeys);
        if (format != null) selector.Format = LanguagePrimitives.ConvertTo<string>(Unwrap(format));
        var alignment = Lookup(dictionary, AlignmentKeys);
        if (alignment != null) selector.Alignment = ParseAlignment(alignment);
        var width = Lookup(dictionary, WidthKeys);
        if (width != null) {
            var parsed = LanguagePrimitives.ConvertTo<double>(Unwrap(width));
            if (double.IsNaN(parsed) || double.IsInfinity(parsed) || parsed <= 0) throw new PSArgumentException("Calculated property Width must be greater than zero.");
            selector.Width = parsed;
        }
        return selector;
    }

    private static bool IsKnownKey(string key) {
        foreach (var group in new[] { NameKeys, ExpressionKeys, FormatKeys, AlignmentKeys, WidthKeys }) {
            foreach (var candidate in group) {
                if (string.Equals(candidate, key, StringComparison.OrdinalIgnoreCase)) return true;
            }
        }
        return false;
    }

    private static object? Lookup(IDictionary dictionary, string[] keys) {
        foreach (DictionaryEntry entry in dictionary) {
            var key = LanguagePrimitives.ConvertTo<string>(entry.Key);
            foreach (var candidate in keys) {
                if (string.Equals(candidate, key, StringComparison.OrdinalIgnoreCase)) return entry.Value;
            }
        }
        return null;
    }

    private static IEnumerable<string> DefaultPropertyNames(PSObject row) {
        var standard = row.Members["PSStandardMembers"]?.Value as PSMemberSet;
        if (standard?.Members["DefaultDisplayPropertySet"]?.Value is PSPropertySet displaySet && displaySet.ReferencedPropertyNames.Count > 0) {
            foreach (var name in displaySet.ReferencedPropertyNames) yield return name;
            yield break;
        }

        foreach (var property in row.Properties) {
            if (property.IsGettable) yield return property.Name;
        }
    }
}
