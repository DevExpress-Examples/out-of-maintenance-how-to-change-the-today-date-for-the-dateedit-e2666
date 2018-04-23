Imports Microsoft.VisualBasic
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Drawing
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraEditors.Controls
Imports System
Imports DevExpress.XtraEditors.Calendar
Imports System.Drawing

Namespace DXSample
	Public Class MyDateEdit
		Inherits DateEdit
		Shared Sub New()
			RepositoryItemMyDateEdit.RegisterMyDateEdit()
		End Sub
		Public Sub New()
			MyBase.New()
		End Sub

		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return RepositoryItemMyDateEdit.MyDateEditName
			End Get
		End Property

		<DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public Shadows ReadOnly Property Properties() As RepositoryItemMyDateEdit
			Get
				Return CType(MyBase.Properties, RepositoryItemMyDateEdit)
			End Get
		End Property

		Protected Overrides Function CreatePopupForm() As PopupBaseForm
			If Properties.IsVistaDisplayModeInternal Then
				Return New MyVistaPopupDateEditForm(Me)
			End If
			Return New MyPopupDateEditForm(Me)
		End Function
	End Class

	<UserRepositoryItem("RegisterMyDateEdit")> _
	Public Class RepositoryItemMyDateEdit
		Inherits RepositoryItemDateEdit
		Shared Sub New()
			RegisterMyDateEdit()
		End Sub
		Public Sub New()
			MyBase.New()
		End Sub

		Friend Const MyDateEditName As String = "MyDateEdit"
		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return MyDateEditName
			End Get
		End Property

		Public Shared Sub RegisterMyDateEdit()
			EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(MyDateEditName, GetType(MyDateEdit), GetType(RepositoryItemMyDateEdit), GetType(DateEditViewInfo), New ButtonEditPainter(), True))
		End Sub

		Friend ReadOnly Property IsVistaDisplayModeInternal() As Boolean
			Get
				Return IsVistaDisplayMode()
			End Get
		End Property

		Private Shared ReadOnly fCustomToday As Object = New Object()
		Public Custom Event CustomToday As EventHandler(Of CustomTodayEventArgs)
			AddHandler(ByVal value As EventHandler(Of CustomTodayEventArgs))
				Events.AddHandler(fCustomToday, value)
			End AddHandler
			RemoveHandler(ByVal value As EventHandler(Of CustomTodayEventArgs))
				Events.RemoveHandler(fCustomToday, value)
			End RemoveHandler
            RaiseEvent(ByVal sender As System.Object, ByVal e As CustomTodayEventArgs)
            End RaiseEvent
		End Event
		Public Function GetCustomToday() As DateTime
			Dim handler As EventHandler(Of CustomTodayEventArgs) = CType(Events(fCustomToday), EventHandler(Of CustomTodayEventArgs))
			If handler Is Nothing Then
				Return DateTime.Today
			End If
			Dim args As New CustomTodayEventArgs()
			handler(Me, args)
			Return args.Today
		End Function

		Public Overrides Sub Assign(ByVal item As RepositoryItem)
			MyBase.Assign(item)
			BeginUpdate()
			Try
				Dim source As RepositoryItemMyDateEdit = CType(item, RepositoryItemMyDateEdit)
				Events.AddHandler(fCustomToday, source.Events(fCustomToday))
			Finally
				EndUpdate()
			End Try
		End Sub
	End Class

	Public Class MyPopupDateEditForm
		Inherits PopupDateEditForm
		Public Sub New(ByVal ownerEdit As MyDateEdit)
			MyBase.New(ownerEdit)
		End Sub

		Public Shadows ReadOnly Property OwnerEdit() As MyDateEdit
			Get
				Return CType(MyBase.OwnerEdit, MyDateEdit)
			End Get
		End Property

		Protected Overrides Function CreateCalendar() As DateEditCalendar
			Return New MyDateEditCalendar(OwnerEdit.Properties, OwnerEdit.EditValue)
		End Function

		Protected Overrides Sub ResetState()
			Dim [date] As DateTime = OwnerEdit.DateTime
			If [date] = DateTime.MinValue Then
				[date] = OwnerEdit.Properties.GetCustomToday()
			End If
			Calendar.ResetState(OwnerEdit.EditValue, [date])
		End Sub
	End Class

	Public Class MyVistaPopupDateEditForm
		Inherits VistaPopupDateEditForm
		Public Sub New(ByVal ownerEdit As MyDateEdit)
			MyBase.New(ownerEdit)
		End Sub

		Public Shadows ReadOnly Property OwnerEdit() As MyDateEdit
			Get
				Return CType(MyBase.OwnerEdit, MyDateEdit)
			End Get
		End Property

		Protected Overrides Function CreateCalendar() As DateEditCalendar
			Dim result As VistaDateEditCalendar = New MyVistaDateEditCalendar(OwnerEdit.Properties, OwnerEdit.EditValue)
            AddHandler result.OkClick, AddressOf OnOkClick
			Return result
		End Function

		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso Calendar IsNot Nothing Then
                RemoveHandler CType(Calendar, VistaDateEditCalendar).OkClick, AddressOf OnOkClick
			End If
			MyBase.Dispose(disposing)
		End Sub

		Protected Overrides Sub ResetState()
			Dim [date] As DateTime = OwnerEdit.DateTime
			If [date] = DateTime.MinValue Then
				[date] = OwnerEdit.Properties.GetCustomToday()
			End If
			Calendar.ResetState(OwnerEdit.EditValue, [date])
		End Sub
	End Class

	Public Class MyDateEditCalendar
		Inherits DateEditCalendar
		Public Sub New(ByVal item As RepositoryItemMyDateEdit, ByVal editDate As Object)
			MyBase.New(item, editDate)
		End Sub

		<DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public Shadows ReadOnly Property Properties() As RepositoryItemMyDateEdit
			Get
				Return CType(MyBase.Properties, RepositoryItemMyDateEdit)
			End Get
		End Property

		Protected Overrides Sub OnTodayButtonClick(ByVal sender As Object, ByVal e As EventArgs)
			Dim today As DateTime = Properties.GetCustomToday()
			SetSelection(today)
			OnDateTimeCommit(today, False)
		End Sub

		Protected Overrides Function CreatePainter() As DateEditPainter
			Return New MyDateEditPainter(Me)
		End Function
	End Class

	Public Class MyVistaDateEditCalendar
		Inherits VistaDateEditCalendar
		Public Sub New(ByVal item As RepositoryItemMyDateEdit, ByVal editDate As Object)
			MyBase.New(item, editDate)
		End Sub

		<DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public Shadows ReadOnly Property Properties() As RepositoryItemMyDateEdit
			Get
				Return CType(MyBase.Properties, RepositoryItemMyDateEdit)
			End Get
		End Property

		Protected Overrides Sub OnTodayButtonClick(ByVal sender As Object, ByVal e As EventArgs)
			Dim today As DateTime = Properties.GetCustomToday()
			SetSelection(today)
			OnDateTimeCommit(today, False)
		End Sub

		Protected Overrides Function CreatePainter() As DateEditPainter
			Return New MyVistaDateEditPainter(Me)
		End Function

		Protected Overrides Function CreateInfoArgs() As DateEditInfoArgs
			Return New MyVistaDateEditInfoArgs(Me)
		End Function
	End Class

	Public Class CustomTodayEventArgs
		Inherits EventArgs
		Private fToday As DateTime = DateTime.Today
		Public Property Today() As DateTime
			Get
				Return fToday
			End Get
			Set(ByVal value As DateTime)
				fToday = value
			End Set
		End Property
	End Class

	Public Class MyDateEditPainter
		Inherits DateEditPainter
		Public Sub New(ByVal calendar As DateEditCalendarBase)
			MyBase.New(calendar)
		End Sub

		Protected Overrides Sub DrawDayCell(ByVal info As CalendarObjectInfoArgs, ByVal cell As DayNumberCellInfo)
			cell.Today = cell.Date = fToday
			MyBase.DrawDayCell(info, cell)
		End Sub

		Private fToday As DateTime
		Protected Overrides Sub DrawMonthNumbers(ByVal info As CalendarObjectInfoArgs)
			fToday = (CType(Calendar, MyDateEditCalendar)).Properties.GetCustomToday()
			MyBase.DrawMonthNumbers(info)
		End Sub
	End Class

	Public Class MyVistaDateEditPainter
		Inherits VistaDateEditPainter
		Public Sub New(ByVal calendar As DateEditCalendarBase)
			MyBase.New(calendar)
		End Sub

		Protected Overrides Sub DrawDayCell(ByVal info As CalendarObjectInfoArgs, ByVal cell As DayNumberCellInfo)
			cell.Today = cell.Date = fToday
			MyBase.DrawDayCell(info, cell)
		End Sub

		Private fToday As DateTime
		Protected Overrides Sub DrawMonthNumbers(ByVal info As CalendarObjectInfoArgs)
			fToday = (CType(Calendar, MyVistaDateEditCalendar)).Properties.GetCustomToday()
			MyBase.DrawMonthNumbers(info)
		End Sub
	End Class

	Public Class MyVistaDateEditInfoArgs
		Inherits VistaDateEditInfoArgs
		Public Sub New(ByVal calendar As DateEditCalendarBase)
			MyBase.New(calendar)
		End Sub

		Private fToday As DateTime
		Protected Overrides Sub CalcDayNumberCells()
			fToday = (CType(Calendar, MyVistaDateEditCalendar)).Properties.GetCustomToday()
			MyBase.CalcDayNumberCells()
		End Sub

		Protected Overrides Sub UpdateCellAppearance(ByVal cell As DayNumberCellInfo)
			cell.Today = fToday = cell.Date
			MyBase.UpdateCellAppearance(cell)
		End Sub
	End Class
End Namespace