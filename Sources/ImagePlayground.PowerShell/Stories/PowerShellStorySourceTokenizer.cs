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
        Parser.ParseInput(source, out var tokens, out _);
        var ranges = new List<SemanticRange>();
        foreach (var token in tokens) {
            Collect(token, ranges);
        }
        ranges.Sort((left, right) => left.Start.CompareTo(right.Start));
        var previousEnd = 0;
        foreach (var range in ranges) {
            if (range.Start < previousEnd || range.End <= range.Start || range.End > source.Length) continue;
            result.AddSpan(range.Start, range.End - range.Start, range.Kind);
            previousEnd = range.End;
        }
        return result;
    }

    private static void Collect(Token token, List<SemanticRange> output) {
        if (token is StringExpandableToken expandable && expandable.NestedTokens != null && expandable.NestedTokens.Count > 0) {
            var nested = new List<SemanticRange>();
            foreach (var nestedToken in expandable.NestedTokens) Collect(nestedToken, nested);
            nested.Sort((left, right) => left.Start.CompareTo(right.Start));

            var cursor = token.Extent.StartOffset;
            foreach (var range in nested) {
                if (range.Start < cursor || range.End > token.Extent.EndOffset || range.End <= range.Start) continue;
                Add(output, cursor, range.Start, StorySyntaxKind.String);
                Add(output, range.Start, range.End, range.Kind);
                cursor = range.End;
            }
            Add(output, cursor, token.Extent.EndOffset, StorySyntaxKind.String);
            return;
        }

        Add(output, token.Extent.StartOffset, token.Extent.EndOffset, Map(token));
    }

    private static void Add(List<SemanticRange> output, int start, int end, StorySyntaxKind kind) {
        if (kind == StorySyntaxKind.Plain || end <= start) return;
        output.Add(new SemanticRange(start, end, kind));
    }

    private static StorySyntaxKind Map(Token token) {
        var flags = token.TokenFlags;
        if ((flags & TokenFlags.Keyword) != 0) return StorySyntaxKind.Keyword;
        if ((flags & TokenFlags.TypeName) != 0 || (flags & TokenFlags.AttributeName) != 0) return StorySyntaxKind.Type;
        if ((flags & TokenFlags.CommandName) != 0) return StorySyntaxKind.Command;
        if ((flags & TokenFlags.MemberName) != 0) return StorySyntaxKind.Property;
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
                return StorySyntaxKind.Punctuation;
            default:
                return StorySyntaxKind.Plain;
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
