using System;
using System.Collections.Generic;
using System.Text;
using ChartForgeX.Stories;
using TreeSitter;

namespace ImagePlayground.Syntax.TreeSitter;

public sealed partial class TreeSitterStorySourceTokenizer {
    private static List<SemanticRange> SplitDynamicBashCommandRanges(
        string source,
        IReadOnlyList<SemanticRange> ranges) {
        var bytes = Encoding.UTF8.GetBytes(source);
        var output = new List<SemanticRange>(ranges.Count);
        foreach (var range in ranges) {
            if (range.Kind != StorySyntaxKind.Command) {
                output.Add(range);
                continue;
            }
            var cursor = range.StartByte;
            var index = range.StartByte;
            while (index < range.EndByte) {
                if (bytes[index] != (byte)'$' || index + 1 >= range.EndByte) {
                    index++;
                    continue;
                }
                var end = index + 1;
                if (bytes[end] == (byte)'{') {
                    end++;
                    while (end < range.EndByte && bytes[end] != (byte)'}') end++;
                    if (end >= range.EndByte) {
                        index++;
                        continue;
                    }
                    end++;
                } else if (IsBashVariableStart(bytes[end])) {
                    end++;
                    while (end < range.EndByte && IsBashVariablePart(bytes[end])) end++;
                } else if ("?#@*!-$0123456789".IndexOf((char)bytes[end]) >= 0) {
                    end++;
                } else {
                    index++;
                    continue;
                }
                Add(output, cursor, index, StorySyntaxKind.Command);
                Add(output, index, end, StorySyntaxKind.Variable);
                cursor = end;
                index = end;
            }
            Add(output, cursor, range.EndByte, StorySyntaxKind.Command);
        }
        return output;
    }

    private static bool IsBashVariableStart(byte value) =>
        value == (byte)'_' ||
        value is >= (byte)'A' and <= (byte)'Z' ||
        value is >= (byte)'a' and <= (byte)'z';

    private static bool IsBashVariablePart(byte value) =>
        IsBashVariableStart(value) || value is >= (byte)'0' and <= (byte)'9';

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
        foreach (var child in node.Children) CollectBashCommandNameOverrides(child, overrides);
        if (overrides.Count == 0) CollectBashCommandNameTextVariables(node, overrides);
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

    private void CollectBashCommandNameOverrides(Node node, List<SemanticRange> output) {
        if (Contains(node.Type, "expansion")) {
            CollectBashExpansion(node, output);
            return;
        }
        foreach (var child in node.Children) CollectBashCommandNameOverrides(child, output);
    }

    private static void CollectBashCommandNameTextVariables(Node node, List<SemanticRange> output) {
        var text = node.Text;
        for (var index = 0; index < text.Length; index++) {
            if (text[index] != '$' || index + 1 >= text.Length) continue;
            var end = index + 1;
            if (text[end] == '{') {
                var closing = text.IndexOf('}', end + 1);
                if (closing < 0) continue;
                end = closing + 1;
            } else if (char.IsLetter(text[end]) || text[end] == '_') {
                end++;
                while (end < text.Length && (char.IsLetterOrDigit(text[end]) || text[end] == '_')) end++;
            } else if ("?#@*!-$0123456789".IndexOf(text[end]) >= 0) {
                end++;
            } else {
                continue;
            }
            var startByte = node.StartIndex + Encoding.UTF8.GetByteCount(text.Substring(0, index));
            var endByte = node.StartIndex + Encoding.UTF8.GetByteCount(text.Substring(0, end));
            Add(output, startByte, endByte, StorySyntaxKind.Variable);
            index = end - 1;
        }
    }

    private void CollectBashExpansionOverrides(Node node, List<SemanticRange> output) {
        if (Contains(node.Type, "command_substitution")) {
            CollectBashCommandSubstitution(node, output);
            return;
        }
        if (Contains(node.Type, "command_name")) {
            CollectBashCommandName(node, output);
            return;
        }
        var kind = ContainerKind(node);
        if (kind == StorySyntaxKind.Command) {
            CollectBashCommandName(node, output);
            return;
        }
        if (kind == StorySyntaxKind.String) {
            CollectString(node, output);
            return;
        }
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

    private void CollectBashCommandSubstitution(Node node, List<SemanticRange> output) {
        var overrides = new List<SemanticRange>();
        foreach (var child in node.Children) CollectBashCommandSubstitutionOverrides(child, overrides);
        overrides.Sort((left, right) => left.StartByte.CompareTo(right.StartByte));

        var cursor = node.StartIndex;
        var hasOverride = false;
        foreach (var range in overrides) {
            if (range.StartByte < cursor || range.EndByte > node.EndIndex || range.EndByte <= range.StartByte) continue;
            if (hasOverride) Add(output, cursor, range.StartByte, StorySyntaxKind.Plain);
            Add(output, range.StartByte, range.EndByte, range.Kind);
            cursor = range.EndByte;
            hasOverride = true;
        }
        if (hasOverride) Add(output, cursor, node.EndIndex, StorySyntaxKind.Plain);
    }

    private void CollectBashCommandSubstitutionOverrides(Node node, List<SemanticRange> output) {
        if (Contains(node.Type, "command_name")) {
            CollectBashCommandName(node, output);
            return;
        }
        if (Contains(node.Type, "expansion")) {
            CollectBashExpansion(node, output);
            return;
        }

        var kind = ContainerKind(node);
        if (kind == StorySyntaxKind.Command) {
            CollectBashCommandName(node, output);
            return;
        }
        if (kind == StorySyntaxKind.String) {
            CollectString(node, output);
            return;
        }
        if (kind != StorySyntaxKind.Plain) {
            Add(output, node, kind);
            return;
        }
        if (node.Children.Count > 0) {
            foreach (var child in node.Children) CollectBashCommandSubstitutionOverrides(child, output);
            return;
        }

        if (node.Text == "$(") return;
        Add(output, node, LeafKind(node));
    }

    private bool IsBashStructuralOperator(Node node) {
        if (Language != "bash") {
            return false;
        }
        var current = node;
        var insideArithmeticExpansion = false;
        while (current != null) {
            if (Contains(current.Type, "arithmetic_expansion") ||
                Contains(current.Type, "arithmetic_expression")) {
                insideArithmeticExpansion = true;
            }
            if (string.Equals(current.Type, "word", StringComparison.Ordinal)) {
                return insideArithmeticExpansion;
            }
            if (string.Equals(current.Type, "command", StringComparison.Ordinal)) {
                break;
            }
            current = current.Parent;
        }
        return true;
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

}
