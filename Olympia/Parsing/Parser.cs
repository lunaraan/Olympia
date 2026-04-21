using Olympia.LexicalAnalyzer;

namespace Olympia.Parsing
{
    internal static class Parser
    {
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
            AccessModifier access = EatAccessModifierToken(tokens);
            SkipToken(tokens, "class");
            string identifier = EatIdentifierToken(tokens);
            SkipToken(tokens, "{");
            ClassBodyNode classBody = EatClassBodyToken(tokens);

            ClassDeclarationNode classDeclarationNode = new(access, identifier, classBody);
            return classDeclarationNode;
        }

        private static ClassBodyNode EatClassBodyToken(List<Token> tokens)
        {
            List<FieldAssignmentStatementNode> fields = [];

            while (tokens.First().Lexeme != "}" && tokens.Count > 0)
            {
                fields.Add(EatFieldAssignmentStatementToken(tokens));
            }

            ClassBodyNode classBody = new([], fields);

            SkipToken(tokens, "}");
            return classBody;
        }

        private static FieldAssignmentStatementNode EatFieldAssignmentStatementToken(List<Token> tokens)
        {
            AccessModifier access = EatAccessModifierToken(tokens);
            AssignmentStatementNode assignmentStatement = EatAssignmentStatementNode(tokens);
            return new FieldAssignmentStatementNode(access, assignmentStatement);
        }

        private static AssignmentStatementNode EatAssignmentStatementNode(List<Token> tokens)
        {
            NodeType nodeType = EatNodeTypeToken(tokens);
            string identifier = EatIdentifierToken(tokens);
            SkipToken(tokens, "=");
            ValueNode valueNode = EatValueToken(tokens);
            SkipToken(tokens, ";");

            return new AssignmentStatementNode(nodeType, identifier, valueNode);
        }

        private static ValueNode EatValueToken(List<Token> tokens)
        {
            Token valueToken = EatToken(tokens);
            return valueToken.Type switch
            {
                TokenType.IntLiteral => new IntLiteralNode(int.Parse(valueToken.Lexeme)),
                TokenType.FloatLiteral => new FloatLiteralNode(float.Parse(valueToken.Lexeme)),
                _ => new IdentifierLiteralNode(valueToken.Lexeme)
            };
        }

        private static string EatIdentifierToken(List<Token> tokens)
        {
            Token identifierToken = EatToken(tokens);
            return identifierToken.Lexeme;
        }

        private static NodeType EatNodeTypeToken(List<Token> tokens)
        {
            Token nodeTypeToken = EatToken(tokens);
            return nodeTypeToken.Type switch
            {
                TokenType.Int => NodeType.Int,
                TokenType.Float => NodeType.Float,
                _ => NodeType.InvalidNodeType
            };
        }

        private static AccessModifier EatAccessModifierToken(List<Token> tokens)
        {
            Token accessModifier = EatToken(tokens);
            return accessModifier.Type switch
            {
                TokenType.Private => AccessModifier.Private,
                TokenType.Public => AccessModifier.Public,
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
