using System.Text;
using Olympia.Parsing;

namespace Olympia.Emitting
{
    internal static class Emitter
    {
        public static void EmitLuau(ProgramNode program)
        {
            StringBuilder strBuilder = new();
            strBuilder.AppendLine("""
                                  --[[
                                  Olympia version: Beta 0.1
                                  
                                  File was compiled using Olympia.
                                  --]]
                                  
                                  """);

            strBuilder.AppendLine(EmitClass(program.ClassDeclarations.First()));
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "file.luau"), strBuilder.ToString());
        }

        private static string EmitClass(ClassDeclarationNode classDeclaration)
        {
            StringBuilder strBuilder = new();
            strBuilder.AppendLine($"local {classDeclaration.Identifier} = {{}}");
            strBuilder.AppendLine($"{classDeclaration.Identifier}.__index  = {classDeclaration.Identifier}");

            strBuilder.AppendLine($"function {classDeclaration.Identifier}.New()");
            strBuilder.AppendLine($"    local self = setmetatable({{}}, {classDeclaration.Identifier})");

            foreach (FieldAssignmentStatementNode field in classDeclaration.ClassBody.Fields)
            {
                switch (field.Assignment.Value)
                {
                    case IntLiteralNode intLiteral:
                        strBuilder.AppendLine($"    self.{field.Assignment.Identifier} = {intLiteral.Value}");
                        break;
                    case FloatLiteralNode floatLiteral:
                        strBuilder.AppendLine($"    self.{field.Assignment.Identifier} = {floatLiteral.Value}");
                        break;
                    case IdentifierLiteralNode identifierLiteral:
                        strBuilder.AppendLine($"    self.{field.Assignment.Identifier} = {identifierLiteral.Identifier}");
                        break;
                }
            }

            strBuilder.AppendLine($"    return self");
            strBuilder.AppendLine("end");

            foreach (MethodDeclarationNode method in classDeclaration.ClassBody.Methods)
            {
                strBuilder.Append($"function {classDeclaration.Identifier}:{method.Identifier}(");

                int i = 0;
                foreach (ParameterNode parameter in method.Parameters.Parameters)
                {
                    if (i >= method.Parameters.Parameters.Count - 1)
                    {
                        strBuilder.Append($"{parameter.Identifier}");
                    }
                    else
                    {
                        strBuilder.Append($"{parameter.Identifier}, ");
                    }
                    i++;
                }

                strBuilder.AppendLine(")");

                foreach (StatementNode statement in method.Statements)
                {
                    //strBuilder.AppendLine($"    {statement}");
                }

                strBuilder.AppendLine("end");
            }

            strBuilder.AppendLine($"return {classDeclaration.Identifier}");
            return strBuilder.ToString();
        }
    }
}
