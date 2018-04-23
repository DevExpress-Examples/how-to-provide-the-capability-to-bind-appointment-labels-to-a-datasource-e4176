// Developer Express Code Central Example:
// How to provide the capability to bind appointment labels to a datasource
// 
// Due to numerous requests from our customers regarding the capability to bind
// appointment labels/statuses to a datasource, we have created this sample. Note
// that in the past, we tried to address this issue in the context of the following
// examples:
// http://www.devexpress.com/scid=E2028
// http://www.devexpress.com/scid=E2087
// They
// illustrate how to load labels form an external datasource. However, one
// limitation is still there. It is related to the default meaning of the
// Appointment.LabelId Property
// (http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerAppointment_LabelIdtopic).
// The value of this property represents an index of a label in the
// AppointmentStorage.Labels
// (http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerAppointmentStorage_Labelstopic)
// (this label is used for this appointment).This mean that once you remove a
// particular label for this collection, indexes will be shifted. Take a moment to
// look at the http://www.devexpress.com/scid=Q413689 ticket, which describes this
// issue in detail.
// Apparently, a more advanced labels/status identification
// mechanism is required. This example illustrates how to implement this mechanism
// for labels (you can use the same approach for statuses) by extending the
// SchedulerControl Class
// (http://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraSchedulerSchedulerControltopic).
// The main idea of the approach illustrated here is to define a separate
// datasource for appointment labels (the LabelsDataSource property) and mapped
// field names for Id, Color and DisplayName (the LabelIdMappedName,
// LabelColorMappedName and LabelDisplayNameMappedName properties). If the
// datasource is not specified, we are using default label items (see the
// PopulateDefaultLabels() method). Otherwise, labels from a datasource are used.
// Note that the Appointment.LabelId property has another meaning in this scenario.
// The value of this property is used to look up a corresponding label item in the
// SchedulerControl.AppointmentViewInfoCustomizing Event
// (http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerSchedulerControl_AppointmentViewInfoCustomizingtopic)
// in order to assign a color defined in this label to the appointment. In
// addition, we handle the SchedulerControl.PopupMenuShowing Event
// (http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerSchedulerControl_PopupMenuShowingtopic)
// to populate the LabelSubMenu with custom menu items created on the fly based on
// the rows in the datasource with labels.
// To correctly display custom
// appointments' labels in the EditAppointmentForm, we override the UpdateFormCore
// and edtLabel_EditValueChanged methods in a corresponding EditAppointmentForm
// descendant. The important thing is that a SchedulerStorage instance should
// contain custom appointments' labels in its internal collection. We use the
// SchedulerControl.PopulateLabelsStorage method to replace the collection of
// pre-defined labels with a custom one.
// 
// Note that we are using a custom-made
// DataBindingController class to operate with a label datasource in a generic
// manner. This means that our approach should work correctly regardless of the
// actual datasource type, be it a DataTable or a List<T>.
// Here is a screenshot
// that illustrates meaning of the Appointment.LabelId property (pay attention to
// the CarScheduling.Label and Labels.Id field values):
// 
// Finally, we have
// implemented design-time support for the aforementioned properties:
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E4176

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SchedulerMappedLabels")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("My Company")]
[assembly: AssemblyProduct("SchedulerMappedLabels")]
[assembly: AssemblyCopyright("Copyright © My Company 2008")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("fe48d7d2-3f32-4206-b3dc-47b9cd150403")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
