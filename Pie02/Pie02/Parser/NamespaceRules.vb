Imports Irony.Parsing

Public Class NamespaceRules

    Public NamespaceDeclaration As New NonTerminal("namespaceDeclaration")
    Public NamespaceMember As New NonTerminal("namespaceMember")
    Public NamespaceMemberList As New NonTerminal("namespaceMemberList")

    Public Sub New(grammar As PieGrammar)

        NamespaceDeclaration.Rule = grammar.NamespaceT + grammar.QualifiedIdentifier + grammar.Colon + grammar.Eos

        NamespaceMember.Rule = grammar.ImportRules.ImportDeclaration Or NamespaceDeclaration Or grammar.ClassRules.ClassDeclaration
        grammar.MarkTransient(NamespaceMember)



        NamespaceMemberList.Rule = grammar.MakeStarRule(NamespaceMemberList, NamespaceMember)
    End Sub

End Class
