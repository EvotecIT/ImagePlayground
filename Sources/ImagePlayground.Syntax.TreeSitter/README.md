# ImagePlayground.Syntax.TreeSitter

Optional AST-backed syntax spans for ImagePlayground and ChartForgeX visual stories.

The package keeps the native Tree-sitter runtime and language grammars outside the default
ImagePlayground PowerShell module. Use `TreeSitterStorySourceTokenizer` for C# or Bash, then
pass the resulting `StorySourceText` to `New-ImageStoryPanel`.

```powershell
$tokenizer = [ImagePlayground.Syntax.TreeSitter.TreeSitterStorySourceTokenizer]::Create('bash')
$source = $tokenizer.Tokenize("curl -s https://example.test/api | jq '.status'")
New-ImageStoryPanel -Id source -Source $source -Title Bash
```
