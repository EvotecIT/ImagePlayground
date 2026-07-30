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
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == "WriteLine");
        Assert.DoesNotContain(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == "Console");
        Assert.All(result.Spans, span => Assert.InRange(span.End, 1, source.Length));
    }

    [Fact]
    public void CSharpRecognizesContextualKeywordsAndOperators() {
        const string source = "public partial record Demo { required int Value { get; init; } async Task Run() { await Work(); Value++; value ??= fallback; flags <<= 1; } }";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var keyword in new[] { "partial", "record", "required", "init", "async", "await" }) {
            Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Keyword && Slice(result, span) == keyword);
        }
        foreach (var operation in new[] { "++", "??=", "<<=" }) {
            Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Operator && Slice(result, span) == operation);
        }
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == "Work");
    }

    [Fact]
    public void CSharpMapsManyUnicodeTokenOffsetsWithoutLosingText() {
        var source = string.Join("\n", Enumerable.Range(0, 500).Select(index => "var café" + index + " = \"😀\";"));
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        Assert.Equal(source, result.Text);
        Assert.True(result.Spans.Count > 1000);
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
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Variable && Slice(result, span) == "$status");
        Assert.DoesNotContain(result.Spans, span =>
            span.Kind == StorySyntaxKind.String &&
            span.Start <= source.IndexOf("$status", StringComparison.Ordinal) &&
            span.End >= source.IndexOf("$status", StringComparison.Ordinal) + "$status".Length);
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Comment && Slice(result, span) == "# result");
    }

    [Fact]
    public void BashLeavesOrdinaryWordsPlain() {
        const string source = "echo ready";
        var result = TreeSitterStorySourceTokenizer.Create("bash").Tokenize(source);

        Assert.DoesNotContain(result.Spans, span =>
            span.Kind == StorySyntaxKind.Variable &&
            Slice(result, span) == "ready");
    }

    [Fact]
    public void UnsupportedLanguagesAreExplicit() {
        Assert.Throws<NotSupportedException>(() => TreeSitterStorySourceTokenizer.Create("powershell"));
    }

    private static string Slice(StorySourceText source, StorySourceSpan span) =>
        source.Text.Substring(span.Start, span.Length);
}
