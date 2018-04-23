Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Globalization

Namespace SchedulerMappedLabels
    Public Class LabelColumnNameConverter
        Inherits StringConverter

        Private Const noneString As String = "(none)"

        Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
            Dim columnNames As List(Of String) = GetColumnNames(context)
            columnNames.Add(String.Empty)
            Return New StandardValuesCollection(columnNames)
        End Function

        Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Private Function GetColumnNames(ByVal context As ITypeDescriptorContext) As List(Of String)
            If context Is Nothing Then
                Return New List(Of String)()
            End If

            Dim control As CustomSchedulerControl = TryCast(context.Instance, CustomSchedulerControl)

            If control Is Nothing AndAlso control.LabelsDataSource Is Nothing Then
                Return New List(Of String)()
            End If

            Return (New DataBindingController(control.LabelsDataSource, String.Empty)).GetColumnNames()
        End Function

        Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
            If String.IsNullOrEmpty(TryCast(value, String)) Then
                Return noneString
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

        Public Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
            Dim s As String = TryCast(value, String)
            If String.IsNullOrEmpty(s) OrElse String.Compare(s, noneString, True, CultureInfo.CurrentCulture) = 0 Then
                Return String.Empty
            End If
            Return MyBase.ConvertFrom(context, culture, value)
        End Function
    End Class
End Namespace