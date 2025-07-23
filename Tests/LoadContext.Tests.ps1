Describe 'Assembly Load Context' {

    It 'creates a dedicated context on import' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        $context = [System.Runtime.Loader.AssemblyLoadContext]::All | Where-Object Name -eq 'ImagePlayground'
        $context | Should -Not -BeNullOrEmpty
    }
}
