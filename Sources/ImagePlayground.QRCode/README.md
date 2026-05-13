# ImagePlayground.QRCode

ImagePlayground.QRCode contains the ImagePlayground QR code wrapper API backed by CodeGlyphX.

This package remains available for compatibility with projects that intentionally reference the split assembly. New ImagePlayground users should usually reference `ImagePlayground`; applications that need direct QR engine access should reference `CodeGlyphX`.

## Supported Areas

- Raw text and URL QR codes.
- Contact cards, WiFi payloads, SMS, phone numbers, email, calendar, OTP, geolocation, ShadowSocks, Skype call, Bitcoin, Monero, Girocode, BezahlCode, Swiss QR, and Slovenian UPN QR payloads.
- QR code reading from image files.
- Transparent output, custom foreground/background colors, logo overlay, and async helpers.

## Related Package

- `CodeGlyphX` is the direct engine for QR codes, barcode generation, barcode reading, payload builders, and rendering primitives.
