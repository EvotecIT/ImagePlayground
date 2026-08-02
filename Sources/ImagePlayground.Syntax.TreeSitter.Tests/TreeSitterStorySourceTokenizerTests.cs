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
        const string source = "public partial record Demo { required int Value { get; init; } async Task Run() { await Work(); checked { Value++; } value ??= fallback; flags <<= 1; var result = ready ? success : failure; } }";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var keyword in new[] { "partial", "record", "required", "init", "async", "await", "checked" }) {
            Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Keyword && Slice(result, span) == keyword);
        }
        foreach (var operation in new[] { "++", "??=", "<<=", "?" }) {
            Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Operator && Slice(result, span) == operation);
        }
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == "Work");
    }

    [Fact]
    public void CSharpRecognizesDeclaredTypeNames() {
        const string source = "class ClassType { } struct StructType { } interface InterfaceType { } enum EnumType { Ready } record RecordType; delegate void Callback();";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var typeName in new[] { "ClassType", "StructType", "InterfaceType", "EnumType", "RecordType", "Callback" }) {
            Assert.Contains(result.Spans, span =>
                span.Kind == StorySyntaxKind.Type &&
                Slice(result, span) == typeName);
        }
        Assert.Contains(result.Spans, span =>
            span.Kind == StorySyntaxKind.Property &&
            Slice(result, span) == "Ready");
    }

    [Fact]
    public void CSharpRecognizesUserDefinedTypeReferences() {
        const string source = "Widget item = new Widget(); List<string> items = new(); Widget Build(Widget input) => input;";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var typeName in new[] { "Widget", "List" }) {
            Assert.Contains(result.Spans, span =>
                span.Kind == StorySyntaxKind.Type &&
                Slice(result, span) == typeName);
        }
        foreach (var variableName in new[] { "item", "items", "input" }) {
            Assert.DoesNotContain(result.Spans, span =>
                span.Kind == StorySyntaxKind.Type &&
                Slice(result, span) == variableName);
        }
    }

    [Fact]
    public void CSharpLeavesContextualWordsPlainWhenTheyAreIdentifiers() {
        const string source = "var value = obj.value; int record = 1; int required = record; int async = required;";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var identifier in new[] { "value", "record", "required", "async" }) {
            Assert.DoesNotContain(result.Spans, span =>
                span.Kind == StorySyntaxKind.Keyword &&
                Slice(result, span) == identifier);
        }
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Keyword && Slice(result, span) == "var");
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
    public void CSharpRecognizesGenericInvocationsAndPreprocessorDirectives() {
        const string source = "#nullable enable\n#if DEBUG\nclient.SendAsync<string>();\nWork<Widget>();\n#endif";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var command in new[] { "SendAsync", "Work" }) {
            Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == command);
        }
        Assert.DoesNotContain(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == "client");
        foreach (var typeName in new[] { "string", "Widget" }) {
            Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Type && Slice(result, span) == typeName);
        }
        foreach (var directive in new[] { "#nullable", "#if", "#endif" }) {
            Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Keyword && Slice(result, span) == directive);
        }
        Assert.DoesNotContain(result.Spans, span => span.Kind == StorySyntaxKind.Keyword && Slice(result, span) == "DEBUG");
    }

    [Fact]
    public void CSharpRecognizesDeclarationNamesByRole() {
        const string source = "class Demo { void Work() { } int Count { get; } }";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == "Work");
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Property && Slice(result, span) == "Count");
    }

    [Fact]
    public void CSharpRecognizesGenericEventAndTupleDeclarationRoles() {
        const string source = "class Box<T> { public event EventHandler Changed; public (int count, string name) Value; public T Item; void Map<TItem>(TItem value) { } }";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var typeParameter in new[] { "T", "TItem" }) {
            Assert.Contains(result.Spans, span =>
                span.Kind == StorySyntaxKind.Type &&
                Slice(result, span) == typeParameter);
            Assert.DoesNotContain(result.Spans, span =>
                span.Kind == StorySyntaxKind.Variable &&
                Slice(result, span) == typeParameter);
        }
        Assert.Contains(result.Spans, span =>
            span.Kind == StorySyntaxKind.Property &&
            Slice(result, span) == "Changed");
        foreach (var tupleLabel in new[] { "count", "name" }) {
            Assert.DoesNotContain(result.Spans, span =>
                span.Kind == StorySyntaxKind.Type &&
                Slice(result, span) == tupleLabel);
        }
    }

    [Fact]
    public void CSharpRecognizesNamedArgumentsAsParameters() {
        const string source = "client.Send(timeout: 30, cancellationToken: token);";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var parameter in new[] { "timeout", "cancellationToken" }) {
            Assert.Contains(result.Spans, span =>
                span.Kind == StorySyntaxKind.Parameter &&
                Slice(result, span) == parameter);
            Assert.DoesNotContain(result.Spans, span =>
                span.Kind == StorySyntaxKind.Variable &&
                Slice(result, span) == parameter);
        }
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == "Send");
    }

    [Fact]
    public void CSharpRecognizesFormalParameterNamesAsParameters() {
        const string source = "class Demo { void Send(int timeout, CancellationToken cancellationToken) { } }";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var parameter in new[] { "timeout", "cancellationToken" }) {
            Assert.Contains(result.Spans, span =>
                span.Kind == StorySyntaxKind.Parameter &&
                Slice(result, span) == parameter);
            Assert.DoesNotContain(result.Spans, span =>
                span.Kind == StorySyntaxKind.Variable &&
                Slice(result, span) == parameter);
        }
    }

    [Fact]
    public void CSharpRecognizesSingleIdentifierLambdaParameters() {
        const string source = "var names = items.Select(item => item.Name);";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);
        var declarationStart = source.IndexOf("item =>", StringComparison.Ordinal);

        Assert.True(result.Spans.Any(span =>
            span.Kind == StorySyntaxKind.Parameter &&
            span.Start == declarationStart &&
            Slice(result, span) == "item"), DescribeSpans(result));
        Assert.Contains(result.Spans, span =>
            span.Kind == StorySyntaxKind.Variable &&
            span.Start > declarationStart &&
            Slice(result, span) == "item");
    }

    [Fact]
    public void CSharpPreservesMemberReceiverRolesAndRecognizesAttributes() {
        const string source = "[Obsolete] class Demo { void Run() { obj.Value.ToString(); Console.WriteLine(obj.Value); } }";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Type && Slice(result, span) == "Obsolete");
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Type && Slice(result, span) == "Console");
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Variable && Slice(result, span) == "obj");
        Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Property && Slice(result, span) == "Value");
        Assert.DoesNotContain(result.Spans, span => span.Kind == StorySyntaxKind.Property && Slice(result, span) == "obj");
    }

    [Fact]
    public void CSharpLeavesNamespaceAndImportNamesPlain() {
        const string source = "namespace Company.Product; using System.Text; using Alias = Third.Party; class Demo { System.Text.StringBuilder field; }";
        var result = TreeSitterStorySourceTokenizer.Create("csharp").Tokenize(source);

        foreach (var name in new[] { "Company", "Product", "System", "Text", "Alias", "Third", "Party" }) {
            Assert.DoesNotContain(result.Spans, span =>
                span.Kind == StorySyntaxKind.Variable &&
                Slice(result, span) == name);
        }
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
        const string source = "echo done + % ^\nprintf '%s' if\nif true; then echo ready; fi";
        var result = TreeSitterStorySourceTokenizer.Create("bash").Tokenize(source);

        Assert.DoesNotContain(result.Spans, span =>
            span.Kind == StorySyntaxKind.Variable &&
            Slice(result, span) == "ready");
        Assert.Equal(
            0,
            result.Spans.Count(span =>
                span.Kind == StorySyntaxKind.Keyword &&
                (Slice(result, span) == "done" || Slice(result, span) == "if") &&
                span.Start < source.IndexOf("\nif true", StringComparison.Ordinal)));
        foreach (var keyword in new[] { "if", "then", "fi" }) {
            Assert.Contains(result.Spans, span =>
                span.Kind == StorySyntaxKind.Keyword &&
                Slice(result, span) == keyword &&
                span.Start >= source.IndexOf("\nif true", StringComparison.Ordinal));
        }
        foreach (var literal in new[] { "+", "%", "^" }) {
            Assert.DoesNotContain(result.Spans, span =>
                span.Kind == StorySyntaxKind.Operator &&
                Slice(result, span) == literal &&
                span.Start < source.IndexOf("\nprintf", StringComparison.Ordinal));
        }
    }

    [Fact]
    public void BashRecognizesRedirectionOperators() {
        const string source = "run 2>&1\nrun &>out\nrun &>>out\nrun >|out\nrun 3<&0\ncat <<< \"$value\"\ncat <<-EOF\nready\nEOF";
        var result = TreeSitterStorySourceTokenizer.Create("bash").Tokenize(source);

        foreach (var operation in new[] { ">&", "&>", "&>>", ">|", "<&", "<<<", "<<-" }) {
            Assert.Contains(result.Spans, span =>
                span.Kind == StorySyntaxKind.Operator &&
                Slice(result, span) == operation);
        }
    }

    [Fact]
    public void BashRecognizesFunctionNamesAndCompoundOperators() {
        const string source = "deploy() { build |& tee log; }\nfunction publish { true; }\ncase $mode in ready) ship ;;& *) stop ;; esac\ncase $mode in ready) ship ;& *) stop ;; esac";
        var result = TreeSitterStorySourceTokenizer.Create("bash").Tokenize(source);

        foreach (var command in new[] { "deploy", "publish" }) {
            Assert.True(
                result.Spans.Any(span => span.Kind == StorySyntaxKind.Command && Slice(result, span) == command),
                "Expected Bash function declaration name '" + command + "' to be a command. Spans: " +
                string.Join(", ", result.Spans.Select(span => span.Kind + ":" + Slice(result, span))));
        }
        foreach (var operation in new[] { "|&", ";;", ";&", ";;&" }) {
            Assert.Contains(result.Spans, span => span.Kind == StorySyntaxKind.Operator && Slice(result, span) == operation);
        }
    }

    [Fact]
    public void BashPreservesNestedSyntaxInsideCompoundExpansions() {
        const string source = "printf '%s' \"${value:-$(compute --fallback)}\"";
        var result = TreeSitterStorySourceTokenizer.Create("bash").Tokenize(source);

        Assert.True(result.Spans.Any(span =>
            span.Kind == StorySyntaxKind.Variable &&
            Slice(result, span).Contains("${value:-$(", StringComparison.Ordinal)), DescribeSpans(result));
        Assert.True(result.Spans.Any(span =>
            span.Kind == StorySyntaxKind.Command &&
            Slice(result, span) == "compute"), DescribeSpans(result));
    }

    [Fact]
    public void BashRecognizesTestOperatorsWithoutColoringCommandFlags() {
        const string source = "if [[ $count -eq 1 && $count -ne 2 && $count -lt 3 && $file -nt $other && -f $file && -n $name && $name =~ ^a && $(rm -f nested.txt) ]]; then rm -f output.txt; fi";
        var result = TreeSitterStorySourceTokenizer.Create("bash").Tokenize(source);

        foreach (var operation in new[] { "-eq", "-ne", "-lt", "-nt", "-f", "-n", "=~" }) {
            Assert.Contains(result.Spans, span =>
                span.Kind == StorySyntaxKind.Operator &&
                Slice(result, span) == operation &&
                span.Start < source.IndexOf("rm -f", StringComparison.Ordinal));
        }
        Assert.DoesNotContain(result.Spans, span =>
            span.Kind == StorySyntaxKind.Operator &&
            Slice(result, span) == "-f" &&
            (span.Start == source.IndexOf("rm -f nested", StringComparison.Ordinal) + 3 ||
             span.Start == source.LastIndexOf("rm -f", StringComparison.Ordinal) + 3));
    }

    [Fact]
    public void UnsupportedLanguagesAreExplicit() {
        Assert.Throws<NotSupportedException>(() => TreeSitterStorySourceTokenizer.Create("powershell"));
    }

    private static string Slice(StorySourceText source, StorySourceSpan span) =>
        source.Text.Substring(span.Start, span.Length);

    private static string DescribeSpans(StorySourceText source) =>
        string.Join(", ", source.Spans.Select(span => $"{span.Kind}:{span.Start}:{Slice(source, span)}"));
}
