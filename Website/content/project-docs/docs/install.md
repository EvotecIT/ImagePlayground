---
title: "Install ImagePlayground"
description: "Install ImagePlayground from PowerShell Gallery or NuGet."
layout: docs
meta.generated_by: "powerforge.project-docs-sync"
meta.project_base_slug: "imageplayground"
meta.project_name: "ImagePlayground"
meta.project_section: "docs"
meta.project_hub_path: "/projects/imageplayground/"
meta.project_link_docs: "/projects/imageplayground/docs/"
---

Choose the package source that matches your automation.

## PowerShell Gallery

```powershell
Install-Module ImagePlayground -Scope CurrentUser
```

Import it and inspect the commands available in your installed version:

```powershell
Import-Module ImagePlayground
Get-Command -Module ImagePlayground
```

## .NET package

```bash
dotnet add package ImagePlayground
```

The PowerShell module supports Windows PowerShell 5.1 and PowerShell 7+. Some formats and platform integrations depend on the capabilities available on the current operating system; use the API reference for the exact command contract in the version shown by the project page.

## Next steps

- Browse the [curated examples](/projects/imageplayground/examples/)
- Choose an [image or visual reporting workflow](../workflows/)
- Open the [PowerShell API reference](/projects/imageplayground/api/)
- Open the [project source](https://github.com/EvotecIT/ImagePlayground)
