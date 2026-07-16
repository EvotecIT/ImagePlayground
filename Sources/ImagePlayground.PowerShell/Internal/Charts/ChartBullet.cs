using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Bullet chart definition.</summary>
public sealed class ChartBullet : ChartDefinition {
    /// <summary>Current value.</summary>
    public double Value { get; }

    /// <summary>Target value.</summary>
    public double Target { get; }

    /// <summary>Minimum scale value.</summary>
    public double Minimum { get; }

    /// <summary>Maximum scale value.</summary>
    public double Maximum { get; }

    /// <summary>Optional qualitative range end values.</summary>
    public IList<double>? RangeEnds { get; }

    /// <summary>Bullet color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a bullet chart definition.</summary>
    /// <param name="name">Bullet label.</param>
    /// <param name="value">Current value.</param>
    /// <param name="target">Target value.</param>
    /// <param name="minimum">Minimum scale value.</param>
    /// <param name="maximum">Maximum scale value.</param>
    /// <param name="rangeEnds">Optional qualitative range end values.</param>
    /// <param name="color">Optional bullet color.</param>
    public ChartBullet(string name, double value, double target, double minimum = 0, double maximum = 100, IList<double>? rangeEnds = null, ChartColor? color = null) : base(name) {
        Value = value;
        Target = target;
        Minimum = minimum;
        Maximum = maximum;
        RangeEnds = rangeEnds;
        Color = color;
    }
}
