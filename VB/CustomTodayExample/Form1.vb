Imports Microsoft.VisualBasic
Imports System
Imports DXSample
Imports System.Windows.Forms

Namespace CustomTodayExample
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub myDateEdit1_Properties_CustomToday(ByVal sender As Object, ByVal e As CustomTodayEventArgs) Handles myDateEdit1.Properties.CustomToday
			e.Today = DateTime.Today.AddDays(1)
		End Sub
	End Class
End Namespace