using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using System.ComponentModel;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Controls;
using System;
using DevExpress.XtraEditors.Calendar;
using System.Drawing;

namespace DXSample {
    public class MyDateEdit :DateEdit {
        static MyDateEdit() { RepositoryItemMyDateEdit.RegisterMyDateEdit(); }
        public MyDateEdit() : base() { }

        public override string EditorTypeName { get { return RepositoryItemMyDateEdit.MyDateEditName; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemMyDateEdit Properties { get { return (RepositoryItemMyDateEdit)base.Properties; } }

        protected override PopupBaseForm CreatePopupForm() {
            if (Properties.IsVistaDisplayModeInternal) return new MyVistaPopupDateEditForm(this);
            return new MyPopupDateEditForm(this);
        }
    }

    [UserRepositoryItem("RegisterMyDateEdit")]
    public class RepositoryItemMyDateEdit :RepositoryItemDateEdit {
        static RepositoryItemMyDateEdit() { RegisterMyDateEdit(); }
        public RepositoryItemMyDateEdit() : base() { }

        internal const string MyDateEditName = "MyDateEdit";
        public override string EditorTypeName { get { return MyDateEditName; } }

        public static void RegisterMyDateEdit() {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(MyDateEditName, typeof(MyDateEdit), typeof(RepositoryItemMyDateEdit),
                typeof(DateEditViewInfo), new ButtonEditPainter(), true));
        }

        internal bool IsVistaDisplayModeInternal { get { return IsVistaDisplayMode(); } }

        static readonly object fCustomToday = new object();
        public event EventHandler<CustomTodayEventArgs> CustomToday {
            add { Events.AddHandler(fCustomToday, value); }
            remove { Events.RemoveHandler(fCustomToday, value); }
        }
        public DateTime GetCustomToday() {
            EventHandler<CustomTodayEventArgs> handler = (EventHandler<CustomTodayEventArgs>)Events[fCustomToday];
            if (handler == null) return DateTime.Today;
            CustomTodayEventArgs args = new CustomTodayEventArgs();
            handler(this, args);
            return args.Today;
        }

        public override void Assign(RepositoryItem item) {
            base.Assign(item);
            BeginUpdate();
            try {
                RepositoryItemMyDateEdit source = (RepositoryItemMyDateEdit)item;
                Events.AddHandler(fCustomToday, source.Events[fCustomToday]);
            } finally { EndUpdate(); }
        }
    }

    public class MyPopupDateEditForm :PopupDateEditForm {
        public MyPopupDateEditForm(MyDateEdit ownerEdit) : base(ownerEdit) { }

        public new MyDateEdit OwnerEdit { get { return (MyDateEdit)base.OwnerEdit; } }

        protected override DateEditCalendar CreateCalendar() {
            return new MyDateEditCalendar(OwnerEdit.Properties, OwnerEdit.EditValue);
        }

        protected override void ResetState() {
            DateTime date = OwnerEdit.DateTime;
            if (date == DateTime.MinValue) date = OwnerEdit.Properties.GetCustomToday();
            Calendar.ResetState(OwnerEdit.EditValue, date);
        }
    }

    public class MyVistaPopupDateEditForm :VistaPopupDateEditForm {
        public MyVistaPopupDateEditForm(MyDateEdit ownerEdit) : base(ownerEdit) { }

        public new MyDateEdit OwnerEdit { get { return (MyDateEdit)base.OwnerEdit; } }

        protected override DateEditCalendar CreateCalendar() {
            VistaDateEditCalendar result = new MyVistaDateEditCalendar(OwnerEdit.Properties, OwnerEdit.EditValue);
            result.OkClick += OnOkClick;
            return result;
        }

        protected override void Dispose(bool disposing) {
            if (disposing && Calendar != null)
                ((VistaDateEditCalendar)Calendar).OkClick -= OnOkClick;
            base.Dispose(disposing);
        }

        protected override void ResetState() {
            DateTime date = OwnerEdit.DateTime;
            if (date == DateTime.MinValue) date = OwnerEdit.Properties.GetCustomToday();
            Calendar.ResetState(OwnerEdit.EditValue, date);
        }
    }

    public class MyDateEditCalendar :DateEditCalendar {
        public MyDateEditCalendar(RepositoryItemMyDateEdit item, object editDate) : base(item, editDate) { }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemMyDateEdit Properties { get { return (RepositoryItemMyDateEdit)base.Properties; } }

        protected override void OnTodayButtonClick(object sender, EventArgs e) {
            DateTime today = Properties.GetCustomToday();
            SetSelection(today);
            OnDateTimeCommit(today, false);
        }

        protected override DateEditPainter CreatePainter() {
            return new MyDateEditPainter(this);
        }
    }

    public class MyVistaDateEditCalendar :VistaDateEditCalendar {
        public MyVistaDateEditCalendar(RepositoryItemMyDateEdit item, object editDate) : base(item, editDate) { }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemMyDateEdit Properties { get { return (RepositoryItemMyDateEdit)base.Properties; } }

        protected override void OnTodayButtonClick(object sender, EventArgs e) {
            DateTime today = Properties.GetCustomToday();
            SetSelection(today);
            OnDateTimeCommit(today, false);
        }

        protected override DateEditPainter CreatePainter() {
            return new MyVistaDateEditPainter(this);
        }

        protected override DateEditInfoArgs CreateInfoArgs() {
            return new MyVistaDateEditInfoArgs(this);
        }
    }

    public class CustomTodayEventArgs :EventArgs {
        private DateTime fToday = DateTime.Today;
        public DateTime Today {
            get { return fToday; }
            set { fToday = value; }
        }
    }

    public class MyDateEditPainter :DateEditPainter {
        public MyDateEditPainter(DateEditCalendarBase calendar) : base(calendar) { }

        protected override void DrawDayCell(CalendarObjectInfoArgs info, DayNumberCellInfo cell) {
            cell.Today = cell.Date == fToday;
            base.DrawDayCell(info, cell);
        }

        DateTime fToday;
        protected override void DrawMonthNumbers(CalendarObjectInfoArgs info) {
            fToday = ((MyDateEditCalendar)Calendar).Properties.GetCustomToday();
            base.DrawMonthNumbers(info);
        }
    }

    public class MyVistaDateEditPainter :VistaDateEditPainter {
        public MyVistaDateEditPainter(DateEditCalendarBase calendar) : base(calendar) { }

        protected override void DrawDayCell(CalendarObjectInfoArgs info, DayNumberCellInfo cell) {
            cell.Today = cell.Date == fToday;
            base.DrawDayCell(info, cell);
        }

        DateTime fToday;
        protected override void DrawMonthNumbers(CalendarObjectInfoArgs info) {
            fToday = ((MyVistaDateEditCalendar)Calendar).Properties.GetCustomToday();
            base.DrawMonthNumbers(info);
        }
    }

    public class MyVistaDateEditInfoArgs :VistaDateEditInfoArgs {
        public MyVistaDateEditInfoArgs(DateEditCalendarBase calendar) : base(calendar) { }

        DateTime fToday;
        protected override void CalcDayNumberCells() {
            fToday = ((MyVistaDateEditCalendar)Calendar).Properties.GetCustomToday();
            base.CalcDayNumberCells();
        }

        protected override void UpdateCellAppearance(DayNumberCellInfo cell) {
            cell.Today = fToday == cell.Date;
            base.UpdateCellAppearance(cell);
        }
    }
}