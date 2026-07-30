using System;
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
        foreach (var token in tokens) {
            var start = token.Extent.StartOffset;
            var length = token.Extent.EndOffset - start;
            if (length <= 0 || start < 0 || start + length > source.Length) continue;
            var kind = Map(token);
            if (kind == StorySyntaxKind.Plain) continue;
            result.AddSpan(start, length, kind);
        }
        return result;
    }

    private static StorySyntaxKind Map(Token token) {
        var flags = token.TokenFlags;
        if ((flags & TokenFlags.Keyword) != 0) return StorySyntaxKind.Keyword;
        if ((flags & TokenFlags.TypeName) != 0 || (flags & TokenFlags.AttributeName) != 0) return StorySyntaxKind.Type;
        if ((flags & TokenFlags.CommandName) != 0) return StorySyntaxKind.Command;
        if ((flags & TokenFlags.MemberName) != 0) return StorySyntaxKind.Property;
        if ((flags & (TokenFlags.BinaryOperator | TokenFlags.UnaryOperator | TokenFlags.AssignmentOperator | TokenFlags.PrefixOrPostfixOperator)) != 0) return StorySyntaxKind.Operator;
        switch (token.Kind) {
            case TokenKind.Variable:
            case TokenKind.SplattedVariable:
                return StorySyntaxKind.Variable;
            case TokenKind.Parameter:
                return StorySyntaxKind.Parameter;
            case TokenKind.Number:
                return StorySyntaxKind.Number;
            case TokenKind.Comment:
                return StorySyntaxKind.Comment;
            case TokenKind.StringLiteral:
            case TokenKind.StringExpandable:
            case TokenKind.HereStringLiteral:
            case TokenKind.HereStringExpandable:
                return StorySyntaxKind.String;
            case TokenKind.Identifier:
            case TokenKind.Generic:
                return StorySyntaxKind.Variable;
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
}
