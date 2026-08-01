using System;
using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Declares and activates a persistent tab in an ImagePlayground console story.</summary>
/// <para>Each tab owns its title, prompt dialect, working directory, palette, icon, and transcript buffer. Declared tabs remain visible in tab-aware window chrome and can be revisited with Select-ImageConsoleStoryTab.</para>
/// <example>
///   <summary>Open an Ubuntu tab</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStoryTab -Id ubuntu -Profile Ubuntu -Title Ubuntu -WorkingDirectory '~/src'</code>
///   <para>Creates a typed story step that opens and activates an Ubuntu-styled Bash session.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryTab")]
[OutputType(typeof(ImageConsoleStoryStep))]
public sealed class NewImageConsoleStoryTabCmdlet : PSCmdlet {
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

    /// <summary>Optional custom palette, normally created by New-ImageConsoleStoryPalette.</summary>
    [Parameter]
    public TerminalTheme? Palette { get; set; }

    /// <summary>Duration of the visual switch from the previously active tab.</summary>
    [Parameter]
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

        WriteObject(new ImageConsoleStoryStep(
            TerminalStoryStepKind.OpenTab,
            string.Empty,
            TerminalTextTone.Default,
            TransitionSeconds,
            null,
            Id,
            string.IsNullOrWhiteSpace(Title) ? defaultTitle : Title!,
            dialect,
            string.IsNullOrWhiteSpace(WorkingDirectory) ? defaultDirectory : WorkingDirectory!,
            Palette ?? ConsoleStoryPaletteResolver.Resolve(defaultPalette),
            icon));
    }
}
