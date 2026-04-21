using Olympia.Emitting;
using Olympia.LexicalAnalyzer;
using Olympia.Parsing;

namespace Olympia
{
    internal class Program
    {
        private static string? GetFileExtension(string filename)
        {
            if (!filename.Contains('.'))
            {
                return null;
            }

            string extension = filename[(filename.IndexOf('.')+1)..];
            return extension;
        }

        private static void PrintAST(ProgramNode program)
        {
            foreach (ClassDeclarationNode classDeclaration in program.ClassDeclarations)
            {
                Console.WriteLine(classDeclaration.Access);
                Console.WriteLine(classDeclaration.Identifier);
                
                Console.WriteLine("/* Fields */");

                foreach (FieldAssignmentStatementNode field in classDeclaration.ClassBody.Fields)
                {
                    Console.WriteLine(field.Access);
                    Console.WriteLine(field.Assignment.Identifier);
                    Console.WriteLine(field.Assignment.Type);
                    Console.WriteLine(field.Assignment.Value);

                    switch (field.Assignment.Value)
                    {
                        case IntLiteralNode intLiteral:
                            Console.WriteLine(intLiteral.Value);
                            break;
                        case FloatLiteralNode floatLiteral:
                            Console.WriteLine(floatLiteral.Value);
                            break;
                        case IdentifierLiteralNode identifierLiteral:
                            Console.WriteLine(identifierLiteral.Identifier);
                            break;
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Debugger.PrintError("Not enough args provided");
                return;
            }

            string filename = args.Last();
            if (GetFileExtension(filename) != "cs")
            {
                Debugger.PrintError("File provided is not a .cs file");
                return;
            }

            StreamReader reader = new(filename);
            List<Token> tokens = Lexer.Tokenize(reader);
            ProgramNode program = Parser.Parse(tokens);

            PrintAST(program);

            Emitter.EmitLuau(program);
        }
    }
}
