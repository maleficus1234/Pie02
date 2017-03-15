using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class ClassRules
    {
        public NonTerminal classType;
        public NonTerminal ClassDeclaration;
        public NonTerminal ClassMember;
        public NonTerminal ClassMemberList;
        public NonTerminal ClassBody;
        public NonTerminal ClassInherit;

        public ClassRules(PieGrammar grammar)
        {
            classType = new NonTerminal("class_type");
            classType.Rule = grammar.ToTerm("type") | grammar.ToTerm("module") | grammar.ToTerm("interface");
            grammar.MarkTransient(classType);

            ClassDeclaration = new NonTerminal("class_declaration");

            ClassMember = new NonTerminal("class_member");
            ClassMember.Rule = ClassDeclaration;
            ClassMember.Rule |= grammar.MethodDeclarationRules.MethodDeclaration;
            ClassMember.Rule |= grammar.FieldDeclarationRules.FieldDeclaration;
            ClassMember.Rule |= grammar.MethodDeclarationRules.ConstructorDeclaration;
            ClassMember.Rule |= grammar.TestRules.TestDeclaration;

            ClassMemberList = new NonTerminal("class_member_list");
            ClassMemberList.Rule = grammar.MakeStarRule(ClassMemberList, ClassMember);

            ClassBody = new NonTerminal("class_body");
            ClassBody.Rule = grammar.Indent + ClassMemberList + grammar.Dedent;
            ClassBody.Rule |= grammar.Empty;

            ClassInherit = new NonTerminal("class_inherit");
            ClassInherit.Rule = grammar.LParens + grammar.IdentifierList + grammar.RParens;
            ClassInherit.Rule |= grammar.Empty;

            ClassDeclaration.Rule = grammar.ModifierList + classType + grammar.Identifier + ClassInherit + ":" + grammar.Eos + ClassBody;
        }
    }
}
