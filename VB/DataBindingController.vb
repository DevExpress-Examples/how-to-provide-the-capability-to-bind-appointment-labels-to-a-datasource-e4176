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
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace SchedulerMappedLabels
	Public Class DataBindingController
		Private Shared bindingContext As New BindingContext()
		Private dataSource As Object
		Private dataMember As String

		Public Sub New(ByVal dataSource As Object, ByVal dataMember As String)
			Me.dataSource = dataSource
			Me.dataMember = dataMember
		End Sub

		Private Function GetBindingManager() As BindingManagerBase
			Dim bindingManager As BindingManagerBase = Nothing

			If dataSource IsNot Nothing Then
				If dataMember.Length > 0 Then
					bindingManager = bindingContext(dataSource, dataMember)
				Else
					bindingManager = bindingContext(dataSource)
				End If
			End If

			Return bindingManager
		End Function

		Public ReadOnly Property ItemsCount() As Integer
			Get
				Dim bindingManager As BindingManagerBase = GetBindingManager()
				Return If((bindingManager IsNot Nothing), bindingManager.Count, 0)
			End Get
		End Property

		Private Function GetItemProperties() As PropertyDescriptorCollection
			Dim bindingManager As BindingManagerBase = GetBindingManager()
			Return If((bindingManager IsNot Nothing), bindingManager.GetItemProperties(), Nothing)
		End Function

		Public Function GetColumnNames() As List(Of String)
			Dim list As New List(Of String)()
			Dim itemProperties As PropertyDescriptorCollection = GetItemProperties()

			If itemProperties IsNot Nothing Then
				Dim count As Integer = itemProperties.Count
				For i As Integer = 0 To count - 1
					list.Add(itemProperties(i).Name)
				Next i
			End If

			Return list
		End Function

		Public Function GetRowValue(ByVal columnName As String, ByVal rowIndex As Integer) As Object
			Dim bindingManager As BindingManagerBase = GetBindingManager()
			Dim itemProperties As PropertyDescriptorCollection = GetItemProperties()

			If bindingManager IsNot Nothing AndAlso itemProperties IsNot Nothing Then
				Dim prop As PropertyDescriptor = itemProperties.Find(columnName, False)

				If prop Is Nothing Then
					Throw New ArgumentException(String.Format("'{0}' column does not exist", columnName))
				End If

				If TypeOf bindingManager Is CurrencyManager Then
					Return prop.GetValue((CType(bindingManager, CurrencyManager)).List(rowIndex))
				Else
					If rowIndex <> 0 Then
						Throw New ArgumentOutOfRangeException("rowIndex")
					End If
					Return prop.GetValue((CType(bindingManager, PropertyManager)).Current)
				End If
			End If

			Return Nothing
		End Function
	End Class
End Namespace