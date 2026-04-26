using Olympia.LexicalAnalyzer;

namespace Olympia.Parsing
{
    internal static class Parser
    {
        private static bool NextIsMethod(List<Token> tokens)
        {
            const int tokensForLookahead = 4;
            if (tokens.Count < tokensForLookahead)
            {
                return false;
            }

            return tokens[0].Type == TokenType.AccessModifier && tokens[1].Type == TokenType.DataType && tokens[2].Type == TokenType.Identifier &&
                   tokens[3].Type == TokenType.OpenParen;
        }

        public static ProgramNode Parse(List<Token> tokens)
        {
            ProgramNode program = new([]);
            while (tokens.Count > 0)
            {
                program.ClassDeclarations.Add(ParseClassDeclaration(tokens));
            }
            return program;
        }

        private static ClassDeclarationNode ParseClassDeclaration(List<Token> tokens)
        {
            AccessModifier access = ParseAccessModifier(tokens);
            SkipToken(tokens, "class");
            string identifier = ParseIdentifier(tokens);
            SkipToken(tokens, "{");
            ClassBodyNode classBody = ParseClassBody(tokens);

            ClassDeclarationNode classDeclarationNode = new(access, identifier, classBody);
            return classDeclarationNode;
        }

        private static ClassBodyNode ParseClassBody(List<Token> tokens)
        {
            List<MethodDeclarationNode> methods = [];
            List<FieldAssignmentStatementNode> fields = [];

            while (tokens.First().Lexeme != "}" && tokens.Count > 0)
            {
                if (NextIsMethod(tokens))
                {
                    methods.Add(ParseMethodDeclaration(tokens));
                }
                else
                {
                    fields.Add(ParseFieldAssignmentStatement(tokens));
                }
            }

            ClassBodyNode classBody = new(methods, fields);

            SkipToken(tokens, "}");
            return classBody;
        }

        private static MethodDeclarationNode ParseMethodDeclaration(List<Token> tokens)
        {
            AccessModifier access = ParseAccessModifier(tokens);
            NodeType type = ParseNodeType(tokens);
            string identifier = ParseIdentifier(tokens);
            SkipToken(tokens, "(");
            ParameterListNode parameters = ParseParameterList(tokens);
            SkipToken(tokens, "{");
            List<StatementNode> statements = [];

            while (tokens.First().Lexeme != "}" && tokens.Count > 0)
            {
                statements.Add(ParseAssignmentStatement(tokens));
            }

            SkipToken(tokens, "}");

            return new MethodDeclarationNode(access, type, identifier, parameters, statements);
        }

        private static ParameterListNode ParseParameterList(List<Token> tokens)
        {
            List<ParameterNode> parameters = [];

            while (tokens.Count > 0 && tokens[0].Lexeme != ")")
            {
                NodeType type = ParseNodeType(tokens);
                string identifier = ParseIdentifier(tokens);

                if (tokens[0].Lexeme == ",")
                {
                    SkipToken(tokens, ",");
                }

                parameters.Add(new ParameterNode(type, identifier));
            }

            SkipToken(tokens, ")");
            return new ParameterListNode(parameters);
        }

        private static FieldAssignmentStatementNode ParseFieldAssignmentStatement(List<Token> tokens)
        {
            AccessModifier access = ParseAccessModifier(tokens);
            AssignmentStatementNode assignmentStatement = ParseAssignmentStatement(tokens);
            return new FieldAssignmentStatementNode(access, assignmentStatement);
        }

        private static AssignmentStatementNode ParseAssignmentStatement(List<Token> tokens)
        {
            NodeType nodeType = ParseNodeType(tokens);
            string identifier = ParseIdentifier(tokens);
            SkipToken(tokens, "=");
            ValueNode valueNode = ParseValueType(tokens);
            SkipToken(tokens, ";");

            return new AssignmentStatementNode(nodeType, identifier, valueNode);
        }

        private static ValueNode ParseValueType(List<Token> tokens)
        {
            Token valueToken = EatToken(tokens);
            return valueToken.Type switch
            {
                TokenType.IntLiteral => new IntLiteralNode(int.Parse(valueToken.Lexeme)),
                TokenType.FloatLiteral => new FloatLiteralNode(float.Parse(valueToken.Lexeme)),
                _ => new IdentifierLiteralNode(valueToken.Lexeme)
            };
        }

        private static string ParseIdentifier(List<Token> tokens)
        {
            Token identifierToken = EatToken(tokens);
            return identifierToken.Lexeme;
        }

        private static NodeType ParseNodeType(List<Token> tokens)
        {
            Token nodeTypeToken = EatToken(tokens);
            return nodeTypeToken.Lexeme switch
            {
                "int" => NodeType.Int,
                "float" => NodeType.Float,
                _ => NodeType.InvalidNodeType
            };
        }

        private static AccessModifier ParseAccessModifier(List<Token> tokens)
        {
            Token accessModifier = EatToken(tokens);
            return accessModifier.Lexeme switch
            {
                "private" => AccessModifier.Private,
                "public" => AccessModifier.Public,
                _ => AccessModifier.InvalidAccessModifier
            };
        }

        private static Token EatToken(List<Token> tokens)
        {
            Token token = tokens.First();
            tokens.RemoveAt(0);
            return token;
        }

        private static void SkipToken(List<Token> tokens, string lexemeToSkip)
        {
            Token token = tokens.First();
            if (token.Lexeme != lexemeToSkip)
            {
                Debugger.PrintError($"Expected '{lexemeToSkip}', got '{token.Lexeme}'");
                return;
            }

            tokens.RemoveAt(0);
        }
    }
}
