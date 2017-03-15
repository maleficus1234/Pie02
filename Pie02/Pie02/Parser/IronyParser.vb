
Public Delegate Sub BuildExpressionDelegate(parser As IronyParser, parentExpression As Expression, node As Irony.Parsing.ParseTreeNode)

Public Class IronyParser
    Private parser As Irony.Parsing.Parser

    Private builders As Dictionary(Of String, BuildExpressionDelegate)

    Public Sub New()
        parser = New Irony.Parsing.Parser(New PieGrammar())

        builders = New Dictionary(Of String, BuildExpressionDelegate) From
            {
                {"qualifiedImport", AddressOf QualifiedImport.Build},
                {"namespaceDeclaration", AddressOf NamespaceDeclaration.Build}
            }
    End Sub

    Public Sub Parse(root As Expression, source As String)

        Dim tree = Me.parser.Parse(source)
        ConsumeParseTree(root, tree.Root)

    End Sub

    Public Sub ConsumeParseTree(parentExpression As Expression, node As Irony.Parsing.ParseTreeNode)
        For Each child In node.ChildNodes
            If builders.ContainsKey(child.Term.ToString()) Then
                builders(child.Term.ToString())(Me, parentExpression, child)
            Else
                ConsumeParseTree(parentExpression, child)
            End If
        Next
    End Sub

End Class
