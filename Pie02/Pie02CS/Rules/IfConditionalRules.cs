using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class IfConditionalRules
    {
        public NonTerminal IfConditional;


        public IfConditionalRules(PieGrammar grammar)
        {
            IfConditional = new NonTerminal("if_conditional");
            IfConditional.Rule = grammar.ToTerm("if") + grammar.Expression + grammar.Statement;
            IfConditional.Rule |= grammar.ToTerm("if") + grammar.Expression + ":" + grammar.Eos + grammar.MethodBody;
            IfConditional.Rule |= grammar.ToTerm("if") + grammar.Expression + grammar.Statement + grammar.ToTerm("else") + grammar.Statement;
            IfConditional.Rule |= grammar.ToTerm("if") + grammar.Expression + grammar.Statement + grammar.ToTerm("else") + ":" + grammar.Eos + grammar.MethodBody;
            IfConditional.Rule |= grammar.ToTerm("if") + grammar.Expression + ":" + grammar.Eos + grammar.MethodBody + grammar.ToTerm("else") + grammar.Statement;
            IfConditional.Rule |= grammar.ToTerm("if") + grammar.Expression + ":" + grammar.Eos + grammar.MethodBody + grammar.ToTerm("else") + ":" + grammar.Eos + grammar.MethodBody;
        }
    }
}
