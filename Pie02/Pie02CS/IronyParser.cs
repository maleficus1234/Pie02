using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

using Pie02CS.Rules;
using Pie02CS.Expressions;

namespace Pie02CS
{
    public delegate void BuildExpressionDelegate(IronyParser parser, Expression parent, ParseTreeNode node);

    public class IronyParser
    {
        public Parser parser;
        Dictionary<string, BuildExpressionDelegate> Builders;

        public IronyParser()
        {
            parser = new Parser(new PieGrammar());
            Builders = new Dictionary<string, BuildExpressionDelegate>()
            {
                {"class_declaration", ClassDeclaration.Build },
                {"namespace_declaration", NamespaceDeclaration.Build },
                {"qualified_import", QualifiedImport.Build },
                {"method_declaration", MethodDeclaration.Build },
                {"generic_list", GenericList.Build },
                {"assignment", Assignment.Build },
                {"string_literal", Literal.Build },
                {"number_literal", Literal.Build },
                {"parameter", Parameter.Build },
                {"return_statement", ReturnStatement.Build },
                {"qualified_identifier", QualifiedIdentifier.Build },
                {"field_declaration", FieldDeclaration.Build },
                { "method_invocation", MethodInvocation.Build },
                { "import_declaration", ImportDeclaration.Build },
                { "constructor_declaration", MethodDeclaration.Build },
                { "binary_operation", BinaryOperator.Build },
                { "if_conditional", IfConditional.Build },
                { "foreach_loop", ForEachLoop.Build },
                { "directioned_identifier", DirectionedIdentifier.Build },
                { "statement", Statement.Build },
                { "indexed_identifier", IndexedIdentifier.Build },
                { "identifier", Identifier.Build },
                { "base", Identifier.Build },
                { "accessed_expression", AccessedExpression.Build },
                { "test_declaration", TestDeclaration.Build },
                { "assert_statement", AssertStatement.Build },
                { "pre_unary_operation", UnaryOperation.Build },
                { "post_unary_operation", UnaryOperation.Build },
                { "parened_expression", ParenedExpression.Build },
                { "anonymous_method", AnonymousMethod.Build },
                { "while_loop", WhileLoop.Build },
                { "reserved_expression", ReservedExpression.Build },
                { "switch_block", SwitchBlock.Build },
                { "case_block", CaseBlock.Build },
                { "for_loop", ForLoop.Build }
            };
        }

        public Expression Parse(string source)
        {
            var root = new Expression(null);
            var tree = parser.Parse(source);
            if (tree.ParserMessages.Count > 0)
            {
                foreach (var error in tree.ParserMessages)
                    Console.WriteLine(error.Location + ": " + error.Message);
                return null;
            }
            else
                ConsumeParseTree(root, tree.Root);
            return root;
        }

        public void ConsumeParseTree(Expression parent, ParseTreeNode node)
        {
            var parentKey = node.Term.ToString();
            if (Builders.ContainsKey(parentKey))
                Builders[parentKey](this, parent, node);
            else
            foreach (var childNode in node.ChildNodes)
            {
                var key = childNode.Term.ToString();
                if (Builders.ContainsKey(key))
                    Builders[key](this, parent, childNode);
                else
                    ConsumeParseTree(parent, childNode);
            }
        }
    }
}
