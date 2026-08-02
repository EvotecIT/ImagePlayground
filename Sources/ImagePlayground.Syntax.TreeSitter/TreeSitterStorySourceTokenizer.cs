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
        var indexesAreUtf16 = tree.RootNode.EndIndex == source.Length;
        var utf16Offsets = indexesAreUtf16 ? null : BuildUtf16OffsetMap(source);
        var previousEnd = 0;
        foreach (var range in ranges) {
            var start = indexesAreUtf16 ? range.StartByte : Utf16Offset(utf16Offsets!, range.StartByte);
            var end = indexesAreUtf16 ? range.EndByte : Utf16Offset(utf16Offsets!, range.EndByte);
            if (start < previousEnd || end <= start || end > source.Length) continue;
            sourceText.AddSpan(start, end - start, range.Kind);
            previousEnd = end;
        }
        return sourceText;
    }

    private void Collect(Node node, List<SemanticRange> output) {
        if (Language == "bash" && string.Equals(node.Type, "function_definition", StringComparison.Ordinal)) {
            foreach (var field in node.Fields) {
                if (string.Equals(field.Key, "name", StringComparison.Ordinal)) {
                    Add(output, field.Value, StorySyntaxKind.Command);
                    break;
                }
            }
        }
        if (Language == "bash" && Contains(node.Type, "expansion")) {
            CollectBashExpansion(node, output);
            return;
        }
        if (Language == "bash" && Contains(node.Type, "command_name") && node.Children.Count > 0) {
            CollectBashCommandName(node, output);
            return;
        }
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

    private void CollectBashExpansion(Node node, List<SemanticRange> output) {
        var overrides = new List<SemanticRange>();
        foreach (var child in node.Children) CollectBashExpansionOverrides(child, overrides);
        overrides.Sort((left, right) => left.StartByte.CompareTo(right.StartByte));

        var cursor = node.StartIndex;
        foreach (var range in overrides) {
            if (range.StartByte < cursor || range.EndByte > node.EndIndex || range.EndByte <= range.StartByte) continue;
            Add(output, cursor, range.StartByte, StorySyntaxKind.Variable);
            Add(output, range.StartByte, range.EndByte, range.Kind);
            cursor = range.EndByte;
        }
        Add(output, cursor, node.EndIndex, StorySyntaxKind.Variable);
    }

    private void CollectBashCommandName(Node node, List<SemanticRange> output) {
        var overrides = new List<SemanticRange>();
        foreach (var child in node.Children) Collect(child, overrides);
        overrides.Sort((left, right) => left.StartByte.CompareTo(right.StartByte));

        var cursor = node.StartIndex;
        foreach (var range in overrides) {
            if (range.StartByte < cursor || range.EndByte > node.EndIndex || range.EndByte <= range.StartByte) continue;
            Add(output, cursor, range.StartByte, StorySyntaxKind.Command);
            Add(output, range.StartByte, range.EndByte, range.Kind);
            cursor = range.EndByte;
        }
        Add(output, cursor, node.EndIndex, StorySyntaxKind.Command);
    }

    private void CollectBashExpansionOverrides(Node node, List<SemanticRange> output) {
        var kind = ContainerKind(node);
        if (kind != StorySyntaxKind.Plain && kind != StorySyntaxKind.Variable) {
            Add(output, node, kind);
            return;
        }
        if (node.Children.Count > 0) {
            foreach (var child in node.Children) CollectBashExpansionOverrides(child, output);
            return;
        }
        kind = LeafKind(node);
        if (kind != StorySyntaxKind.Plain && kind != StorySyntaxKind.Variable) Add(output, node, kind);
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
        if (Language == "bash" && Contains(node.Type, "expansion")) {
            CollectBashExpansion(node, output);
            return;
        }
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
        if (Language == "csharp" && string.Equals(type, "implicit_parameter", StringComparison.Ordinal)) return StorySyntaxKind.Parameter;
        if (Language == "bash") {
            if (Contains(type, "command_name")) return StorySyntaxKind.Command;
            if (Contains(type, "expansion") || Contains(type, "variable_name")) return StorySyntaxKind.Variable;
        }
        return StorySyntaxKind.Plain;
    }

    private StorySyntaxKind LeafKind(Node node) {
        var text = node.Text;
        if (IsPreprocessorDirective(node)) return StorySyntaxKind.Keyword;
        if (IsReservedKeyword(node) || IsContextualKeyword(node)) return StorySyntaxKind.Keyword;
        if (IsBashTestOperator(node)) return StorySyntaxKind.Operator;
        if ((Language == "csharp" || IsBashStructuralOperator(node)) && IsOperator(text)) return StorySyntaxKind.Operator;
        if (IsPunctuation(text)) return StorySyntaxKind.Punctuation;
        if (node.Type == "identifier") {
            var parentType = node.Parent?.Type ?? string.Empty;
            if (IsCSharpNamespaceName(node)) return StorySyntaxKind.Plain;
            if (IsCSharpAttributeName(node)) return StorySyntaxKind.Type;
            if (IsCSharpTypeParameterDeclaration(node)) return StorySyntaxKind.Type;
            if (IsCSharpTypeParameterConstraint(node)) return StorySyntaxKind.Type;
            if (IsCSharpTypeReference(node)) return StorySyntaxKind.Type;
            if (IsCSharpMemberReceiverType(node)) return StorySyntaxKind.Type;
            if (IsCSharpFormalParameter(node)) return StorySyntaxKind.Parameter;
            if (IsCSharpNamedArgument(node)) return StorySyntaxKind.Parameter;
            if (IsDeclaredTypeName(parentType)) return StorySyntaxKind.Type;
            if (IsCSharpDeclarationName(node, "method_declaration") ||
                IsCSharpDeclarationName(node, "local_function_statement") ||
                IsCSharpDeclarationName(node, "constructor_declaration") ||
                IsCSharpDeclarationName(node, "destructor_declaration")) return StorySyntaxKind.Command;
            if (IsCSharpDeclarationName(node, "property_declaration") ||
                IsCSharpDeclarationName(node, "event_declaration") ||
                IsCSharpEventFieldName(node) ||
                IsCSharpDeclarationName(node, "enum_member_declaration") ||
                IsCSharpInitializerMemberName(node)) return StorySyntaxKind.Property;
            if (IsInvokedMember(node)) return StorySyntaxKind.Command;
            if (IsCSharpMemberName(node)) return StorySyntaxKind.Property;
            if (Contains(parentType, "invocation")) return StorySyntaxKind.Command;
            return StorySyntaxKind.Variable;
        }
        if (Language == "bash" && node.Type == "variable_name") return StorySyntaxKind.Variable;
        return StorySyntaxKind.Plain;
    }

    private bool IsReservedKeyword(Node node) {
        var value = node.Text;
        if (Language == "csharp") {
            switch (value) {
                case "abstract": case "as": case "base": case "bool": case "break": case "byte":
                case "case": case "catch": case "char": case "checked": case "class": case "const": case "continue":
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
                case "virtual": case "void": case "volatile": case "while":
                    return true;
            }
            return false;
        }
        if (!string.Equals(node.Type, value, StringComparison.Ordinal)) return false;
        switch (value) {
            case "if": case "then": case "elif": case "else": case "fi": case "for": case "while":
            case "until": case "do": case "done": case "case": case "esac": case "in": case "function":
            case "select": case "time": case "coproc":
                return true;
            default:
                return false;
        }
    }

    private bool IsContextualKeyword(Node node) {
        if (Language != "csharp" || node.Type == "identifier") return false;
        switch (node.Text) {
            case "add": case "alias": case "and": case "ascending": case "args": case "async":
            case "await": case "by": case "descending": case "dynamic": case "equals": case "file":
            case "from": case "get": case "global": case "group": case "init": case "into":
            case "join": case "let": case "managed": case "nameof": case "not": case "notnull":
            case "nint": case "nuint": case "on": case "or": case "orderby": case "partial":
            case "record": case "remove": case "required": case "scoped": case "select": case "set":
            case "unmanaged": case "value": case "var": case "when": case "where": case "with":
            case "yield":
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
            case "/=": case "<<": case ">>": case ">>>": case "++": case "--": case "??=":
            case "%=": case "&=": case "|=": case "^=": case "<<=": case ">>=": case ">>>=":
            case "~": case "?": case "->": case "..": case ">&": case "&>": case "&>>": case ">|":
            case "<&": case "<<<": case "<<-": case "|&": case ";;": case ";&": case ";;&":
                return true;
            default:
                return false;
        }
    }

    private static bool IsDeclaredTypeName(string parentType) {
        switch (parentType) {
            case "class_declaration":
            case "struct_declaration":
            case "interface_declaration":
            case "enum_declaration":
            case "record_declaration":
            case "delegate_declaration":
                return true;
            default:
                return false;
        }
    }

    private bool IsCSharpTypeReference(Node node) {
        if (Language != "csharp") return false;
        if (IsCSharpTupleElementName(node)) return false;
        var current = node.Parent;
        while (current != null) {
            if (string.Equals(current.Type, "type_argument_list", StringComparison.Ordinal)) {
                return true;
            }
            foreach (var field in current.Fields) {
                if ((string.Equals(field.Key, "type", StringComparison.Ordinal) ||
                     string.Equals(field.Key, "return_type", StringComparison.Ordinal)) &&
                    field.Value.StartIndex <= node.StartIndex &&
                    field.Value.EndIndex >= node.EndIndex) {
                    return true;
                }
            }
            current = current.Parent;
        }
        return false;
    }

    private bool IsCSharpTypeParameterDeclaration(Node node) {
        var parent = node.Parent;
        return Language == "csharp" &&
               parent != null &&
               string.Equals(parent.Type, "type_parameter", StringComparison.Ordinal) &&
               IsInsideField(parent, node, "name");
    }

    private bool IsCSharpTypeParameterConstraint(Node node) {
        var parent = node.Parent;
        if (Language != "csharp" ||
            parent == null ||
            !string.Equals(parent.Type, "type_parameter_constraints_clause", StringComparison.Ordinal)) {
            return false;
        }
        foreach (var child in parent.Children) {
            if (!string.Equals(child.Type, "identifier", StringComparison.Ordinal)) continue;
            return child.StartIndex == node.StartIndex && child.EndIndex == node.EndIndex;
        }
        return false;
    }

    private bool IsCSharpInitializerMemberName(Node node) {
        if (Language != "csharp") return false;
        var assignment = node.Parent;
        if (assignment == null ||
            !string.Equals(assignment.Type, "assignment_expression", StringComparison.Ordinal) ||
            !IsInsideField(assignment, node, "left")) {
            return false;
        }

        var current = assignment.Parent;
        while (current != null) {
            if (string.Equals(current.Type, "initializer_expression", StringComparison.Ordinal)) return true;
            if (Contains(current.Type, "statement") || Contains(current.Type, "declaration")) return false;
            current = current.Parent;
        }
        return false;
    }

    private bool IsCSharpEventFieldName(Node node) {
        var parent = node.Parent;
        if (Language != "csharp" ||
            parent == null ||
            !string.Equals(parent.Type, "variable_declarator", StringComparison.Ordinal) ||
            !IsInsideField(parent, node, "name")) {
            return false;
        }

        var current = parent.Parent;
        while (current != null) {
            if (string.Equals(current.Type, "event_field_declaration", StringComparison.Ordinal)) {
                return true;
            }
            if (!string.Equals(current.Type, "variable_declaration", StringComparison.Ordinal) &&
                Contains(current.Type, "declaration")) {
                return false;
            }
            current = current.Parent;
        }
        return false;
    }

    private bool IsCSharpTupleElementName(Node node) {
        var parent = node.Parent;
        return Language == "csharp" &&
               parent != null &&
               string.Equals(parent.Type, "tuple_element", StringComparison.Ordinal) &&
               IsInsideField(parent, node, "name");
    }

    private bool IsCSharpAttributeName(Node node) {
        if (Language != "csharp") return false;
        var current = node.Parent;
        while (current != null) {
            if (Contains(current.Type, "attribute")) {
                return IsInsideField(current, node, "name");
            }
            if (Contains(current.Type, "declaration")) return false;
            current = current.Parent;
        }
        return false;
    }

    private bool IsCSharpMemberName(Node node) {
        if (Language != "csharp") return false;
        var parent = node.Parent;
        if (parent == null ||
            (!Contains(parent.Type, "member_access") && !Contains(parent.Type, "member_binding"))) {
            return false;
        }
        return IsInsideField(parent, node, "name");
    }

    private bool IsCSharpNamedArgument(Node node) {
        var parent = node.Parent;
        return Language == "csharp" &&
               parent != null &&
               string.Equals(parent.Type, "argument", StringComparison.Ordinal) &&
               IsInsideField(parent, node, "name");
    }

    private bool IsCSharpFormalParameter(Node node) {
        var parent = node.Parent;
        if (Language != "csharp" || parent == null) return false;
        if (string.Equals(parent.Type, "parameter", StringComparison.Ordinal) &&
            IsInsideField(parent, node, "name")) {
            return true;
        }
        if (!string.Equals(parent.Type, "lambda_expression", StringComparison.Ordinal)) return false;
        foreach (var field in parent.Fields) {
            if (string.Equals(field.Key, "parameters", StringComparison.Ordinal) &&
                field.Value.StartIndex == node.StartIndex &&
                field.Value.EndIndex == node.EndIndex) {
                return true;
            }
        }
        return false;
    }

    private bool IsCSharpMemberReceiverType(Node node) {
        if (Language != "csharp" || node.Text.Length == 0 || !char.IsUpper(node.Text[0])) return false;
        var current = node.Parent;
        while (current != null) {
            if (Contains(current.Type, "member_access") && IsInsideField(current, node, "expression")) {
                return true;
            }
            if (!Contains(current.Type, "member_access") && !Contains(current.Type, "member_binding")) break;
            current = current.Parent;
        }
        return false;
    }

    private bool IsCSharpNamespaceName(Node node) {
        if (Language != "csharp") return false;
        var current = node.Parent;
        while (current != null) {
            if (string.Equals(current.Type, "using_directive", StringComparison.Ordinal)) return true;
            if (Contains(current.Type, "namespace_declaration")) {
                foreach (var field in current.Fields) {
                    if (string.Equals(field.Key, "name", StringComparison.Ordinal) &&
                        field.Value.StartIndex <= node.StartIndex &&
                        field.Value.EndIndex >= node.EndIndex) {
                        return true;
                    }
                }
                return false;
            }
            current = current.Parent;
        }
        return false;
    }

    private bool IsCSharpDeclarationName(Node node, string declarationType) {
        if (Language != "csharp") return false;
        var current = node.Parent;
        while (current != null) {
            if (string.Equals(current.Type, declarationType, StringComparison.Ordinal)) {
                foreach (var field in current.Fields) {
                    if (string.Equals(field.Key, "name", StringComparison.Ordinal) &&
                        field.Value.StartIndex <= node.StartIndex &&
                        field.Value.EndIndex >= node.EndIndex) {
                        return true;
                    }
                }
                return false;
            }
            if (Contains(current.Type, "declaration")) {
                return false;
            }
            current = current.Parent;
        }
        return false;
    }

    private bool IsBashStructuralOperator(Node node) {
        if (Language != "bash") {
            return false;
        }
        var current = node;
        while (current != null) {
            if (string.Equals(current.Type, "word", StringComparison.Ordinal)) {
                return false;
            }
            if (string.Equals(current.Type, "command", StringComparison.Ordinal)) {
                break;
            }
            current = current.Parent;
        }
        return true;
    }

    private static bool IsInsideField(Node owner, Node candidate, string fieldName) {
        foreach (var field in owner.Fields) {
            if (string.Equals(field.Key, fieldName, StringComparison.Ordinal) &&
                field.Value.StartIndex <= candidate.StartIndex &&
                field.Value.EndIndex >= candidate.EndIndex) {
                return true;
            }
        }
        return false;
    }

    private bool IsBashTestOperator(Node node) {
        if (Language != "bash" || !IsBashTestOperatorText(node.Text)) return false;
        var current = node.Parent;
        while (current != null) {
            if (Contains(current.Type, "test") ||
                Contains(current.Type, "binary_expression") ||
                Contains(current.Type, "unary_expression")) {
                return true;
            }
            if (string.Equals(current.Type, "command", StringComparison.Ordinal) ||
                Contains(current.Type, "command_substitution")) {
                return false;
            }
            current = current.Parent;
        }
        return false;
    }

    private static bool IsBashTestOperatorText(string value) {
        switch (value) {
            case "=~":
            case "-eq": case "-ne": case "-lt": case "-le": case "-gt": case "-ge":
            case "-nt": case "-ot": case "-ef":
            case "-a": case "-b": case "-c": case "-d": case "-e": case "-f": case "-g":
            case "-G": case "-h": case "-k": case "-L": case "-N": case "-O": case "-p":
            case "-r": case "-R": case "-S": case "-s": case "-t": case "-u": case "-v":
            case "-w": case "-x": case "-n": case "-z": case "-o":
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

    private static int[] BuildUtf16OffsetMap(string source) {
        var byteLength = Encoding.UTF8.GetByteCount(source);
        var offsets = new int[byteLength + 1];
        var byteIndex = 0;
        var charIndex = 0;
        while (charIndex < source.Length) {
            var value = source[charIndex];
            var charCount = char.IsHighSurrogate(value) &&
                            charIndex + 1 < source.Length &&
                            char.IsLowSurrogate(source[charIndex + 1])
                ? 2
                : 1;
            int encodedBytes;
            if (charCount == 2) {
                encodedBytes = 4;
            } else if (value <= 0x7F) {
                encodedBytes = 1;
            } else if (value <= 0x7FF) {
                encodedBytes = 2;
            } else {
                encodedBytes = 3;
            }
            for (var index = 0; index < encodedBytes; index++) offsets[byteIndex + index] = charIndex;
            byteIndex += encodedBytes;
            charIndex += charCount;
            offsets[byteIndex] = charIndex;
        }
        if (byteIndex != byteLength) throw new InvalidOperationException("Unable to map the source's UTF-8 offsets.");
        return offsets;
    }

    private static int Utf16Offset(int[] offsets, int byteOffset) {
        if (byteOffset < 0 || byteOffset >= offsets.Length) throw new InvalidOperationException("Tree-sitter returned an invalid UTF-8 source offset.");
        return offsets[byteOffset];
    }

    private static bool Contains(string value, string fragment) =>
        value.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) >= 0;

    private static bool IsInvokedMember(Node node) {
        var target = node;
        if (Contains(node.Parent?.Type ?? string.Empty, "generic_name")) target = node.Parent!;
        var parent = target.Parent;
        if (Contains(parent?.Type ?? string.Empty, "invocation")) return true;
        if (parent == null ||
            (!Contains(parent.Type, "member_access") && !Contains(parent.Type, "member_binding")) ||
            !Contains(parent.Parent?.Type ?? string.Empty, "invocation")) {
            return false;
        }

        foreach (var sibling in parent.Children) {
            if ((sibling.Type == "identifier" || Contains(sibling.Type, "generic_name")) &&
                sibling.StartIndex > target.StartIndex) {
                return false;
            }
        }
        return true;
    }

    private bool IsPreprocessorDirective(Node node) {
        if (Language != "csharp" || !node.Text.StartsWith("#", StringComparison.Ordinal)) return false;
        var current = node.Parent;
        while (current != null) {
            if (Contains(current.Type, "preproc")) return true;
            current = current.Parent;
        }
        return false;
    }

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
