using System;
using System.Collections.Generic;
using System.Text;

namespace ImagePlayground {
    public class Charts {

        public static void Generate(string content) {
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatter(dataX, dataY);
            plt.SaveFig("quickstart.png");
        }
    }
}
