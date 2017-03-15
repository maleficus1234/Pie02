from Irony.Parsing import *

class NamespaceRules(object):
    """description of class"""

    def __init__(self, grammar):
        self.NamespaceDeclaration = NonTerminal("namespace_declaration")

        self.NamespaceMember = NonTerminal("namespace_member")
        self.NamespaceMember.Rule = grammar.ImportRules.ImportDeclaration
        self.NamespaceMember.Rule |= self.NamespaceDeclaration
        self.NamespaceMember.Rule |= grammar.ClassRules.ClassDeclaration

        self.NamespaceMemberList = NonTerminal("namespace_member_list")
        self.NamespaceMemberList.Rule = grammar.MakeStarRule(self.NamespaceMemberList, self.NamespaceMember)

        self.NamespaceBody = grammar.Indent + self.NamespaceMemberList + grammar.Dedent
        self.NamespaceBody |= grammar.Empty

        self.NamespaceDeclaration.Rule = grammar.Namespace + grammar.QualifiedIdentifier + grammar.Colon + grammar.Eos + self.NamespaceBody