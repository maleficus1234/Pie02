using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class TestRules
    {
        public NonTerminal TestDeclaration;
        public NonTerminal AssertStatement;
        public TestRules(PieGrammar grammar)
        {
            TestDeclaration = new NonTerminal("test_declaration");
            TestDeclaration.Rule = grammar.ToTerm("test") + grammar.Identifier + ":" + grammar.Eos + grammar.MethodBody;

            AssertStatement = new NonTerminal("assert_statement");
            AssertStatement.Rule = grammar.ToTerm("assert") + grammar.Expression + grammar.Eos;
            grammar.Statement.Rule |= AssertStatement;
        }
    }
}
