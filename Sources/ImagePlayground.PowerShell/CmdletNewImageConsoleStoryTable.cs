using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a typed table step from ordinary PowerShell objects.</summary>
/// <para>Property selects source properties, Header optionally replaces their displayed names, and Align maps a property or displayed header to Left or Right alignment.</para>
/// <example>
///   <summary>Build a table from pipeline objects</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$projects | New-ImageConsoleStoryTable -Property Name, Language, Stars -Header PROJECT, STACK, STARS -Align @{ Stars = 'Right' }</code>
///   <para>Creates one table step without exposing the ChartForgeX table builder.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryTable")]
[OutputType(typeof(ImageConsoleStoryStep))]
public sealed class NewImageConsoleStoryTableCmdlet : PSCmdlet {
    private readonly List<PSObject> _rows = new();

    /// <summary>Objects used as table rows.</summary>
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public PSObject[]? InputObject { get; set; }

    /// <summary>Source properties included in the table, in display order.</summary>
    [Parameter(Mandatory = true)]
    [Alias("Properties")]
    [ValidateNotNullOrEmpty]
    public string[] Property { get; set; } = Array.Empty<string>();

    /// <summary>Displayed column names. Defaults to Property.</summary>
    [Parameter]
    [Alias("Columns")]
    public string[]? Header { get; set; }

    /// <summary>Column alignment keyed by source property or displayed header. Values are Left or Right.</summary>
    [Parameter]
    [Alias("Alignment")]
    public IDictionary? Align { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (InputObject == null) {
            return;
        }
        foreach (var item in InputObject) {
            if (item != null) {
                _rows.Add(item);
            }
        }
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var columns = Header ?? Property;
        if (columns.Length != Property.Length) {
            var exception = new PSArgumentException("Header must contain the same number of entries as Property.", nameof(Header));
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryTableHeaderCount", ErrorCategory.InvalidArgument, Header));
        }

        var table = TerminalTable.Create().WithColumns(columns);
        ApplyAlignment(table, columns);

        foreach (var row in _rows) {
            var values = new object[Property.Length];
            for (var index = 0; index < Property.Length; index++) {
                values[index] = ReadProperty(row, Property[index]);
            }
            table.AddRow(values);
        }

        WriteObject(new ImageConsoleStoryStep(
            TerminalStoryStepKind.Table,
            string.Empty,
            TerminalTextTone.Default,
            0,
            table));
    }

    private void ApplyAlignment(TerminalTable table, IReadOnlyList<string> columns) {
        if (Align == null) {
            return;
        }

        foreach (DictionaryEntry entry in Align) {
            var name = LanguagePrimitives.ConvertTo<string>(entry.Key);
            var index = FindColumn(name, columns);
            if (index < 0) {
                var exception = new PSArgumentException("Align contains an unknown property or header: " + name + ".", nameof(Align));
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryTableUnknownAlignment", ErrorCategory.InvalidArgument, name));
            }

            TerminalColumnAlignment alignment;
            try {
                alignment = LanguagePrimitives.ConvertTo<TerminalColumnAlignment>(Unwrap(entry.Value));
            } catch (PSInvalidCastException exception) {
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryTableInvalidAlignment", ErrorCategory.InvalidArgument, entry.Value));
                throw;
            }
            table.AlignColumn(index, alignment);
        }
    }

    private int FindColumn(string name, IReadOnlyList<string> columns) {
        for (var index = 0; index < Property.Length; index++) {
            if (string.Equals(Property[index], name, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(columns[index], name, StringComparison.OrdinalIgnoreCase)) {
                return index;
            }
        }
        return -1;
    }

    private object ReadProperty(PSObject row, string propertyName) {
        if (row.BaseObject is IDictionary dictionary) {
            foreach (DictionaryEntry entry in dictionary) {
                if (string.Equals(LanguagePrimitives.ConvertTo<string>(entry.Key), propertyName, StringComparison.OrdinalIgnoreCase)) {
                    return Unwrap(entry.Value) ?? string.Empty;
                }
            }
        }

        var property = row.Properties[propertyName];
        if (property == null) {
            var exception = new PSArgumentException("Input object does not contain property '" + propertyName + "'.", nameof(InputObject));
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryTableMissingProperty", ErrorCategory.InvalidData, row));
        }
        return Unwrap(property!.Value) ?? string.Empty;
    }

    private static object? Unwrap(object? value) {
        while (value is PSObject psObject) {
            value = psObject.BaseObject;
        }
        return value;
    }
}
