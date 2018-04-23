Imports System

Imports System.Windows.Forms

Namespace CustomTodayExample
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
			dateEdit1.Properties.TodayDate = New Date(2015, 11, 30, 0, 0, 0, 0)
		End Sub
	End Class
End Namespace