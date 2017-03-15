using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class MethodDeclarationRules
    {

        public NonTerminal MethodDeclaration;

        public NonTerminal ConstructorDeclaration;

        public MethodDeclarationRules(PieGrammar grammar)
        {


            MethodDeclaration = new NonTerminal("method_declaration");
            MethodDeclaration.Rule = grammar.ModifierList + grammar.MethodType + grammar.Identifier + grammar.LParens + grammar.ParameterList + grammar.RParens + ":" + grammar.Eos + grammar.MethodBody;

            ConstructorDeclaration = new NonTerminal("constructor_declaration");
            ConstructorDeclaration.Rule = grammar.ModifierList + grammar.ToTerm("new") + grammar.LParens + grammar.ParameterList + grammar.RParens + ":" + grammar.Eos + grammar.MethodBody;

        }
    }
}
