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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Native;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SchedulerMappedLabels {
    public class CustomSchedulerControl : SchedulerControl {
        public CustomSchedulerControl() {
            this.PopupMenuShowing += new PopupMenuShowingEventHandler(CustomSchedulerControl_PopupMenuShowing);
            this.AppointmentViewInfoCustomizing += new AppointmentViewInfoCustomizingEventHandler(CustomSchedulerControl_AppointmentViewInfoCustomizing);

            LabelIdMappedName = defaultLabelIdMappedName;
            LabelColorMappedName = defaultLabelColorMappedName;
            LabelDisplayNameMappedName = defaultLabelDisplayNameMappedName;

            _customInnerLables = new Dictionary<int, AppointmentLabel>();
        }

        private object labelsDataSource;

        [AttributeProvider(typeof(IListSource))]
        [Category("Data")]
        public object LabelsDataSource {
            get { return labelsDataSource; }
            set {
                labelsDataSource = value;
                PopulateLabelsStorage();
                Refresh();
            }
        }

        private Dictionary<int, AppointmentLabel> _customInnerLables;
        internal Dictionary<int, AppointmentLabel> CustomInnerLables { get { return _customInnerLables; } }

        void CustomSchedulerControl_ListChanged(object sender, ListChangedEventArgs e) {
            
        }

        private const string defaultLabelIdMappedName = "Id";
        private const string defaultLabelColorMappedName = "Color";
        private const string defaultLabelDisplayNameMappedName = "DisplayName";

        [Category("Data")]
        [DefaultValue(defaultLabelIdMappedName)]
        [TypeConverter(typeof(LabelColumnNameConverter))]
        public string LabelIdMappedName { get; set; }
        [Category("Data")]
        [DefaultValue(defaultLabelColorMappedName)]
        [TypeConverter(typeof(LabelColumnNameConverter))]
        public string LabelColorMappedName { get; set; }
        [Category("Data")]
        [DefaultValue(defaultLabelDisplayNameMappedName)]
        [TypeConverter(typeof(LabelColumnNameConverter))]
        public string LabelDisplayNameMappedName { get; set; }

        public void PopulateLabelsStorage() {
            DataBindingController labelsController = new DataBindingController(labelsDataSource, string.Empty);
            if(this.Storage != null) this.Storage.Appointments.Labels.Clear();
            if(_customInnerLables != null) _customInnerLables.Clear();
            for(int i = 0; i < labelsController.ItemsCount; i++) {
                Color currentColor = Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i)));
                int iWidth = 16;
                int iHeight = 16;
                Bitmap bmp = new Bitmap(iWidth, iHeight);
                using(Graphics g = Graphics.FromImage(bmp)) {
                    g.DrawRectangle(new Pen(Color.Black, 2), 0, 0, iWidth, iHeight);
                    g.FillRectangle(new SolidBrush(currentColor), 1, 1, iWidth - 2, iHeight - 2);

                }
                AppointmentLabel newLabel = new AppointmentLabel(Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i))), labelsController.GetRowValue(LabelDisplayNameMappedName, i).ToString());
                this.Storage.Appointments.Labels.Add(newLabel);
                _customInnerLables.Add(Convert.ToInt32(labelsController.GetRowValue(LabelIdMappedName, i)), newLabel);
            }        
        }

        public void PopulateDefaultLabels() {
            DataTable defaultLabels = new DataTable("DefaultLabels");

            defaultLabels.Columns.Add(LabelIdMappedName, typeof(int));
            defaultLabels.Columns.Add(LabelColorMappedName, typeof(int));
            defaultLabels.Columns.Add(LabelDisplayNameMappedName, typeof(string));

            AppointmentLabelCollection innerLabels = this.Storage.Appointments.Labels;

            for (int i = 0; i < innerLabels.Count; i++) {
                defaultLabels.Rows.Add(new object[] { i, innerLabels[i].Color.ToArgb(), innerLabels[i].DisplayName });
            }

            LabelsDataSource = defaultLabels;
        }

        void CustomSchedulerControl_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e) {
            if(e.Menu.Id == SchedulerMenuItemId.AppointmentMenu) {
                SchedulerPopupMenu labelSubMenu = e.Menu.GetPopupMenuById(SchedulerMenuItemId.LabelSubMenu);
                DataBindingController labelsController = new DataBindingController(LabelsDataSource, string.Empty);

                if(labelsController.ItemsCount == 0) {
                    e.Menu.RemoveMenuItem(SchedulerMenuItemId.LabelSubMenu);
                    return;
                }

                labelSubMenu.Items.Clear();

                for(int i = 0; i < labelsController.ItemsCount; i++) {
                    object labelId = labelsController.GetRowValue(LabelIdMappedName, i);

                    SchedulerMenuCheckItem menuItem = new SchedulerMenuCheckItem(
                        labelsController.GetRowValue(LabelDisplayNameMappedName, i).ToString(),
                        this.SelectedAppointments[0].LabelId.Equals(labelId),
                        UserInterfaceObjectHelper.CreateBitmap(new AppointmentLabel(Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i))), string.Empty), 0x10, 0x10),
                        OnCheckedChanged);

                    menuItem.Tag = labelId;
                    labelSubMenu.Items.Add(menuItem);
                }
            }
        }

        private void OnCheckedChanged(object sender, EventArgs e) {
            this.SelectedAppointments[0].LabelId = Convert.ToInt32(((DXMenuItem)sender).Tag, System.Globalization.CultureInfo.InvariantCulture);
        }

        private void CustomSchedulerControl_AppointmentViewInfoCustomizing(object sender, AppointmentViewInfoCustomizingEventArgs e) {
            if(LabelsDataSource != null)
                e.ViewInfo.Appearance.BackColor = GetColorByLabelId(e.ViewInfo.Appointment.LabelId);
        }

        private Color GetColorByLabelId(int Id) {
            DataBindingController labelsController = new DataBindingController(LabelsDataSource, string.Empty);

            for (int i = 0; i < labelsController.ItemsCount; i++) {
                if (Convert.ToInt32(labelsController.GetRowValue(LabelIdMappedName, i)) == Id)
                    return Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i)));
            }

            return Color.White;
        }

        protected override DevExpress.XtraScheduler.UI.AppointmentForm CreateAppointmentForm(SchedulerControl control, Appointment apt, bool openRecurrenceForm) {
            return new CustomAppointmentForm(control, apt, openRecurrenceForm);
        }
    }
}