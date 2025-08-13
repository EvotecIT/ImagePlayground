using System;
using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a calendar event QR code image.</summary>
/// <example>
///   <summary>Create calendar event QR</summary>
///   <code>New-ImageQRCodeCalendar -Entry 'Meeting' -Message 'Discuss' -Location 'Office' -From (Get-Date) -To (Get-Date).AddHours(1) -FilePath qr.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeCalendar")]
public sealed class NewImageQrCodeCalendarCmdlet : PSCmdlet {
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
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 5)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Specifies whether the event spans the full day.</summary>
    [Parameter]
    public SwitchParameter AllDayEvent { get; set; }

    /// <summary>Calendar encoding.</summary>
    [Parameter]
    public PayloadGenerator.CalendarEvent.EventEncoding EventEncoding { get; set; } = PayloadGenerator.CalendarEvent.EventEncoding.iCalComplete;

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

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (PixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(PixelSize));
        }

        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeCalendar - No file path specified, saving to {FilePath}");
        }

        ImagePlayground.QrCode.GenerateCalendarEvent(Entry, Message, Location, From, To, FilePath, AllDayEvent.IsPresent, EventEncoding, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(Helpers.ResolvePath(FilePath), true);
        }
    }
}