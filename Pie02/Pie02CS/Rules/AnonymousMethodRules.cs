using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class AnonymousMethodRules
    {
        public NonTerminal AnonymousMethod;

        public AnonymousMethodRules(PieGrammar grammar)
        {
            var expressionList = new NonTerminal("expression_list");
            expressionList.Rule = grammar.ToTerm("{") + grammar.MakePlusRule(expressionList, grammar.Expression) + "}";

            AnonymousMethod = new NonTerminal("anonymous_method");
             AnonymousMethod.Rule = grammar.MethodType
                 + grammar.LParens
                 + grammar.ParameterList
                 + grammar.RParens
                 + "->"
                 + grammar.Expression;
            AnonymousMethod.Rule |= grammar.MethodType
                + grammar.LParens
                + grammar.ParameterList
                + grammar.RParens
                + "->"
                + grammar.Eos
                + grammar.ExpressionList;
        }
    }
}
