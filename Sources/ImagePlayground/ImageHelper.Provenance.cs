using System.Collections.Generic;
using OfficeIMO.Provenance;

namespace ImagePlayground;

/// <summary>Image workflows over the shared raster and metadata engine.</summary>
public partial class ImageHelper {
    /// <summary>Inspects embedded C2PA carriers and standardized XMP generative-AI declarations.</summary>
    /// <param name="filePath">Path to the image to inspect.</param>
    /// <returns>Detected provenance signals and their metadata sources.</returns>
    /// <remarks>Carrier and XMP interpretation delegate to OfficeIMO.Core. This API does not interpret an active
    /// C2PA claim or validate signatures, trust chains, or asset hashes.</remarks>
    public static ImageProvenanceInfo InspectProvenance(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        OfficeImageMetadata metadata = Image.ReadMetadataFile(fullPath, OfficeImageMetadataProfileKinds.Xmp);
        return InspectProvenanceCore(fullPath, metadata.XmpProfile);
    }

    private static ImageProvenanceInfo InspectProvenanceCore(string fullPath, byte[]? xmp) {
        var evidence = new List<ImageProvenanceEvidence>();
        byte[] encoded = Helpers.ReadEncodedFile(fullPath);
        var options = new OfficeProvenanceOptions { MaxAssetBytes = 128L * 1024L * 1024L };
        OfficeProvenanceReport carrierReport = OfficeProvenanceInspector.Inspect(encoded, fullPath, options);
        if (carrierReport.HasC2paManifest) {
            AddEvidence(evidence, ImageProvenanceSource.C2pa, ImageProvenanceSignal.C2paManifest, "C2PA");
        }
        if (xmp != null) {
            OfficeProvenanceReport xmpReport = OfficeProvenanceInspector.InspectXmp(xmp, options);
            foreach (OfficeProvenanceEvidence item in xmpReport.Evidence) {
                if (item.DigitalSourceKind == OfficeProvenanceDigitalSourceKind.TrainedAlgorithmicMedia) {
                    AddEvidence(evidence, ImageProvenanceSource.Xmp, ImageProvenanceSignal.CreatedUsingGenerativeAi, item.Value ?? string.Empty);
                } else if (item.DigitalSourceKind == OfficeProvenanceDigitalSourceKind.CompositeWithTrainedAlgorithmicMedia) {
                    AddEvidence(evidence, ImageProvenanceSource.Xmp, ImageProvenanceSignal.EditedUsingGenerativeAi, item.Value ?? string.Empty);
                }
            }
        }
        return new ImageProvenanceInfo(fullPath, evidence.AsReadOnly());
    }

    private static void AddEvidence(List<ImageProvenanceEvidence> evidence, ImageProvenanceSource source, ImageProvenanceSignal signal, string value) {
        foreach (ImageProvenanceEvidence item in evidence) {
            if (item.Source == source && item.Signal == signal) {
                return;
            }
        }
        evidence.Add(new ImageProvenanceEvidence(source, signal, value));
    }
}
