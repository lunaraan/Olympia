namespace Olympia.LexicalAnalyzer
{
    internal enum TokenType
    {
        Identifier,
        IntLiteral,
        FloatLiteral,

        /* Reserved */
        Class,
        Return,
        DataType,
        AccessModifier,

        /* Operators */
        OperatorEquals,

        /* Punctuation */
        Semicolon,
        OpenParen,
        CloseParen,
        OpenBrace,
        CloseBrace,
        Comma
    }
}