using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace SchedulerMappedLabels {
    public class LabelColumnNameConverter : StringConverter {
        private const string noneString = "(none)";

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            List<string> columnNames = GetColumnNames(context);
            columnNames.Add(String.Empty);
            return new StandardValuesCollection(columnNames);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }

        private List<string> GetColumnNames(ITypeDescriptorContext context) {
            if (context == null)
                return new List<string>();

            CustomSchedulerControl control = context.Instance as CustomSchedulerControl;

            if (control == null && control.LabelsDataSource == null)
                return new List<string>();

            return new DataBindingController(control.LabelsDataSource, string.Empty).GetColumnNames();
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (String.IsNullOrEmpty(value as String))
                return noneString;
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            string s = value as string;
            if (String.IsNullOrEmpty(s) || String.Compare(s, noneString, true, CultureInfo.CurrentCulture) == 0)
                return String.Empty;
            return base.ConvertFrom(context, culture, value);
        }
    }
}