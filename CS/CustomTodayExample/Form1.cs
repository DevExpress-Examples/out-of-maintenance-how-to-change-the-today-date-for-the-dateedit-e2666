using System;
using DXSample;
using System.Windows.Forms;

namespace CustomTodayExample {
    public partial class Form1 :Form {
        public Form1() {
            InitializeComponent();
        }

        private void myDateEdit1_Properties_CustomToday(object sender, CustomTodayEventArgs e) {
            e.Today = DateTime.Today.AddDays(1);
        }
    }
}