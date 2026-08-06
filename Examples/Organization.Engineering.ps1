Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

New-ImageOrganizationChart -TeamId engineering -TeamLabel 'Engineering' -LayoutPolicy Auto -MemberDefinition {
    New-ImageOrganizationMember -Id director -Name 'Avery Stone' -Role 'Director of Engineering' -Status Healthy -LayoutPolicy Standard
    New-ImageOrganizationMember -Id platform -Name 'Morgan Lee' -Role 'Platform Lead' -ParentId director -Status Healthy -LayoutPolicy Compact
    New-ImageOrganizationMember -Id product -Name 'Sam Park' -Role 'Product Lead' -ParentId director -Status Healthy -LayoutPolicy Vertical
    New-ImageOrganizationMember -Id runtime -Name 'Alex Kim' -Role 'Runtime Engineer' -ParentId platform -Status Warning
    New-ImageOrganizationMember -Id tooling -Name 'Jordan Bell' -Role 'Tooling Engineer' -ParentId platform -Status Healthy
    New-ImageOrganizationMember -Id reliability -Name 'Casey Ward' -Role 'Reliability Engineer' -ParentId platform -Status Healthy
    New-ImageOrganizationMember -Id applications -Name 'Robin Shaw' -Role 'Applications Engineer' -ParentId platform -Status Healthy
    New-ImageOrganizationMember -Id ux -Name 'Taylor Reed' -Role 'Product Designer' -ParentId product -Status Healthy
    New-ImageOrganizationMember -Id research -Name 'Cameron Hall' -Role 'UX Researcher' -ParentId product -Status Healthy
} -FilePath "$PSScriptRoot\organization-engineering.svg" -Width 1100 -Height 620
