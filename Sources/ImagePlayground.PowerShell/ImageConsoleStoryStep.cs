using System;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Describes one composable command, output, table, blank line, or pause in an ImagePlayground console story.</summary>
/// <para>Use the New-ImageConsoleStoryCommand, New-ImageConsoleStoryOutput, New-ImageConsoleStoryTable, New-ImageConsoleStoryBlankLine, and New-ImageConsoleStoryPause cmdlets to create steps.</para>
public sealed class ImageConsoleStoryStep {
    internal ImageConsoleStoryStep(
        TerminalStoryStepKind kind,
        string text,
        TerminalTextTone tone,
        double durationSeconds,
        TerminalTable? table) {
        Kind = kind;
        Text = text;
        Tone = tone;
        DurationSeconds = durationSeconds;
        Table = table;
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
            default:
                throw new InvalidOperationException("Unknown console story step kind.");
        }
    }
}
