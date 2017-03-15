using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class NamespaceRules
    {
        public NonTerminal NamespaceDeclaration;
        public NonTerminal NamespaceMember;
        public NonTerminal NamespaceMemberList;
        public NonTerminal NamespaceBody;

        public NamespaceRules(PieGrammar grammar)
        {
            NamespaceDeclaration = new NonTerminal("namespace_declaration");

            NamespaceMember = new NonTerminal("namespace_member");
            NamespaceMember.Rule = grammar.ImportRules.ImportDeclaration;
            NamespaceMember.Rule |= NamespaceDeclaration;
            NamespaceMember.Rule |= grammar.ClassRules.ClassDeclaration;

            NamespaceMemberList = new NonTerminal("namespace_member_list");
            NamespaceMemberList.Rule = grammar.MakeStarRule(NamespaceMemberList, NamespaceMember);
            NamespaceMemberList.Rule |= grammar.Empty;

            NamespaceBody = new NonTerminal("namespace_body");
            NamespaceBody.Rule = grammar.Indent + NamespaceMemberList + grammar.Dedent;
            NamespaceBody.Rule |= grammar.Empty;

            NamespaceDeclaration.Rule = grammar.ToTerm("namespace") + grammar.QualifiedIdentifier + ":" + grammar.Eos + NamespaceBody;
        }
    }
}
