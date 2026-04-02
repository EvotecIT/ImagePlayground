using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using ImagePlayground.Gdi;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Renders charts using GDI+.</summary>
public static class ChartRenderer {
    /// <summary>Renders a bar chart.</summary>
    public static Image RenderBar(ChartBarOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var items = GetLegendItems(options.Series.Select(s => (s.Name, s.Color)).ToArray());
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, items, fonts);

            DrawPlotBackground(graphics, layout.PlotArea, options.Style);
            if (options.Series.Count == 0) {
                DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
                return;
            }

            var categories = BuildCategories(options.Categories, options.Series.Select(s => s.Values.Count).DefaultIfEmpty(0).Max());
            var yScale = options.Stacked
                ? AxisScale.FromSeries(GetStackedBarExtents(options, categories.Count), options.YAxis, includeZero: true)
                : AxisScale.FromSeries(options.Series.SelectMany(s => s.Values), options.YAxis, includeZero: true);

            DrawYAxis(graphics, layout.PlotArea, yScale, options.YAxis, fonts, options.Style);
            DrawXAxisLabels(graphics, layout.PlotArea, categories, fonts, options.Style);

            var groupWidth = layout.PlotArea.Width / Math.Max(1, categories.Count);
            var groupGap = GdiMath.Clamp(options.GroupGap, 0f, 0.9f);
            var barGap = GdiMath.Clamp(options.BarGap, 0f, 0.8f);
            var availableGroupWidth = groupWidth * (1f - groupGap);
            var barCount = options.Stacked ? 1 : Math.Max(1, options.Series.Count);
            var barWidth = barCount == 0 ? 0 : availableGroupWidth / barCount;
            var barInnerGap = barWidth * barGap;
            var actualBarWidth = Math.Max(1f, barWidth - barInnerGap);

            for (var c = 0; c < categories.Count; c++) {
                var baseX = layout.PlotArea.Left + c * groupWidth + (groupWidth - availableGroupWidth) / 2f;
                if (options.Stacked) {
                    var positiveTotal = 0d;
                    var negativeTotal = 0d;
                    foreach (var series in options.Series) {
                        if (c >= series.Values.Count) continue;
                        var value = series.Values[c];
                        double startValue;
                        double endValue;
                        if (value >= 0) {
                            startValue = positiveTotal;
                            positiveTotal += value;
                            endValue = positiveTotal;
                        } else {
                            startValue = negativeTotal;
                            negativeTotal += value;
                            endValue = negativeTotal;
                        }

                        var startY = yScale.ToY(layout.PlotArea, startValue);
                        var endY = yScale.ToY(layout.PlotArea, endValue);
                        var rect = new RectangleF(baseX, Math.Min(startY, endY), Math.Max(1f, availableGroupWidth), Math.Abs(endY - startY));
                        FillRect(graphics, rect, series.Color);
                    }
                } else {
                    for (var s = 0; s < options.Series.Count; s++) {
                        var series = options.Series[s];
                        if (c >= series.Values.Count) continue;
                        var value = series.Values[c];
                        var x = baseX + s * barWidth + barInnerGap / 2f;
                        var y = yScale.ToY(layout.PlotArea, value);
                        var zeroY = yScale.ToY(layout.PlotArea, 0);
                        var height = zeroY - y;
                        var rect = new RectangleF(x, Math.Min(y, zeroY), actualBarWidth, Math.Abs(height));
                        FillRect(graphics, rect, series.Color);
                    }
                }
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
            DrawLegend(graphics, layout.LegendArea, options.Legend, items, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a line or area chart.</summary>
    public static Image RenderLine(ChartLineOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var items = GetLegendItems(options.Series.Select(s => (s.Name, s.Color)).ToArray());
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, items, fonts);

            DrawPlotBackground(graphics, layout.PlotArea, options.Style);
            if (options.Series.Count == 0) {
                DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
                return;
            }

            var maxCount = options.Series.Select(s => s.Values.Count).DefaultIfEmpty(0).Max();
            var categories = BuildCategories(options.Categories, maxCount);
            var yScale = AxisScale.FromSeries(options.Series.SelectMany(s => s.Values), options.YAxis, includeZero: false);

            DrawYAxis(graphics, layout.PlotArea, yScale, options.YAxis, fonts, options.Style);
            DrawXAxisLabels(graphics, layout.PlotArea, categories, fonts, options.Style);

            var step = maxCount > 1 ? layout.PlotArea.Width / (maxCount - 1) : 0f;
            foreach (var series in options.Series) {
                if (series.Values.Count == 0) continue;
                var points = new PointF[series.Values.Count];
                for (var i = 0; i < series.Values.Count; i++) {
                    var x = layout.PlotArea.Left + step * i;
                    var y = yScale.ToY(layout.PlotArea, series.Values[i]);
                    points[i] = new PointF(x, y);
                }

                if (options.FillArea || series.FillColor.HasValue) {
                    var fill = series.FillColor ?? Color.FromArgb(80, series.Color);
                    using var brush = new SolidBrush(fill);
                    var poly = new List<PointF>(points.Length + 2) {
                        new(points[0].X, layout.PlotArea.Bottom)
                    };
                    poly.AddRange(points);
                    poly.Add(new PointF(points[points.Length - 1].X, layout.PlotArea.Bottom));
                    graphics.FillPolygon(brush, poly.ToArray());
                }

                using var pen = new Pen(series.Color, series.Thickness);
                if (points.Length > 1) {
                    graphics.DrawLines(pen, points);
                } else {
                    graphics.DrawEllipse(pen, points[0].X - 1, points[0].Y - 1, 2, 2);
                }

                if (series.MarkerShape != ChartMarkerShape.None) {
                    DrawMarkers(graphics, points, series);
                }
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
            DrawLegend(graphics, layout.LegendArea, options.Legend, items, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a scatter chart.</summary>
    public static Image RenderScatter(ChartScatterOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var items = GetLegendItems(options.Series.Select(s => (s.Name, s.Color)).ToArray());
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, items, fonts);

            DrawPlotBackground(graphics, layout.PlotArea, options.Style);
            if (options.Series.Count == 0) {
                DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
                return;
            }

            var allPoints = options.Series.SelectMany(s => s.Points).ToArray();
            var xScale = AxisScale.FromSeries(allPoints.Select(p => p.X), options.XAxis, includeZero: false);
            var yScale = AxisScale.FromSeries(allPoints.Select(p => p.Y), options.YAxis, includeZero: false);

            DrawYAxis(graphics, layout.PlotArea, yScale, options.YAxis, fonts, options.Style);
            DrawXAxisNumeric(graphics, layout.PlotArea, xScale, options.XAxis, fonts, options.Style);

            foreach (var series in options.Series) {
                using var brush = new SolidBrush(series.Color);
                foreach (var point in series.Points) {
                    var x = xScale.ToX(layout.PlotArea, point.X);
                    var y = yScale.ToY(layout.PlotArea, point.Y);
                    var size = series.MarkerSize;
                    graphics.FillEllipse(brush, x - size / 2f, y - size / 2f, size, size);
                }
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
            DrawLegend(graphics, layout.LegendArea, options.Legend, items, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a bubble chart.</summary>
    public static Image RenderBubble(ChartBubbleOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var items = GetLegendItems(options.Series.Select(s => (s.Name, s.Color)).ToArray());
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, items, fonts);

            DrawPlotBackground(graphics, layout.PlotArea, options.Style);
            if (options.Series.Count == 0) {
                DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
                return;
            }

            var allPoints = options.Series.SelectMany(s => s.Points).ToArray();
            var xScale = AxisScale.FromSeries(allPoints.Select(p => p.X), options.XAxis, includeZero: false);
            var yScale = AxisScale.FromSeries(allPoints.Select(p => p.Y), options.YAxis, includeZero: false);
            var sizeScale = AxisScale.FromSeries(allPoints.Select(p => p.Size), new ChartAxis(), includeZero: true);

            DrawYAxis(graphics, layout.PlotArea, yScale, options.YAxis, fonts, options.Style);
            DrawXAxisNumeric(graphics, layout.PlotArea, xScale, options.XAxis, fonts, options.Style);

            foreach (var series in options.Series) {
                using var brush = new SolidBrush(Color.FromArgb(180, series.Color));
                foreach (var point in series.Points) {
                    var x = xScale.ToX(layout.PlotArea, point.X);
                    var y = yScale.ToY(layout.PlotArea, point.Y);
                    var t = sizeScale.Normalize(point.Size);
                    var radius = series.MinRadius + (series.MaxRadius - series.MinRadius) * t;
                    graphics.FillEllipse(brush, x - radius, y - radius, radius * 2f, radius * 2f);
                }
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
            DrawLegend(graphics, layout.LegendArea, options.Legend, items, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a pie or donut chart.</summary>
    public static Image RenderPie(ChartPieOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var items = GetLegendItems(options.Slices.Select(s => (s.Label, s.Color)).ToArray());
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, items, fonts);

            DrawPlotBackground(graphics, layout.PlotArea, options.Style);
            var total = options.Slices.Sum(s => Math.Max(0, s.Value));
            if (total <= 0) {
                DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
                return;
            }

            var center = new PointF(layout.PlotArea.Left + layout.PlotArea.Width / 2f, layout.PlotArea.Top + layout.PlotArea.Height / 2f);
            var radius = Math.Min(layout.PlotArea.Width, layout.PlotArea.Height) / 2f * 0.9f;
            var rect = new RectangleF(center.X - radius, center.Y - radius, radius * 2f, radius * 2f);

            var startAngle = -90f;
            foreach (var slice in options.Slices) {
                var sweep = (float)(slice.Value / total * 360f);
                using var brush = new SolidBrush(slice.Color);
                graphics.FillPie(brush, Rectangle.Round(rect), startAngle, sweep);
                startAngle += sweep;
            }

            if (options.InnerRadiusRatio > 0f) {
                var innerRadius = radius * GdiMath.Clamp(options.InnerRadiusRatio, 0f, 0.9f);
                var innerRect = new RectangleF(center.X - innerRadius, center.Y - innerRadius, innerRadius * 2f, innerRadius * 2f);
                using var brush = new SolidBrush(options.Style.BackgroundColor);
                graphics.FillEllipse(brush, innerRect);
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
            DrawLegend(graphics, layout.LegendArea, options.Legend, items, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a histogram chart.</summary>
    public static Image RenderHistogram(ChartHistogramOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, Array.Empty<LegendItem>(), fonts);

            DrawPlotBackground(graphics, layout.PlotArea, options.Style);
            if (options.Values.Count == 0 || options.BinCount <= 0) {
                DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
                return;
            }

            var min = options.Values.Min();
            var max = options.Values.Max();
            if (Math.Abs(max - min) < double.Epsilon) {
                max = min + 1;
            }

            var bins = new int[options.BinCount];
            var binSize = (max - min) / options.BinCount;
            foreach (var value in options.Values) {
                var index = (int)Math.Floor((value - min) / binSize);
                if (index >= options.BinCount) index = options.BinCount - 1;
                if (index < 0) index = 0;
                bins[index]++;
            }

            var yScale = AxisScale.FromSeries(bins.Select(b => (double)b), options.YAxis, includeZero: true);
            DrawYAxis(graphics, layout.PlotArea, yScale, options.YAxis, fonts, options.Style);

            var barWidth = layout.PlotArea.Width / options.BinCount;
            for (var i = 0; i < bins.Length; i++) {
                var x = layout.PlotArea.Left + i * barWidth;
                var y = yScale.ToY(layout.PlotArea, bins[i]);
                var zeroY = yScale.ToY(layout.PlotArea, 0);
                var rect = new RectangleF(x + 1, Math.Min(y, zeroY), barWidth - 2, Math.Abs(zeroY - y));
                FillRect(graphics, rect, options.Color);
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a heatmap chart.</summary>
    public static Image RenderHeatmap(ChartHeatmapOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, Array.Empty<LegendItem>(), fonts);
            DrawPlotBackground(graphics, layout.PlotArea, options.Style);

            var rows = options.Values.GetLength(0);
            var cols = options.Values.GetLength(1);
            if (rows == 0 || cols == 0) {
                DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
                return;
            }

            var min = options.Min ?? options.Values.Cast<double>().Min();
            var max = options.Max ?? options.Values.Cast<double>().Max();
            if (Math.Abs(max - min) < double.Epsilon) {
                max = min + 1;
            }

            var cellWidth = layout.PlotArea.Width / cols;
            var cellHeight = layout.PlotArea.Height / rows;
            for (var r = 0; r < rows; r++) {
                for (var c = 0; c < cols; c++) {
                    var value = options.Values[r, c];
                    var t = (float)((value - min) / (max - min));
                    var color = options.Gradient.GetColor(t);
                    var rect = new RectangleF(layout.PlotArea.Left + c * cellWidth, layout.PlotArea.Top + r * cellHeight, cellWidth, cellHeight);
                    using var brush = new SolidBrush(color);
                    graphics.FillRectangle(brush, rect);
                }
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a polar chart.</summary>
    public static Image RenderPolar(ChartPolarOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var items = GetLegendItems(options.Series.Select(s => (s.Name, s.Color)).ToArray());
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, items, fonts);
            DrawPlotBackground(graphics, layout.PlotArea, options.Style);

            var center = new PointF(layout.PlotArea.Left + layout.PlotArea.Width / 2f, layout.PlotArea.Top + layout.PlotArea.Height / 2f);
            var radius = Math.Min(layout.PlotArea.Width, layout.PlotArea.Height) / 2f;
            var maxRadius = options.Series.SelectMany(s => s.Points.Select(p => p.Radius)).DefaultIfEmpty(0).Max();
            if (maxRadius <= 0) maxRadius = 1;

            using var gridPen = new Pen(options.Style.GridColor, 1f) { DashStyle = DashStyle.Dot };
            for (var i = 1; i <= 4; i++) {
                var r = radius * i / 4f;
                graphics.DrawEllipse(gridPen, center.X - r, center.Y - r, r * 2f, r * 2f);
            }

            foreach (var series in options.Series) {
                if (series.Points.Count == 0) continue;
                var points = series.Points.Select(p => {
                    var r = (float)(p.Radius / maxRadius) * radius;
                    var angleRad = (float)(Math.PI / 180d * p.Angle);
                    return new PointF(center.X + r * (float)Math.Cos(angleRad), center.Y + r * (float)Math.Sin(angleRad));
                }).ToArray();

                using var pen = new Pen(series.Color, series.Thickness);
                if (points.Length > 1) {
                    graphics.DrawPolygon(pen, points);
                } else if (points.Length == 1) {
                    graphics.DrawEllipse(pen, points[0].X - 2, points[0].Y - 2, 4, 4);
                }
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
            DrawLegend(graphics, layout.LegendArea, options.Legend, items, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a radial bar chart.</summary>
    public static Image RenderRadial(ChartRadialOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var items = GetLegendItems(options.Series.Select(s => (s.Name, s.Color)).ToArray());
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, options.Legend, items, fonts);
            DrawPlotBackground(graphics, layout.PlotArea, options.Style);

            var categories = BuildCategories(options.Categories, options.Series.Select(s => s.Values.Count).DefaultIfEmpty(0).Max());
            if (categories.Count == 0) {
                DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
                return;
            }

            var center = new PointF(layout.PlotArea.Left + layout.PlotArea.Width / 2f, layout.PlotArea.Top + layout.PlotArea.Height / 2f);
            var radius = Math.Min(layout.PlotArea.Width, layout.PlotArea.Height) / 2f * 0.9f;
            var maxValue = options.Series.SelectMany(s => s.Values).DefaultIfEmpty(0).Max();
            if (maxValue <= 0) maxValue = 1;

            var angleStep = 360f / categories.Count;
            for (var c = 0; c < categories.Count; c++) {
                var startAngle = -90f + c * angleStep;
                foreach (var series in options.Series) {
                    if (c >= series.Values.Count) continue;
                    var value = series.Values[c];
                    var sweep = angleStep * 0.7f;
                    var outer = (float)(value / maxValue) * radius;
                    var rect = new RectangleF(center.X - outer, center.Y - outer, outer * 2f, outer * 2f);
                    using var brush = new SolidBrush(series.Color);
                    graphics.FillPie(brush, Rectangle.Round(rect), startAngle + angleStep * 0.15f, sweep);
                }
            }

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
            DrawLegend(graphics, layout.LegendArea, options.Legend, items, fonts, options.Style);
        });

        return image;
    }

    /// <summary>Renders a gauge chart.</summary>
    public static Image RenderGauge(ChartGaugeOptions options) {
        if (options is null) throw new ArgumentNullException(nameof(options));
        var image = new Image();
        image.Create(string.Empty, options.Canvas.Width, options.Canvas.Height, options.Style.BackgroundColor);

        image.WithGraphics(graphics => {
            ConfigureGraphics(graphics, options.Style);
            using var fonts = new ChartFonts(graphics, options.Style);
            var layout = ChartLayout.Build(graphics, options.Canvas, options.Style, options.Title, new ChartLegend { Show = false }, Array.Empty<LegendItem>(), fonts);
            DrawPlotBackground(graphics, layout.PlotArea, options.Style);

            var center = new PointF(layout.PlotArea.Left + layout.PlotArea.Width / 2f, layout.PlotArea.Bottom);
            var radius = Math.Min(layout.PlotArea.Width, layout.PlotArea.Height) * 0.9f;
            var startAngle = 180f;
            var sweepAngle = 180f;
            var hasValueRange = Math.Abs(options.Max - options.Min) >= double.Epsilon;

            var ranges = options.Ranges.Count > 0 ? options.Ranges : new[] {
                new ChartGaugeRange { Start = options.Min, End = options.Max, Color = Color.LimeGreen }
            };

            if (hasValueRange) {
                foreach (var range in ranges) {
                    var rangeStart = (range.Start - options.Min) / (options.Max - options.Min);
                    var rangeEnd = (range.End - options.Min) / (options.Max - options.Min);
                    var rangeSweep = (float)((rangeEnd - rangeStart) * sweepAngle);
                    var rangeAngle = startAngle + (float)(rangeStart * sweepAngle);
                    using var pen = new Pen(range.Color, 18f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
                    graphics.DrawArc(pen, center.X - radius / 2f, center.Y - radius / 2f, radius, radius, rangeAngle, rangeSweep);
                }
            } else {
                // Degenerate gauges still render deterministically instead of propagating NaN/Infinity into GDI.
                var fallbackColor = ranges[ranges.Count - 1].Color;
                using var pen = new Pen(fallbackColor, 18f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
                graphics.DrawArc(pen, center.X - radius / 2f, center.Y - radius / 2f, radius, radius, startAngle, sweepAngle);
            }

            var valueT = hasValueRange
                ? GdiMath.Clamp((options.Value - options.Min) / (options.Max - options.Min), 0d, 1d)
                : 0.5d;
            var needleAngle = startAngle + (float)(valueT * sweepAngle);
            var needleRad = (float)(Math.PI / 180f * needleAngle);
            var needleLength = radius * 0.45f;
            var needleEnd = new PointF(center.X + needleLength * (float)Math.Cos(needleRad), center.Y + needleLength * (float)Math.Sin(needleRad));
            using var needlePen = new Pen(Color.DimGray, 3f);
            graphics.DrawLine(needlePen, center, needleEnd);
            using var hubBrush = new SolidBrush(Color.DimGray);
            graphics.FillEllipse(hubBrush, center.X - 6, center.Y - 6, 12, 12);

            var valueText = options.Value.ToString("0.##");
            using var valueFont = new Font(options.Style.FontFamilyName, options.Style.FontSize + 4f, FontStyle.Bold, GraphicsUnit.Pixel);
            var valueSize = graphics.MeasureString(valueText, valueFont);
            graphics.DrawString(valueText, valueFont, new SolidBrush(options.Style.TextColor), center.X - valueSize.Width / 2f, center.Y - valueSize.Height - 10f);

            DrawTitle(graphics, layout.TitleArea, options.Title, fonts, options.Style);
        });

        return image;
    }

    private static void ConfigureGraphics(Graphics graphics, ChartStyle style) {
        if (style.AntiAlias) {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        } else {
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = PixelOffsetMode.None;
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
        }
    }

    private static void DrawPlotBackground(Graphics graphics, RectangleF plotArea, ChartStyle style) {
        using var brush = new SolidBrush(style.PlotAreaColor);
        graphics.FillRectangle(brush, plotArea);
    }

    private static void DrawTitle(Graphics graphics, RectangleF area, ChartTitle title, ChartFonts fonts, ChartStyle style) {
        if (string.IsNullOrWhiteSpace(title.Text) && string.IsNullOrWhiteSpace(title.Subtitle)) return;
        var y = area.Top;
        if (!string.IsNullOrWhiteSpace(title.Text)) {
            var size = graphics.MeasureString(title.Text, fonts.TitleFont);
            graphics.DrawString(title.Text, fonts.TitleFont, new SolidBrush(style.TextColor), area.Left + (area.Width - size.Width) / 2f, y);
            y += size.Height + 2f;
        }
        if (!string.IsNullOrWhiteSpace(title.Subtitle)) {
            var size = graphics.MeasureString(title.Subtitle, fonts.BaseFont);
            graphics.DrawString(title.Subtitle, fonts.BaseFont, new SolidBrush(style.TextColor), area.Left + (area.Width - size.Width) / 2f, y);
        }
    }

    private static void DrawLegend(Graphics graphics, RectangleF area, ChartLegend legend, IReadOnlyList<LegendItem> items, ChartFonts fonts, ChartStyle style) {
        if (!legend.Show || legend.Position == ChartLegendPosition.None || items.Count == 0) return;
        using var textBrush = new SolidBrush(style.TextColor);
        using var outlinePen = new Pen(style.AxisColor, 1f);
        var x = area.Left;
        var y = area.Top;

        if (legend.Position == ChartLegendPosition.Left || legend.Position == ChartLegendPosition.Right) {
            foreach (var item in items) {
                using var swatchBrush = new SolidBrush(item.Color);
                graphics.FillRectangle(swatchBrush, x, y + 2f, legend.SwatchSize, legend.SwatchSize);
                graphics.DrawRectangle(outlinePen, x, y + 2f, legend.SwatchSize, legend.SwatchSize);
                graphics.DrawString(item.Label, fonts.LegendFont, textBrush, x + legend.SwatchSize + 6f, y);
                y += fonts.LegendFont.Height + legend.ItemSpacing;
            }
        } else {
            foreach (var item in items) {
                using var swatchBrush = new SolidBrush(item.Color);
                graphics.FillRectangle(swatchBrush, x, y + 2f, legend.SwatchSize, legend.SwatchSize);
                graphics.DrawRectangle(outlinePen, x, y + 2f, legend.SwatchSize, legend.SwatchSize);
                graphics.DrawString(item.Label, fonts.LegendFont, textBrush, x + legend.SwatchSize + 6f, y);
                var textWidth = graphics.MeasureString(item.Label, fonts.LegendFont).Width;
                x += legend.SwatchSize + 6f + textWidth + legend.ItemSpacing;
            }
        }
    }

    private static void DrawYAxis(Graphics graphics, RectangleF plotArea, AxisScale scale, ChartAxis axis, ChartFonts fonts, ChartStyle style) {
        using var axisPen = new Pen(style.AxisColor, 1f);
        using var gridPen = new Pen(style.GridColor, 1f) { DashStyle = DashStyle.Dot };
        using var textBrush = new SolidBrush(style.TextColor);

        graphics.DrawLine(axisPen, plotArea.Left, plotArea.Top, plotArea.Left, plotArea.Bottom);
        graphics.DrawLine(axisPen, plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);

        var tickCount = Math.Max(2, axis.TickCount);
        for (var i = 0; i < tickCount; i++) {
            var t = i / (float)(tickCount - 1);
            var value = scale.Min + (scale.Max - scale.Min) * (tickCount - 1 - i) / (tickCount - 1);
            var y = plotArea.Top + t * plotArea.Height;
            if (axis.ShowGrid) {
                graphics.DrawLine(gridPen, plotArea.Left, y, plotArea.Right, y);
            }
            var label = value.ToString(axis.LabelFormat);
            var size = graphics.MeasureString(label, fonts.BaseFont);
            graphics.DrawString(label, fonts.BaseFont, textBrush, plotArea.Left - size.Width - 6f, y - size.Height / 2f);
        }
    }

    private static void DrawXAxisLabels(Graphics graphics, RectangleF plotArea, IReadOnlyList<string> categories, ChartFonts fonts, ChartStyle style) {
        if (categories.Count == 0) return;
        using var textBrush = new SolidBrush(style.TextColor);
        var step = plotArea.Width / categories.Count;
        for (var i = 0; i < categories.Count; i++) {
            var label = categories[i];
            if (string.IsNullOrWhiteSpace(label)) continue;
            var size = graphics.MeasureString(label, fonts.BaseFont);
            var x = plotArea.Left + step * i + step / 2f - size.Width / 2f;
            var y = plotArea.Bottom + 4f;
            graphics.DrawString(label, fonts.BaseFont, textBrush, x, y);
        }
    }

    private static void DrawXAxisNumeric(Graphics graphics, RectangleF plotArea, AxisScale scale, ChartAxis axis, ChartFonts fonts, ChartStyle style) {
        using var textBrush = new SolidBrush(style.TextColor);
        var tickCount = Math.Max(2, axis.TickCount);
        for (var i = 0; i < tickCount; i++) {
            var t = i / (float)(tickCount - 1);
            var value = scale.Min + (scale.Max - scale.Min) * t;
            var x = plotArea.Left + t * plotArea.Width;
            var label = value.ToString(axis.LabelFormat);
            var size = graphics.MeasureString(label, fonts.BaseFont);
            graphics.DrawString(label, fonts.BaseFont, textBrush, x - size.Width / 2f, plotArea.Bottom + 4f);
        }
    }

    private static void DrawMarkers(Graphics graphics, PointF[] points, ChartLineSeries series) {
        using var brush = new SolidBrush(series.Color);
        foreach (var point in points) {
            var size = series.MarkerSize;
            var rect = new RectangleF(point.X - size / 2f, point.Y - size / 2f, size, size);
            switch (series.MarkerShape) {
                case ChartMarkerShape.Circle:
                    graphics.FillEllipse(brush, rect);
                    break;
                case ChartMarkerShape.Square:
                    graphics.FillRectangle(brush, rect);
                    break;
                case ChartMarkerShape.Diamond:
                    var center = new PointF(rect.Left + rect.Width / 2f, rect.Top + rect.Height / 2f);
                    var diamond = new[] {
                        new PointF(center.X, rect.Top),
                        new PointF(rect.Right, center.Y),
                        new PointF(center.X, rect.Bottom),
                        new PointF(rect.Left, center.Y)
                    };
                    graphics.FillPolygon(brush, diamond);
                    break;
            }
        }
    }

    private static IReadOnlyList<string> BuildCategories(IReadOnlyList<string> categories, int count) {
        if (categories.Count >= count) return categories;
        if (count <= 0) return Array.Empty<string>();
        var list = new List<string>(count);
        for (var i = 0; i < count; i++) {
            list.Add(i < categories.Count ? categories[i] : (i + 1).ToString());
        }
        return list;
    }

    private static void FillRect(Graphics graphics, RectangleF rect, Color color) {
        using var brush = new SolidBrush(color);
        graphics.FillRectangle(brush, rect);
    }

    private static LegendItem[] GetLegendItems((string Name, Color Color)[] items) {
        return items
            .Where(i => !string.IsNullOrWhiteSpace(i.Name))
            .Select(i => new LegendItem(i.Name, i.Color))
            .ToArray();
    }

    private static IEnumerable<double> GetStackedBarExtents(ChartBarOptions options, int categoryCount) {
        for (var c = 0; c < categoryCount; c++) {
            var positiveTotal = 0d;
            var negativeTotal = 0d;
            foreach (var series in options.Series) {
                if (c >= series.Values.Count) continue;
                var value = series.Values[c];
                if (value >= 0) {
                    positiveTotal += value;
                } else {
                    negativeTotal += value;
                }
            }

            yield return positiveTotal;
            yield return negativeTotal;
        }
    }

    private readonly struct LegendItem {
        public string Label { get; }
        public Color Color { get; }

        public LegendItem(string label, Color color) {
            Label = label;
            Color = color;
        }
    }

    private sealed class ChartFonts : IDisposable {
        public Font BaseFont { get; }
        public Font TitleFont { get; }
        public Font LegendFont { get; }

        public ChartFonts(Graphics graphics, ChartStyle style) {
            BaseFont = CreateFont(style.FontFamilyName, style.FontSize);
            TitleFont = CreateFont(style.FontFamilyName, style.TitleFontSize, FontStyle.Bold);
            LegendFont = CreateFont(style.FontFamilyName, style.LegendFontSize);
        }

        public void Dispose() {
            BaseFont.Dispose();
            TitleFont.Dispose();
            LegendFont.Dispose();
        }

        private static Font CreateFont(string family, float size, FontStyle style = FontStyle.Regular) {
            try {
                return new Font(family, size, style, GraphicsUnit.Pixel);
            } catch (ArgumentException) {
                return new Font(FontFamily.GenericSansSerif, size, style, GraphicsUnit.Pixel);
            }
        }
    }

    private readonly struct AxisScale {
        public double Min { get; }
        public double Max { get; }

        private AxisScale(double min, double max) {
            Min = min;
            Max = max;
        }

        public float ToY(RectangleF plotArea, double value) {
            if (Math.Abs(Max - Min) < double.Epsilon) return plotArea.Bottom;
            var t = (value - Min) / (Max - Min);
            t = GdiMath.Clamp(t, 0d, 1d);
            return plotArea.Bottom - (float)(t * plotArea.Height);
        }

        public float ToX(RectangleF plotArea, double value) {
            if (Math.Abs(Max - Min) < double.Epsilon) return plotArea.Left;
            var t = (value - Min) / (Max - Min);
            t = GdiMath.Clamp(t, 0d, 1d);
            return plotArea.Left + (float)(t * plotArea.Width);
        }

        public float Normalize(double value) {
            if (Math.Abs(Max - Min) < double.Epsilon) return 0f;
            var t = (value - Min) / (Max - Min);
            return (float)GdiMath.Clamp(t, 0d, 1d);
        }

        public static AxisScale FromSeries(IEnumerable<double> values, ChartAxis axis, bool includeZero) {
            var list = values.ToList();
            var min = list.Count == 0 ? 0 : list.Min();
            var max = list.Count == 0 ? 1 : list.Max();
            if (includeZero) {
                min = Math.Min(min, 0);
                max = Math.Max(max, 0);
            }
            var hasExplicitBounds = axis.Min.HasValue || axis.Max.HasValue;
            if (axis.Min.HasValue) min = axis.Min.Value;
            if (axis.Max.HasValue) max = axis.Max.Value;
            if (Math.Abs(max - min) < double.Epsilon) {
                max = min + 1;
            }
            var padding = hasExplicitBounds ? 0 : (max - min) * 0.05;
            return new AxisScale(min - padding, max + padding);
        }
    }

    private sealed class ChartLayout {
        public RectangleF PlotArea { get; private set; }
        public RectangleF TitleArea { get; private set; }
        public RectangleF LegendArea { get; private set; }

        public static ChartLayout Build(Graphics graphics, ChartCanvas canvas, ChartStyle style, ChartTitle title, ChartLegend legend, IReadOnlyList<LegendItem> items, ChartFonts fonts) {
            var layout = new ChartLayout();
            var full = new RectangleF(0, 0, canvas.Width, canvas.Height);
            var plot = canvas.Padding.Apply(full);

            var titleHeight = 0f;
            if (!string.IsNullOrWhiteSpace(title.Text)) {
                titleHeight += graphics.MeasureString(title.Text, fonts.TitleFont).Height + 4f;
            }
            if (!string.IsNullOrWhiteSpace(title.Subtitle)) {
                titleHeight += graphics.MeasureString(title.Subtitle, fonts.BaseFont).Height + 2f;
            }

            layout.TitleArea = new RectangleF(plot.Left, plot.Top - titleHeight, plot.Width, titleHeight);
            plot.Y += titleHeight;
            plot.Height -= titleHeight;

            if (legend.Show && legend.Position != ChartLegendPosition.None && items.Count > 0) {
                var legendHeight = fonts.LegendFont.Height + legend.ItemSpacing;
                var legendWidth = items.Select(i => graphics.MeasureString(i.Label, fonts.LegendFont).Width).DefaultIfEmpty(0f).Max() + legend.SwatchSize + 20f;
                switch (legend.Position) {
                    case ChartLegendPosition.Top:
                        layout.LegendArea = new RectangleF(plot.Left, plot.Top, plot.Width, legendHeight + 4f);
                        plot.Y += layout.LegendArea.Height;
                        plot.Height -= layout.LegendArea.Height;
                        break;
                    case ChartLegendPosition.Bottom:
                        layout.LegendArea = new RectangleF(plot.Left, plot.Bottom - (legendHeight + 4f), plot.Width, legendHeight + 4f);
                        plot.Height -= layout.LegendArea.Height;
                        break;
                    case ChartLegendPosition.Left:
                        layout.LegendArea = new RectangleF(plot.Left, plot.Top, legendWidth, plot.Height);
                        plot.X += legendWidth;
                        plot.Width -= legendWidth;
                        break;
                    case ChartLegendPosition.Right:
                        layout.LegendArea = new RectangleF(plot.Right - legendWidth, plot.Top, legendWidth, plot.Height);
                        plot.Width -= legendWidth;
                        break;
                }
            }

            layout.PlotArea = plot;
            return layout;
        }
    }
}
