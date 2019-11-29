using System;
using System.Windows.Forms;

namespace SchedulerMappedLabels {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            // TODO: This line of code loads data into the 'carsDBDataSet.CarScheduling' table. You can move, or remove it, as needed.
            this.carSchedulingTableAdapter.Fill(this.carsDBDataSet.CarScheduling);
            // TODO: This line of code loads data into the 'carsDBDataSet.Labels' table. You can move, or remove it, as needed.
            this.labelsTableAdapter.Fill(this.carsDBDataSet.Labels);


            if (schedulerControl1.DataStorage.Appointments.Count > 0) {
                DateTime start = schedulerControl1.DataStorage.Appointments[0].Start;
                schedulerControl1.Start = start.Date;
                schedulerControl1.DayView.TopRowTime = start.TimeOfDay;
            }
        }
    }
}