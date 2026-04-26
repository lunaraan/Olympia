using System.Text;

namespace Olympia.LexicalAnalyzer
{
    internal static class Lexer
    {
        private static bool IsValidPeek(int peeked)
        {
            return peeked != -1;
        }

        private static string GetWord(char consumed, StreamReader reader)
        {
            StringBuilder strBuilder = new();
            strBuilder.Append(consumed);
            int peeked = reader.Peek();

            while (IsValidPeek(peeked) && char.IsAsciiLetterOrDigit((char)peeked))
            {
                consumed = (char)reader.Read();
                strBuilder.Append(consumed);
                peeked = reader.Peek();
            }

            return strBuilder.ToString();
        }

        private static string GetNumber(char consumed, StreamReader reader)
        {
            StringBuilder strBuilder = new();
            strBuilder.Append(consumed);
            int peeked = reader.Peek();

            while (IsValidPeek(peeked) && (char.IsDigit((char)peeked) || (char)peeked == '.' || (char)peeked == '_'))
            {
                consumed = (char)reader.Read();
                strBuilder.Append(consumed);
                peeked = reader.Peek();
            }

            return strBuilder.ToString();
        }

        public static List<Token> Tokenize(StreamReader reader)
        {
            List<Token> tokens = [];

            int peeked = reader.Peek();
            while (IsValidPeek(peeked))
            {
                char consumed = (char)reader.Read();
                if (char.IsWhiteSpace(consumed))
                {
                    continue;
                }
                
                string consumedStr = consumed.ToString();
                switch (consumed)
                {
                    case '=':
                        tokens.Add(new Token(consumedStr, TokenType.OperatorEquals));
                        break;
                    case '(':
                        tokens.Add(new Token(consumedStr, TokenType.OpenParen));
                        break;
                    case ')':
                        tokens.Add(new Token(consumedStr, TokenType.CloseParen));
                        break;
                    case ';':
                        tokens.Add(new Token(consumedStr, TokenType.Semicolon));
                        break;
                    case '{':
                        tokens.Add(new Token(consumedStr, TokenType.OpenBrace));
                        break;
                    case '}':
                        tokens.Add(new Token(consumedStr, TokenType.CloseBrace));
                        break;
                    case ',':
                        tokens.Add(new Token(consumedStr, TokenType.Comma));
                        break;
                    default:
                        if (char.IsDigit(consumed))
                        {
                            string numberLexeme = GetNumber(consumed, reader);
                            TokenType type = numberLexeme.Contains('.') ? TokenType.FloatLiteral : TokenType.IntLiteral;

                            tokens.Add(new Token(numberLexeme, type));
                            break;
                        }

                        string word = GetWord(consumed, reader);
                        switch (word)
                        {
                            case "int" or "float" or "void":
                                tokens.Add(new Token(word, TokenType.DataType));
                                break;
                            case "return":
                                tokens.Add(new Token(word, TokenType.Return));
                                break;
                            case "class":
                                tokens.Add(new Token(word, TokenType.Class));
                                break;
                            case "public" or "private":
                                tokens.Add(new Token(word, TokenType.AccessModifier));
                                break;
                            default:
                                tokens.Add(new Token(word, TokenType.Identifier));
                                break;
                        }

                        break;
                }

                peeked = reader.Peek();
            }

            return tokens;
        }
    }
}
