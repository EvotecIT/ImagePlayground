Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$xs = 1, 2, 3, 4, 5
$ys = 1, 4, 9, 16, 25
$plt = [ScottPlot.Plot]::new(400, 300);
$null = $plt.AddScatter($xs, $ys);
$Path = $plt.SaveFig("$PSScriptRoot\quickstart.png");