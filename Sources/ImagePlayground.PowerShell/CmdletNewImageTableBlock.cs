using ChartForgeX.VisualBlocks;
using ImagePlayground;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a visual table block from PowerShell objects.</summary>
/// <para>Pipe objects in and choose columns with -Property, the same way as Select-Object or Format-Table. Property accepts property names and calculated property hashtables with Name (or Label), Expression, FormatString, Alignment, and Width keys.</para>
/// <para>Rows can be PowerShell objects, hashtables, or arrays of values in column order. Without -Property, the columns come from the first row.</para>
/// <example>
///   <summary>Build a table from pipeline objects</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Get-Service | Select-Object -First 5 | New-ImageTableBlock -Property Name, Status -StatusColumn Status -Title Services</code>
///   <para>Creates one table block with a Name and a Status column. Missing properties produce empty cells.</para>
/// </example>
/// <example>
///   <summary>Format and align columns with calculated properties</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$services | New-ImageTableBlock -Title Services -StatusColumn Status -Width 616 -Height 270 -Property Name, Status,
///     @{ Name = 'Requests'; Expression = { [int] $_.Requests }; FormatString = 'N0'; Alignment = 'Right' },
///     @{ Name = 'P95'; Expression = { "$($_.P95) ms" }; Alignment = 'Right' }</code>
///   <para>Formats the request count with thousands separators, appends a unit to latency, and sizes the block for a dashboard grid.</para>
/// </example>
/// <example>
///   <summary>Supply rows explicitly</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTableBlock -Title Services -Column Name, Status -Row @{ Name = 'API'; Status = 'Healthy' }, @{ Name = 'Database'; Status = 'Warning' } -Dense</code>
///   <para>-Column and -Row remain available as aliases of -Property and -InputObject.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTableBlock")]
[OutputType(typeof(ChartTable))]
public sealed class NewImageTableBlockCmdlet : PSCmdlet {
    private readonly List<PSObject> _rows = new();

    /// <summary>Columns to show, in display order: property names or calculated property hashtables.</summary>
    /// <para>Calculated properties accept Name (or Label), Expression (a property name or a script block using $_), FormatString, Alignment (Left, Center, Right), and Width.</para>
    /// <para>When omitted, the columns are the properties of the first row.</para>
    [Parameter(Position = 0)]
    [Alias("Column", "Columns")]
    public object[]? Property { get; set; }

    /// <summary>Row objects, supplied through the pipeline or directly.</summary>
    /// <para>Rows can be objects, hashtables, or arrays of values in column order.</para>
    [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
    [Alias("Row", "Rows")]
    [AllowEmptyCollection]
    public PSObject[]? InputObject { get; set; }

    /// <summary>Displayed column names. Defaults to the property names.</summary>
    [Parameter]
    public string[]? Header { get; set; }

    /// <summary>Column alignment keyed by property or displayed header. Values are Left, Center, or Right.</summary>
    [Parameter]
    [Alias("Alignment")]
    public IDictionary? Align { get; set; }

    /// <summary>Optional block title.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional block subtitle.</summary>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>Column, by property or displayed header, that contains semantic status values such as Healthy, Warning, or Failed.</summary>
    [Parameter]
    [Alias("StatusProperty")]
    public string StatusColumn { get; set; } = string.Empty;

    /// <summary>Use compact row spacing.</summary>
    [Parameter]
    public SwitchParameter Dense { get; set; }

    /// <summary>Hide the table header.</summary>
    [Parameter]
    public SwitchParameter NoHeader { get; set; }

    /// <summary>Disable alternating row surfaces.</summary>
    [Parameter]
    public SwitchParameter NoRowStriping { get; set; }

    /// <summary>Block width in pixels. The height keeps its default unless -Height is also supplied.</summary>
    [Parameter]
    [ValidateRange(1, VisualBlockSize.Maximum)]
    public int Width { get; set; }

    /// <summary>Block height in pixels. The width keeps its default unless -Width is also supplied.</summary>
    [Parameter]
    [ValidateRange(1, VisualBlockSize.Maximum)]
    public int Height { get; set; }

    /// <summary>ChartForgeX theme.</summary>
    [Parameter]
    public ChartTheme Theme { get; set; } = ChartTheme.Default;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (InputObject == null) {
            return;
        }

        foreach (var row in InputObject) {
            if (row != null) {
                _rows.Add(row);
            }
        }
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        List<TableColumnSelector> columns;
        try {
            columns = ResolveColumns();
            ApplyHeaders(columns);
            ApplyAlignment(columns);
        } catch (PSArgumentException exception) {
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageTableBlockInvalidColumns", ErrorCategory.InvalidArgument, Property));
            return;
        }

        var table = ChartTable.Create()
            .WithDenseMode(Dense.IsPresent)
            .WithHeader(!NoHeader.IsPresent)
            .WithRowStriping(!NoRowStriping.IsPresent)
            .WithTheme(ChartThemeResolver.Resolve(Theme));
        foreach (var column in columns) {
            table.AddColumn(column.Header, column.Alignment, column.Width, column.Format);
        }
        if (!string.IsNullOrWhiteSpace(Title)) table.WithTitle(Title);
        if (!string.IsNullOrWhiteSpace(Subtitle)) table.WithSubtitle(Subtitle);
        foreach (var row in _rows) {
            table.AddRow(ResolveValues(row, columns));
        }
        if (!string.IsNullOrWhiteSpace(StatusColumn)) {
            table.WithStatusColumn(ResolveStatusHeader(columns));
        }
        VisualBlockSize.Apply(this, table, Width, Height);
        WriteObject(table);
    }

    private List<TableColumnSelector> ResolveColumns() {
        if (Property != null && Property.Length > 0) {
            return Property.Select(TableColumnSelector.Create).ToList();
        }

        if (_rows.Count == 0) {
            throw new PSArgumentException("Specify -Property when no rows are supplied.");
        }
        if (IsValueArray(_rows[0])) {
            throw new PSArgumentException("Specify -Property (column names) when rows are arrays of values.");
        }

        var detected = TableColumnSelector.FromRow(_rows[0]);
        if (detected.Count == 0) {
            throw new PSArgumentException("The first row has no properties. Specify -Property.");
        }
        return detected;
    }

    private void ApplyHeaders(List<TableColumnSelector> columns) {
        if (Header == null) {
            return;
        }
        if (Header.Length != columns.Count) {
            throw new PSArgumentException("Header must contain the same number of entries as Property.");
        }
        for (var index = 0; index < columns.Count; index++) {
            columns[index].Header = Header[index];
        }
    }

    private void ApplyAlignment(List<TableColumnSelector> columns) {
        if (Align == null) {
            return;
        }
        foreach (DictionaryEntry entry in Align) {
            var name = LanguagePrimitives.ConvertTo<string>(entry.Key);
            var index = FindColumn(columns, name);
            if (index < 0) {
                throw new PSArgumentException("Align contains an unknown property or header: " + name + ".");
            }
            columns[index].Alignment = TableColumnSelector.ParseAlignment(entry.Value);
        }
    }

    private string ResolveStatusHeader(List<TableColumnSelector> columns) {
        var index = FindColumn(columns, StatusColumn);
        if (index < 0) {
            var exception = new PSArgumentException("StatusColumn must name one of the table columns: " + string.Join(", ", columns.Select(column => column.Header)) + ".");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageTableBlockUnknownStatusColumn", ErrorCategory.InvalidArgument, StatusColumn));
        }
        return columns[index].Header;
    }

    private static int FindColumn(List<TableColumnSelector> columns, string name) {
        for (var index = 0; index < columns.Count; index++) {
            if (string.Equals(columns[index].Header, name, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(columns[index].PropertyName, name, StringComparison.OrdinalIgnoreCase)) {
                return index;
            }
        }
        return -1;
    }

    private object?[] ResolveValues(PSObject row, List<TableColumnSelector> columns) {
        if (IsValueArray(row)) {
            var values = new List<object?>();
            foreach (var item in (IEnumerable)TableColumnSelector.Unwrap(row)!) {
                values.Add(TableColumnSelector.Unwrap(item));
            }
            if (values.Count != columns.Count) {
                var exception = new PSArgumentException("A row array has " + values.Count + " values but the table has " + columns.Count + " columns.");
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageTableBlockRowValueCount", ErrorCategory.InvalidData, row));
            }
            return values.ToArray();
        }

        if (IsScalar(row) && columns.Count == 1 && !columns[0].CanRead(row)) {
            return new[] { TableColumnSelector.Unwrap(row) };
        }

        var result = new object?[columns.Count];
        for (var index = 0; index < columns.Count; index++) {
            result[index] = columns[index].Read(row);
        }
        return result;
    }

    private static bool IsValueArray(PSObject row) {
        var value = TableColumnSelector.Unwrap(row);
        return value is IEnumerable && value is not string && value is not IDictionary;
    }

    private static bool IsScalar(PSObject row) {
        var value = TableColumnSelector.Unwrap(row);
        return value is string || value is ValueType;
    }
}
