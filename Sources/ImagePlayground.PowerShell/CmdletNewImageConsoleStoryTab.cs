using System;
using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a persistent tab in an ImagePlayground console story.</summary>
/// <para>By default, the new tab opens and becomes active after the current tab's configured reading dwell. Use Active for the initial tab, Background to pre-stage a visible inactive tab, and Select-ImageConsoleStoryTab to revisit any existing tab without clearing its buffer.</para>
/// <example>
///   <summary>Define the initial active PowerShell tab</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active</code>
///   <para>Names and styles the initial persistent tab without creating an extra transition.</para>
/// </example>
/// <example>
///   <summary>Open an Ubuntu tab</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStoryTab -Id ubuntu -Profile Ubuntu -Title Ubuntu -WorkingDirectory '~/src'</code>
///   <para>Opens and activates an Ubuntu-styled Bash session only after the previous tab has finished its configured reading dwell.</para>
/// </example>
/// <example>
///   <summary>Prepare a background tab for a later jump</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStoryTab -Id logs -Title Logs -Profile PowerShell -Background
/// Select-ImageConsoleStoryTab -Id logs</code>
///   <para>The declaration makes the tab available in the strip without interrupting the active session. Selection later activates its retained buffer.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryTab", DefaultParameterSetName = OpenSet)]
[OutputType(typeof(ImageConsoleStoryStep))]
public sealed class NewImageConsoleStoryTabCmdlet : PSCmdlet {
    private const string OpenSet = "Open";
    private const string InitialSet = "Initial";
    private const string BackgroundSet = "Background";

    /// <summary>Stable identifier used by later tab-selection steps.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Visible tab title. Defaults to the selected profile name.</summary>
    [Parameter(Position = 1)]
    public string? Title { get; set; }

    /// <summary>Built-in profile that supplies prompt behavior, default working directory, palette, and icon.</summary>
    [Parameter]
    [ValidateSet("PowerShell", "WindowsPowerShell", "Ubuntu", "Bash", "CommandPrompt")]
    public string Profile { get; set; } = "PowerShell";

    /// <summary>Working directory shown by this tab. Defaults to C:\ for Windows profiles and ~ for POSIX profiles.</summary>
    [Parameter]
    public string? WorkingDirectory { get; set; }

    /// <summary>Configure this declaration as the initial active tab. It must be the first content step.</summary>
    [Parameter(Mandatory = true, ParameterSetName = InitialSet)]
    public SwitchParameter Active { get; set; }

    /// <summary>Declare the tab without activating it. Use Select-ImageConsoleStoryTab for the later intentional switch.</summary>
    [Parameter(Mandatory = true, ParameterSetName = BackgroundSet)]
    public SwitchParameter Background { get; set; }

    /// <summary>Optional custom palette, normally created by New-ImageConsoleStoryPalette.</summary>
    [Parameter]
    public TerminalTheme? Palette { get; set; }

    /// <summary>Duration of the visual switch from the previously active tab.</summary>
    [Parameter]
    [PSDefaultValue(Value = 0.2, Help = "0.2")]
    [ValidateRange(0, 2)]
    public double TransitionSeconds { get; set; } = 0.2;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var profile = Profile.ToUpperInvariant();
        var dialect = TerminalDialect.PowerShell;
        var icon = TerminalTabIcon.PowerShell;
        var defaultTitle = "PowerShell";
        var defaultDirectory = @"C:\";
        var defaultPalette = "PowerShell";
        switch (profile) {
            case "WINDOWSPOWERSHELL":
                icon = TerminalTabIcon.WindowsPowerShell;
                defaultTitle = "Windows PowerShell";
                defaultPalette = "WindowsPowerShell";
                break;
            case "UBUNTU":
                dialect = TerminalDialect.Bash;
                icon = TerminalTabIcon.Ubuntu;
                defaultTitle = "Ubuntu";
                defaultDirectory = "~";
                defaultPalette = "Ubuntu";
                break;
            case "BASH":
                dialect = TerminalDialect.Bash;
                icon = TerminalTabIcon.Bash;
                defaultTitle = "Bash";
                defaultDirectory = "~";
                defaultPalette = "Dark";
                break;
            case "COMMANDPROMPT":
                dialect = TerminalDialect.CommandPrompt;
                icon = TerminalTabIcon.CommandPrompt;
                defaultTitle = "Command Prompt";
                defaultPalette = "Campbell";
                break;
        }

        var isInitialTab = Active.IsPresent;
        WriteObject(new ImageConsoleStoryStep(
            Background.IsPresent ? TerminalStoryStepKind.DeclareTab : TerminalStoryStepKind.OpenTab,
            string.Empty,
            TerminalTextTone.Default,
            TransitionSeconds,
            null,
            Id,
            string.IsNullOrWhiteSpace(Title) ? defaultTitle : Title!,
            dialect,
            string.IsNullOrWhiteSpace(WorkingDirectory) ? defaultDirectory : WorkingDirectory!,
            Palette ?? ConsoleStoryPaletteResolver.Resolve(defaultPalette),
            icon,
            isInitialTab: isInitialTab));
    }
}
