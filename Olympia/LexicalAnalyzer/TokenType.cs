namespace Olympia.LexicalAnalyzer
{
    internal enum TokenType
    {
        Identifier,
        IntLiteral,
        FloatLiteral,

        /* Reserved Keywords */
        Int,
        Float,
        Class,

        /* Access Modifiers */
        Public,
        Private,

        /* Operators */
        OperatorEquals,

        /* Punctuation */
        Semicolon,
        OpenParen,
        CloseParen,
        OpenBracket,
        CloseBracket
    }
}
