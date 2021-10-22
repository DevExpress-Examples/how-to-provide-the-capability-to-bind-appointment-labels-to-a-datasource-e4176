Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace SchedulerMappedLabels

    Public Class DataBindingController

        Private Shared bindingContext As BindingContext = New BindingContext()

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

        Public ReadOnly Property ItemsCount As Integer
            Get
                Dim bindingManager As BindingManagerBase = GetBindingManager()
                Return If(bindingManager IsNot Nothing, bindingManager.Count, 0)
            End Get
        End Property

        Private Function GetItemProperties() As PropertyDescriptorCollection
            Dim bindingManager As BindingManagerBase = GetBindingManager()
            Return If(bindingManager IsNot Nothing, bindingManager.GetItemProperties(), Nothing)
        End Function

        Public Function GetColumnNames() As List(Of String)
            Dim list As List(Of String) = New List(Of String)()
            Dim itemProperties As PropertyDescriptorCollection = GetItemProperties()
            If itemProperties IsNot Nothing Then
                Dim count As Integer = itemProperties.Count
                For i As Integer = 0 To count - 1
                    list.Add(itemProperties(i).Name)
                Next
            End If

            Return list
        End Function

        Public Function GetRowValue(ByVal columnName As String, ByVal rowIndex As Integer) As Object
            Dim bindingManager As BindingManagerBase = GetBindingManager()
            Dim itemProperties As PropertyDescriptorCollection = GetItemProperties()
            If bindingManager IsNot Nothing AndAlso itemProperties IsNot Nothing Then
                Dim prop As PropertyDescriptor = itemProperties.Find(columnName, False)
                If prop Is Nothing Then Throw New ArgumentException(String.Format("'{0}' column does not exist", columnName))
                If TypeOf bindingManager Is CurrencyManager Then
                    Return prop.GetValue(CType(bindingManager, CurrencyManager).List(rowIndex))
                Else
                    If rowIndex <> 0 Then Throw New ArgumentOutOfRangeException("rowIndex")
                    Return prop.GetValue(CType(bindingManager, PropertyManager).Current)
                End If
            End If

            Return Nothing
        End Function
    End Class
End Namespace
