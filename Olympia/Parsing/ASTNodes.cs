namespace Olympia.Parsing
{
    internal enum AccessModifier
    {
        Private,
        Public,
        InvalidAccessModifier
    }

    internal enum NodeType
    {
        Int,
        Float,
        Identifier,
        InvalidNodeType
    }

    internal enum ClassMemberType
    {
        FieldAssignmentStatement,
        MethodDeclaration
    }

    internal abstract class StatementNode(NodeType type)
    {
        public readonly NodeType Type = type;
    }

    internal abstract class ValueNode { }

    internal interface IClassMember
    {
        public ClassMemberType GetMemberType();
    }

    internal class IntLiteralNode(int value) : ValueNode
    {
        public readonly int Value = value;
    }

    internal class FloatLiteralNode(float value) : ValueNode
    {
        public readonly float Value = value;
    }

    internal class IdentifierLiteralNode(string identifier) : ValueNode
    {
        public readonly string Identifier = identifier;
    }

    internal class ReturnStatementNode(NodeType type, ValueNode valueNode) : StatementNode(type)
    {
        public readonly ValueNode ValueNode = valueNode;
    }

    internal class AssignmentStatementNode(NodeType type, string identifier, ValueNode valueNode) : StatementNode(type)
    {
        public readonly string Identifier = identifier;
        public readonly ValueNode Value = valueNode;
    }

    internal class FieldAssignmentStatementNode(AccessModifier accessModifier, AssignmentStatementNode assignmentStatement) : StatementNode(assignmentStatement.Type)
    {
        public readonly AccessModifier Access = accessModifier;
        public readonly AssignmentStatementNode Assignment = assignmentStatement;
    }

    internal class ParameterNode(NodeType type, string identifier)
    {
        public readonly NodeType Type = type;
        public readonly string Identifier = identifier;
    }

    internal class ParameterListNode(List<ParameterNode> parameters)
    {
        public readonly List<ParameterNode> Parameters = parameters;
    }

    internal class MethodDeclarationNode(
        AccessModifier access,
        NodeType type,
        string identifier,
        ParameterListNode parameters,
        List<StatementNode> statements)
    {
        public readonly AccessModifier Access = access;
        public readonly NodeType Type = type;
        public readonly string Identifier = identifier;
        public readonly ParameterListNode Parameters = parameters;
        public readonly List<StatementNode> Statements = statements;
    }

    internal class ClassBodyNode(List<MethodDeclarationNode> methods, List<FieldAssignmentStatementNode> fields)
    {
        public List<MethodDeclarationNode> Methods = methods;
        public List<FieldAssignmentStatementNode> Fields = fields;
    }

    internal class ClassDeclarationNode(AccessModifier access, string identifier, ClassBodyNode classBody)
    {
        public readonly AccessModifier Access = access;
        public readonly string Identifier = identifier;
        public readonly ClassBodyNode ClassBody = classBody;
    }

    internal class ProgramNode(List<ClassDeclarationNode> classDeclarations)
    {
        public readonly List<ClassDeclarationNode> ClassDeclarations = classDeclarations;
    }
}
