using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground;

/// <summary>
/// Reads and updates metadata from HEIF/HEIC containers without decoding image pixels.
/// </summary>
internal static class HeifMetadataReader {
    private const string ExifItemType = "Exif";
    private const string XmpMimeType = "application/rdf+xml";

    public static bool TryReadInfo(string filePath, out HeifImageInfo? info) {
        info = null;

        byte[] data = File.ReadAllBytes(filePath);
        if (!TryReadFileType(data, out string majorBrand, out uint minorVersion, out List<string>? compatibleBrands)) {
            return false;
        }

        if (!TryFindMetaBox(data, out Box metaBox)) {
            return false;
        }

        int metaChildrenStart = metaBox.DataOffset + 4;
        if (metaChildrenStart > metaBox.EndOffset) {
            return false;
        }

        Box? itemInfoBox = null;
        Box? itemLocationBox = null;
        Box? itemDataBox = null;
        Box? itemPropertiesBox = null;
        Box? itemReferenceBox = null;
        uint? primaryItemId = null;

        foreach (Box childBox in EnumerateBoxes(data, metaChildrenStart, metaBox.EndOffset)) {
            if (childBox.Type == "iinf") {
                itemInfoBox = childBox;
            } else if (childBox.Type == "iloc") {
                itemLocationBox = childBox;
            } else if (childBox.Type == "idat") {
                itemDataBox = childBox;
            } else if (childBox.Type == "iprp") {
                itemPropertiesBox = childBox;
            } else if (childBox.Type == "iref") {
                itemReferenceBox = childBox;
            } else if (childBox.Type == "pitm" && TryReadPrimaryItemId(data, childBox, out uint childPrimaryItemId)) {
                primaryItemId = childPrimaryItemId;
            }
        }

        var itemBuilders = new List<HeifItemInfoBuilder>();
        if (itemInfoBox is not null && !TryReadItemInfos(data, itemInfoBox.Value, itemBuilders)) {
            return false;
        }

        if (itemPropertiesBox is not null) {
            TryApplyImageProperties(data, itemPropertiesBox.Value, itemBuilders);
        }

        Dictionary<uint, HeifItemLocationInfo> locations = itemLocationBox is not null
            ? ReadItemLocations(data, itemLocationBox.Value, itemDataBox)
            : new Dictionary<uint, HeifItemLocationInfo>();

        var items = itemBuilders
            .Select(item => new HeifItemInfo(
                item.ItemId,
                item.ItemType,
                item.ItemName,
                item.ItemProtectionIndex,
                item.IsHidden,
                item.MimeType,
                item.ContentEncoding,
                locations.TryGetValue(item.ItemId, out HeifItemLocationInfo? location) ? location : null,
                item.PropertyAssociations,
                primaryItemId.HasValue && item.ItemId == primaryItemId.Value,
                item.ItemType == ExifItemType,
                IsXmpMimeType(item.MimeType),
                item.Width,
                item.Height,
                item.RotationDegrees,
                item.IsMirrored,
                item.PixelAspectRatioHorizontalSpacing,
                item.PixelAspectRatioVerticalSpacing,
                item.PixelBitDepths,
                item.ColorType,
                item.ColorPrimaries,
                item.TransferCharacteristics,
                item.MatrixCoefficients,
                item.FullRangeFlag,
                item.CodecConfigurationType,
                item.CodecConfigurationBytes,
                item.AuxiliaryType,
                item.AuxiliarySubtypes))
            .ToList();
        List<HeifItemReference> references = itemReferenceBox is not null
            ? ReadItemReferences(data, itemReferenceBox.Value)
            : new List<HeifItemReference>();

        info = new HeifImageInfo(majorBrand, minorVersion, compatibleBrands!, primaryItemId, items, references);
        return true;
    }

    public static bool TryReadExifProfile(string filePath, out ExifProfile? profile) {
        profile = null;

        byte[] data = File.ReadAllBytes(filePath);
        if (!TryFindMetaBox(data, out Box metaBox)) {
            return false;
        }

        int metaChildrenStart = metaBox.DataOffset + 4;
        if (metaChildrenStart > metaBox.EndOffset) {
            return false;
        }

        Box? itemInfoBox = null;
        Box? itemLocationBox = null;
        Box? itemDataBox = null;

        foreach (Box childBox in EnumerateBoxes(data, metaChildrenStart, metaBox.EndOffset)) {
            if (childBox.Type == "iinf") {
                itemInfoBox = childBox;
            } else if (childBox.Type == "iloc") {
                itemLocationBox = childBox;
            } else if (childBox.Type == "idat") {
                itemDataBox = childBox;
            }
        }

        if (itemInfoBox is null || itemLocationBox is null) {
            return false;
        }

        if (!TryFindExifItemId(data, itemInfoBox.Value, out uint itemId)) {
            return false;
        }

        if (!TryFindItemExtents(data, itemLocationBox.Value, itemDataBox, itemId, out IlocItem item)) {
            return false;
        }

        if (!TryCopyExtents(data, item.Extents, out byte[]? exifItemData)) {
            return false;
        }

        if (exifItemData!.Length == 0) {
            profile = null;
            return true;
        }

        if (!TryGetTiffPayload(exifItemData, out byte[]? tiffPayload)) {
            return false;
        }

        try {
            profile = new ExifProfile(tiffPayload!);
            return true;
        } catch {
            profile = null;
            return false;
        }
    }

    public static bool TryReadXmp(string filePath, out string? xmp) {
        xmp = null;

        byte[] data = File.ReadAllBytes(filePath);
        if (!TryFindMetaBox(data, out Box metaBox)) {
            return false;
        }

        int metaChildrenStart = metaBox.DataOffset + 4;
        if (metaChildrenStart > metaBox.EndOffset) {
            return false;
        }

        Box? itemInfoBox = null;
        Box? itemLocationBox = null;
        Box? itemDataBox = null;

        foreach (Box childBox in EnumerateBoxes(data, metaChildrenStart, metaBox.EndOffset)) {
            if (childBox.Type == "iinf") {
                itemInfoBox = childBox;
            } else if (childBox.Type == "iloc") {
                itemLocationBox = childBox;
            } else if (childBox.Type == "idat") {
                itemDataBox = childBox;
            }
        }

        if (itemInfoBox is null || itemLocationBox is null) {
            return false;
        }

        if (!TryFindXmpItemId(data, itemInfoBox.Value, out uint itemId)) {
            return false;
        }

        if (!TryFindItemExtents(data, itemLocationBox.Value, itemDataBox, itemId, out IlocItem item)) {
            return false;
        }

        if (!TryCopyExtents(data, item.Extents, out byte[]? itemData)) {
            return false;
        }

        xmp = System.Text.Encoding.UTF8.GetString(itemData!);
        return true;
    }

    public static bool HasExifItem(string filePath) {
        byte[] data = File.ReadAllBytes(filePath);
        return TryFindItemInfoBox(data, out Box itemInfoBox) &&
               TryFindExifItemId(data, itemInfoBox, out _);
    }

    public static bool HasXmpItem(string filePath) {
        byte[] data = File.ReadAllBytes(filePath);
        return TryFindItemInfoBox(data, out Box itemInfoBox) &&
               TryFindXmpItemId(data, itemInfoBox, out _);
    }

    public static bool TryWriteExifProfile(string filePath, string outputPath, ExifProfile? profile) {
        byte[] data = File.ReadAllBytes(filePath);
        if (!TryFindMetaBox(data, out Box metaBox)) {
            return false;
        }

        int metaChildrenStart = metaBox.DataOffset + 4;
        if (metaChildrenStart > metaBox.EndOffset) {
            return false;
        }

        Box? itemInfoBox = null;
        Box? itemLocationBox = null;

        foreach (Box childBox in EnumerateBoxes(data, metaChildrenStart, metaBox.EndOffset)) {
            if (childBox.Type == "iinf") {
                itemInfoBox = childBox;
            } else if (childBox.Type == "iloc") {
                itemLocationBox = childBox;
            }
        }

        if (itemInfoBox is null || itemLocationBox is null) {
            return false;
        }

        if (!TryFindExifItemId(data, itemInfoBox.Value, out uint itemId)) {
            return false;
        }

        if (!TryFindItemExtents(data, itemLocationBox.Value, null, itemId, out IlocItem item) ||
            item.ConstructionMethod != 0 ||
            item.DataReferenceIndex != 0 ||
            item.Extents.Count != 1) {
            return false;
        }

        ItemExtent extent = item.Extents[0];
        byte[] exifItemData = profile is null
            ? Array.Empty<byte>()
            : CreateExifItemData(profile.ToByteArray());

        return TryWriteItemData(data, item, extent, outputPath, exifItemData);
    }

    public static bool TryWriteXmp(string filePath, string outputPath, string? xmp) {
        byte[] data = File.ReadAllBytes(filePath);
        if (!TryFindMetaBox(data, out Box metaBox)) {
            return false;
        }

        int metaChildrenStart = metaBox.DataOffset + 4;
        if (metaChildrenStart > metaBox.EndOffset) {
            return false;
        }

        Box? itemInfoBox = null;
        Box? itemLocationBox = null;

        foreach (Box childBox in EnumerateBoxes(data, metaChildrenStart, metaBox.EndOffset)) {
            if (childBox.Type == "iinf") {
                itemInfoBox = childBox;
            } else if (childBox.Type == "iloc") {
                itemLocationBox = childBox;
            }
        }

        if (itemInfoBox is null || itemLocationBox is null) {
            return false;
        }

        if (!TryFindXmpItemId(data, itemInfoBox.Value, out uint itemId)) {
            return false;
        }

        if (!TryFindItemExtents(data, itemLocationBox.Value, null, itemId, out IlocItem item) ||
            item.ConstructionMethod != 0 ||
            item.DataReferenceIndex != 0 ||
            item.Extents.Count != 1) {
            return false;
        }

        ItemExtent extent = item.Extents[0];
        byte[] xmpItemData = xmp is null
            ? Array.Empty<byte>()
            : System.Text.Encoding.UTF8.GetBytes(xmp);

        return TryWriteItemData(data, item, extent, outputPath, xmpItemData);
    }

    private static bool TryWriteItemData(byte[] data, IlocItem item, ItemExtent extent, string outputPath, byte[] itemData) {
        bool isClearingItemData = itemData.Length == 0;
        int newOffset = isClearingItemData
            ? extent.Offset
            : data.Length;

        if ((ulong)newOffset < item.BaseOffset) {
            return false;
        }

        ulong extentOffset = (ulong)newOffset - item.BaseOffset;
        if (!CanWriteVariableUInt(extentOffset, extent.OffsetSize) ||
            !CanWriteVariableUInt((ulong)itemData.Length, extent.LengthSize)) {
            return false;
        }

        var output = new byte[data.Length + itemData.Length];
        Buffer.BlockCopy(data, 0, output, 0, data.Length);
        if (extent.Length > 0) {
            if (extent.Offset < 0 || extent.Offset > data.Length - extent.Length) {
                return false;
            }

            Array.Clear(output, extent.Offset, extent.Length);
        }

        WriteVariableUInt(output, extent.OffsetPosition, extent.OffsetSize, extentOffset);
        WriteVariableUInt(output, extent.LengthPosition, extent.LengthSize, (ulong)itemData.Length);

        if (itemData.Length > 0) {
            Buffer.BlockCopy(itemData, 0, output, data.Length, itemData.Length);
        }

        Helpers.CreateParentDirectory(outputPath);
        File.WriteAllBytes(outputPath, output);
        return true;
    }

    private static bool TryFindMetaBox(byte[] data, out Box metaBox) {
        foreach (Box box in EnumerateBoxes(data, 0, data.Length)) {
            if (box.Type == "meta") {
                metaBox = box;
                return true;
            }
        }

        metaBox = default;
        return false;
    }

    private static bool TryFindItemInfoBox(byte[] data, out Box itemInfoBox) {
        if (!TryFindMetaBox(data, out Box metaBox)) {
            itemInfoBox = default;
            return false;
        }

        int metaChildrenStart = metaBox.DataOffset + 4;
        if (metaChildrenStart > metaBox.EndOffset) {
            itemInfoBox = default;
            return false;
        }

        foreach (Box childBox in EnumerateBoxes(data, metaChildrenStart, metaBox.EndOffset)) {
            if (childBox.Type == "iinf") {
                itemInfoBox = childBox;
                return true;
            }
        }

        itemInfoBox = default;
        return false;
    }

    private static IEnumerable<Box> EnumerateBoxes(byte[] data, int startOffset, int endOffset) {
        int offset = startOffset;
        while (offset + 8 <= endOffset) {
            if (!TryReadBox(data, offset, endOffset, out Box box)) {
                yield break;
            }

            yield return box;
            offset = box.EndOffset;
        }
    }

    private static bool TryReadBox(byte[] data, int offset, int endOffset, out Box box) {
        box = default;
        if (offset < 0 || offset + 8 > endOffset) {
            return false;
        }

        ulong size = ReadUInt32(data, offset);
        string type = ReadAscii(data, offset + 4, 4);
        int headerSize = 8;

        if (size == 1) {
            if (offset + 16 > endOffset) {
                return false;
            }

            size = ReadUInt64(data, offset + 8);
            headerSize = 16;
        } else if (size == 0) {
            size = (ulong)(endOffset - offset);
        }

        if (size < (ulong)headerSize || size > int.MaxValue || size > (ulong)(endOffset - offset)) {
            return false;
        }

        int boxEnd = offset + (int)size;
        box = new Box(offset, boxEnd, offset + headerSize, type);
        return true;
    }

    private static bool TryFindExifItemId(byte[] data, Box itemInfoBox, out uint itemId) {
        itemId = 0;

        int offset = itemInfoBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, itemInfoBox.EndOffset, out byte version, out _, out offset)) {
            return false;
        }

        uint entryCount;
        if (version == 0) {
            if (!TryReadUInt16(data, offset, itemInfoBox.EndOffset, out ushort count)) {
                return false;
            }

            entryCount = count;
            offset += 2;
        } else {
            if (!TryReadUInt32(data, offset, itemInfoBox.EndOffset, out entryCount)) {
                return false;
            }

            offset += 4;
        }

        for (uint index = 0; index < entryCount; index++) {
            if (!TryReadBox(data, offset, itemInfoBox.EndOffset, out Box entryBox)) {
                return false;
            }

            if (entryBox.Type == "infe" && TryReadItemInfoEntry(data, entryBox, out uint entryItemId, out string itemType, out _, out _, out _, out _, out _) && itemType == ExifItemType) {
                itemId = entryItemId;
                return true;
            }

            offset = entryBox.EndOffset;
        }

        return false;
    }

    private static bool TryFindXmpItemId(byte[] data, Box itemInfoBox, out uint itemId) {
        itemId = 0;

        int offset = itemInfoBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, itemInfoBox.EndOffset, out byte version, out _, out offset)) {
            return false;
        }

        uint entryCount;
        if (version == 0) {
            if (!TryReadUInt16(data, offset, itemInfoBox.EndOffset, out ushort count)) {
                return false;
            }

            entryCount = count;
            offset += 2;
        } else {
            if (!TryReadUInt32(data, offset, itemInfoBox.EndOffset, out entryCount)) {
                return false;
            }

            offset += 4;
        }

        for (uint index = 0; index < entryCount; index++) {
            if (!TryReadBox(data, offset, itemInfoBox.EndOffset, out Box entryBox)) {
                return false;
            }

            if (entryBox.Type == "infe" &&
                TryReadItemInfoEntry(data, entryBox, out uint entryItemId, out _, out _, out _, out _, out string? mimeType, out _) &&
                IsXmpMimeType(mimeType)) {
                itemId = entryItemId;
                return true;
            }

            offset = entryBox.EndOffset;
        }

        return false;
    }

    private static bool TryReadItemInfos(byte[] data, Box itemInfoBox, List<HeifItemInfoBuilder> items) {
        int offset = itemInfoBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, itemInfoBox.EndOffset, out byte version, out _, out offset)) {
            return false;
        }

        uint entryCount;
        if (version == 0) {
            if (!TryReadUInt16(data, offset, itemInfoBox.EndOffset, out ushort count)) {
                return false;
            }

            entryCount = count;
            offset += 2;
        } else {
            if (!TryReadUInt32(data, offset, itemInfoBox.EndOffset, out entryCount)) {
                return false;
            }

            offset += 4;
        }

        for (uint index = 0; index < entryCount; index++) {
            if (!TryReadBox(data, offset, itemInfoBox.EndOffset, out Box entryBox)) {
                return false;
            }

            if (entryBox.Type == "infe" && TryReadItemInfoEntry(data, entryBox, out uint itemId, out string itemType, out string itemName, out ushort itemProtectionIndex, out bool isHidden, out string? mimeType, out string? contentEncoding)) {
                items.Add(new HeifItemInfoBuilder(itemId, itemType, itemName, itemProtectionIndex, isHidden, mimeType, contentEncoding));
            }

            offset = entryBox.EndOffset;
        }

        return true;
    }

    private static bool TryReadItemInfoEntry(byte[] data, Box entryBox, out uint itemId, out string itemType, out string itemName, out ushort itemProtectionIndex, out bool isHidden, out string? mimeType, out string? contentEncoding) {
        itemId = 0;
        itemType = string.Empty;
        itemName = string.Empty;
        itemProtectionIndex = 0;
        isHidden = false;
        mimeType = null;
        contentEncoding = null;

        int offset = entryBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, entryBox.EndOffset, out byte version, out uint flags, out offset)) {
            return false;
        }

        isHidden = (flags & 1) == 1;

        if (version == 2) {
            if (!TryReadUInt16(data, offset, entryBox.EndOffset, out ushort shortItemId)) {
                return false;
            }

            itemId = shortItemId;
            offset += 2;
        } else if (version == 3) {
            if (!TryReadUInt32(data, offset, entryBox.EndOffset, out itemId)) {
                return false;
            }

            offset += 4;
        } else {
            return false;
        }

        if (!TryReadUInt16(data, offset, entryBox.EndOffset, out itemProtectionIndex)) {
            return false;
        }

        offset += 2;
        if (offset + 4 > entryBox.EndOffset) {
            return false;
        }

        itemType = ReadAscii(data, offset, 4);
        offset += 4;
        itemName = ReadNullTerminatedString(data, offset, entryBox.EndOffset, out offset);
        if (itemType == "mime" && offset < entryBox.EndOffset) {
            mimeType = ReadNullTerminatedString(data, offset, entryBox.EndOffset, out offset);
            if (offset < entryBox.EndOffset) {
                contentEncoding = ReadNullTerminatedString(data, offset, entryBox.EndOffset, out _);
            }
        }

        return true;
    }

    private static bool TryReadFileType(byte[] data, out string majorBrand, out uint minorVersion, out List<string>? compatibleBrands) {
        majorBrand = string.Empty;
        minorVersion = 0;
        compatibleBrands = null;

        foreach (Box box in EnumerateBoxes(data, 0, data.Length)) {
            if (box.Type != "ftyp") {
                continue;
            }

            if (box.DataOffset + 8 > box.EndOffset) {
                return false;
            }

            majorBrand = ReadAscii(data, box.DataOffset, 4);
            minorVersion = ReadUInt32(data, box.DataOffset + 4);
            compatibleBrands = new List<string>();
            for (int offset = box.DataOffset + 8; offset + 4 <= box.EndOffset; offset += 4) {
                compatibleBrands.Add(ReadAscii(data, offset, 4));
            }

            return true;
        }

        return false;
    }

    private static bool IsXmpMimeType(string? mimeType) =>
        string.Equals(mimeType, XmpMimeType, StringComparison.OrdinalIgnoreCase);

    private static bool TryReadPrimaryItemId(byte[] data, Box primaryItemBox, out uint primaryItemId) {
        primaryItemId = 0;

        int offset = primaryItemBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, primaryItemBox.EndOffset, out byte version, out _, out offset)) {
            return false;
        }

        if (version == 0) {
            if (!TryReadUInt16(data, offset, primaryItemBox.EndOffset, out ushort shortItemId)) {
                return false;
            }

            primaryItemId = shortItemId;
            return true;
        }

        return TryReadUInt32(data, offset, primaryItemBox.EndOffset, out primaryItemId);
    }

    private static void TryApplyImageProperties(byte[] data, Box itemPropertiesBox, List<HeifItemInfoBuilder> itemBuilders) {
        Box? itemPropertyContainerBox = null;
        Box? itemPropertyAssociationBox = null;

        foreach (Box childBox in EnumerateBoxes(data, itemPropertiesBox.DataOffset, itemPropertiesBox.EndOffset)) {
            if (childBox.Type == "ipco") {
                itemPropertyContainerBox = childBox;
            } else if (childBox.Type == "ipma") {
                itemPropertyAssociationBox = childBox;
            }
        }

        if (itemPropertyContainerBox is null || itemPropertyAssociationBox is null) {
            return;
        }

        Dictionary<int, HeifImageProperty> imageProperties = ReadImageProperties(data, itemPropertyContainerBox.Value);
        Dictionary<uint, List<HeifItemPropertyAssociation>> associations = ReadItemPropertyAssociations(data, itemPropertyAssociationBox.Value);
        if (associations.Count == 0) {
            return;
        }

        Dictionary<uint, HeifItemInfoBuilder> buildersById = itemBuilders.ToDictionary(item => item.ItemId);
        foreach (KeyValuePair<uint, List<HeifItemPropertyAssociation>> association in associations) {
            if (!buildersById.TryGetValue(association.Key, out HeifItemInfoBuilder? itemBuilder)) {
                continue;
            }

            var publicAssociations = new List<HeifItemPropertyAssociationInfo>(association.Value.Count);
            foreach (HeifItemPropertyAssociation propertyAssociation in association.Value) {
                if (!imageProperties.TryGetValue(propertyAssociation.PropertyIndex, out HeifImageProperty imageProperty)) {
                    continue;
                }

                publicAssociations.Add(new HeifItemPropertyAssociationInfo(propertyAssociation.PropertyIndex, imageProperty.PropertyType, propertyAssociation.IsEssential));
                if (imageProperty.Width.HasValue) {
                    itemBuilder.Width = imageProperty.Width;
                }

                if (imageProperty.Height.HasValue) {
                    itemBuilder.Height = imageProperty.Height;
                }

                if (imageProperty.RotationDegrees.HasValue) {
                    itemBuilder.RotationDegrees = imageProperty.RotationDegrees;
                }

                if (imageProperty.IsMirrored) {
                    itemBuilder.IsMirrored = true;
                }

                if (imageProperty.PixelAspectRatioHorizontalSpacing.HasValue) {
                    itemBuilder.PixelAspectRatioHorizontalSpacing = imageProperty.PixelAspectRatioHorizontalSpacing;
                }

                if (imageProperty.PixelAspectRatioVerticalSpacing.HasValue) {
                    itemBuilder.PixelAspectRatioVerticalSpacing = imageProperty.PixelAspectRatioVerticalSpacing;
                }

                if (imageProperty.PixelBitDepths is not null) {
                    itemBuilder.PixelBitDepths = imageProperty.PixelBitDepths;
                }

                if (imageProperty.ColorType is not null) {
                    itemBuilder.ColorType = imageProperty.ColorType;
                    itemBuilder.ColorPrimaries = imageProperty.ColorPrimaries;
                    itemBuilder.TransferCharacteristics = imageProperty.TransferCharacteristics;
                    itemBuilder.MatrixCoefficients = imageProperty.MatrixCoefficients;
                    itemBuilder.FullRangeFlag = imageProperty.FullRangeFlag;
                }

                if (imageProperty.AuxiliaryType is not null) {
                    itemBuilder.AuxiliaryType = imageProperty.AuxiliaryType;
                    itemBuilder.AuxiliarySubtypes = imageProperty.AuxiliarySubtypes ?? Array.Empty<byte>();
                }

                if (imageProperty.CodecConfigurationType is not null) {
                    itemBuilder.CodecConfigurationType = imageProperty.CodecConfigurationType;
                    itemBuilder.CodecConfigurationBytes = imageProperty.CodecConfigurationBytes ?? Array.Empty<byte>();
                }
            }

            itemBuilder.PropertyAssociations = publicAssociations;
        }
    }

    private static Dictionary<int, HeifImageProperty> ReadImageProperties(byte[] data, Box itemPropertyContainerBox) {
        var imageProperties = new Dictionary<int, HeifImageProperty>();
        int propertyIndex = 1;
        foreach (Box propertyBox in EnumerateBoxes(data, itemPropertyContainerBox.DataOffset, itemPropertyContainerBox.EndOffset)) {
            imageProperties[propertyIndex] = HeifImageProperty.CreateUnknown(propertyBox.Type);
            if (propertyBox.Type == "ispe") {
                int offset = propertyBox.DataOffset;
                if (TryReadFullBoxHeader(data, offset, propertyBox.EndOffset, out _, out _, out offset) &&
                    TryReadUInt32(data, offset, propertyBox.EndOffset, out uint width) &&
                    TryReadUInt32(data, offset + 4, propertyBox.EndOffset, out uint height)) {
                    imageProperties[propertyIndex] = HeifImageProperty.CreateSpatialExtent(width, height);
                }
            } else if (propertyBox.Type == "irot") {
                if (propertyBox.DataOffset < propertyBox.EndOffset) {
                    imageProperties[propertyIndex] = HeifImageProperty.CreateRotation((data[propertyBox.DataOffset] & 0x03) * 90);
                }
            } else if (propertyBox.Type == "imir") {
                if (propertyBox.DataOffset < propertyBox.EndOffset) {
                    imageProperties[propertyIndex] = HeifImageProperty.CreateMirror();
                }
            } else if (propertyBox.Type == "pasp") {
                if (TryReadUInt32(data, propertyBox.DataOffset, propertyBox.EndOffset, out uint horizontalSpacing) &&
                    TryReadUInt32(data, propertyBox.DataOffset + 4, propertyBox.EndOffset, out uint verticalSpacing)) {
                    imageProperties[propertyIndex] = HeifImageProperty.CreatePixelAspectRatio(horizontalSpacing, verticalSpacing);
                }
            } else if (propertyBox.Type == "pixi") {
                if (TryReadPixelInformation(data, propertyBox, out List<byte>? pixelBitDepths)) {
                    imageProperties[propertyIndex] = HeifImageProperty.CreatePixelInformation(pixelBitDepths!);
                }
            } else if (propertyBox.Type == "colr") {
                if (TryReadColorInformation(data, propertyBox, out HeifImageProperty colorProperty)) {
                    imageProperties[propertyIndex] = colorProperty;
                }
            } else if (propertyBox.Type == "auxC") {
                if (TryReadAuxiliaryType(data, propertyBox, out string? auxiliaryType, out byte[]? auxiliarySubtypes)) {
                    imageProperties[propertyIndex] = HeifImageProperty.CreateAuxiliaryType(auxiliaryType!, auxiliarySubtypes!);
                }
            } else if (propertyBox.Type == "hvcC" || propertyBox.Type == "av1C" || propertyBox.Type == "avcC") {
                imageProperties[propertyIndex] = HeifImageProperty.CreateCodecConfiguration(
                    propertyBox.Type,
                    CopyRange(data, propertyBox.DataOffset, propertyBox.EndOffset - propertyBox.DataOffset));
            }

            propertyIndex++;
        }

        return imageProperties;
    }

    private static bool TryReadPixelInformation(byte[] data, Box propertyBox, out List<byte>? pixelBitDepths) {
        pixelBitDepths = null;
        int offset = propertyBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, propertyBox.EndOffset, out _, out _, out offset) ||
            offset + 1 > propertyBox.EndOffset) {
            return false;
        }

        int channelCount = data[offset++];
        if (channelCount < 0 || offset + channelCount > propertyBox.EndOffset) {
            return false;
        }

        pixelBitDepths = new List<byte>(channelCount);
        for (int index = 0; index < channelCount; index++) {
            pixelBitDepths.Add(data[offset++]);
        }

        return true;
    }

    private static bool TryReadColorInformation(byte[] data, Box propertyBox, out HeifImageProperty colorProperty) {
        colorProperty = default;
        if (propertyBox.DataOffset + 4 > propertyBox.EndOffset) {
            return false;
        }

        string colorType = ReadAscii(data, propertyBox.DataOffset, 4);
        if (colorType == "nclx" || colorType == "nclc") {
            int offset = propertyBox.DataOffset + 4;
            if (!TryReadUInt16(data, offset, propertyBox.EndOffset, out ushort colorPrimaries) ||
                !TryReadUInt16(data, offset + 2, propertyBox.EndOffset, out ushort transferCharacteristics) ||
                !TryReadUInt16(data, offset + 4, propertyBox.EndOffset, out ushort matrixCoefficients)) {
                return false;
            }

            bool? fullRangeFlag = null;
            if (colorType == "nclx" && offset + 7 <= propertyBox.EndOffset) {
                fullRangeFlag = (data[offset + 6] & 0x80) != 0;
            }

            colorProperty = HeifImageProperty.CreateColorInformation(colorType, colorPrimaries, transferCharacteristics, matrixCoefficients, fullRangeFlag);
            return true;
        }

        colorProperty = HeifImageProperty.CreateColorInformation(colorType, null, null, null, null);
        return true;
    }

    private static bool TryReadAuxiliaryType(byte[] data, Box propertyBox, out string? auxiliaryType, out byte[]? auxiliarySubtypes) {
        auxiliaryType = null;
        auxiliarySubtypes = null;

        int offset = propertyBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, propertyBox.EndOffset, out _, out _, out offset) ||
            offset >= propertyBox.EndOffset) {
            return false;
        }

        auxiliaryType = ReadNullTerminatedString(data, offset, propertyBox.EndOffset, out offset);
        auxiliarySubtypes = offset < propertyBox.EndOffset
            ? CopyRange(data, offset, propertyBox.EndOffset - offset)
            : Array.Empty<byte>();
        return !string.IsNullOrEmpty(auxiliaryType);
    }

    private static Dictionary<uint, List<HeifItemPropertyAssociation>> ReadItemPropertyAssociations(byte[] data, Box itemPropertyAssociationBox) {
        var associations = new Dictionary<uint, List<HeifItemPropertyAssociation>>();
        int offset = itemPropertyAssociationBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, itemPropertyAssociationBox.EndOffset, out byte version, out uint flags, out offset)) {
            return associations;
        }

        if (!TryReadUInt32(data, offset, itemPropertyAssociationBox.EndOffset, out uint entryCount)) {
            return associations;
        }

        offset += 4;
        bool associationUsesLargePropertyIndex = (flags & 1) == 1;
        for (uint index = 0; index < entryCount; index++) {
            uint itemId;
            if (version < 1) {
                if (!TryReadUInt16(data, offset, itemPropertyAssociationBox.EndOffset, out ushort shortItemId)) {
                    return associations;
                }

                itemId = shortItemId;
                offset += 2;
            } else {
                if (!TryReadUInt32(data, offset, itemPropertyAssociationBox.EndOffset, out itemId)) {
                    return associations;
                }

                offset += 4;
            }

            if (offset + 1 > itemPropertyAssociationBox.EndOffset) {
                return associations;
            }

            int associationCount = data[offset++];
            var propertyAssociations = new List<HeifItemPropertyAssociation>();
            for (int associationIndex = 0; associationIndex < associationCount; associationIndex++) {
                if (associationUsesLargePropertyIndex) {
                    if (!TryReadUInt16(data, offset, itemPropertyAssociationBox.EndOffset, out ushort association)) {
                        return associations;
                    }

                    int propertyIndex = association & 0x7FFF;
                    if (propertyIndex > 0) {
                        propertyAssociations.Add(new HeifItemPropertyAssociation(propertyIndex, (association & 0x8000) != 0));
                    }

                    offset += 2;
                } else {
                    if (offset + 1 > itemPropertyAssociationBox.EndOffset) {
                        return associations;
                    }

                    byte association = data[offset++];
                    int propertyIndex = association & 0x7F;
                    if (propertyIndex > 0) {
                        propertyAssociations.Add(new HeifItemPropertyAssociation(propertyIndex, (association & 0x80) != 0));
                    }
                }
            }

            associations[itemId] = propertyAssociations;
        }

        return associations;
    }

    private static List<HeifItemReference> ReadItemReferences(byte[] data, Box itemReferenceBox) {
        var references = new List<HeifItemReference>();
        int offset = itemReferenceBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, itemReferenceBox.EndOffset, out byte version, out _, out offset)) {
            return references;
        }

        foreach (Box referenceTypeBox in EnumerateBoxes(data, offset, itemReferenceBox.EndOffset)) {
            int referenceOffset = referenceTypeBox.DataOffset;
            uint fromItemId;
            if (version == 0) {
                if (!TryReadUInt16(data, referenceOffset, referenceTypeBox.EndOffset, out ushort shortFromItemId)) {
                    continue;
                }

                fromItemId = shortFromItemId;
                referenceOffset += 2;
            } else {
                if (!TryReadUInt32(data, referenceOffset, referenceTypeBox.EndOffset, out fromItemId)) {
                    continue;
                }

                referenceOffset += 4;
            }

            if (!TryReadUInt16(data, referenceOffset, referenceTypeBox.EndOffset, out ushort referenceCount)) {
                continue;
            }

            referenceOffset += 2;
            var toItemIds = new List<uint>();
            for (ushort index = 0; index < referenceCount; index++) {
                if (version == 0) {
                    if (!TryReadUInt16(data, referenceOffset, referenceTypeBox.EndOffset, out ushort shortToItemId)) {
                        break;
                    }

                    toItemIds.Add(shortToItemId);
                    referenceOffset += 2;
                } else {
                    if (!TryReadUInt32(data, referenceOffset, referenceTypeBox.EndOffset, out uint toItemId)) {
                        break;
                    }

                    toItemIds.Add(toItemId);
                    referenceOffset += 4;
                }
            }

            references.Add(new HeifItemReference(referenceTypeBox.Type, fromItemId, toItemIds));
        }

        return references;
    }

    private static Dictionary<uint, HeifItemLocationInfo> ReadItemLocations(byte[] data, Box itemLocationBox, Box? itemDataBox) {
        var locations = new Dictionary<uint, HeifItemLocationInfo>();
        int offset = itemLocationBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, itemLocationBox.EndOffset, out byte version, out _, out offset) ||
            offset + 2 > itemLocationBox.EndOffset) {
            return locations;
        }

        int offsetSize = data[offset] >> 4;
        int lengthSize = data[offset] & 0x0F;
        offset++;
        int baseOffsetSize = data[offset] >> 4;
        int indexSize = version == 1 || version == 2
            ? data[offset] & 0x0F
            : 0;
        offset++;

        if (!IsSupportedFieldSize(offsetSize) || !IsSupportedFieldSize(lengthSize) || !IsSupportedFieldSize(baseOffsetSize) || !IsSupportedFieldSize(indexSize)) {
            return locations;
        }

        uint itemCount;
        if (version < 2) {
            if (!TryReadUInt16(data, offset, itemLocationBox.EndOffset, out ushort shortItemCount)) {
                return locations;
            }

            itemCount = shortItemCount;
            offset += 2;
        } else {
            if (!TryReadUInt32(data, offset, itemLocationBox.EndOffset, out itemCount)) {
                return locations;
            }

            offset += 4;
        }

        for (uint index = 0; index < itemCount; index++) {
            if (!TryReadIlocItem(data, itemLocationBox.EndOffset, version, offsetSize, lengthSize, baseOffsetSize, indexSize, ref offset, out IlocItem item)) {
                return locations;
            }

            if (!TryCreateLocationExtents(item, itemDataBox, out List<HeifItemExtentInfo> extents)) {
                continue;
            }

            locations[item.ItemId] = new HeifItemLocationInfo(item.ConstructionMethod, item.DataReferenceIndex, item.BaseOffset, extents);
        }

        return locations;
    }

    private static bool TryCreateLocationExtents(IlocItem item, Box? itemDataBox, out List<HeifItemExtentInfo> extents) {
        extents = new List<HeifItemExtentInfo>(item.Extents.Count);
        foreach (ItemExtent extent in item.Extents) {
            int resolvedOffset = extent.Offset;
            if (item.ConstructionMethod == 1 && itemDataBox is not null) {
                if (extent.Offset > int.MaxValue - itemDataBox.Value.DataOffset) {
                    return false;
                }

                resolvedOffset = itemDataBox.Value.DataOffset + extent.Offset;
            }

            extents.Add(new HeifItemExtentInfo(resolvedOffset, extent.Length));
        }

        return true;
    }

    private static bool TryFindItemExtents(byte[] data, Box itemLocationBox, Box? itemDataBox, uint itemId, out IlocItem item) {
        item = default;

        int offset = itemLocationBox.DataOffset;
        if (!TryReadFullBoxHeader(data, offset, itemLocationBox.EndOffset, out byte version, out _, out offset)) {
            return false;
        }

        if (offset + 2 > itemLocationBox.EndOffset) {
            return false;
        }

        int offsetSize = data[offset] >> 4;
        int lengthSize = data[offset] & 0x0F;
        offset++;
        int baseOffsetSize = data[offset] >> 4;
        int indexSize = version == 1 || version == 2
            ? data[offset] & 0x0F
            : 0;
        offset++;

        if (!IsSupportedFieldSize(offsetSize) || !IsSupportedFieldSize(lengthSize) || !IsSupportedFieldSize(baseOffsetSize) || !IsSupportedFieldSize(indexSize)) {
            return false;
        }

        uint itemCount;
        if (version < 2) {
            if (!TryReadUInt16(data, offset, itemLocationBox.EndOffset, out ushort shortItemCount)) {
                return false;
            }

            itemCount = shortItemCount;
            offset += 2;
        } else {
            if (!TryReadUInt32(data, offset, itemLocationBox.EndOffset, out itemCount)) {
                return false;
            }

            offset += 4;
        }

        for (uint index = 0; index < itemCount; index++) {
            if (!TryReadIlocItem(data, itemLocationBox.EndOffset, version, offsetSize, lengthSize, baseOffsetSize, indexSize, ref offset, out IlocItem candidateItem)) {
                return false;
            }

            if (candidateItem.ItemId == itemId) {
                if (candidateItem.DataReferenceIndex != 0) {
                    return false;
                }

                if (candidateItem.ConstructionMethod == 0) {
                    item = candidateItem;
                    return true;
                }

                if (candidateItem.ConstructionMethod == 1 && itemDataBox is not null) {
                    var extents = new List<ItemExtent>(candidateItem.Extents.Count);
                    foreach (ItemExtent extent in candidateItem.Extents) {
                        if (extent.Offset > int.MaxValue - itemDataBox.Value.DataOffset) {
                            return false;
                        }

                        extents.Add(new ItemExtent(
                            itemDataBox.Value.DataOffset + extent.Offset,
                            extent.Length,
                            extent.OffsetPosition,
                            extent.OffsetSize,
                            extent.LengthPosition,
                            extent.LengthSize));
                    }

                    item = new IlocItem(candidateItem.ItemId, candidateItem.ConstructionMethod, candidateItem.DataReferenceIndex, 0, extents);
                    return true;
                }

                return false;
            }
        }

        return false;
    }

    private static bool TryReadIlocItem(byte[] data, int endOffset, byte version, int offsetSize, int lengthSize, int baseOffsetSize, int indexSize, ref int offset, out IlocItem item) {
        item = default;

        uint itemId;
        if (version < 2) {
            if (!TryReadUInt16(data, offset, endOffset, out ushort shortItemId)) {
                return false;
            }

            itemId = shortItemId;
            offset += 2;
        } else {
            if (!TryReadUInt32(data, offset, endOffset, out itemId)) {
                return false;
            }

            offset += 4;
        }

        ushort constructionMethod = 0;
        if (version == 1 || version == 2) {
            if (!TryReadUInt16(data, offset, endOffset, out ushort constructionMethodRaw)) {
                return false;
            }

            constructionMethod = (ushort)(constructionMethodRaw & 0x000F);
            offset += 2;
        }

        if (!TryReadUInt16(data, offset, endOffset, out ushort dataReferenceIndex)) {
            return false;
        }

        offset += 2;

        if (!TryReadVariableUInt(data, offset, endOffset, baseOffsetSize, out ulong baseOffset, out offset)) {
            return false;
        }

        if (!TryReadUInt16(data, offset, endOffset, out ushort extentCount)) {
            return false;
        }

        offset += 2;

        var extents = new List<ItemExtent>();
        for (ushort index = 0; index < extentCount; index++) {
            if (indexSize > 0 && !TryReadVariableUInt(data, offset, endOffset, indexSize, out _, out offset)) {
                return false;
            }

            int extentOffsetPosition = offset;
            if (!TryReadVariableUInt(data, offset, endOffset, offsetSize, out ulong extentOffset, out offset)) {
                return false;
            }

            int extentLengthPosition = offset;
            if (!TryReadVariableUInt(data, offset, endOffset, lengthSize, out ulong extentLength, out offset)) {
                return false;
            }

            if (baseOffset + extentOffset > int.MaxValue || extentLength > int.MaxValue) {
                return false;
            }

            extents.Add(new ItemExtent((int)(baseOffset + extentOffset), (int)extentLength, extentOffsetPosition, offsetSize, extentLengthPosition, lengthSize));
        }

        item = new IlocItem(itemId, constructionMethod, dataReferenceIndex, baseOffset, extents);
        return true;
    }

    private static bool TryCopyExtents(byte[] data, List<ItemExtent> extents, out byte[]? itemData) {
        itemData = null;
        if (extents.Count == 0) {
            return false;
        }

        int totalLength = 0;
        foreach (ItemExtent extent in extents) {
            if (extent.Offset < 0 ||
                extent.Length < 0 ||
                extent.Offset > data.Length ||
                extent.Length > data.Length - extent.Offset ||
                extent.Length > int.MaxValue - totalLength) {
                return false;
            }

            totalLength += extent.Length;
        }

        itemData = new byte[totalLength];
        int destinationOffset = 0;
        foreach (ItemExtent extent in extents) {
            Buffer.BlockCopy(data, extent.Offset, itemData, destinationOffset, extent.Length);
            destinationOffset += extent.Length;
        }

        return true;
    }

    private static bool TryGetTiffPayload(byte[] exifItemData, out byte[]? tiffPayload) {
        tiffPayload = null;

        if (LooksLikeTiffHeader(exifItemData, 0)) {
            tiffPayload = exifItemData;
            return true;
        }

        if (exifItemData.Length >= 10) {
            uint tiffHeaderOffset = ReadUInt32(exifItemData, 0);
            ulong tiffStart = 4UL + tiffHeaderOffset;
            if (tiffStart <= (ulong)exifItemData.Length && tiffStart <= int.MaxValue && LooksLikeTiffHeader(exifItemData, (int)tiffStart)) {
                tiffPayload = CopyTail(exifItemData, (int)tiffStart);
                return true;
            }
        }

        if (exifItemData.Length >= 12 && ReadAscii(exifItemData, 0, 6) == "Exif\0\0" && LooksLikeTiffHeader(exifItemData, 6)) {
            tiffPayload = CopyTail(exifItemData, 6);
            return true;
        }

        return false;
    }

    private static bool TryReadFullBoxHeader(byte[] data, int offset, int endOffset, out byte version, out uint flags, out int nextOffset) {
        version = 0;
        flags = 0;
        nextOffset = offset;

        if (offset + 4 > endOffset) {
            return false;
        }

        version = data[offset];
        flags = (uint)((data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3]);
        nextOffset = offset + 4;
        return true;
    }

    private static bool IsSupportedFieldSize(int size) =>
        size == 0 || size == 4 || size == 8;

    private static bool TryReadVariableUInt(byte[] data, int offset, int endOffset, int size, out ulong value, out int nextOffset) {
        value = 0;
        nextOffset = offset;

        if (size == 0) {
            return true;
        }

        if (offset + size > endOffset) {
            return false;
        }

        value = size == 4
            ? ReadUInt32(data, offset)
            : ReadUInt64(data, offset);
        nextOffset = offset + size;
        return true;
    }

    private static bool TryReadUInt16(byte[] data, int offset, int endOffset, out ushort value) {
        value = 0;
        if (offset + 2 > endOffset) {
            return false;
        }

        value = (ushort)((data[offset] << 8) | data[offset + 1]);
        return true;
    }

    private static bool TryReadUInt32(byte[] data, int offset, int endOffset, out uint value) {
        value = 0;
        if (offset + 4 > endOffset) {
            return false;
        }

        value = ReadUInt32(data, offset);
        return true;
    }

    private static uint ReadUInt32(byte[] data, int offset) =>
        ((uint)data[offset] << 24) |
        ((uint)data[offset + 1] << 16) |
        ((uint)data[offset + 2] << 8) |
        data[offset + 3];

    private static ulong ReadUInt64(byte[] data, int offset) =>
        ((ulong)ReadUInt32(data, offset) << 32) | ReadUInt32(data, offset + 4);

    private static string ReadAscii(byte[] data, int offset, int length) =>
        System.Text.Encoding.ASCII.GetString(data, offset, length);

    private static string ReadNullTerminatedString(byte[] data, int offset, int endOffset, out int nextOffset) {
        int terminatorOffset = offset;
        while (terminatorOffset < endOffset && data[terminatorOffset] != 0) {
            terminatorOffset++;
        }

        nextOffset = terminatorOffset < endOffset
            ? terminatorOffset + 1
            : terminatorOffset;

        return terminatorOffset == offset
            ? string.Empty
            : System.Text.Encoding.UTF8.GetString(data, offset, terminatorOffset - offset);
    }

    private static bool LooksLikeTiffHeader(byte[] data, int offset) {
        if (offset + 4 > data.Length) {
            return false;
        }

        bool littleEndian = data[offset] == (byte)'I' && data[offset + 1] == (byte)'I' && data[offset + 2] == 42 && data[offset + 3] == 0;
        bool bigEndian = data[offset] == (byte)'M' && data[offset + 1] == (byte)'M' && data[offset + 2] == 0 && data[offset + 3] == 42;
        return littleEndian || bigEndian;
    }

    private static byte[] CopyTail(byte[] data, int offset) {
        var tail = new byte[data.Length - offset];
        Buffer.BlockCopy(data, offset, tail, 0, tail.Length);
        return tail;
    }

    private static byte[] CopyRange(byte[] data, int offset, int length) {
        var range = new byte[length];
        Buffer.BlockCopy(data, offset, range, 0, range.Length);
        return range;
    }

    private static byte[] CreateExifItemData(byte[] tiffPayload) {
        var exifItemData = new byte[10 + tiffPayload.Length];
        WriteVariableUInt(exifItemData, 0, 4, 6);
        exifItemData[4] = (byte)'E';
        exifItemData[5] = (byte)'x';
        exifItemData[6] = (byte)'i';
        exifItemData[7] = (byte)'f';
        exifItemData[8] = 0;
        exifItemData[9] = 0;
        Buffer.BlockCopy(tiffPayload, 0, exifItemData, 10, tiffPayload.Length);
        return exifItemData;
    }

    private static bool CanWriteVariableUInt(ulong value, int size) =>
        size == 0
            ? value == 0
            : size == 8 || (size == 4 && value <= uint.MaxValue);

    private static void WriteVariableUInt(byte[] data, int offset, int size, ulong value) {
        if (size == 0) {
            return;
        }

        if (size == 8) {
            data[offset] = (byte)(value >> 56);
            data[offset + 1] = (byte)(value >> 48);
            data[offset + 2] = (byte)(value >> 40);
            data[offset + 3] = (byte)(value >> 32);
            data[offset + 4] = (byte)(value >> 24);
            data[offset + 5] = (byte)(value >> 16);
            data[offset + 6] = (byte)(value >> 8);
            data[offset + 7] = (byte)value;
            return;
        }

        data[offset] = (byte)(value >> 24);
        data[offset + 1] = (byte)(value >> 16);
        data[offset + 2] = (byte)(value >> 8);
        data[offset + 3] = (byte)value;
    }

    private readonly struct Box {
        public Box(int offset, int endOffset, int dataOffset, string type) {
            Offset = offset;
            EndOffset = endOffset;
            DataOffset = dataOffset;
            Type = type;
        }

        public int Offset { get; }

        public int EndOffset { get; }

        public int DataOffset { get; }

        public string Type { get; }
    }

    private readonly struct IlocItem {
        public IlocItem(uint itemId, ushort constructionMethod, ushort dataReferenceIndex, ulong baseOffset, List<ItemExtent> extents) {
            ItemId = itemId;
            ConstructionMethod = constructionMethod;
            DataReferenceIndex = dataReferenceIndex;
            BaseOffset = baseOffset;
            Extents = extents;
        }

        public uint ItemId { get; }

        public ushort ConstructionMethod { get; }

        public ushort DataReferenceIndex { get; }

        public ulong BaseOffset { get; }

        public List<ItemExtent> Extents { get; }
    }

    private readonly struct ItemExtent {
        public ItemExtent(int offset, int length, int offsetPosition, int offsetSize, int lengthPosition, int lengthSize) {
            Offset = offset;
            Length = length;
            OffsetPosition = offsetPosition;
            OffsetSize = offsetSize;
            LengthPosition = lengthPosition;
            LengthSize = lengthSize;
        }

        public int Offset { get; }

        public int Length { get; }

        public int OffsetPosition { get; }

        public int OffsetSize { get; }

        public int LengthPosition { get; }

        public int LengthSize { get; }
    }

    private sealed class HeifItemInfoBuilder {
        public HeifItemInfoBuilder(uint itemId, string itemType, string itemName, ushort itemProtectionIndex, bool isHidden, string? mimeType, string? contentEncoding) {
            ItemId = itemId;
            ItemType = itemType;
            ItemName = itemName;
            ItemProtectionIndex = itemProtectionIndex;
            IsHidden = isHidden;
            MimeType = mimeType;
            ContentEncoding = contentEncoding;
        }

        public uint ItemId { get; }

        public string ItemType { get; }

        public string ItemName { get; }

        public ushort ItemProtectionIndex { get; }

        public bool IsHidden { get; }

        public string? MimeType { get; }

        public string? ContentEncoding { get; }

        public IReadOnlyList<HeifItemPropertyAssociationInfo> PropertyAssociations { get; set; } = Array.Empty<HeifItemPropertyAssociationInfo>();

        public uint? Width { get; set; }

        public uint? Height { get; set; }

        public int? RotationDegrees { get; set; }

        public bool IsMirrored { get; set; }

        public uint? PixelAspectRatioHorizontalSpacing { get; set; }

        public uint? PixelAspectRatioVerticalSpacing { get; set; }

        public IReadOnlyList<byte> PixelBitDepths { get; set; } = Array.Empty<byte>();

        public string? ColorType { get; set; }

        public ushort? ColorPrimaries { get; set; }

        public ushort? TransferCharacteristics { get; set; }

        public ushort? MatrixCoefficients { get; set; }

        public bool? FullRangeFlag { get; set; }

        public string? CodecConfigurationType { get; set; }

        public IReadOnlyList<byte> CodecConfigurationBytes { get; set; } = Array.Empty<byte>();

        public string? AuxiliaryType { get; set; }

        public IReadOnlyList<byte> AuxiliarySubtypes { get; set; } = Array.Empty<byte>();
    }

    private readonly struct HeifItemPropertyAssociation {
        public HeifItemPropertyAssociation(int propertyIndex, bool isEssential) {
            PropertyIndex = propertyIndex;
            IsEssential = isEssential;
        }

        public int PropertyIndex { get; }

        public bool IsEssential { get; }
    }

    private readonly struct HeifImageProperty {
        private HeifImageProperty(string propertyType, uint? width, uint? height, int? rotationDegrees, bool isMirrored, uint? pixelAspectRatioHorizontalSpacing, uint? pixelAspectRatioVerticalSpacing, IReadOnlyList<byte>? pixelBitDepths, string? colorType, ushort? colorPrimaries, ushort? transferCharacteristics, ushort? matrixCoefficients, bool? fullRangeFlag, string? codecConfigurationType, IReadOnlyList<byte>? codecConfigurationBytes, string? auxiliaryType, IReadOnlyList<byte>? auxiliarySubtypes) {
            PropertyType = propertyType;
            Width = width;
            Height = height;
            RotationDegrees = rotationDegrees;
            IsMirrored = isMirrored;
            PixelAspectRatioHorizontalSpacing = pixelAspectRatioHorizontalSpacing;
            PixelAspectRatioVerticalSpacing = pixelAspectRatioVerticalSpacing;
            PixelBitDepths = pixelBitDepths;
            ColorType = colorType;
            ColorPrimaries = colorPrimaries;
            TransferCharacteristics = transferCharacteristics;
            MatrixCoefficients = matrixCoefficients;
            FullRangeFlag = fullRangeFlag;
            CodecConfigurationType = codecConfigurationType;
            CodecConfigurationBytes = codecConfigurationBytes;
            AuxiliaryType = auxiliaryType;
            AuxiliarySubtypes = auxiliarySubtypes;
        }

        public static HeifImageProperty CreateUnknown(string propertyType) =>
            new(propertyType, null, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null);

        public static HeifImageProperty CreateSpatialExtent(uint width, uint height) =>
            new("ispe", width, height, null, false, null, null, null, null, null, null, null, null, null, null, null, null);

        public static HeifImageProperty CreateRotation(int rotationDegrees) =>
            new("irot", null, null, rotationDegrees, false, null, null, null, null, null, null, null, null, null, null, null, null);

        public static HeifImageProperty CreateMirror() =>
            new("imir", null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null);

        public static HeifImageProperty CreatePixelAspectRatio(uint horizontalSpacing, uint verticalSpacing) =>
            new("pasp", null, null, null, false, horizontalSpacing, verticalSpacing, null, null, null, null, null, null, null, null, null, null);

        public static HeifImageProperty CreatePixelInformation(IReadOnlyList<byte> pixelBitDepths) =>
            new("pixi", null, null, null, false, null, null, pixelBitDepths, null, null, null, null, null, null, null, null, null);

        public static HeifImageProperty CreateColorInformation(string colorType, ushort? colorPrimaries, ushort? transferCharacteristics, ushort? matrixCoefficients, bool? fullRangeFlag) =>
            new("colr", null, null, null, false, null, null, null, colorType, colorPrimaries, transferCharacteristics, matrixCoefficients, fullRangeFlag, null, null, null, null);

        public static HeifImageProperty CreateAuxiliaryType(string auxiliaryType, IReadOnlyList<byte> auxiliarySubtypes) =>
            new("auxC", null, null, null, false, null, null, null, null, null, null, null, null, null, null, auxiliaryType, auxiliarySubtypes);

        public static HeifImageProperty CreateCodecConfiguration(string codecConfigurationType, IReadOnlyList<byte> codecConfigurationBytes) =>
            new(codecConfigurationType, null, null, null, false, null, null, null, null, null, null, null, null, codecConfigurationType, codecConfigurationBytes, null, null);

        public string PropertyType { get; }

        public uint? Width { get; }

        public uint? Height { get; }

        public int? RotationDegrees { get; }

        public bool IsMirrored { get; }

        public uint? PixelAspectRatioHorizontalSpacing { get; }

        public uint? PixelAspectRatioVerticalSpacing { get; }

        public IReadOnlyList<byte>? PixelBitDepths { get; }

        public string? ColorType { get; }

        public ushort? ColorPrimaries { get; }

        public ushort? TransferCharacteristics { get; }

        public ushort? MatrixCoefficients { get; }

        public bool? FullRangeFlag { get; }

        public string? CodecConfigurationType { get; }

        public IReadOnlyList<byte>? CodecConfigurationBytes { get; }

        public string? AuxiliaryType { get; }

        public IReadOnlyList<byte>? AuxiliarySubtypes { get; }
    }
}
