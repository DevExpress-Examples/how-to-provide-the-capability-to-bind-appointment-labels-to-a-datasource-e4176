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
            Me.schedulerControl1 = New SchedulerMappedLabels.CustomSchedulerControl()
            Me.labelsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.carsDBDataSet_Renamed = New SchedulerMappedLabels.CarsDBDataSet()
            Me.schedulerStorage1 = New DevExpress.XtraScheduler.SchedulerStorage(Me.components)
            Me.carSchedulingBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.carSchedulingTableAdapter = New SchedulerMappedLabels.CarsDBDataSetTableAdapters.CarSchedulingTableAdapter()
            Me.labelsTableAdapter = New SchedulerMappedLabels.CarsDBDataSetTableAdapters.LabelsTableAdapter()
            CType(Me.schedulerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.labelsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.carsDBDataSet_Renamed, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.schedulerStorage1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.carSchedulingBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' schedulerControl1
            ' 
            Me.schedulerControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.schedulerControl1.LabelIdMappedName = "ID"
            Me.schedulerControl1.LabelsDataSource = Me.labelsBindingSource
            Me.schedulerControl1.Location = New System.Drawing.Point(0, 0)
            Me.schedulerControl1.Name = "schedulerControl1"
            Me.schedulerControl1.Size = New System.Drawing.Size(888, 436)
            Me.schedulerControl1.Start = New Date(2008, 9, 4, 0, 0, 0, 0)
            Me.schedulerControl1.Storage = Me.schedulerStorage1
            Me.schedulerControl1.TabIndex = 0
            Me.schedulerControl1.Text = "schedulerControl1"
            Me.schedulerControl1.Views.DayView.AllDayAreaScrollBarVisible = False
            Me.schedulerControl1.Views.FullWeekView.AllDayAreaScrollBarVisible = False
            Me.schedulerControl1.Views.TimelineView.TimelineScrollBarVisible = False
            Me.schedulerControl1.Views.WorkWeekView.AllDayAreaScrollBarVisible = False
            ' 
            ' labelsBindingSource
            ' 
            Me.labelsBindingSource.DataMember = "Labels"
            Me.labelsBindingSource.DataSource = Me.carsDBDataSet_Renamed
            ' 
            ' carsDBDataSet
            ' 
            Me.carsDBDataSet_Renamed.DataSetName = "CarsDBDataSet"
            Me.carsDBDataSet_Renamed.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
            Me.carSchedulingBindingSource.DataSource = Me.carsDBDataSet_Renamed
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
            CType(Me.schedulerControl1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.labelsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.carsDBDataSet_Renamed, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.schedulerStorage1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.carSchedulingBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        #End Region

        Private schedulerControl1 As CustomSchedulerControl
        Private schedulerStorage1 As DevExpress.XtraScheduler.SchedulerStorage

        Private carsDBDataSet_Renamed As CarsDBDataSet
        Private carSchedulingBindingSource As System.Windows.Forms.BindingSource
        Private carSchedulingTableAdapter As CarsDBDataSetTableAdapters.CarSchedulingTableAdapter
        Private labelsBindingSource As System.Windows.Forms.BindingSource
        Private labelsTableAdapter As CarsDBDataSetTableAdapters.LabelsTableAdapter
    End Class
End Namespace

