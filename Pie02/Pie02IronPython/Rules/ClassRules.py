from Irony.Parsing import *

class ClassRules(object):
    """description of class"""

    def __init__(self, grammar):    
        self.classType = NonTerminal("class_type")
        self.classType.Rule = grammar.Class | grammar.Module | grammar.Struct | grammar.Interface
        grammar.MarkTransient(self.classType)
           
        self.ClassDeclaration = NonTerminal("class_declaration")

        self.ClassMember = NonTerminal("class_member")
        self.ClassMember.Rule = self.ClassDeclaration
        self.ClassMember.Rule |= grammar.MethodDeclarationRules.MethodDeclaration

        self.ClassMemberList = NonTerminal("class_member_list")
        self.ClassMemberList.Rule = grammar.MakeStarRule(self.ClassMemberList, self.ClassMember)

        self.ClassBody = NonTerminal("class_body")
        self.ClassBody.Rule = grammar.Indent + self.ClassMemberList + grammar.Dedent
        self.ClassBody.Rule |= grammar.Empty

        self.ClassDeclaration.Rule = grammar.ModifierList + self.classType + grammar.Identifier + grammar.Colon + grammar.Eos + self.ClassBody