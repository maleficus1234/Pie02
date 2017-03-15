Imports Irony.Parsing

Public Class PieGrammar
    Inherits Grammar

    Public ClassT As New KeyTerm("class", "class")
    Public Colon As New KeyTerm(":", ":")
    Public Comma As New KeyTerm(",", ",")
    Public Dot As New KeyTerm(".", ".")
    Public Identifier As New IdentifierTerminal("identifier")
    Public Import As New KeyTerm("import", "import")
    Public NamespaceT As New KeyTerm("namespace", "namespace")

    Public ImportRules As ImportRules
    Public NamespaceRules As NamespaceRules
    Public ClassRules As ClassRules

    Public QualifiedIdentifier As New NonTerminal("qualifiedIdentifier")

    Public Sub New()

        QualifiedIdentifier.Rule = MakePlusRule(QualifiedIdentifier, Dot, Identifier)

        ImportRules = New ImportRules(Me)
        ClassRules = New ClassRules(Me)
        NamespaceRules = New NamespaceRules(Me)

        Dim compileUnit = New NonTerminal("compileUnit")
        compileUnit.Rule = NamespaceRules.NamespaceMemberList

        Me.Root = compileUnit
    End Sub

    Public Overrides Sub CreateTokenFilters(language As LanguageData, filters As TokenFilterList)
        Dim outlineFilter = New CodeOutlineFilter(language.GrammarData,
              OutlineOptions.ProduceIndents Or OutlineOptions.CheckOperator Or OutlineOptions.CheckBraces, ToTerm("\"))
        filters.Add(outlineFilter)
    End Sub


End Class
