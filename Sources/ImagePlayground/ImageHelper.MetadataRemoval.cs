namespace ImagePlayground;

/// <summary>Selective metadata removal delegated to the shared container engine.</summary>
public partial class ImageHelper {
    /// <summary>Removes selected metadata without re-encoding the image pixel data.</summary>
    public static ImageMetadataRemovalResult RemoveMetadata(ImageMetadataRemovalOptions options) {
        if(options==null)throw new ArgumentNullException(nameof(options));
        if((options.MetadataTypes&~ImageMetadataType.All)!=0)throw new ArgumentOutOfRangeException(nameof(options.MetadataTypes));
        string input=Helpers.ResolvePath(options.FilePath),output=Helpers.ResolvePath(options.OutputPath);Helpers.CreateParentDirectory(output);
        long originalLength=new FileInfo(input).Length;ImageMetadataType removed;
        if(Helpers.IsHeifExtension(input))removed=RemoveHeifMetadata(input,output,options.MetadataTypes);
        else {
            OfficeImageMetadataProfileKinds kinds=OfficeImageMetadataProfileKinds.None;
            if((options.MetadataTypes&ImageMetadataType.Exif)!=0)kinds|=OfficeImageMetadataProfileKinds.Exif;
            if((options.MetadataTypes&ImageMetadataType.Xmp)!=0)kinds|=OfficeImageMetadataProfileKinds.Xmp;
            if((options.MetadataTypes&ImageMetadataType.Icc)!=0)kinds|=OfficeImageMetadataProfileKinds.Icc;
            if((options.MetadataTypes&ImageMetadataType.Iptc)!=0)kinds|=OfficeImageMetadataProfileKinds.Iptc;
            if((options.MetadataTypes&ImageMetadataType.C2pa)!=0)kinds|=OfficeImageMetadataProfileKinds.C2pa;
            var result=OfficeImageMetadata.Remove(Helpers.ReadEncodedFile(input),kinds);
            File.WriteAllBytes(output,result.EncodedBytes);removed=FromProfileKinds(result.RemovedProfiles);
        }
        return new ImageMetadataRemovalResult(input,output,options.MetadataTypes,removed,false,originalLength,new FileInfo(output).Length);
    }
    private static ImageMetadataType FromProfileKinds(OfficeImageMetadataProfileKinds kinds) {
        ImageMetadataType result=ImageMetadataType.None;
        if((kinds&OfficeImageMetadataProfileKinds.Exif)!=0)result|=ImageMetadataType.Exif;
        if((kinds&OfficeImageMetadataProfileKinds.Xmp)!=0)result|=ImageMetadataType.Xmp;
        if((kinds&OfficeImageMetadataProfileKinds.Icc)!=0)result|=ImageMetadataType.Icc;
        if((kinds&OfficeImageMetadataProfileKinds.Iptc)!=0)result|=ImageMetadataType.Iptc;
        if((kinds&OfficeImageMetadataProfileKinds.C2pa)!=0)result|=ImageMetadataType.C2pa;
        return result;
    }
}