using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a calendar event QR code image.</summary>
/// <para>Use this cmdlet to encode meeting or appointment details into a QR code that can be scanned into calendar applications.</para>
/// <example>
///   <summary>Create a timed meeting QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeCalendar -Entry 'Project Sync' -Message 'Weekly delivery review' -Location 'Office' -From (Get-Date) -To (Get-Date).AddHours(1) -FilePath qr.png</code>
///   <para>Creates a QR code for a calendar event with explicit start and end times.</para>
/// </example>
/// <example>
///   <summary>Create an all-day calendar event QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeCalendar -Entry 'Company Offsite' -Location 'Gdansk' -From (Get-Date).Date -To (Get-Date).Date.AddDays(1) -AllDayEvent -EventEncoding ICalComplete -FilePath offsite.png -Show</code>
///   <para>Generates an all-day event payload and opens the QR image after creation.</para>
/// </example>

[Cmdlet(VerbsCommon.New, "ImageQRCodeCalendar")]
public sealed class NewImageQrCodeCalendarCmdlet : AsyncQrCodeCmdlet {

    /// <summary>Event title.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Entry { get; set; } = string.Empty;

    /// <summary>Event description.</summary>
    [Parameter(Position = 1)]
    public string? Message { get; set; }

    /// <summary>Event location.</summary>
    [Parameter(Position = 2)]
    public string? Location { get; set; }

    /// <summary>Start date.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public DateTime From { get; set; }

    /// <summary>End date.</summary>
    [Parameter(Mandatory = true, Position = 4)]
    public DateTime To { get; set; }

    /// <summary>Output path for the QR code image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 5)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Specifies whether the event spans the full day.</summary>
    [Parameter]
    public SwitchParameter AllDayEvent { get; set; }

    /// <summary>Calendar encoding.</summary>
    [Parameter]
    public QrCalendarEncoding EventEncoding { get; set; } = QrCalendarEncoding.ICalComplete;

    /// <summary>Open the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Foreground color of QR modules.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color ForegroundColor { get; set; } = SixLabors.ImageSharp.Color.Black;

    /// <summary>Background color of the QR code.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color BackgroundColor { get; set; } = SixLabors.ImageSharp.Color.White;

    /// <summary>Pixel size for each QR module.</summary>
    [Parameter]
    public int PixelSize { get; set; } = 20;

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        ValidatePixelSize(PixelSize);
        FilePath = EnsureQrOutputPath(FilePath);
        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateCalendarEventAsync(Entry, Message, Location, From, To, FilePath, AllDayEvent.IsPresent, EventEncoding, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateCalendarEvent(Entry, Message, Location, From, To, FilePath, AllDayEvent.IsPresent, EventEncoding, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}
