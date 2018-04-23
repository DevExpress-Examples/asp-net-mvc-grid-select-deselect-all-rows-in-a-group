Imports System.Data
Imports System.Data.OleDb
Imports System.Web.Configuration

Public Class NorthwindDataProvider
	Public Shared Function GetProducts() As DataTable
		Dim connection As New OleDbConnection(WebConfigurationManager.ConnectionStrings("Northwind").ConnectionString)
		Dim selectCommand As New OleDbCommand("SELECT * FROM Products ORDER BY ProductID", connection)
		Dim da As New OleDbDataAdapter(selectCommand)
		Dim ds As New DataSet()

		selectCommand.Connection = connection
		da.Fill(ds, "Products")

		Return ds.Tables("Products")
	End Function
End Class
