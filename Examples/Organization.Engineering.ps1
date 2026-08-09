Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

New-ImageOrganizationChart -TeamId engineering -TeamLabel 'Engineering' -MemberDefinition {
    New-ImageOrganizationMember -Id director -Name 'Avery Stone' -Role 'Director of Engineering' -Status Healthy
    New-ImageOrganizationMember -Id platform -Name 'Morgan Lee' -Role 'Platform Lead' -ParentId director -Status Healthy
    New-ImageOrganizationMember -Id product -Name 'Sam Park' -Role 'Product Lead' -ParentId director -Status Healthy
    New-ImageOrganizationMember -Id runtime -Name 'Alex Kim' -Role 'Runtime Engineer' -ParentId platform -Status Warning
    New-ImageOrganizationMember -Id ux -Name 'Taylor Reed' -Role 'Product Designer' -ParentId product -Status Healthy
} -FilePath "$PSScriptRoot\organization-engineering.svg" -Width 1100 -Height 620
