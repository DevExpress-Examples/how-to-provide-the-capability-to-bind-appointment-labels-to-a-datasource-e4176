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