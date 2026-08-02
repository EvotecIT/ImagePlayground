using System;
using System.Collections.Generic;
using System.Management.Automation.Language;
using ChartForgeX.Stories;

namespace ImagePlayground.PowerShell.Stories;

/// <summary>Maps the PowerShell parser's native tokens into renderer-neutral ChartForgeX story spans.</summary>
public sealed class PowerShellStorySourceTokenizer : IStorySourceTokenizer {
    /// <inheritdoc />
    public string Language => "powershell";

    /// <inheritdoc />
    public StorySourceText Tokenize(string source) {
        if (source == null) throw new ArgumentNullException(nameof(source));
        var result = StorySourceText.Create(source, Language);
        var ast = Parser.ParseInput(source, out var tokens, out _);
        var declaredCommandNames = DeclaredCommandNames(ast, tokens);
        var declaredTypeNames = DeclaredTypeNames(ast, tokens);
        var declaredPropertyNames = DeclaredPropertyNames(ast, tokens);
        var declaredParameterNames = DeclaredParameterNames(ast);
        var semanticOperators = SemanticOperators(ast, tokens);
        var ranges = new List<SemanticRange>();
        foreach (var token in tokens) {
            Collect(token, ranges, declaredCommandNames, declaredTypeNames, declaredPropertyNames, declaredParameterNames, semanticOperators);
        }
        ranges.Sort((left, right) => left.Start.CompareTo(right.Start));
        var previousEnd = 0;
        foreach (var range in ranges) {
            if (range.Start < previousEnd || range.End <= range.Start || range.End > source.Length) continue;
            if (range.Kind != StorySyntaxKind.Plain) {
                result.AddSpan(range.Start, range.End - range.Start, range.Kind);
            }
            previousEnd = range.End;
        }
        return result;
    }

    private static void Collect(
        Token token,
        List<SemanticRange> output,
        HashSet<long> declaredCommandNames,
        HashSet<long> declaredTypeNames,
        HashSet<long> declaredPropertyNames,
        HashSet<long> declaredParameterNames,
        HashSet<long> semanticOperators) {
        if (IsUnscannedSubExpression(token)) {
            CollectUnscannedSubExpression(token, output);
            return;
        }
        if (token is StringExpandableToken expandable && expandable.NestedTokens != null && expandable.NestedTokens.Count > 0) {
            var nested = new List<SemanticRange>();
            foreach (var nestedToken in expandable.NestedTokens) {
                Collect(nestedToken, nested, declaredCommandNames, declaredTypeNames, declaredPropertyNames, declaredParameterNames, semanticOperators);
            }
            nested.Sort((left, right) => left.Start.CompareTo(right.Start));

            var cursor = token.Extent.StartOffset;
            foreach (var range in nested) {
                if (range.Start < cursor || range.End > token.Extent.EndOffset || range.End <= range.Start) continue;
                Add(output, cursor, range.Start, ExpandableLiteralKind(token));
                Add(output, range.Start, range.End, range.Kind);
                cursor = range.End;
            }
            Add(output, cursor, token.Extent.EndOffset, ExpandableLiteralKind(token));
            return;
        }

        Add(output, token.Extent.StartOffset, token.Extent.EndOffset, Map(token, declaredCommandNames, declaredTypeNames, declaredPropertyNames, declaredParameterNames, semanticOperators));
    }

    private static bool IsUnscannedSubExpression(Token token) =>
        token.Kind == TokenKind.StringLiteral &&
        string.Equals(token.GetType().Name, "UnscannedSubExprToken", StringComparison.Ordinal) &&
        token.Text.StartsWith("$(", StringComparison.Ordinal) &&
        token.Text.EndsWith(")", StringComparison.Ordinal);

    private static void CollectUnscannedSubExpression(Token token, List<SemanticRange> output) {
        var ast = Parser.ParseInput(token.Text, out var nestedTokens, out _);
        var declaredCommandNames = DeclaredCommandNames(ast, nestedTokens);
        var declaredTypeNames = DeclaredTypeNames(ast, nestedTokens);
        var declaredPropertyNames = DeclaredPropertyNames(ast, nestedTokens);
        var declaredParameterNames = DeclaredParameterNames(ast);
        var semanticOperators = SemanticOperators(ast, nestedTokens);
        var nested = new List<SemanticRange>();
        foreach (var nestedToken in nestedTokens) {
            Collect(nestedToken, nested, declaredCommandNames, declaredTypeNames, declaredPropertyNames, declaredParameterNames, semanticOperators);
        }
        nested.Sort((left, right) => left.Start.CompareTo(right.Start));
        var cursor = 0;
        foreach (var range in nested) {
            if (range.Start < cursor || range.End > token.Text.Length || range.End <= range.Start) continue;
            if (range.Start > cursor) {
                output.Add(new SemanticRange(
                    token.Extent.StartOffset + cursor,
                    token.Extent.StartOffset + range.Start,
                    StorySyntaxKind.Plain));
            }
            output.Add(new SemanticRange(
                token.Extent.StartOffset + range.Start,
                token.Extent.StartOffset + range.End,
                range.Kind));
            cursor = range.End;
        }
        if (cursor < token.Text.Length) {
            output.Add(new SemanticRange(
                token.Extent.StartOffset + cursor,
                token.Extent.StartOffset + token.Text.Length,
                StorySyntaxKind.Plain));
        }
    }

    private static void Add(List<SemanticRange> output, int start, int end, StorySyntaxKind kind) {
        if (kind == StorySyntaxKind.Plain || end <= start) return;
        output.Add(new SemanticRange(start, end, kind));
    }

    private static StorySyntaxKind Map(
        Token token,
        HashSet<long> declaredCommandNames,
        HashSet<long> declaredTypeNames,
        HashSet<long> declaredPropertyNames,
        HashSet<long> declaredParameterNames,
        HashSet<long> semanticOperators) {
        var rangeKey = RangeKey(token.Extent.StartOffset, token.Extent.EndOffset);
        if (declaredTypeNames.Contains(rangeKey)) return StorySyntaxKind.Type;
        if (declaredCommandNames.Contains(rangeKey)) return StorySyntaxKind.Command;
        if (declaredPropertyNames.Contains(rangeKey)) return StorySyntaxKind.Property;
        if (declaredParameterNames.Contains(rangeKey)) return StorySyntaxKind.Parameter;
        var flags = token.TokenFlags;
        if ((flags & TokenFlags.Keyword) != 0) return StorySyntaxKind.Keyword;
        if ((flags & TokenFlags.TypeName) != 0 || (flags & TokenFlags.AttributeName) != 0) return StorySyntaxKind.Type;
        if ((flags & TokenFlags.CommandName) != 0) return StorySyntaxKind.Command;
        if ((flags & TokenFlags.MemberName) != 0) return StorySyntaxKind.Property;
        if (semanticOperators.Contains(rangeKey)) return StorySyntaxKind.Operator;
        if (IsPunctuation(token.Kind)) return StorySyntaxKind.Punctuation;
        if ((flags & (TokenFlags.BinaryOperator | TokenFlags.UnaryOperator | TokenFlags.AssignmentOperator | TokenFlags.PrefixOrPostfixOperator | TokenFlags.SpecialOperator)) != 0) return StorySyntaxKind.Operator;
        switch (token.Kind) {
            case TokenKind.Variable:
            case TokenKind.SplattedVariable:
                return StorySyntaxKind.Variable;
            case TokenKind.Parameter:
                return StorySyntaxKind.Parameter;
            case TokenKind.Number:
                return StorySyntaxKind.Number;
            case TokenKind.Redirection:
                return StorySyntaxKind.Operator;
            case TokenKind.Comment:
                return StorySyntaxKind.Comment;
            case TokenKind.StringLiteral:
            case TokenKind.StringExpandable:
            case TokenKind.HereStringLiteral:
            case TokenKind.HereStringExpandable:
                return StorySyntaxKind.String;
            default:
                return StorySyntaxKind.Plain;
        }
    }

    private static HashSet<long> DeclaredCommandNames(ScriptBlockAst ast, Token[] tokens) {
        var ranges = new HashSet<long>();
        foreach (var candidate in ast.FindAll(node => node is FunctionDefinitionAst, true)) {
            var function = (FunctionDefinitionAst)candidate;
            var bodyStart = function.Body.Extent.StartOffset;
            foreach (var token in tokens) {
                if (token.Extent.StartOffset < function.Extent.StartOffset || token.Extent.EndOffset > bodyStart) continue;
                if (!string.Equals(token.Text, function.Name, StringComparison.OrdinalIgnoreCase)) continue;
                ranges.Add(RangeKey(token.Extent.StartOffset, token.Extent.EndOffset));
                break;
            }
        }
        foreach (var candidate in ast.FindAll(node => node is InvokeMemberExpressionAst, true)) {
            var invocation = (InvokeMemberExpressionAst)candidate;
            if (invocation.Member is StringConstantExpressionAst) {
                ranges.Add(RangeKey(invocation.Member.Extent.StartOffset, invocation.Member.Extent.EndOffset));
            }
        }
        return ranges;
    }

    private static HashSet<long> DeclaredPropertyNames(ScriptBlockAst ast, Token[] tokens) {
        var ranges = new HashSet<long>();
        foreach (var candidate in ast.FindAll(node => node is PropertyMemberAst, true)) {
            var property = (PropertyMemberAst)candidate;
            var declarationStart = property.PropertyType?.Extent.EndOffset ?? property.Extent.StartOffset;
            foreach (var attribute in property.Attributes) {
                declarationStart = Math.Max(declarationStart, attribute.Extent.EndOffset);
            }
            var declarationEnd = property.InitialValue?.Extent.StartOffset ?? property.Extent.EndOffset;
            foreach (var token in tokens) {
                if (!(token is VariableToken variable) ||
                    token.Extent.StartOffset < declarationStart ||
                    token.Extent.EndOffset > declarationEnd ||
                    !string.Equals(variable.VariablePath.UserPath, property.Name, StringComparison.OrdinalIgnoreCase)) {
                    continue;
                }
                ranges.Add(RangeKey(token.Extent.StartOffset, token.Extent.EndOffset));
                break;
            }
        }
        return ranges;
    }

    private static HashSet<long> DeclaredParameterNames(ScriptBlockAst ast) {
        var ranges = new HashSet<long>();
        foreach (var candidate in ast.FindAll(node => node is ParameterAst, true)) {
            var parameter = (ParameterAst)candidate;
            ranges.Add(RangeKey(parameter.Name.Extent.StartOffset, parameter.Name.Extent.EndOffset));
        }
        return ranges;
    }

    private static StorySyntaxKind ExpandableLiteralKind(Token token) {
        return (token.TokenFlags & TokenFlags.CommandName) != 0
            ? StorySyntaxKind.Command
            : StorySyntaxKind.String;
    }

    private static HashSet<long> DeclaredTypeNames(ScriptBlockAst ast, Token[] tokens) {
        var ranges = new HashSet<long>();
        foreach (var candidate in ast.FindAll(node => node is TypeDefinitionAst, true)) {
            var type = (TypeDefinitionAst)candidate;
            var declarationEnd = type.Members.Count > 0
                ? type.Members[0].Extent.StartOffset
                : type.Extent.EndOffset;
            foreach (var token in tokens) {
                if (token.Extent.StartOffset <= type.Extent.StartOffset || token.Extent.EndOffset > declarationEnd) continue;
                if (!string.Equals(token.Text, type.Name, StringComparison.OrdinalIgnoreCase)) continue;
                ranges.Add(RangeKey(token.Extent.StartOffset, token.Extent.EndOffset));
                break;
            }
        }
        return ranges;
    }

    private static HashSet<long> SemanticOperators(ScriptBlockAst ast, Token[] tokens) {
        var ranges = new HashSet<long>();
        foreach (var candidate in ast.FindAll(node => node is CommandAst, true)) {
            var command = (CommandAst)candidate;
            if (command.InvocationOperator != TokenKind.Dot) continue;
            foreach (var token in tokens) {
                if (token.Kind != TokenKind.Dot || token.Extent.StartOffset != command.Extent.StartOffset) continue;
                ranges.Add(RangeKey(token.Extent.StartOffset, token.Extent.EndOffset));
                break;
            }
        }
        return ranges;
    }

    private static long RangeKey(int start, int end) => ((long)(uint)start << 32) | (uint)end;

    private static bool IsPunctuation(TokenKind kind) {
        switch (kind) {
            case TokenKind.LParen:
            case TokenKind.RParen:
            case TokenKind.LCurly:
            case TokenKind.RCurly:
            case TokenKind.LBracket:
            case TokenKind.RBracket:
            case TokenKind.AtParen:
            case TokenKind.AtCurly:
            case TokenKind.DollarParen:
            case TokenKind.Semi:
            case TokenKind.Comma:
            case TokenKind.Dot:
            case TokenKind.Colon:
            case TokenKind.ColonColon:
                return true;
            default:
                return false;
        }
    }

    private readonly struct SemanticRange {
        public SemanticRange(int start, int end, StorySyntaxKind kind) {
            Start = start;
            End = end;
            Kind = kind;
        }

        public int Start { get; }
        public int End { get; }
        public StorySyntaxKind Kind { get; }
    }
}
