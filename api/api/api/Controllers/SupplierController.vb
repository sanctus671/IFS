Imports System.Net
Imports System.Web.Http

using api.Models;

Namespace Controllers
    Public Class SupplierController
        Inherits ApiController

        ' GET: api/Supplier
        Public Function GetValues() As IEnumerable(Of String)
            Return New String() {"value1", "value2"}
        End Function

        ' GET: api/Supplier/5
        Public Function GetValue(ByVal id As Integer) As String
            Return "value"
        End Function

        ' POST: api/Supplier
        Public Sub PostValue(<FromBody()> ByVal value As String)

        End Sub

        ' PUT: api/Supplier/5
        Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        End Sub

        ' DELETE: api/Supplier/5
        Public Sub DeleteValue(ByVal id As Integer)

        End Sub
    End Class
End Namespace