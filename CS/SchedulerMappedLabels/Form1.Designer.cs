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

namespace SchedulerMappedLabels
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraScheduler.TimeRuler timeRuler1 = new DevExpress.XtraScheduler.TimeRuler();
            DevExpress.XtraScheduler.TimeRuler timeRuler2 = new DevExpress.XtraScheduler.TimeRuler();
            this.schedulerControl1 = new SchedulerMappedLabels.CustomSchedulerControl();
            this.labelsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.carsDBDataSet = new SchedulerMappedLabels.CarsDBDataSet();
            this.schedulerStorage1 = new DevExpress.XtraScheduler.SchedulerStorage(this.components);
            this.carSchedulingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.carSchedulingTableAdapter = new SchedulerMappedLabels.CarsDBDataSetTableAdapters.CarSchedulingTableAdapter();
            this.labelsTableAdapter = new SchedulerMappedLabels.CarsDBDataSetTableAdapters.LabelsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carsDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carSchedulingBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // schedulerControl1
            // 
            this.schedulerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schedulerControl1.LabelIdMappedName = "ID";
            this.schedulerControl1.LabelsDataSource = this.labelsBindingSource;
            this.schedulerControl1.Location = new System.Drawing.Point(12, 12);
            this.schedulerControl1.Name = "schedulerControl1";
            this.schedulerControl1.Size = new System.Drawing.Size(864, 381);
            this.schedulerControl1.Start = new System.DateTime(2008, 9, 4, 0, 0, 0, 0);
            this.schedulerControl1.Storage = this.schedulerStorage1;
            this.schedulerControl1.TabIndex = 0;
            this.schedulerControl1.Text = "schedulerControl1";
            timeRuler1.TimeZone.DaylightBias = System.TimeSpan.Parse("-01:00:00");
            timeRuler1.TimeZone.DaylightZoneName = "Russian Daylight Time";
            timeRuler1.TimeZone.DisplayName = "(UTC+04:00) Moscow, St. Petersburg, Volgograd";
            timeRuler1.TimeZone.StandardZoneName = "Russian Standard Time";
            timeRuler1.TimeZone.UtcOffset = System.TimeSpan.Parse("04:00:00");
            timeRuler1.UseClientTimeZone = false;
            this.schedulerControl1.Views.DayView.TimeRulers.Add(timeRuler1);
            timeRuler2.TimeZone.DaylightBias = System.TimeSpan.Parse("-01:00:00");
            timeRuler2.TimeZone.DaylightZoneName = "Russian Daylight Time";
            timeRuler2.TimeZone.DisplayName = "(UTC+04:00) Moscow, St. Petersburg, Volgograd";
            timeRuler2.TimeZone.StandardZoneName = "Russian Standard Time";
            timeRuler2.TimeZone.UtcOffset = System.TimeSpan.Parse("04:00:00");
            timeRuler2.UseClientTimeZone = false;
            this.schedulerControl1.Views.WorkWeekView.TimeRulers.Add(timeRuler2);
            // 
            // labelsBindingSource
            // 
            this.labelsBindingSource.DataMember = "Labels";
            this.labelsBindingSource.DataSource = this.carsDBDataSet;
            // 
            // carsDBDataSet
            // 
            this.carsDBDataSet.DataSetName = "CarsDBDataSet";
            this.carsDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // schedulerStorage1
            // 
            this.schedulerStorage1.Appointments.CommitIdToDataSource = false;
            this.schedulerStorage1.Appointments.DataSource = this.carSchedulingBindingSource;
            this.schedulerStorage1.Appointments.Mappings.AppointmentId = "ID";
            this.schedulerStorage1.Appointments.Mappings.Description = "Description";
            this.schedulerStorage1.Appointments.Mappings.End = "EndTime";
            this.schedulerStorage1.Appointments.Mappings.Label = "Label";
            this.schedulerStorage1.Appointments.Mappings.Start = "StartTime";
            this.schedulerStorage1.Appointments.Mappings.Subject = "Subject";
            // 
            // carSchedulingBindingSource
            // 
            this.carSchedulingBindingSource.DataMember = "CarScheduling";
            this.carSchedulingBindingSource.DataSource = this.carsDBDataSet;
            // 
            // carSchedulingTableAdapter
            // 
            this.carSchedulingTableAdapter.ClearBeforeFill = true;
            // 
            // labelsTableAdapter
            // 
            this.labelsTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 436);
            this.Controls.Add(this.schedulerControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carsDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carSchedulingBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomSchedulerControl schedulerControl1;
        private DevExpress.XtraScheduler.SchedulerStorage schedulerStorage1;
        private CarsDBDataSet carsDBDataSet;
        private System.Windows.Forms.BindingSource carSchedulingBindingSource;
        private CarsDBDataSetTableAdapters.CarSchedulingTableAdapter carSchedulingTableAdapter;
        private System.Windows.Forms.BindingSource labelsBindingSource;
        private CarsDBDataSetTableAdapters.LabelsTableAdapter labelsTableAdapter;
    }
}

