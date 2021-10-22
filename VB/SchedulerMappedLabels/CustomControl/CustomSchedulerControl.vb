Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports DevExpress.XtraScheduler

Namespace SchedulerMappedLabels

    Public Class CustomSchedulerControl
        Inherits SchedulerControl

        Private labelsDataSourceField As Object

        Const defaultLabelIdMappedName As String = "Id"

        Const defaultLabelColorMappedName As String = "Color"

        Const defaultLabelDisplayNameMappedName As String = "DisplayName"

        Public Sub New()
            LabelIdMappedName = defaultLabelIdMappedName
            LabelColorMappedName = defaultLabelColorMappedName
            LabelDisplayNameMappedName = defaultLabelDisplayNameMappedName
        End Sub

        <AttributeProvider(GetType(IListSource))>
        <Category("Data")>
        Public Property LabelsDataSource As Object
            Get
                Return labelsDataSourceField
            End Get

            Set(ByVal value As Object)
                labelsDataSourceField = value
                PopulateLabelsStorage()
                Refresh()
            End Set
        End Property

        <Category("Data")>
        <DefaultValue(defaultLabelIdMappedName)>
        <TypeConverter(GetType(LabelColumnNameConverter))>
        Public Property LabelIdMappedName As String

        <Category("Data")>
        <DefaultValue(defaultLabelColorMappedName)>
        <TypeConverter(GetType(LabelColumnNameConverter))>
        Public Property LabelColorMappedName As String

        <Category("Data")>
        <DefaultValue(defaultLabelDisplayNameMappedName)>
        <TypeConverter(GetType(LabelColumnNameConverter))>
        Public Property LabelDisplayNameMappedName As String

        Public Sub PopulateLabelsStorage()
            Dim labelsController As DataBindingController = New DataBindingController(labelsDataSourceField, String.Empty)
            If Storage Is Nothing Then Return
            Storage.Appointments.Labels.Clear()
            For i As Integer = 0 To labelsController.ItemsCount - 1
                Dim labelId As Object = labelsController.GetRowValue(LabelIdMappedName, i)
                Dim labelDisplayName As String = labelsController.GetRowValue(LabelDisplayNameMappedName, i).ToString()
                Dim labelColor As Color = Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i)))
                Storage.Appointments.Labels.Add(labelId, labelDisplayName, labelDisplayName, labelColor)
            Next
        End Sub
    End Class
End Namespace
