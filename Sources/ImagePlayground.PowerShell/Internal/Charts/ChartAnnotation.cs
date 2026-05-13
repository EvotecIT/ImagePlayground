namespace ImagePlayground;

/// <summary>Chart annotation definition.</summary>
public sealed class ChartAnnotation {
    /// <summary>X coordinate of the annotation.</summary>
    public double X { get; }

    /// <summary>Y coordinate of the annotation.</summary>
    public double Y { get; }

    /// <summary>Annotation text.</summary>
    public string Text { get; }

    /// <summary>Display arrow.</summary>
    public bool Arrow { get; }

    /// <summary>Create an annotation.</summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="text">Annotation text.</param>
    /// <param name="arrow">Display arrow.</param>
    public ChartAnnotation(double x, double y, string text, bool arrow = false) {
        X = x;
        Y = y;
        Text = text;
        Arrow = arrow;
    }
}

