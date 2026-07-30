using System;
using System.Linq;
using ChartForgeX.Stories;
using Xunit;

namespace ImagePlayground.Syntax.TreeSitter.Tests;

public sealed class TreeSitterStorySourceTokenizerTests {
    [Fact]
    public void CSharpUsesAstSpansWithoutBreakingUnicodeOffsets() {
        const string source = "using System;\nvar café = \"😀\"; // outcome\nConsole.WriteLine(café);";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        Assert.Equal(source, result.Text);
        Assert.Equal("csharp", result.Language);
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Keyword && Slice(result, span) == "using");
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.String && Slice(result, span) == "\"😀\"");
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Comment && Slice(result, span) == "// outcome");
        Assert.All(result.Spans, span => Assert.InRange(span.End, 1, source.Length));
    }

    [Fact]
    public void BashUsesAstSpansForCommandsStringsVariablesAndComments() {
        const string source = "status=\"ready\"\nprintf '%s\\n' \"$status\" # result";
        var result = TreeSitterStorySourceTokenizer.Create("bash").Tokenize(source);

        Assert.Equal(source, result.Text);
        Assert.Equal("bash", result.Language);
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span).Contains("printf", StringComparison.Ordinal));
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.String);
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Variable);
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Comment && Slice(result, span) == "# result");
    }

    [Fact]
    public void UnsupportedLanguagesAreExplicit() {
        Assert.Throws<NotSupportedException>(() => TreeSitterStorySourceTokenizer.Create("powershell"));
    }

    private static string Slice(StorySourceText source, StorySourceSpan span) =>
        source.Text.Substring(span.Start, span.Length);
}
