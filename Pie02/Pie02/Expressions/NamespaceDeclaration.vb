Public Class NamespaceDeclaration
    Inherits Expression

    Public Identifiers As New List(Of String)

    Public Sub New(parentExpression As Expression)
        MyBase.New(parentExpression)


    End Sub

    Public Shared Sub Build(parser As IronyParser, parentExpression As Expression, node As Irony.Parsing.ParseTreeNode)

        Dim n As New NamespaceDeclaration(parentExpression)
        parentExpression.ChildExpressions.Add(n)

        For Each child In node.ChildNodes(1).ChildNodes
            n.Identifiers.Add(child.FindTokenAndGetText())
        Next
    End Sub



    Public Overrides Sub Generate(ByRef source As String)
        source += "namespace "

        source += Identifiers(0)

        For i = 1 To Identifiers.Count - 1
            source += "." + Identifiers(i)
        Next

        source += Environment.NewLine

        source += "{" + Environment.NewLine

        MyBase.Generate(source)

        source += "}" + Environment.NewLine
    End Sub
End Class
