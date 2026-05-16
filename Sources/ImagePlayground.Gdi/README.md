# ImagePlayground.Gdi

ImagePlayground.Gdi is an optional Windows-focused GDI+ sidecar for image composition scenarios that need System.Drawing behavior.

This package is intentionally separate from the main cross-platform ImagePlayground package. It remains useful for desktop overlay and wallpaper scenarios, including PowerBGInfo-style rendering.

## Supported Areas

- Load, create, resize, and save images through GDI+.
- Measure and draw text using Windows fonts.
- Draw text with optional shadow and outline.
- Draw basic lines, rectangles, ellipses, and filled shapes.
- Compose one image onto another.
- Access a configured `System.Drawing.Graphics` surface for custom rendering.
- Legacy GDI+ chart helpers for small Windows-only scenarios.

For new chart rendering, prefer `ChartForgeX`. The chart helpers in this package are kept as lightweight legacy GDI+ helpers and are not the primary ImagePlayground chart engine.
