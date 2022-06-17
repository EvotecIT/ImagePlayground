using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;
using SixLabors.ImageSharp;

namespace ImagePlayground {
    public class QrCode {
        public static void Generate(string content, string filePath, ImageFormat imageFormat, bool transparent = false, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q) {

            ImageFormat imageFormatDetected;
            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Extension == ".png") {
                imageFormatDetected = ImageFormat.Png;
            } else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
                imageFormatDetected = ImageFormat.Jpeg;
            } else if (fileInfo.Extension == ".ico") {
                imageFormatDetected = ImageFormat.Icon;
            } else {
                throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
            }

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator()) {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel)) {
                    using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData)) {
                        using (Bitmap qrCodeImage = qrCode.GetGraphic(20)) {
                            if (transparent) {
                                qrCodeImage.MakeTransparent();
                            }

                            qrCodeImage.Save(filePath, imageFormatDetected);
                        }
                    }
                }
            }
        }
        public static void GenerateWiFi(string ssid, string password, string filePath, ImageFormat imageFormat, bool transparent = false) {
            PayloadGenerator.WiFi generator = new PayloadGenerator.WiFi(ssid, password, PayloadGenerator.WiFi.Authentication.WPA);
            Generate(generator.ToString(), filePath, imageFormat, transparent);
        }

        public static void GenerateWhatsAppMessage(string message, string filePath, ImageFormat imageFormat, bool transparent = false) {
            PayloadGenerator.WhatsAppMessage generator = new PayloadGenerator.WhatsAppMessage(message);
            Generate(generator.ToString(), filePath, imageFormat, transparent);
        }
        public static void GenerateUrl(string url, string filePath, ImageFormat imageFormat, bool transparent = false) {
            PayloadGenerator.Url generator = new PayloadGenerator.Url(url);
            Generate(generator.ToString(), filePath, imageFormat, transparent);
        }
        public static void GenerateBookmark(string bookmarkUrl, string bookmarkName, string filePath, ImageFormat imageFormat, bool transparent = false) {
            PayloadGenerator.Bookmark generator = new PayloadGenerator.Bookmark(bookmarkUrl, bookmarkName);
            Generate(generator.ToString(), filePath, imageFormat, transparent);
        }

        public static void GenerateCalendarEvent(string calendarEntry, string calendarMessage, string calendarGeoLocation, DateTime calendarFrom, DateTime calendarTo, string filePath, ImageFormat imageFormat, bool allDayEvent, PayloadGenerator.CalendarEvent.EventEncoding calendarEventEncoding = PayloadGenerator.CalendarEvent.EventEncoding.iCalComplete, bool transparent = false) {
            PayloadGenerator.CalendarEvent generator = new PayloadGenerator.CalendarEvent(calendarEntry, calendarMessage, calendarGeoLocation, calendarFrom, calendarTo, allDayEvent, calendarEventEncoding);
            Generate(generator.ToString(), filePath, imageFormat, transparent);
        }

        public static void GenerateContact(string filePath, ImageFormat imageFormat, PayloadGenerator.ContactData.ContactOutputType outputType, string firstname, string lastname, string nickname = null, string phone = null, string mobilePhone = null, string workPhone = null, string email = null, DateTime? birthday = null, string website = null, string street = null, string houseNumber = null, string city = null, string zipCode = null, string country = null, string note = null, string stateRegion = null, PayloadGenerator.ContactData.AddressOrder addressOrder = PayloadGenerator.ContactData.AddressOrder.Default, string org = null, string orgTitle = null, bool transparent = false) {
            PayloadGenerator.ContactData generator = new PayloadGenerator.ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note, stateRegion, addressOrder, org, orgTitle);
            Generate(generator.ToString(), filePath, imageFormat, transparent);
        }
    }
}