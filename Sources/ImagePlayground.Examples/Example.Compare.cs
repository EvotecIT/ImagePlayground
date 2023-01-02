using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePlayground.Examples {
    internal partial class Example {
        public static void Compare(string folderPath) {
            Console.WriteLine("[*] Comparing two images - showing output");
            string filePath = System.IO.Path.Combine(folderPath, "CalculatorBefore.png");
            string filePathToCompare = System.IO.Path.Combine(folderPath, "CalculatorAfter.png");
            var results = ImageHelper.Compare(filePath, filePathToCompare);
            Console.WriteLine("+ AbsoluteError: " + results.AbsoluteError);
            Console.WriteLine("+ MeanError: " + results.MeanError);
            Console.WriteLine("+ PixelErrorCount: " + results.PixelErrorCount);
            Console.WriteLine("+ PixelErrorPercentage: " + results.PixelErrorPercentage);

            Console.WriteLine("[*] Comparing two images - saving output");
            filePath = System.IO.Path.Combine(folderPath, "CalculatorBefore.png");
            filePathToCompare = System.IO.Path.Combine(folderPath, "CalculatorAfter.png");
            string filePathOutput = System.IO.Path.Combine(folderPath, "CalculatorDifferenceMask.png");
            ImageHelper.Compare(filePath, filePathToCompare, filePathOutput);
        }
    }
}
