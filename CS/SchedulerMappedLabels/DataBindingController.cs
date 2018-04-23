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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SchedulerMappedLabels {
    public class DataBindingController {
        private static BindingContext bindingContext = new BindingContext();
        private object dataSource;
        private string dataMember;

        public DataBindingController(object dataSource, string dataMember) {
            this.dataSource = dataSource;
            this.dataMember = dataMember;
        }

        private BindingManagerBase GetBindingManager() {
            BindingManagerBase bindingManager = null;

            if (dataSource != null) {
                if (dataMember.Length > 0)
                    bindingManager = bindingContext[dataSource, dataMember];
                else
                    bindingManager = bindingContext[dataSource];
            }

            return bindingManager;
        }

        public int ItemsCount {
            get {
                BindingManagerBase bindingManager = GetBindingManager();
                return (bindingManager != null) ? bindingManager.Count : 0;
            }
        }

        private PropertyDescriptorCollection GetItemProperties() {
            BindingManagerBase bindingManager = GetBindingManager();
            return (bindingManager != null) ? bindingManager.GetItemProperties() : null;
        }

        public List<string> GetColumnNames() {
            List<string> list = new List<string>();
            PropertyDescriptorCollection itemProperties = GetItemProperties();

            if (itemProperties != null) {
                int count = itemProperties.Count;
                for (int i = 0; i < count; i++) {
                    list.Add(itemProperties[i].Name);
                }
            }

            return list;
        }

        public object GetRowValue(string columnName, int rowIndex) {
            BindingManagerBase bindingManager = GetBindingManager();
            PropertyDescriptorCollection itemProperties = GetItemProperties();

            if (bindingManager != null && itemProperties != null) {
                PropertyDescriptor prop = itemProperties.Find(columnName, false);
                
                if (prop == null)
                    throw new ArgumentException(string.Format("'{0}' column does not exist", columnName));

                if (bindingManager is CurrencyManager)
                    return prop.GetValue(((CurrencyManager)bindingManager).List[rowIndex]);
                else {
                    if (rowIndex != 0)
                        throw new ArgumentOutOfRangeException("rowIndex");
                    return prop.GetValue(((PropertyManager)bindingManager).Current);
                }
            }

            return null;
        }
    }
}