namespace ImagePlayground;

/// <summary>
/// Decoding status for barcode/QR operations.
/// </summary>
public enum Status {
    /// <summary>
    /// No barcode found.
    /// </summary>
    NotFound,
    /// <summary>
    /// Barcode found and decoded.
    /// </summary>
    Found,
    /// <summary>
    /// Error while decoding.
    /// </summary>
    Error
}

/// <summary>
/// Minimal result for barcode/QR decoding.
/// </summary>
public sealed class BarcodeResult<TPixel> {
    /// <summary>
    /// Decoding status.
    /// </summary>
    public Status Status { get; set; } = Status.NotFound;

    /// <summary>
    /// Decoded value (when available).
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Decoded message (alias for value).
    /// </summary>
    public string? Message { get; set; }
}
