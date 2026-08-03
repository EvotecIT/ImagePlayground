using System.Collections.Generic;

namespace ImagePlayground;

/// <summary>
/// Identifies the metadata source that supplied image provenance evidence.
/// </summary>
public enum ImageProvenanceSource {
    /// <summary>The evidence was found in a C2PA Content Credentials manifest.</summary>
    C2pa,

    /// <summary>The evidence was found in an XMP metadata profile.</summary>
    Xmp
}

/// <summary>
/// Identifies a provenance signal found in an image.
/// </summary>
public enum ImageProvenanceSignal {
    /// <summary>The image contains an embedded C2PA Content Credentials manifest.</summary>
    C2paManifest,

    /// <summary>The image declares IPTC <c>trainedAlgorithmicMedia</c>.</summary>
    CreatedUsingGenerativeAi,

    /// <summary>The image declares IPTC <c>compositeWithTrainedAlgorithmicMedia</c>.</summary>
    EditedUsingGenerativeAi
}

/// <summary>
/// Describes one provenance signal found in an image.
/// </summary>
public sealed class ImageProvenanceEvidence {
    /// <summary>
    /// Creates provenance evidence.
    /// </summary>
    /// <param name="source">Metadata source containing the signal.</param>
    /// <param name="signal">Detected signal.</param>
    /// <param name="value">Raw standardized value that caused the match.</param>
    public ImageProvenanceEvidence(ImageProvenanceSource source, ImageProvenanceSignal signal, string value) {
        Source = source;
        Signal = signal;
        Value = value;
    }

    /// <summary>Metadata source containing the signal.</summary>
    public ImageProvenanceSource Source { get; }

    /// <summary>Detected provenance signal.</summary>
    public ImageProvenanceSignal Signal { get; }

    /// <summary>Raw standardized value that caused the match.</summary>
    public string Value { get; }
}

/// <summary>
/// Reports embedded C2PA containers and direct XMP generative-AI provenance declarations.
/// </summary>
/// <remarks>
/// This result does not interpret the active C2PA manifest or cryptographically validate a C2PA claim,
/// certificate chain, trust list, or asset hash.
/// </remarks>
public sealed class ImageProvenanceInfo {
    internal ImageProvenanceInfo(string filePath, IReadOnlyList<ImageProvenanceEvidence> evidence) {
        FilePath = filePath;
        Evidence = evidence;
    }

    /// <summary>Resolved path to the inspected image.</summary>
    public string FilePath { get; }

    /// <summary>Evidence found in the image.</summary>
    public IReadOnlyList<ImageProvenanceEvidence> Evidence { get; }

    /// <summary>Whether an embedded C2PA Content Credentials manifest was found.</summary>
    /// <remarks>A C2PA manifest records provenance but does not by itself mean that the image was made with AI.</remarks>
    public bool HasC2paManifest => HasSignal(ImageProvenanceSignal.C2paManifest);

    /// <summary>Whether direct XMP metadata declares that the image was created using generative AI.</summary>
    public bool XmpDeclaresAiGenerated => HasSignal(ImageProvenanceSignal.CreatedUsingGenerativeAi);

    /// <summary>Whether direct XMP metadata declares that the image was edited using generative AI.</summary>
    public bool XmpDeclaresAiEdited => HasSignal(ImageProvenanceSignal.EditedUsingGenerativeAi);

    /// <summary>Whether a recognized direct XMP generative-AI declaration was found.</summary>
    public bool HasXmpAiDeclaration => XmpDeclaresAiGenerated || XmpDeclaresAiEdited;

    /// <summary>
    /// Whether cryptographic C2PA validation was performed.
    /// </summary>
    /// <remarks>
    /// ImagePlayground performs lightweight local detection, so this property is always <c>false</c>.
    /// Use a conforming C2PA validator when signature and trust verification are required.
    /// </remarks>
    public bool C2paValidationPerformed => false;

    private bool HasSignal(ImageProvenanceSignal signal) {
        foreach (ImageProvenanceEvidence item in Evidence) {
            if (item.Signal == signal) {
                return true;
            }
        }

        return false;
    }
}