using System;
using System.Collections.Generic;
using System.Text;
using ChartForgeX.Stories;
using TreeSitter;

namespace ImagePlayground.Syntax.TreeSitter;

/// <summary>
/// Uses an optional native Tree-sitter grammar to map C# or Bash syntax into renderer-neutral story spans.
/// </summary>
public sealed class TreeSitterStorySourceTokenizer : IStorySourceTokenizer {
    private readonly string _grammar;

    private TreeSitterStorySourceTokenizer(string language, string grammar) {
        Language = language;
        _grammar = grammar;
    }

    /// <inheritdoc />
    public string Language { get; }

    /// <summary>Creates an AST-backed C# or Bash story tokenizer.</summary>
    public static TreeSitterStorySourceTokenizer Create(string language) {
        if (string.IsNullOrWhiteSpace(language)) throw new ArgumentException("A language identifier is required.", nameof(language));
        switch (language.Trim().ToLowerInvariant()) {
            case "c#":
            case "cs":
            case "csharp":
                return new TreeSitterStorySourceTokenizer("csharp", "C-Sharp");
            case "bash":
            case "sh":
            case "shell":
                return new TreeSitterStorySourceTokenizer("bash", "Bash");
            default:
                throw new NotSupportedException("The optional Tree-sitter story adapter currently supports C# and Bash.");
        }
    }

    /// <inheritdoc />
    public StorySourceText Tokenize(string source) {
        if (source == null) throw new ArgumentNullException(nameof(source));
        using var language = new Language(_grammar);
        using var parser = new Parser(language);
        using var tree = parser.Parse(source) ?? throw new InvalidOperationException("Tree-sitter did not return a syntax tree.");
        var ranges = new List<SemanticRange>();
        Collect(tree.RootNode, ranges);
        ranges.Sort((left, right) => left.StartByte.CompareTo(right.StartByte));
        var sourceText = StorySourceText.Create(source, Language);
        var utf8 = Encoding.UTF8.GetBytes(source);
        var indexesAreUtf16 = tree.RootNode.EndIndex == source.Length;
        var previousEnd = 0;
        foreach (var range in ranges) {
            var start = indexesAreUtf16 ? range.StartByte : Utf16Offset(utf8, range.StartByte);
            var end = indexesAreUtf16 ? range.EndByte : Utf16Offset(utf8, range.EndByte);
            if (start < previousEnd || end <= start || end > source.Length) continue;
            sourceText.AddSpan(start, end - start, range.Kind);
            previousEnd = end;
        }
        return sourceText;
    }

    private void Collect(Node node, List<SemanticRange> output) {
        var kind = ContainerKind(node);
        if (kind == StorySyntaxKind.String) {
            CollectString(node, output);
            return;
        }
        if (kind != StorySyntaxKind.Plain) {
            Add(output, node, kind);
            return;
        }
        if (node.Children.Count > 0) {
            foreach (var child in node.Children) Collect(child, output);
            return;
        }
        kind = LeafKind(node);
        if (kind != StorySyntaxKind.Plain) Add(output, node, kind);
    }

    private void CollectString(Node node, List<SemanticRange> output) {
        var overrides = new List<SemanticRange>();
        foreach (var child in node.Children) CollectStringOverrides(child, overrides);
        overrides.Sort((left, right) => left.StartByte.CompareTo(right.StartByte));

        var cursor = node.StartIndex;
        foreach (var range in overrides) {
            if (range.StartByte < cursor || range.EndByte > node.EndIndex || range.EndByte <= range.StartByte) continue;
            Add(output, cursor, range.StartByte, StorySyntaxKind.String);
            Add(output, range.StartByte, range.EndByte, range.Kind);
            cursor = range.EndByte;
        }
        Add(output, cursor, node.EndIndex, StorySyntaxKind.String);
    }

    private void CollectStringOverrides(Node node, List<SemanticRange> output) {
        var kind = ContainerKind(node);
        if (kind != StorySyntaxKind.Plain && kind != StorySyntaxKind.String) {
            Add(output, node, kind);
            return;
        }
        if (node.Children.Count > 0) {
            foreach (var child in node.Children) CollectStringOverrides(child, output);
            return;
        }
        kind = LeafKind(node);
        if (kind != StorySyntaxKind.Plain && kind != StorySyntaxKind.String) Add(output, node, kind);
    }

    private StorySyntaxKind ContainerKind(Node node) {
        var type = node.Type;
        if (Contains(type, "comment")) return StorySyntaxKind.Comment;
        if (Contains(type, "string") || Contains(type, "character_literal") || Contains(type, "heredoc")) return StorySyntaxKind.String;
        if (Contains(type, "integer_literal") || Contains(type, "real_literal") || Contains(type, "number")) return StorySyntaxKind.Number;
        if (Contains(type, "type_identifier") || Contains(type, "predefined_type")) return StorySyntaxKind.Type;
        if (Language == "bash") {
            if (Contains(type, "command_name")) return StorySyntaxKind.Command;
            if (Contains(type, "expansion") || Contains(type, "variable_name")) return StorySyntaxKind.Variable;
        }
        return StorySyntaxKind.Plain;
    }

    private StorySyntaxKind LeafKind(Node node) {
        var text = node.Text;
        if (IsKeyword(text)) return StorySyntaxKind.Keyword;
        if (IsOperator(text)) return StorySyntaxKind.Operator;
        if (IsPunctuation(text)) return StorySyntaxKind.Punctuation;
        if (node.Type == "identifier") {
            var parentType = node.Parent?.Type ?? string.Empty;
            if (Contains(parentType, "member_access") || Contains(parentType, "member_binding")) return StorySyntaxKind.Property;
            if (Contains(parentType, "invocation")) return StorySyntaxKind.Command;
            return StorySyntaxKind.Variable;
        }
        if (Language == "bash" && node.Type == "variable_name") return StorySyntaxKind.Variable;
        return StorySyntaxKind.Plain;
    }

    private bool IsKeyword(string value) {
        if (Language == "csharp") {
            switch (value) {
                case "abstract": case "as": case "base": case "bool": case "break": case "byte":
                case "case": case "catch": case "char": case "class": case "const": case "continue":
                case "decimal": case "default": case "delegate": case "do": case "double": case "else":
                case "enum": case "event": case "explicit": case "extern": case "false": case "finally":
                case "fixed": case "float": case "for": case "foreach": case "goto": case "if":
                case "implicit": case "in": case "int": case "interface": case "internal": case "is":
                case "lock": case "long": case "namespace": case "new": case "null": case "object":
                case "operator": case "out": case "override": case "params": case "private": case "protected":
                case "public": case "readonly": case "ref": case "return": case "sbyte": case "sealed":
                case "short": case "sizeof": case "stackalloc": case "static": case "string": case "struct":
                case "switch": case "this": case "throw": case "true": case "try": case "typeof":
                case "uint": case "ulong": case "unchecked": case "unsafe": case "ushort": case "using":
                case "var": case "virtual": case "void": case "volatile": case "while":
                    return true;
            }
            return false;
        }
        switch (value) {
            case "if": case "then": case "elif": case "else": case "fi": case "for": case "while":
            case "until": case "do": case "done": case "case": case "esac": case "in": case "function":
            case "select": case "time": case "coproc":
                return true;
            default:
                return false;
        }
    }

    private static bool IsOperator(string value) {
        switch (value) {
            case "=": case "==": case "!=": case "=>": case "+": case "-": case "*": case "/":
            case "%": case "&&": case "||": case "!": case "<": case ">": case "<=": case ">=":
            case "|": case "&": case "^": case "??": case "?.": case "+=": case "-=": case "*=":
            case "/=": case "<<": case ">>":
                return true;
            default:
                return false;
        }
    }

    private static bool IsPunctuation(string value) {
        switch (value) {
            case "(": case ")": case "{": case "}": case "[": case "]": case ";": case ",":
            case ".": case ":": case "::":
                return true;
            default:
                return false;
        }
    }

    private static void Add(List<SemanticRange> output, Node node, StorySyntaxKind kind) {
        Add(output, node.StartIndex, node.EndIndex, kind);
    }

    private static void Add(List<SemanticRange> output, int startIndex, int endIndex, StorySyntaxKind kind) {
        if (endIndex <= startIndex) return;
        if (output.Count > 0) {
            var previous = output[output.Count - 1];
            if (previous.EndByte == startIndex && previous.Kind == kind) {
                output[output.Count - 1] = new SemanticRange(previous.StartByte, endIndex, kind);
                return;
            }
        }
        output.Add(new SemanticRange(startIndex, endIndex, kind));
    }

    private static int Utf16Offset(byte[] utf8, int byteOffset) {
        if (byteOffset < 0 || byteOffset > utf8.Length) throw new InvalidOperationException("Tree-sitter returned an invalid UTF-8 source offset.");
        return Encoding.UTF8.GetCharCount(utf8, 0, byteOffset);
    }

    private static bool Contains(string value, string fragment) =>
        value.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) >= 0;

    private readonly struct SemanticRange {
        public SemanticRange(int startByte, int endByte, StorySyntaxKind kind) {
            StartByte = startByte;
            EndByte = endByte;
            Kind = kind;
        }
        public int StartByte { get; }
        public int EndByte { get; }
        public StorySyntaxKind Kind { get; }
    }
}
