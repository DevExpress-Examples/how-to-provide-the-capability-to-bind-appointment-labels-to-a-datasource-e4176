Imports System
Imports System.Windows.Forms

Namespace SchedulerMappedLabels

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            ' TODO: This line of code loads data into the 'carsDBDataSet.CarScheduling' table. You can move, or remove it, as needed.
            carSchedulingTableAdapter.Fill(carsDBDataSet.CarScheduling)
            ' TODO: This line of code loads data into the 'carsDBDataSet.Labels' table. You can move, or remove it, as needed.
            labelsTableAdapter.Fill(carsDBDataSet.Labels)
            schedulerControl1.PopulateLabelsStorage()
            If schedulerControl1.Storage.Appointments.Count > 0 Then
                Dim start As Date = schedulerControl1.Storage.Appointments(0).Start
                schedulerControl1.Start = start.Date
                schedulerControl1.DayView.TopRowTime = start.TimeOfDay
            End If
        End Sub
    End Class
End Namespace
