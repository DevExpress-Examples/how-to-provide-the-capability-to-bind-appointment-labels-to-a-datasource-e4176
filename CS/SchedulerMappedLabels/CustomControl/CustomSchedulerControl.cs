using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraScheduler;

namespace SchedulerMappedLabels {
    public class CustomSchedulerControl : SchedulerControl {
        object labelsDataSource;
        const string defaultLabelIdMappedName = "Id";
        const string defaultLabelColorMappedName = "Color";
        const string defaultLabelDisplayNameMappedName = "DisplayName";

        public CustomSchedulerControl() {
            LabelIdMappedName = defaultLabelIdMappedName;
            LabelColorMappedName = defaultLabelColorMappedName;
            LabelDisplayNameMappedName = defaultLabelDisplayNameMappedName;
        }

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
            if (Storage == null)
                return;

            Storage.Appointments.Labels.Clear();
            for (int i = 0; i < labelsController.ItemsCount; i++) {
                object labelId = labelsController.GetRowValue(LabelIdMappedName, i);
                string labelDisplayName = labelsController.GetRowValue(LabelDisplayNameMappedName, i).ToString();
                Color labelColor = Color.FromArgb(Convert.ToInt32(labelsController.GetRowValue(LabelColorMappedName, i)));
                Storage.Appointments.Labels.Add(labelId, labelDisplayName, labelDisplayName, labelColor);
            }
        }
    }
}