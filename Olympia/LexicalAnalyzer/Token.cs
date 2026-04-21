namespace Olympia.LexicalAnalyzer
{
    internal class Token(string lexeme, TokenType type)
    {
        public readonly string Lexeme = lexeme;
        public readonly TokenType Type = type;

        public override string ToString()
        {
            return $"Lexeme is {Lexeme} and type is {Type.ToString()}";
        }
    }
}
