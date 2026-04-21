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
                                  This file was compiled using Olympia.
                                  Olympia is currently in beta v0.1;
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

            strBuilder.AppendLine($"return {classDeclaration.Identifier}");
            return strBuilder.ToString();
        }
    }
}
