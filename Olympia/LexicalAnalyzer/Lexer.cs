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

            while (IsValidPeek(peeked) && char.IsAsciiLetter((char)peeked))
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

            while (IsValidPeek(peeked) && (char.IsDigit((char)peeked) || (char)peeked == '.'))
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
                        tokens.Add(new Token(consumedStr, TokenType.OpenBracket));
                        break;
                    case '}':
                        tokens.Add(new Token(consumedStr, TokenType.CloseBracket));
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
                            case "int":
                                tokens.Add(new Token(word, TokenType.Int));
                                break;
                            case "float":
                                tokens.Add(new Token(word, TokenType.Float));
                                break;
                            case "class":
                                tokens.Add(new Token(word, TokenType.Class));
                                break;
                            case "private":
                                tokens.Add(new Token(word, TokenType.Private));
                                break;
                            case "public":
                                tokens.Add(new Token(word, TokenType.Public));
                                break;
                            default:
                                tokens.Add(new Token(word, TokenType.Identifier));
                                break;
                        }

                        break;
                }

                peeked = reader.Peek();
            }

            reader.Close();

            return tokens;
        }
    }
}
