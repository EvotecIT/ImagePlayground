# ImagePlayground.Chart

ImagePlayground.Chart contains the ImagePlayground chart wrapper API backed by ChartForgeX.

This package remains available for compatibility with projects that intentionally reference the split assembly. New ImagePlayground users should usually reference `ImagePlayground`; applications that need direct access to the rendering engine should reference `ChartForgeX`.

## Supported Areas

- Bar, line, area, scatter, bubble, polar, radar, heatmap, histogram, pie, donut, radial, gauge, circle, progress, pictorial, and word-cloud chart definitions.
- Chart render options for transparent backgrounds, legends, axes, grids, labels, palette, donut center text, progress bars, pictorial symbols, and word-cloud terms.
- PNG, SVG, and HTML output through ChartForgeX.

## Related Package

- `ChartForgeX` is the direct engine for charts, visual blocks, report tables, metric cards, visual grids, topology diagrams, and raster/SVG/HTML export.
