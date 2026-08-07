---
title: "Render an organization hierarchy"
description: "Build a compact organization hierarchy from PowerShell with branch-level layout policies."
layout: docs
---

Use the organization surface when input data describes reporting relationships rather than a free-form network. A branch can opt into compact or vertical layout while the rest of the hierarchy keeps its normal policy.

```powershell
using module ImagePlayground

New-ImageOrganizationChart -TeamId engineering -TeamLabel 'Engineering' -LayoutPolicy Auto -MemberDefinition {
    New-ImageOrganizationMember -Id director -Name 'Avery Stone' -Role 'Director of Engineering' -Status Healthy -LayoutPolicy Standard
    New-ImageOrganizationMember -Id platform -Name 'Morgan Lee' -Role 'Platform Lead' -ParentId director -Status Healthy -LayoutPolicy Compact
    New-ImageOrganizationMember -Id product -Name 'Sam Park' -Role 'Product Lead' -ParentId director -Status Healthy -LayoutPolicy Vertical
    New-ImageOrganizationMember -Id runtime -Name 'Alex Kim' -Role 'Runtime Engineer' -ParentId platform -Status Warning
    New-ImageOrganizationMember -Id tooling -Name 'Jordan Bell' -Role 'Tooling Engineer' -ParentId platform -Status Healthy
    New-ImageOrganizationMember -Id ux -Name 'Taylor Reed' -Role 'Product Designer' -ParentId product -Status Healthy
} -FilePath '.\engineering-organization.svg' -Width 1100 -Height 620
```

For exact command names and parameter sets in the installed release, use the [ImagePlayground API reference](/projects/imageplayground/api/). The underlying hierarchy and topology model is documented in the [ChartForgeX hub](/projects/chartforgex/).
