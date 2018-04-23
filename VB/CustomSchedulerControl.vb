' Developer Express Code Central Example:
' How to provide the capability to bind appointment labels to a datasource
' 
' Due to numerous requests from our customers regarding the capability to bind
' appointment labels/statuses to a datasource, we have created this sample. Note
' that in the past, we tried to address this issue in the context of the following
' examples:
' http://www.devexpress.com/scid=E2028
' http://www.devexpress.com/scid=E2087
' They
' illustrate how to load labels form an external datasource. However, one
' limitation is still there. It is related to the default meaning of the
' Appointment.LabelId Property
' (http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerAppointment_LabelIdtopic).
' The value of this property represents an index of a label in the
' AppointmentStorage.Labels
' (http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerAppointmentStorage_Labelstopic)
' (this label is used for this appointment).This mean that once you remove a
' particular label for this collection, indexes will be shifted. Take a moment to
' look at the http://www.devexpress.com/scid=Q413689 ticket, which describes this
' issue in detail.
' Apparently, a more advanced labels/status identification
' mechanism is required. This example illustrates how to implement this mechanism
' for labels (you can use the same approach for statuses) by extending the
' SchedulerControl Class
' (http://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraSchedulerSchedulerControltopic).
' The main idea of the approach illustrated here is to define a separate
' datasource for appointment labels (the LabelsDataSource property) and mapped
' field names for Id, Color and DisplayName (the LabelIdMappedName,
' LabelColorMappedName and LabelDisplayNameMappedName properties). If the
' datasource is not specified, we are using default label items (see the
' PopulateDefaultLabels() method). Otherwise, labels from a datasource are used.
' Note that the Appointment.LabelId property has another meaning in this scenario.
' The value of this property is used to look up a corresponding label item in the
' SchedulerControl.AppointmentViewInfoCustomizing Event
' (http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerSchedulerControl_AppointmentViewInfoCustomizingtopic)
' in order to assign a color defined in this label to the appointment. In
' addition, we handle the SchedulerControl.PopupMenuShowing Event
' (http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerSchedulerControl_PopupMenuShowingtopic)
' to populate the LabelSubMenu with custom menu items created on the fly based on
' the rows in the datasource with labels.
' To correctly display custom
' appointments' labels in the EditAppointmentForm, we override the UpdateFormCore
' and edtLabel_EditValueChanged methods in a corresponding EditAppointmentForm
' descendant. The important thing is that a SchedulerStorage instance should
' contain custom appointments' labels in its internal collection. We use the
' SchedulerControl.PopulateLabelsStorage method to replace the collection of
' pre-defined labels with a custom one.
' 
' Note that we are using a custom-made
' DataBindingController class to operate with a label datasource in a generic
' manner. This means that our approach should work correctly regardless of the
' actual datasource type, be it a DataTable or a List<T>.
' Here is a screenshot
' that illustrates meaning of the Appointment.LabelId property (pay attention to
' the CarScheduling.Label and Labels.Id field values):
' 
' Finally, we have
' implemented design-time support for the aforementioned properties:
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E4176


Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports DevExpress.Utils
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Native
Imports System.Windows.Forms
Imports System.Collections.Generic

Namespace SchedulerMappedLabels
	Public Class CustomSchedulerControl
		Inherits SchedulerControl
		Public Sub New()
			AddHandler PopupMenuShowing, AddressOf CustomSchedulerControl_PopupMenuShowing
			AddHandler AppointmentViewInfoCustomizing, AddressOf CustomSchedulerControl_AppointmentViewInfoCustomizing

			LabelIdMappedName = defaultLabelIdMappedName
			LabelColorMappedName = defaultLabelColorMappedName
			LabelDisplayNameMappedName = defaultLabelDisplayNameMappedName

			_customInnerLables = New Dictionary(Of Integer, AppointmentLabel)()
		End Sub

		Private labelsDataSource_Renamed As Object

		<AttributeProvider(GetType(IListSource)), Category("Data")> _
		Public Property LabelsDataSource() As Object
			Get
				Return labelsDataSource_Renamed
			End Get
			Set(ByVal value As Object)
				labelsDataSource_Renamed = value
				PopulateLabelsStorage()
				Refresh()
			End Set
		End Property

		Private _customInnerLables As Dictionary(Of Integer, AppointmentLabel)
		Friend ReadOnly Property CustomInnerLables() As Dictionary(Of Integer, AppointmentLabel)
			Get
				Return _customInnerLables
			End Get
		End Property

		Private Sub CustomSchedulerControl_ListChanged(ByVal sender As Object, ByVal e As ListChangedEventArgs)

		End Sub

		Private Const defaultLabelIdMappedName As String = "Id"
		Private Const defaultLabelColorMappedName As String = "Color"
		Private Const defaultLabelDisplayNameMappedName As String = "DisplayName"

		Private privateLabelIdMappedName As String
		<Category("Data"), DefaultValue(defaultLabelIdMappedName), TypeConverter(GetType(LabelColumnNameConverter))> _
		Public Property LabelIdMappedName() As String
			Get
				Return privateLabelIdMappedName
			End Get
			Set(ByVal value As String)
				privateLabelIdMappedName = value
			End Set
		End Property
		Private privateLabelColorMappedName As String
		<Category("Data"), DefaultValue(defaultLabelColorMappedName), TypeConverter(GetType(LabelColumnNameConverter))> _
		Public Property LabelColorMappedName() As String
			Get
				Return privateLabelColorMappedName
			End Get
			Set(ByVal value As String)
				privateLabelColorMappedName = value
			End Set
		End Property
		Private privateLabelDisplayNameMappedName As String
		<Category("Data"), DefaultValue(defaultLabelDisplayNameMappedName), TypeConverter(GetType(LabelColumnNameConverter))> _
		Public Property LabelDisplayNameMappedName() As String
			Get
				Return privateLabelDisplayNameMappedName
			End Get
			Set(ByVal value As String)
				privateLabelDisplayNameMappedName = value
			End Set
		End Property

		Public Sub PopulateLabelsStorage()
			Dim labelsController As New DataBindingController(labelsDataSource_Renamed, String.Empty)
			If Me.Storage IsNot Nothing Then
				Me.Storage.Appointments.Labels.Clear()
			End If
			If _customInnerLables IsNot Nothing Then
				_customInnerLables.Clear()
			End If
			For i As Integer = 0 To labelsController.ItemsCount - 1
				Dim currentColor As Color = Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i)))
				Dim iWidth As Integer = 16
				Dim iHeight As Integer = 16
				Dim bmp As New Bitmap(iWidth, iHeight)
				Using g As Graphics = Graphics.FromImage(bmp)
					g.DrawRectangle(New Pen(Color.Black, 2), 0, 0, iWidth, iHeight)
					g.FillRectangle(New SolidBrush(currentColor), 1, 1, iWidth - 2, iHeight - 2)

				End Using
				Dim newLabel As New AppointmentLabel(Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i))), labelsController.GetRowValue(LabelDisplayNameMappedName, i).ToString())
				Me.Storage.Appointments.Labels.Add(newLabel)
				_customInnerLables.Add(Convert.ToInt32(labelsController.GetRowValue(LabelIdMappedName, i)), newLabel)
			Next i
		End Sub

		Public Sub PopulateDefaultLabels()
			Dim defaultLabels As New DataTable("DefaultLabels")

			defaultLabels.Columns.Add(LabelIdMappedName, GetType(Integer))
			defaultLabels.Columns.Add(LabelColorMappedName, GetType(Integer))
			defaultLabels.Columns.Add(LabelDisplayNameMappedName, GetType(String))

			Dim innerLabels As AppointmentLabelCollection = Me.Storage.Appointments.Labels

			For i As Integer = 0 To innerLabels.Count - 1
				defaultLabels.Rows.Add(New Object() { i, innerLabels(i).Color.ToArgb(), innerLabels(i).DisplayName })
			Next i

			LabelsDataSource = defaultLabels
		End Sub

		Private Sub CustomSchedulerControl_PopupMenuShowing(ByVal sender As Object, ByVal e As PopupMenuShowingEventArgs)
			If e.Menu.Id = SchedulerMenuItemId.AppointmentMenu Then
				Dim labelSubMenu As SchedulerPopupMenu = e.Menu.GetPopupMenuById(SchedulerMenuItemId.LabelSubMenu)
				Dim labelsController As New DataBindingController(LabelsDataSource, String.Empty)

				If labelsController.ItemsCount = 0 Then
					e.Menu.RemoveMenuItem(SchedulerMenuItemId.LabelSubMenu)
					Return
				End If

				labelSubMenu.Items.Clear()

				For i As Integer = 0 To labelsController.ItemsCount - 1
					Dim labelId As Object = labelsController.GetRowValue(LabelIdMappedName, i)

					Dim menuItem As New SchedulerMenuCheckItem(labelsController.GetRowValue(LabelDisplayNameMappedName, i).ToString(), Me.SelectedAppointments(0).LabelId.Equals(labelId), UserInterfaceObjectHelper.CreateBitmap(New AppointmentLabel(Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i))), String.Empty), &H10, &H10), AddressOf OnCheckedChanged)

					menuItem.Tag = labelId
					labelSubMenu.Items.Add(menuItem)
				Next i
			End If
		End Sub

		Private Sub OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
			Me.SelectedAppointments(0).LabelId = Convert.ToInt32((CType(sender, DXMenuItem)).Tag, System.Globalization.CultureInfo.InvariantCulture)
		End Sub

		Private Sub CustomSchedulerControl_AppointmentViewInfoCustomizing(ByVal sender As Object, ByVal e As AppointmentViewInfoCustomizingEventArgs)
			If LabelsDataSource IsNot Nothing Then
				e.ViewInfo.Appearance.BackColor = GetColorByLabelId(e.ViewInfo.Appointment.LabelId)
			End If
		End Sub

		Private Function GetColorByLabelId(ByVal Id As Integer) As Color
			Dim labelsController As New DataBindingController(LabelsDataSource, String.Empty)

			For i As Integer = 0 To labelsController.ItemsCount - 1
				If Convert.ToInt32(labelsController.GetRowValue(LabelIdMappedName, i)) = Id Then
					Return Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i)))
				End If
			Next i

			Return Color.White
		End Function

		Protected Overrides Function CreateAppointmentForm(ByVal control As SchedulerControl, ByVal apt As Appointment, ByVal openRecurrenceForm As Boolean) As DevExpress.XtraScheduler.UI.AppointmentForm
			Return New CustomAppointmentForm(control, apt, openRecurrenceForm)
		End Function
	End Class
End Namespace