using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class MethodInvocationRules
    {
        public NonTerminal MethodInvocation;
        public NonTerminal AccessedMethodInvocation;
        public MethodInvocationRules(PieGrammar grammar)
        {
            MethodInvocation = new NonTerminal("method_invocation");
            MethodInvocation.Rule = grammar.Identifier + grammar.LParens + grammar.ExpressionList + grammar.RParens;
            MethodInvocation.Rule |= grammar.ToTerm("base") + grammar.LParens + grammar.ExpressionList + grammar.RParens;

            AccessedMethodInvocation = new NonTerminal("accessed_method_invocation");
            AccessedMethodInvocation.Rule = grammar.Expression + "." + MethodInvocation;
        }
    }
}
