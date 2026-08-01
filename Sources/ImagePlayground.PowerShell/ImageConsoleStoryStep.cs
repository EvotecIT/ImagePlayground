using System;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Describes one composable content or tab-state step in an ImagePlayground console story.</summary>
/// <para>Use the console story command, output, table, blank-line, pause, tab, and tab-selection cmdlets to create steps.</para>
public sealed class ImageConsoleStoryStep {
    internal ImageConsoleStoryStep(
        TerminalStoryStepKind kind,
        string text,
        TerminalTextTone tone,
        double durationSeconds,
        TerminalTable? table,
        string tabId = "",
        string tabTitle = "",
        TerminalDialect tabDialect = TerminalDialect.PowerShell,
        string tabWorkingDirectory = "",
        TerminalTheme? tabTheme = null,
        TerminalTabIcon tabIcon = TerminalTabIcon.Terminal,
        string tabCustomPrompt = "",
        bool isInitialTab = false) {
        Kind = kind;
        Text = text;
        Tone = tone;
        DurationSeconds = durationSeconds;
        Table = table;
        TabId = tabId;
        TabTitle = tabTitle;
        TabDialect = tabDialect;
        TabWorkingDirectory = tabWorkingDirectory;
        TabTheme = tabTheme;
        TabIcon = tabIcon;
        TabCustomPrompt = tabCustomPrompt;
        IsInitialTab = isInitialTab;
    }

    /// <summary>Gets the kind of terminal story step.</summary>
    public TerminalStoryStepKind Kind { get; }

    /// <summary>Gets the command or output text.</summary>
    public string Text { get; }

    /// <summary>Gets the semantic output tone.</summary>
    public TerminalTextTone Tone { get; }

    /// <summary>Gets the explicit command or pause duration.</summary>
    public double DurationSeconds { get; }

    /// <summary>Gets the formatted table carried by a table step.</summary>
    public TerminalTable? Table { get; }

    /// <summary>Gets the terminal tab identifier affected by a tab step.</summary>
    public string TabId { get; }

    /// <summary>Gets the visible title carried by an open-tab step.</summary>
    public string TabTitle { get; }

    /// <summary>Gets the prompt dialect carried by an open-tab step.</summary>
    public TerminalDialect TabDialect { get; }

    /// <summary>Gets the working directory carried by an open-tab step.</summary>
    public string TabWorkingDirectory { get; }

    /// <summary>Gets the independent palette carried by an open-tab step.</summary>
    public TerminalTheme? TabTheme { get; }

    /// <summary>Gets the semantic icon carried by an open-tab step.</summary>
    public TerminalTabIcon TabIcon { get; }

    /// <summary>Gets the custom prompt carried by an open-tab step.</summary>
    public string TabCustomPrompt { get; }

    /// <summary>Gets whether this declaration configures the initial active tab.</summary>
    public bool IsInitialTab { get; }

    internal void ApplyTo(TerminalStory story) {
        if (story == null) {
            throw new ArgumentNullException(nameof(story));
        }

        switch (Kind) {
            case TerminalStoryStepKind.Command:
                story.Command(Text, DurationSeconds);
                break;
            case TerminalStoryStepKind.Output:
                story.Output(Text, Tone);
                break;
            case TerminalStoryStepKind.Blank:
                story.Blank();
                break;
            case TerminalStoryStepKind.Pause:
                story.Pause(DurationSeconds);
                break;
            case TerminalStoryStepKind.Table:
                story.Table(Table ?? throw new InvalidOperationException("Console story table steps require a table."));
                break;
            case TerminalStoryStepKind.DeclareTab:
                story.DeclareTab(
                    TabId,
                    TabTitle,
                    TabDialect,
                    TabWorkingDirectory,
                    TabTheme ?? throw new InvalidOperationException("Console story tab steps require a palette."),
                    TabIcon,
                    string.IsNullOrEmpty(TabCustomPrompt) ? null : TabCustomPrompt);
                break;
            case TerminalStoryStepKind.OpenTab:
                var theme = TabTheme ?? throw new InvalidOperationException("Console story tab steps require a palette.");
                if (IsInitialTab) {
                    story.WithInitialTab(
                        TabId,
                        TabTitle,
                        TabDialect,
                        TabWorkingDirectory,
                        theme,
                        TabIcon,
                        string.IsNullOrEmpty(TabCustomPrompt) ? null : TabCustomPrompt);
                } else {
                    story.OpenTab(
                        TabId,
                        TabTitle,
                        TabDialect,
                        TabWorkingDirectory,
                        theme,
                        TabIcon,
                        string.IsNullOrEmpty(TabCustomPrompt) ? null : TabCustomPrompt,
                        DurationSeconds);
                }
                break;
            case TerminalStoryStepKind.SelectTab:
                story.SelectTab(TabId, DurationSeconds);
                break;
            default:
                throw new InvalidOperationException("Unknown console story step kind.");
        }
    }
}
