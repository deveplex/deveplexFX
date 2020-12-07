using System;
using System.Windows.Forms;

namespace Deveplex.Forms
{
    public class VirtualListView : ListView
    {
        public VirtualListView()
        {
            SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint, true);

            // Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

            UpdateStyles();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (this.Columns.Count <= 0)
                return;

            if (!this.Columns.ContainsKey("_rowfooter_"))
            {
                ColumnHeader rowFooter = new ColumnHeader()
                {
                    Name = "_rowfooter_",
                    Text = "",
                    Width = 20,
                };
                this.Columns.Add(rowFooter);
            }
        }

        protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
        {
            base.OnColumnWidthChanging(e);
            ColumnHeader current = this.Columns[e.ColumnIndex];
            if (current.Name == "_rowfooter_") return;

            int index = this.Columns.IndexOfKey("_rowfooter_");
            if (index < 0) return;

            this.Columns[index].Width += current.Width - e.NewWidth;
        }

        protected override void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
        {
            base.OnColumnWidthChanged(e);
            ColumnHeader current = this.Columns[e.ColumnIndex];
            if (current.Name == "_rowfooter_") return;

            //int index = this.Columns.IndexOfKey("_rowfooter_");
            //if (index < 0) return;

            //this.Columns[index].Width -= 4;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int viewWidth = this.Width;
            int colsWidth = 0;
            foreach (ColumnHeader col in this.Columns)
            {
                colsWidth += col.Width;
            }
            int w = (int)(viewWidth - colsWidth);
            if (w >= 0)
                this.Columns[2].Width += w - 4;
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
    }
}
