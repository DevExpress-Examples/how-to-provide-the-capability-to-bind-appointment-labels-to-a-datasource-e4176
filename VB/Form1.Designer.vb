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
Namespace SchedulerMappedLabels
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim timeRuler1 As New DevExpress.XtraScheduler.TimeRuler()
			Dim timeRuler2 As New DevExpress.XtraScheduler.TimeRuler()
			Me.schedulerControl1 = New SchedulerMappedLabels.CustomSchedulerControl()
			Me.labelsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
			Me.carsDBDataSet = New SchedulerMappedLabels.CarsDBDataSet()
			Me.schedulerStorage1 = New DevExpress.XtraScheduler.SchedulerStorage(Me.components)
			Me.carSchedulingBindingSource = New System.Windows.Forms.BindingSource(Me.components)
			Me.carSchedulingTableAdapter = New SchedulerMappedLabels.CarsDBDataSetTableAdapters.CarSchedulingTableAdapter()
			Me.labelsTableAdapter = New SchedulerMappedLabels.CarsDBDataSetTableAdapters.LabelsTableAdapter()
			CType(Me.schedulerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.labelsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.carsDBDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.schedulerStorage1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.carSchedulingBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' schedulerControl1
			' 
			Me.schedulerControl1.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.schedulerControl1.LabelIdMappedName = "ID"
			Me.schedulerControl1.LabelsDataSource = Me.labelsBindingSource
			Me.schedulerControl1.Location = New System.Drawing.Point(12, 12)
			Me.schedulerControl1.Name = "schedulerControl1"
			Me.schedulerControl1.Size = New System.Drawing.Size(864, 381)
			Me.schedulerControl1.Start = New System.DateTime(2008, 9, 4, 0, 0, 0, 0)
			Me.schedulerControl1.Storage = Me.schedulerStorage1
			Me.schedulerControl1.TabIndex = 0
			Me.schedulerControl1.Text = "schedulerControl1"
			' 
			' labelsBindingSource
			' 
			Me.labelsBindingSource.DataMember = "Labels"
			Me.labelsBindingSource.DataSource = Me.carsDBDataSet
			' 
			' carsDBDataSet
			' 
			Me.carsDBDataSet.DataSetName = "CarsDBDataSet"
			Me.carsDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
			' 
			' schedulerStorage1
			' 
			Me.schedulerStorage1.Appointments.CommitIdToDataSource = False
			Me.schedulerStorage1.Appointments.DataSource = Me.carSchedulingBindingSource
			Me.schedulerStorage1.Appointments.Mappings.AppointmentId = "ID"
			Me.schedulerStorage1.Appointments.Mappings.Description = "Description"
			Me.schedulerStorage1.Appointments.Mappings.End = "EndTime"
			Me.schedulerStorage1.Appointments.Mappings.Label = "Label"
			Me.schedulerStorage1.Appointments.Mappings.Start = "StartTime"
			Me.schedulerStorage1.Appointments.Mappings.Subject = "Subject"
			' 
			' carSchedulingBindingSource
			' 
			Me.carSchedulingBindingSource.DataMember = "CarScheduling"
			Me.carSchedulingBindingSource.DataSource = Me.carsDBDataSet
			' 
			' carSchedulingTableAdapter
			' 
			Me.carSchedulingTableAdapter.ClearBeforeFill = True
			' 
			' labelsTableAdapter
			' 
			Me.labelsTableAdapter.ClearBeforeFill = True
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(888, 436)
			Me.Controls.Add(Me.schedulerControl1)
			Me.Name = "Form1"
			Me.Text = "Form1"
'			Me.Load += New System.EventHandler(Me.Form1_Load);
			CType(Me.schedulerControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.labelsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.carsDBDataSet, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.schedulerStorage1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.carSchedulingBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private schedulerControl1 As CustomSchedulerControl
		Private schedulerStorage1 As DevExpress.XtraScheduler.SchedulerStorage
		Private carsDBDataSet As CarsDBDataSet
		Private carSchedulingBindingSource As System.Windows.Forms.BindingSource
		Private carSchedulingTableAdapter As CarsDBDataSetTableAdapters.CarSchedulingTableAdapter
		Private labelsBindingSource As System.Windows.Forms.BindingSource
		Private labelsTableAdapter As CarsDBDataSetTableAdapters.LabelsTableAdapter
	End Class
End Namespace

