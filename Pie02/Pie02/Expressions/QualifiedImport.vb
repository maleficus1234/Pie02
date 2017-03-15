Public Class QualifiedImport
    Inherits Expression

    Public Identifiers As New List(Of String)

    Public Sub New(parentExpression As Expression)
        MyBase.New(parentExpression)


    End Sub

    Public Shared Sub Build(parser As IronyParser, parentExpression As Expression, node As Irony.Parsing.ParseTreeNode)

        Dim import As New QualifiedImport(parentExpression)
        parentExpression.ChildExpressions.Add(import)

        For Each child In node.ChildNodes
            import.Identifiers.Add(child.FindTokenAndGetText())
        Next
    End Sub



    Public Overrides Sub Generate(ByRef source As String)
        source += "using "

        source += Identifiers(0)

        For i = 1 To Identifiers.Count - 1
            source += "." + Identifiers(i)
        Next

        source += ";" + Environment.NewLine
    End Sub

End Class
