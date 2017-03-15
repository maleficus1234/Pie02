Imports Irony.Parsing

Public Class ClassRules

    Public ClassDeclaration As New NonTerminal("classDeclaration")

    Public Sub New(grammar As PieGrammar)

        ClassDeclaration.Rule = grammar.ClassT + grammar.Identifier + grammar.Colon + grammar.Eos

    End Sub

End Class
