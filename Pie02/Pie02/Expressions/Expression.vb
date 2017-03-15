Public Class Expression
    Public ParentExpression As Expression

    Public ChildExpressions As New List(Of Expression)

    Public Sub New(parentExpression As Expression)
        Me.ParentExpression = parentExpression
    End Sub

    Public Overridable Sub Generate(ByRef source As String)
        For Each node In ChildExpressions
            node.Generate(source)
        Next
    End Sub

End Class
