using System.Collections;
using System.Management.Automation;
using ChartForgeX.Stories;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Converts ordered capture records into a resolved replay without executing commands.</summary>
/// <para>Each event supplies TimestampSeconds and Kind. Command, Output, ReplaceLine, Directory and Marker use Text. OpenTab uses TabId, Title, Dialect, WorkingDirectory and CustomPrompt; SelectTab uses TabId. Output and ReplaceLine can supply Tone. Capture scripts remain a separate, explicit caller action.</para>
/// <example>
///   <summary>Prepare recorded output for playback</summary>
///   <code>$events = @(
///     @{ TimestampSeconds = 1; Kind = 'Command'; Text = './Test-Project.ps1' }
///     @{ TimestampSeconds = 12; Kind = 'Output'; Text = '24 checks passed'; Tone = 'Success' }
/// )
/// $replay = New-ImageStoryReplay -DurationSeconds 15 -Events $events
/// $replay.CompressPauses([TimeSpan]::FromSeconds(2))</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageStoryReplay")]
[OutputType(typeof(StoryReplay))]
public sealed class NewImageStoryReplayCmdlet : PSCmdlet {
    private readonly List<PSObject> _events = new();

    /// <summary>Recorded endpoint, including idle time.</summary>
    [Parameter(Mandatory = true)]
    [ValidateRange(0.25, 1800)]
    public double DurationSeconds { get; set; }

    /// <summary>Ordered dictionaries or objects, including objects read with ConvertFrom-Json.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    [Alias("Events")]
    public PSObject[] InputObject { get; set; } = Array.Empty<PSObject>();

    /// <summary>Initial prompt dialect.</summary>
    [Parameter]
    public TerminalDialect Dialect { get; set; } = TerminalDialect.PowerShell;

    /// <summary>Initial prompt directory.</summary>
    [Parameter]
    public string WorkingDirectory { get; set; } = ".";

    /// <summary>Initial session title.</summary>
    [Parameter]
    public string Title { get; set; } = "PowerShell";

    /// <summary>Prompt required by the Custom dialect.</summary>
    [Parameter]
    public string? CustomPrompt { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (_events.Count + InputObject.Length > 4096) {
            throw new PSArgumentException("Replay supports at most 4096 events.");
        }
        _events.AddRange(InputObject);
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var replay = StoryReplay.Create(TimeSpan.FromSeconds(DurationSeconds), Dialect, WorkingDirectory, Title, CustomPrompt);
        foreach (var item in _events) {
            var at = TimeSpan.FromSeconds(Required<double>(item, "TimestampSeconds"));
            var kind = Required<StoryReplayEventKind>(item, "Kind");
            switch (kind) {
                case StoryReplayEventKind.Command: {
                    replay.Command(at, Required<string>(item, "Text"));
                    break;
                }
                case StoryReplayEventKind.Output: {
                    replay.Output(at, Required<string>(item, "Text"), Optional(item, "Tone", TerminalTextTone.Default));
                    break;
                }
                case StoryReplayEventKind.ReplaceLine: {
                    replay.ReplaceLine(at, Required<string>(item, "Text"), Optional(item, "Tone", TerminalTextTone.Default));
                    break;
                }
                case StoryReplayEventKind.Clear: {
                    replay.Clear(at);
                    break;
                }
                case StoryReplayEventKind.Directory: {
                    replay.ChangeDirectory(at, Required<string>(item, "Text"));
                    break;
                }
                case StoryReplayEventKind.Marker: {
                    replay.Marker(at, Required<string>(item, "Text"));
                    break;
                }
                case StoryReplayEventKind.SelectTab: {
                    replay.SelectTab(at, Required<string>(item, "TabId"));
                    break;
                }
                case StoryReplayEventKind.OpenTab: {
                    replay.OpenTab(at, Required<string>(item, "TabId"), Required<string>(item, "Title"),
                        Optional(item, "Dialect", TerminalDialect.PowerShell), Optional(item, "WorkingDirectory", "."), Optional<string?>(item, "CustomPrompt", null));
                    break;
                }
                default: {
                    throw new PSArgumentException("Unknown replay event kind.");
                }
            }
        }
        WriteObject(replay);
    }

    private static T Required<T>(PSObject item, string name) {
        var value = Value(item, name);
        if (value == null) {
            throw new PSArgumentException("Replay event requires " + name + ".");
        }
        return LanguagePrimitives.ConvertTo<T>(value);
    }
    private static T Optional<T>(PSObject item, string name, T fallback) {
        var value = Value(item, name); return value == null ? fallback : LanguagePrimitives.ConvertTo<T>(value);
    }
    private static object? Value(PSObject item, string name) => item.BaseObject is IDictionary dictionary
        ? dictionary[name] : item.Properties[name]?.Value;
}
