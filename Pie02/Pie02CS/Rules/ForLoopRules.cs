using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class ForLoopRules
    {
        public NonTerminal ForEachLoop;
        public NonTerminal WhileLoop;
        public NonTerminal ForLoop;

        public ForLoopRules(PieGrammar grammar)
        {
            ForEachLoop = new NonTerminal("foreach_loop");
            ForEachLoop.Rule = grammar.ToTerm("for") + grammar.QualifiedIdentifier + "in" + grammar.Expression + ":" + grammar.Eos + grammar.MethodBody;
            ForEachLoop.Rule |= grammar.ToTerm("for") + grammar.QualifiedIdentifier + "in" + grammar.Expression + grammar.Statement;

            WhileLoop = new NonTerminal("while_loop");
            WhileLoop.Rule = grammar.ToTerm("while") + grammar.Expression + ":" + grammar.Eos + grammar.MethodBody;
            WhileLoop.Rule |= grammar.ToTerm("while") + grammar.Expression + grammar.Statement;

            ForLoop = new NonTerminal("for_loop");
            ForLoop.Rule = grammar.ToTerm("for") + grammar.Statement + grammar.Statement + grammar.Statement + ":" + grammar.Eos + grammar.MethodBody;
        }
    }
}
