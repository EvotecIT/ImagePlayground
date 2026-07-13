Describe 'Async cmdlet lifecycle' {
    BeforeAll {
        $asyncCmdletType = (Get-Command -Name Resize-Image).ImplementingType.Assembly.GetType(
            'ImagePlayground.PowerShell.AsyncPSCmdlet',
            $true
        )

        $asyncCommands = @(
            'Add-ImageWatermark'
            'Get-ImageBarCode'
            'Get-ImageQRCode'
            'New-ImageBarCode'
            'New-ImageQRCode'
            'Resize-Image'
            'Set-ImageAdjust'
            'Set-ImageBlur'
            'Set-ImageRotation'
            'Set-ImageSharpen'
        )
    }

    It 'uses the async cmdlet lifecycle for async-backed commands' {
        foreach ($commandName in $asyncCommands) {
            $commandType = (Get-Command -Name $commandName).ImplementingType
            $asyncCmdletType.IsAssignableFrom($commandType) | Should -BeTrue -Because "$commandName should inherit AsyncPSCmdlet"
        }
    }
}
