using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using global::ImagePlayground;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_GenerateBarChart() {
            string file = Path.Combine(_directoryWithTests, "chart_bar.png");
            if (File.Exists(file)) File.Delete(file);

            var defs = new List<global::ImagePlayground.Charts.ChartDefinition> {
                new global::ImagePlayground.Charts.ChartBar("A", new List<double> { 1, 2 }),
                new global::ImagePlayground.Charts.ChartBar("B", new List<double> { 3, 4 })
            };

            global::ImagePlayground.Charts.Generate(defs, file, 300, 200);

            Assert.True(File.Exists(file));
        }

        [Fact]
        public void Test_GenerateContactQr() {
            string file = Path.Combine(_directoryWithTests, "contact.png");
            if (File.Exists(file)) File.Delete(file);

            QrCode.GenerateContact(file, QRCoder.PayloadGenerator.ContactData.ContactOutputType.VCard4, "John", "Doe");

            Assert.True(File.Exists(file));
            var read = QrCode.Read(file);
            Assert.Contains("BEGIN", read.Message);
        }

        [Fact]
        public void Test_GenerateMixedChartsThrows() {
            var defs = new List<Charts.ChartDefinition> {
                new Charts.ChartBar("A", new List<double> { 1 }),
                new Charts.ChartLine("B", new List<double> { 2 })
            };

            Assert.Throws<ArgumentException>(() => Charts.Generate(defs, Path.Combine(_directoryWithTests, "mixed.png")));
        }

        [Fact]
        public void Test_GenerateNullDefinitionsThrows() {
            Assert.Throws<ArgumentNullException>(() => Charts.Generate(null!, Path.Combine(_directoryWithTests, "null.png")));
        }

        [Fact]
        public void Test_GenerateEmptyDefinitionsThrows() {
            Assert.Throws<ArgumentException>(() => Charts.Generate(new List<Charts.ChartDefinition>(), Path.Combine(_directoryWithTests, "empty.png")));
        }
    }
}
