namespace ImagePlayground;

/// <summary>
/// Supported barcode formats.
/// </summary>
public enum BarcodeType
{
    /// <summary>Code 128 barcode.</summary>
    Code128,
    /// <summary>Code 93 barcode.</summary>
    Code93,
    /// <summary>Code 39 barcode.</summary>
    Code39,
    /// <summary>KIX code used by Dutch Post.</summary>
    KixCode,
    /// <summary>UPC-E barcode.</summary>
    UPCE,
    /// <summary>UPC-A barcode.</summary>
    UPCA,
    /// <summary>EAN-8/EAN-13 barcode.</summary>
    EAN,
    /// <summary>Data Matrix 2D barcode.</summary>
    DataMatrix,
    /// <summary>PDF417 2D barcode.</summary>
    PDF417
}
