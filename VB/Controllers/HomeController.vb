Imports System.Web.Mvc

Namespace GridViewGroupSelectionMvc
	Public Class HomeController
		Inherits Controller

		Public Function Index() As ActionResult
			Return View()
		End Function

		Public Function GridViewPartial() As ActionResult
			Return PartialView(NorthwindDataProvider.GetProducts())
		End Function

		Public Function GridViewCustomActionPartial(ByVal parameters As String) As ActionResult
			ViewData("parameters") = parameters
			Return PartialView("GridViewPartial", NorthwindDataProvider.GetProducts())
		End Function
	End Class
End Namespace