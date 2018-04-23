using System;

using System.Windows.Forms;

namespace CustomTodayExample {
    public partial class Form1 :Form {
        public Form1() {
            InitializeComponent();
            dateEdit1.Properties.TodayDate = new System.DateTime(2015, 11, 30, 0, 0, 0, 0);
        }
    }
}