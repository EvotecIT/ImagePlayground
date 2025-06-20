using System.Collections.Generic;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground;
public partial class Image {
    public IReadOnlyList<IExifValue> GetExifValues() =>
        _image.Metadata.ExifProfile?.Values ?? new List<IExifValue>();

    public void SetExifValue(ExifTag tag, object value) {
        _image.Metadata.ExifProfile ??= new ExifProfile();
        var method = typeof(ExifProfile).GetMethod("SetValue")!;
        var generic = method.MakeGenericMethod(tag.GetType().GenericTypeArguments[0]);
        generic.Invoke(_image.Metadata.ExifProfile, new[] { tag, value });
    }

    public void RemoveExifValues(params ExifTag[] tags) {
        var profile = _image.Metadata.ExifProfile;
        if (profile is null) { return; }
        foreach (var t in tags) {
            profile.RemoveValue(t);
        }
    }

    public void ClearExifValues() {
        var profile = _image.Metadata.ExifProfile;
        if (profile != null) {
            if (profile.Values is List<IExifValue> list) {
                list.Clear();
            }
        }
    }
}
