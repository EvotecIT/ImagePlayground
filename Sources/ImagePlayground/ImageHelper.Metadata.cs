using System.Text.Json;
using System.Text.Json.Serialization;

namespace ImagePlayground;

/// <summary>Metadata inspection and import/export over the shared profile model.</summary>
public partial class ImageHelper {
    private static readonly JsonSerializerOptions MetadataJsonOptions = CreateMetadataJsonOptions();

    private static JsonSerializerOptions CreateMetadataJsonOptions() {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
    private sealed class SerializedImageMetadata {
        public double HorizontalResolution { get; set; }
        public double VerticalResolution { get; set; }
        public OfficeImageResolutionUnit ResolutionUnits { get; set; }
        public byte[]? ExifProfile { get; set; }
        public byte[]? XmpProfile { get; set; }
        public byte[]? IccProfile { get; set; }
        public byte[]? IptcProfile { get; set; }
    }
    /// <summary>Paths used by a metadata import operation.</summary>
    public sealed class ImportMetadataOptions {
        /// <summary>Creates an import operation, overwriting the input when no output is supplied.</summary>
        public ImportMetadataOptions(string filePath,string metadataPath,string? outputPath=null) { FilePath=filePath; MetadataPath=metadataPath; OutputPath=outputPath; }
        /// <summary>Source image path.</summary>
        public string FilePath { get; }
        /// <summary>Metadata JSON path.</summary>
        public string MetadataPath { get; }
        /// <summary>Optional destination image path.</summary>
        public string? OutputPath { get; }
    }
    /// <summary>Inspects raw metadata profiles and provenance without decoding raster pixels.</summary>
    public static ImageMetadataInfo InspectMetadata(string filePath) {
        string fullPath=Helpers.ResolvePath(filePath); OfficeImageMetadata metadata=Image.ReadMetadataFile(fullPath);
        bool hasContainerResolution = !Helpers.IsHeifExtension(fullPath);
        return new ImageMetadataInfo(fullPath,hasContainerResolution ? metadata.HorizontalResolution : (double?)null,hasContainerResolution ? metadata.VerticalResolution : (double?)null,hasContainerResolution ? metadata.ResolutionUnits : (OfficeImageResolutionUnit?)null,metadata.RequiresOriginalTiffContainer ? null : metadata.ExifProfile,metadata.XmpProfile,metadata.IccProfile,metadata.IptcProfile,InspectProvenanceCore(fullPath,metadata.XmpProfile),metadata.ExifValues,metadata.HasExifProfile,metadata.RequiresOriginalTiffContainer);
    }
    /// <summary>Exports metadata to JSON with base64-encoded binary profiles.</summary>
    public static string ExportMetadata(string filePath) {
        var metadata=Image.ReadMetadataFile(Helpers.ResolvePath(filePath));
        return JsonSerializer.Serialize(new SerializedImageMetadata { HorizontalResolution=metadata.HorizontalResolution,VerticalResolution=metadata.VerticalResolution,ResolutionUnits=metadata.ResolutionUnits,ExifProfile=metadata.ExifProfile,XmpProfile=metadata.XmpProfile,IccProfile=metadata.IccProfile,IptcProfile=metadata.IptcProfile }, MetadataJsonOptions);
    }
    /// <summary>Writes exported metadata JSON to a file.</summary>
    public static void ExportMetadata(string filePath,string outFilePath) { string output=Helpers.ResolvePath(outFilePath);Helpers.CreateParentDirectory(output);File.WriteAllText(output,ExportMetadata(filePath)); }
    /// <summary>Imports metadata and preserves encoded image data where the format supports profile editing.</summary>
    public static void ImportMetadata(ImportMetadataOptions options) {
        if(options==null) throw new ArgumentNullException(nameof(options));
        ImportMetadata(options.FilePath,options.MetadataPath,options.OutputPath??options.FilePath);
    }
    /// <summary>Imports profile JSON into an existing encoded image.</summary>
    public static void ImportMetadata(string filePath,string metadataFilePath,string outFilePath) {
        using var metadataStream = new MemoryStream(Helpers.ReadEncodedFile(Helpers.ResolvePath(metadataFilePath)), writable: false);
        using var metadataReader = new StreamReader(metadataStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        string json = metadataReader.ReadToEnd();
        SerializedImageMetadata data;
        try {
            using(var document=JsonDocument.Parse(json)) {
            var root=document.RootElement;
            if(root.ValueKind!=JsonValueKind.Object || !root.TryGetProperty(nameof(SerializedImageMetadata.HorizontalResolution),out var horizontal) || horizontal.ValueKind!=JsonValueKind.Number || !root.TryGetProperty(nameof(SerializedImageMetadata.VerticalResolution),out var vertical) || vertical.ValueKind!=JsonValueKind.Number || !root.TryGetProperty(nameof(SerializedImageMetadata.ResolutionUnits),out _)) throw new InvalidDataException("Metadata JSON must include numeric resolution values and resolution units.");
            }
            data=JsonSerializer.Deserialize<SerializedImageMetadata>(json, MetadataJsonOptions)??throw new InvalidDataException("Metadata JSON cannot be null.");
        } catch (JsonException exception) { throw new InvalidDataException("Metadata JSON cannot be parsed.", exception); }
        if(!Enum.IsDefined(typeof(OfficeImageResolutionUnit),data.ResolutionUnits)) throw new InvalidDataException("The metadata resolution unit is unknown.");
        var resolution = new OfficeImageResolution(data.HorizontalResolution, data.VerticalResolution, data.ResolutionUnits);
        var metadata = new OfficeImageMetadata { ExifProfile=data.ExifProfile,XmpProfile=data.XmpProfile,IccProfile=data.IccProfile,IptcProfile=data.IptcProfile };
        // Profile parsing restores its stored density; the editable JSON resolution fields take precedence.
        metadata.Resolution = resolution;
        Image.WriteMetadataFile(Helpers.ResolvePath(filePath),outFilePath,metadata);
    }
    /// <summary>Removes all supported metadata profiles.</summary>
    public static void RemoveMetadata(string filePath,string outFilePath) => RemoveMetadata(new ImageMetadataRemovalOptions(filePath,outFilePath));
    private static ImageMetadataType RemoveHeifMetadata(string filePath,string outputPath,ImageMetadataType requested) {
        if((requested&~(ImageMetadataType.Exif|ImageMetadataType.Xmp))!=0 && requested!=ImageMetadataType.All) throw new NotSupportedException("HEIF metadata removal supports EXIF and XMP profiles.");
        ImageMetadataType removed = ImageMetadataType.None;
        OfficeImageMetadataProfileKinds profiles = OfficeImageMetadataProfileKinds.None;
        if ((requested & ImageMetadataType.Exif) != 0 && OfficeHeifMetadataReader.HasExifItem(filePath)) {
            profiles |= OfficeImageMetadataProfileKinds.Exif;
            removed |= ImageMetadataType.Exif;
        }
        if ((requested & ImageMetadataType.Xmp) != 0 && OfficeHeifMetadataReader.HasXmpItem(filePath)) {
            profiles |= OfficeImageMetadataProfileKinds.Xmp;
            removed |= ImageMetadataType.Xmp;
        }
        Image.WriteMetadataFile(filePath, outputPath, new OfficeImageMetadata(), profiles);
        return removed;
    }
}
