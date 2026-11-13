# ImagePlayground

C# image library and binary PowerShell module: image processing, metadata and
EXIF, QR codes and barcodes, charts, canvases, diagrams and animated console
stories. Charts come from ChartForgeX, codes from CodeGlyphX.

## Layout

- `Sources/ImagePlayground/`: the library (`Image.*.cs`, `ImageHelper.*.cs`).
- `Sources/ImagePlayground.PowerShell/`: the cmdlets (`Cmdlet<Verb><Noun>.cs`).
- `Sources/ImagePlayground.Syntax.TreeSitter/`: optional Tree-sitter syntax adapter for
  visual stories.
- `Sources/ImagePlayground.Tests/` and `Sources/ImagePlayground.Syntax.TreeSitter.Tests/`:
  xUnit tests. Fixture images live in `Sources/ImagePlayground.Tests/Images/`.
- `Sources/ImagePlayground.Examples/`: C# examples. `Examples/`: PowerShell examples.
- Root `ImagePlayground.psd1` / `.psm1`: the module. `Tests/`: Pester tests, run
  through `ImagePlayground.Tests.ps1`.
- `Build/`: module and NuGet build scripts.
- `Website/content/`: curated pages and examples for the project site.

## Build and test

```powershell
dotnet build Sources/ImagePlayground.sln -c Release
dotnet test Sources/ImagePlayground.Tests/ImagePlayground.Tests.csproj -c Release --framework net8.0
dotnet test Sources/ImagePlayground.Syntax.TreeSitter.Tests/ImagePlayground.Syntax.TreeSitter.Tests.csproj -c Release
dotnet build Sources/ImagePlayground.PowerShell/ImagePlayground.PowerShell.csproj   # Debug, for Pester
./ImagePlayground.Tests.ps1                                       # Pester suite
./Build/Build-Module.ps1 -RunMode Build -SignModule $false        # package the module
./Build/Build-Project.ps1                                         # NuGet packages (Build/project.build.json)
```

Targets: library `netstandard2.0`, `net472`, `net8.0`, `net10.0`; PowerShell
project `netstandard2.0`, `net472`, `net8.0` (the module ships `net8.0` and
`net472`); tests `net472`, `net8.0`, `net10.0`
(CI runs `net8.0`).
Warnings are errors (`Directory.Build.props`).

`-RunMode Publish` and `Build-Project.ps1 -PublishNuget $true` /
`-PublishGitHub $true` are release-only.

## Generated files

`Docs/*.md` and `en-US/ImagePlayground-help.xml` come from the cmdlet XML
documentation. Run `./Build/Build-Module.ps1 -RunMode Documentation` after
changing cmdlet docs; don't edit the generated files.

## Conventions

- C#: K&R braces, braces on every block, file-scoped namespaces, follow
  `.editorconfig`. Prefer small files; split large classes with `partial`.
- Cmdlets are documented with XML docs in the XmlDoc2CmdletDoc style: `<summary>`
  for the synopsis, `<para>` for longer text, and `<example>` with `<summary>`
  and `<code>`. See `CmdletAddImageText.cs`.
- PowerShell scripts (tests, examples, build) support Windows PowerShell 5.1
  and PowerShell 7. Use K&R braces and full parameter names; avoid `+=` on arrays.
- Commit and PR titles use Conventional Commits with Gitmoji:
  `fix(barcode): 🐛 Preserve barcode options on negative scans`.

## Gotchas

- `ImagePlayground.Tests.ps1` sets `IMAGEPLAYGROUND_DEVELOPMENT=1`, so the module
  loads binaries from `Sources/ImagePlayground.PowerShell/bin/Debug/<net8.0|net472>`.
  Build that project in Debug first, or the tests run against stale binaries.
- ChartForgeX and CodeGlyphX come from NuGet. To build against sibling checkouts,
  pass `-p:UseLocalChartForgeXProject=true` or `-p:UseLocalCodeGlyphXProject=true`
  (or set `ChartForgeXProjectPath` / `CodeGlyphXProjectPath`).
- Tests read fixtures from `Sources/ImagePlayground.Tests/Images/`. Don't
  replace or re-encode them.
