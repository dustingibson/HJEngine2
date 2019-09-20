using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HJCompanion
{
    public partial class CollisionForm : Form
    {
        public string mode;
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        public Bitmap bitmap;
        List<MapInterface.Line> lines;

        public CollisionForm(MapInterface.ImageData imageData)
        {
            InitializeComponent();
            mode = "";
            lines = new List<MapInterface.Line>();
            this.bitmap = imageData.image;
            lines = imageData.collisionVectors;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void imageBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point pnt = me.Location;
            if (mode == "line")
            {
                x1 = pnt.X;
                y1 = pnt.Y;
                mode = "line2";
            }
            else if(mode == "line2")
            {
                x2 = pnt.X;
                y2 = pnt.Y;
                //Place
                MapInterface.Line line = new MapInterface.Line(x1, y1, x2, y2);
                lines.Add(line);
                this.mode = "line";
            }
        }

        private void RefreshBox()
        {
            imageBox.Image = this.bitmap;
            this.Refresh();
        }

        private void lineButton_Click(object sender, EventArgs e)
        {
            mode = "line";
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CollisionForm_Load(object sender, EventArgs e)
        {
            RefreshBox();
        }

        private void imageBox_MouseMove(object sender, MouseEventArgs e)
        {
            Bitmap copyBitmap = new Bitmap(this.bitmap);
            Point mousePoint = e.Location;
            Graphics g;
            g = Graphics.FromImage(copyBitmap);
            Pen dotPen = new Pen(Color.Red);
            Pen plPen = new Pen(Color.Blue);
            if (mode == "line2")
            {
                g.DrawRectangle(dotPen, new Rectangle(x1, y1, 5, 5));
                g.DrawLine(dotPen, x1, y1, mousePoint.X, mousePoint.Y);
            }
            foreach(MapInterface.Line line in lines)
            {
                g.DrawLine(plPen, line.x1, line.y1, line.x2, line.y2);
            }
            imageBox.Image = copyBitmap;
            g.Dispose();
            imageBox.Refresh();
        }
    }
}
