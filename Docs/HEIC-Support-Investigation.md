# HEIF metadata and image support

ImagePlayground exposes OfficeIMO.Core's HEIF container information and metadata operations independently of pixel decoding. `Get-ImageHeifInfo` and `Image.GetHeifInfo` return `OfficeIMO.Drawing.OfficeHeifImageInfo`. HEIC, HEIF, and AVIF can use the same ISO BMFF container while carrying different image codecs; recognizing a container does not establish that its image payload can be decoded.

## Metadata operations

| Operation | Supported behavior |
| --- | --- |
| `Get-ImageHeifInfo` / `Image.GetHeifInfo` | Read brands, the primary item, item types, dimensions, item references, codec configuration, and associated image properties |
| `Get-ImageHeifXmp` / `Image.GetHeifXmp` | Read UTF-8 XMP MIME metadata items |
| `Set-ImageHeifXmp` / `Image.SetHeifXmp` | Replace an existing XMP item with a single writable file extent |
| `Remove-ImageHeifXmp` / `Image.RemoveHeifXmp` | Clear an existing writable XMP item |
| `Get-ImageExif` / `Image.GetExifValues` | Read EXIF values from HEIC/HEIF metadata items |
| `Set-ImageExif` / `Image.SetExifValue` | Update an existing EXIF item with a single writable file extent |
| `Remove-ImageExif` / `Image.RemoveExifValues` | Remove selected tags from an existing EXIF item |
| `Remove-ImageExif -All` / `Image.ClearExifValues` | Clear an existing writable EXIF item |
| `Export-ImageMetadata` / `ImageHelper.ExportMetadata` | Export EXIF and XMP profiles |
| `Import-ImageMetadata` / `ImageHelper.ImportMetadata` | Apply profiles when matching writable metadata items already exist |

The shared container reader handles file-backed, multi-extent, and `idat`-backed metadata locations. Mutation supports the narrower single-extent file-backed case: it appends a replacement payload and updates the existing item-location entry. It rejects unsupported layouts rather than changing their offsets speculatively.

EXIF-only operations read and write only EXIF; XMP-only operations read and write only XMP. An unrelated metadata item's opaque payload, unreadable location, or unsupported write layout does not block those selective operations, and its bytes are preserved. A full metadata import replaces all supported supplied families and rejects ICC or IPTC profiles. Metadata snapshots leave resolution fields null when the container reader does not expose a physical density.

Metadata import and removal finish all selected profile edits in memory before one atomic destination write. No intermediate file contains a partially edited metadata set. An unsupported requested item or invalid UTF-8 XMP rejects the operation without changing an existing destination. Removing an absent profile leaves the encoded container unchanged; importing a nonempty profile requires its matching writable item to exist.

Container information preserves protected item and content-encoding declarations. Reading, replacing, or clearing a protected EXIF item or a protected/encoded XMP item raises an unsupported-operation error; interpreting those payloads requires the corresponding protection or encoding support.

An EXIF item includes a HEIF-specific offset header and a classic TIFF profile. OfficeIMO.Core unwraps the item and exposes its owned EXIF tag and value APIs. Metadata access does not require an HEVC decoder or a native Windows imaging extension.

```powershell
$info = Get-ImageHeifInfo -FilePath '.\photo.heic'
$info
Get-ImageExif -FilePath '.\photo.heic' -Translate
Set-ImageExif -FilePath '.\photo.heic' -FilePathOutput '.\photo-tagged.heic' `
    -ExifTag ([OfficeIMO.Drawing.OfficeExifTag]::Software) -Value 'ImagePlayground'
```

## Pixel decoding and output

ImagePlayground uses OfficeIMO.Core's managed raster decoder. Its supported AVIF payloads are bounded 8/10-bit YUV420 or monochrome still items, including supported alpha. HEVC-compressed HEIC pixels and HEIC/HEIF output encoding are unsupported. A Windows codec installation does not change the managed library's decoding contract.

HEIF metadata operations do not change image dimensions or encoded pixel content. Changing an `ispe` dimension without re-encoding its image payload would make the container misleading. Metadata editing also does not create a new EXIF or XMP item when the container lacks one; that requires a broader item-table rewrite.
