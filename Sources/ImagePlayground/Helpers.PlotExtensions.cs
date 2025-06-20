using System;
using ScottPlot;

namespace ImagePlayground {
    /// <summary>Extension methods for ScottPlot.</summary>
    public static class PlotExtensions {
        /// <summary>Add a polar scatter plot.</summary>
        /// <param name="add">Plottable adder.</param>
        /// <param name="angles">Angle values in radians.</param>
        /// <param name="values">Radius values.</param>
        public static ScottPlot.Plottables.Scatter Polar(this PlottableAdder add, double[] angles, double[] values) {
            if (angles is null) throw new ArgumentNullException(nameof(angles));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (angles.Length != values.Length) throw new ArgumentException("Angles and values must have the same length.");
            var xs = new double[angles.Length];
            var ys = new double[angles.Length];
            for (int i = 0; i < angles.Length; i++) {
                xs[i] = values[i] * Math.Cos(angles[i]);
                ys[i] = values[i] * Math.Sin(angles[i]);
            }
            return add.Scatter(xs, ys);
        }
    }
}
