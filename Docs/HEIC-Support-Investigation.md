# HEIC Support Investigation for Issue #8

This note captures what it would take to add Apple HEIC support to ImagePlayground, with special attention to a "no new NuGet dependency" approach.

## Current state

ImagePlayground currently routes image loading through `SixLabors.ImageSharp.Image.Load(...)` in a few central places:

- `Sources/ImagePlayground.Core/Image.cs`
- `Sources/ImagePlayground.Core/ImageHelper.cs`
- `Sources/ImagePlayground.Core/Image.Image.cs`
- `Sources/ImagePlayground.BarCode/BarCode.cs`
- `Sources/ImagePlayground.QRCode/QRCode.cs`

Supported output formats are also hard-coded today and do not include `.heic` or `.heif`:

- `Sources/ImagePlayground.Core/Helpers.cs`
- `Sources/ImagePlayground.Core/Helpers.Encoder.cs`

That means HEIC support is not a small extension-list change. The current decode stack does not understand HEIC at all.

## Local validation

I checked the current behavior with real `.heic` samples.

### What happens today

Using the current `ImagePlayground.Image.Load(...)` path against a real HEIC file throws:

```text
SixLabors.ImageSharp.UnknownImageFormatException
Image cannot be loaded. Available decoders:
 - PBM : PbmDecoder
 - TIFF : TiffDecoder
 - JPEG : JpegDecoder
 - TGA : TgaDecoder
 - Webp : WebpDecoder
 - GIF : GifDecoder
 - PNG : PngDecoder
 - BMP : BmpDecoder
```

So issue `#8` is real and reproducible.

### Native Windows capability

On this Windows machine, `Microsoft.HEIFImageExtension` is installed and a small WPF probe was able to load the same HEIC file successfully. That proves the operating system can decode HEIC even though ImageSharp cannot.

The same probe reported `Metadata format: heif`, so a Windows-native fallback is technically possible.

### EXIF is the harder part

I also tested a HEIC sample from `dsoprea/heic-exif-samples`, which is explicitly described as containing EXIF metadata.

The Windows-native probe could decode the pixels, but common high-level metadata properties did not populate automatically. In practice that means:

- pixel decode is one problem
- exposing HEIC EXIF through the current `Get-ImageExif` / `Export-ImageMetadata` paths is a second problem

After a deeper check, HEIF EXIF extraction looks much more realistic than full HEIC image decoding. In the tested sample:

- the top-level HEIF file used normal ISO BMFF boxes: `ftyp`, `meta`, and `mdat`
- `meta/iinf` contained an `infe` item with item type `Exif`
- `meta/iloc` pointed that item to an extent in `mdat`
- the extent started with a HEIF-specific 4-byte TIFF-header offset, then `Exif\0\0`, then a normal TIFF EXIF payload
- passing only the TIFF payload to `SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifProfile` produced basic EXIF values such as `Orientation`, `ResolutionUnit`, `XResolution`, and `YResolution`

So a dependency-free HEIF EXIF reader is feasible if the scope is metadata extraction.

## Implemented metadata-only support

This branch now includes dependency-free HEIF/HEIC support for basic container metadata and the metadata-focused EXIF APIs:

| Operation | Status |
| --- | --- |
| `Get-ImageHeifInfo` / `Image.GetHeifInfo(filePath)` | Supported for brands, primary item, item types, hidden/protection/content-encoding item metadata from `infe`, storage locations from `iloc` including file-backed and `idat`-backed items, property associations and essential flags from `ipma`, EXIF/XMP presence, item references, auxiliary image types from `auxC`, dimensions from `ispe`, rotation from `irot`, mirror state from `imir`, pixel aspect ratio spacing from `pasp`, pixel bit depths from `pixi`, color information from `colr`, and raw codec configuration payloads from `hvcC` / `av1C` / `avcC` |
| `Get-ImageHeifXmp` / `Image.GetHeifXmp(filePath)` | Supported for reading UTF-8 XMP MIME metadata items |
| `Set-ImageHeifXmp` / `Image.SetHeifXmp(filePath, output, xmp)` | Supported when the HEIF file already has an XMP item with a single writable file extent |
| `Remove-ImageHeifXmp` / `Image.RemoveHeifXmp(filePath, output)` | Supported for existing HEIF XMP items |
| `Get-ImageExif` / `Image.GetExifValues(filePath)` | Supported for `.heic` and `.heif` |
| `Export-ImageMetadata` / `ImageHelper.ExportMetadata` | Supported for HEIF EXIF payloads and XMP packets |
| `Import-ImageMetadata` / `ImageHelper.ImportMetadata` | Applies serialized EXIF and XMP profiles when the HEIF file already has matching writable metadata items |
| `ImageHelper.RemoveMetadata` | Clears existing writable HEIF EXIF and XMP payloads |
| `Set-ImageExif` / `Image.SetExifValue(filePath, output, tag, value)` | Supported when the HEIF file already has an EXIF item with a single writable file extent |
| `Remove-ImageExif` / `Image.RemoveExifValues(filePath, output, tags)` | Supported for existing HEIF EXIF items |
| `Remove-ImageExif -All` / `Image.ClearExifValues(filePath, output)` | Supported for existing HEIF EXIF items |

The write path intentionally does not try to create a brand-new HEIF EXIF item when one does not already exist. Creating one requires growing the HEIF `meta` box and adjusting item offsets across the container. That is possible, but it is a more invasive container rewrite than updating the existing `iloc` pointer.

Changing dimensions is intentionally not implemented. HEIF dimensions come from item properties such as `ispe`; changing those without changing the encoded image payload would create a misleading or invalid file. The safe no-codec feature is reading dimensions, not rewriting them.

## What a no-dependency implementation would require

### 1. Define scope first

The issue body asks for HEIC support in general. There are three different scopes hidden inside that request:

| Scope | What it means |
| --- | --- |
| Read support | Load HEIC and then resize, watermark, convert, compare, etc. |
| EXIF support | Make `Get-ImageExif`, metadata import/export, and EXIF mutation work with HEIC inputs |
| Write support | Save output as `.heic` |

Write support is the hardest and is not realistic without bringing in a real HEIF encoder stack.

### 2. Add a Windows-native decode fallback

The smallest no-new-package path is:

1. Keep ImageSharp as the default loader.
2. When ImageSharp throws `UnknownImageFormatException` for `.heic` or `.heif`, attempt a Windows-only fallback.
3. Use a native Windows imaging path to decode the file into raw BGRA/RGBA pixels.
4. Copy those pixels into an ImageSharp image so the rest of the library can stay unchanged.

That fallback should be wired into the central load choke-points first, especially:

- `Sources/ImagePlayground.Core/Image.cs`
- `Sources/ImagePlayground.Core/ImageHelper.cs`

Once those two are covered, most higher-level cmdlets inherit the new behavior automatically.

### 3. Decide how to talk to Windows imaging

There are two practical no-new-package choices:

| Option | Pros | Cons |
| --- | --- | --- |
| WPF / `BitmapFrame` / `BitmapSource` | Fast proof of concept on Windows, easy pixel copy | Not available on all target frameworks, awkward for a multi-target library that includes `netstandard2.0` |
| Direct WIC / COM interop | More explicit and lower level, can avoid WPF | Significantly more code, manual interop, more testing burden |

Given the current target frameworks (`netstandard2.0; net472; net8.0; net10.0`), this is where the "no dependencies" idea becomes expensive. The codebase is cross-platform, but a native fallback would be platform-specific and framework-specific.

### 4. Decide what to do about EXIF

If issue `#8` really means "support HEIC EXIF", not just "open HEIC images", then decode alone is not enough.

There are two realistic paths:

| Option | Pros | Cons |
| --- | --- | --- |
| Read metadata through native Windows metadata APIs | Keeps the no-new-package idea on Windows | Still Windows-only, still requires metadata mapping into current EXIF abstractions |
| Parse the HEIF container manually to extract the EXIF item | Could be cross-platform and dependency-free for metadata only | Separate parser work, does not solve image decoding |

Important detail: a metadata-only parser would not make resize/convert/watermark work on HEIC images. It would only help the EXIF-focused cmdlets.

A basic custom parser supports the common case by parsing only enough ISO BMFF structure to find an `Exif` item:

1. Walk top-level boxes to find `meta`.
2. Parse `iinf` / `infe` entries to find the item ID whose item type is `Exif`.
3. Parse `iloc` to locate that item's extent data.
4. Extract the EXIF extent from `mdat` or the referenced construction method.
5. Strip the HEIF EXIF header wrapper and feed the TIFF payload into `ExifProfile`.

The implementation includes defensive parsing for 32-bit and 64-bit box sizes, `iloc` versions 0/1/2, and unsupported construction methods. The write path only updates existing single-extent file-backed EXIF items, then appends the new EXIF payload and repoints the existing `iloc` extent to it.

### 5. Update behavior and tests

Even a read-only implementation would need:

- `.heic` / `.heif` extension handling in user-facing validation and messages
- tests for successful load on supported platforms
- tests for clear failure on unsupported platforms or missing OS codec
- documentation updates clarifying that HEIC is read-only unless a future encoder is added

## Recommendation

If the goal is to close issue `#8` with the least risk and no new NuGet dependency, the most realistic path is:

1. Implement Windows-only HEIC read support behind a fallback loader.
2. Treat HEIC as input-only.
3. Convert/manipulate in memory with ImageSharp after native decode.
4. Be explicit that support depends on the Windows HEIF codec being installed.
5. Either defer EXIF support or scope it as a separate follow-up.

If the goal is full cross-platform HEIC support, "no dependencies" is not a realistic constraint. At that point a dedicated HEIF/HEIC decoder dependency or native wrapper is the more honest path.

## Suggested decision for the issue

| Decision | Outcome |
| --- | --- |
| Accept Windows-only read support | Reasonable no-dependency implementation target |
| Require cross-platform HEIC read support | Likely needs an added dependency or native packaging work |
| Require HEIC EXIF parity with JPEG/PNG/TIFF | More work than decode alone and should be tracked separately if time matters |

## Rough effort estimate

These are ballpark estimates, not commitments:

| Work item | Estimate |
| --- | --- |
| Windows-only proof of concept decode fallback | 0.5 to 1 day |
| Productionizing load fallback and tests | 1 to 2 more days |
| Basic HEIF EXIF read/update/remove parser | Implemented in this branch |
| Windows-native EXIF plumbing | 1 to 2 more days |
| Cross-platform HEIC support without native dependency help | Not recommended / high uncertainty |
