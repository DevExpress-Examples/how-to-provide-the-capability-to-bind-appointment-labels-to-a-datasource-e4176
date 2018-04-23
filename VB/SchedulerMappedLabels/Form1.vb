Imports System
Imports System.Windows.Forms

Namespace SchedulerMappedLabels
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ' TODO: This line of code loads data into the 'carsDBDataSet.CarScheduling' table. You can move, or remove it, as needed.
            Me.carSchedulingTableAdapter.Fill(Me.carsDBDataSet_Renamed.CarScheduling)
            ' TODO: This line of code loads data into the 'carsDBDataSet.Labels' table. You can move, or remove it, as needed.
            Me.labelsTableAdapter.Fill(Me.carsDBDataSet_Renamed.Labels)

            schedulerControl1.PopulateLabelsStorage()

            If schedulerControl1.Storage.Appointments.Count > 0 Then
                Dim start As Date = schedulerControl1.Storage.Appointments(0).Start
                schedulerControl1.Start = start.Date
                schedulerControl1.DayView.TopRowTime = start.TimeOfDay
            End If
        End Sub
    End Class
End Namespace