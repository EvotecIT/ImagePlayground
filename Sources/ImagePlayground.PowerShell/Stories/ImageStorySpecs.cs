using System;
using ChartForgeX.Stories;

namespace ImagePlayground.PowerShell.Stories;

/// <summary>PowerShell-friendly resolved panel specification for New-ImageStoryScene.</summary>
public sealed class ImageStoryPanelSpec {
    /// <summary>Initializes a panel specification.</summary>
    public ImageStoryPanelSpec(string id, string title, VisualStorySurface surface, double weight) {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Surface = surface ?? throw new ArgumentNullException(nameof(surface));
        Weight = weight;
    }

    /// <summary>Gets the panel identifier.</summary>
    public string Id { get; }
    /// <summary>Gets the optional panel title.</summary>
    public string Title { get; }
    /// <summary>Gets the resolved surface.</summary>
    public VisualStorySurface Surface { get; }
    /// <summary>Gets the relative panel weight.</summary>
    public double Weight { get; }
}

/// <summary>PowerShell-friendly resolved scene specification for New-ImageStory.</summary>
public sealed class ImageStorySceneSpec {
    /// <summary>Initializes a scene specification.</summary>
    public ImageStorySceneSpec(string id, string title, double durationSeconds, VisualStorySceneLayout layout, ImageStoryPanelSpec[] panels) {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        DurationSeconds = durationSeconds;
        Layout = layout;
        Panels = panels ?? throw new ArgumentNullException(nameof(panels));
    }

    /// <summary>Gets the scene identifier.</summary>
    public string Id { get; }
    /// <summary>Gets the scene title.</summary>
    public string Title { get; }
    /// <summary>Gets the scene duration.</summary>
    public double DurationSeconds { get; }
    /// <summary>Gets the panel layout.</summary>
    public VisualStorySceneLayout Layout { get; }
    /// <summary>Gets resolved panels.</summary>
    public ImageStoryPanelSpec[] Panels { get; }
}

/// <summary>PowerShell-friendly completed-outcome specification for New-ImageStory.</summary>
public sealed class ImageStoryOutcomeSpec {
    /// <summary>Initializes an outcome specification.</summary>
    public ImageStoryOutcomeSpec(string id, string label, string panelId) {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Label = label ?? throw new ArgumentNullException(nameof(label));
        PanelId = panelId ?? throw new ArgumentNullException(nameof(panelId));
    }

    /// <summary>Gets the outcome identifier.</summary>
    public string Id { get; }
    /// <summary>Gets the displayed label.</summary>
    public string Label { get; }
    /// <summary>Gets the panel that must appear in the completed scene.</summary>
    public string PanelId { get; }
}
