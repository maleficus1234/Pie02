

Imports Irony.Parsing

Public Class ImportRules

    Public Import As New KeyTerm("import", "import")
    Public ImportList As New NonTerminal("importList")
    Public ImportDeclaration As New NonTerminal("importDeclaration")

    Public Sub New(grammar As PieGrammar)

        Dim qualifiedImport = New NonTerminal("qualifiedImport")
        qualifiedImport.Rule = grammar.MakePlusRule(qualifiedImport, grammar.Dot, grammar.Identifier)

        ImportList.Rule = grammar.MakePlusRule(ImportList, grammar.Comma, qualifiedImport)

        ImportDeclaration.Rule = Import + ImportList + grammar.Eos
    End Sub
End Class
