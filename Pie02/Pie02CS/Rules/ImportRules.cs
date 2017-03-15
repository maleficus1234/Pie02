using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class ImportRules
    {
        public NonTerminal ImportedName;
        public NonTerminal ImportList;
        public NonTerminal ImportDeclaration;

        public ImportRules(PieGrammar grammar)
        {
            ImportedName = new NonTerminal("qualified_import");
            ImportedName.Rule = grammar.QualifiedIdentifier;

            ImportList = new NonTerminal("import_list");
            ImportList.Rule = grammar.MakePlusRule(ImportList, grammar.Comma, ImportedName);

            ImportDeclaration = new NonTerminal("import_declaration");
            ImportDeclaration.Rule = grammar.ToTerm("import") + ImportList + grammar.Eos;
        }
    }
}
