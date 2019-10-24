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
        private int x1;
        private int y1;
        private int x2;
        private int y2;
        private int sqW;
        private Point delPnt;
        public Bitmap bitmap;
        public List<MapInterface.Line> lines;
        List<ToolStripButton> buttonGroup;

        public CollisionForm(MapInterface.ImageData imageData)
        {
            InitializeComponent();
            mode = "";
            sqW = 6;
            lines = new List<MapInterface.Line>();
            this.bitmap = imageData.image;
            lines = new List<MapInterface.Line>(imageData.collisionVectors);
            buttonGroup = new List<ToolStripButton>();

            buttonGroup.Add(lineButton);
            buttonGroup.Add(deleteButton);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void imageBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point pnt = me.Location;
            Point? appPoint = approxPoint(pnt);
            pnt = appPoint != null ? (Point)appPoint : pnt;
            if (appPoint != null && mode == "delete")
            {
                delPnt = (Point)appPoint;
                mode = "delete2";
            }
            else if (appPoint != null && mode == "delete2")
            {
                delCorPoint((Point)appPoint);
                mode = "delete";
            }
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

        private void resetButtons(object sender)
        {
            foreach (ToolStripButton button in buttonGroup)
                if (button != sender)
                    button.Checked = false;
        }

        private void lineButton_Click(object sender, EventArgs e)
        {
            lineButton.Checked = lineButton.Checked ? false : true;
            mode = lineButton.Checked ? "line" : "";
            resetButtons(sender);
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CollisionForm_Load(object sender, EventArgs e)
        {
            RefreshBox();
        }

        private void delCorPoint(Point linePnt)
        {
            foreach (MapInterface.Line line in lines)
            {
                if (line.x1 == delPnt.X && line.y1 == delPnt.Y && line.x2 == linePnt.X && line.y2 == linePnt.Y)
                {
                    lines.Remove(line);
                    return;
                }
                else if (line.x2 == delPnt.X && line.y2 == delPnt.Y && line.x1 == linePnt.X && line.y1 == linePnt.Y)
                {
                    lines.Remove(line);
                    return;
                }
            }
        }

        private Point? approxPoint(Point mousePoint)
        {
            foreach (MapInterface.Line line in lines)
            {
                if (mousePoint.X >= line.x1 && mousePoint.X <= line.x1 + sqW)
                {
                    if (mousePoint.Y >= line.y1 && mousePoint.Y <= line.y1 + sqW)
                        return new Point(line.x1, line.y1);
                }
                if (mousePoint.X >= line.x2 && mousePoint.X <= line.x2 + sqW)
                {
                    if (mousePoint.Y >= line.y2 && mousePoint.Y <= line.y2 + sqW)
                        return new Point(line.x2, line.y2);
                }
            }
            return null;
        }

        private void updateImage()
        {
            Bitmap copyBitmap = new Bitmap(this.bitmap);
            Graphics g = Graphics.FromImage(copyBitmap);
 
            Pen plPen = new Pen(Color.Blue);

            foreach (MapInterface.Line line in lines)
            {
                g.DrawRectangle(plPen, line.x1 - sqW / 2, line.y1 - sqW / 2, sqW, sqW);
                g.DrawRectangle(plPen, line.x2 - sqW / 2, line.y2 - sqW / 2, sqW, sqW);
                g.DrawLine(plPen, line.x1, line.y1, line.x2, line.y2);
            }
            imageBox.Image = copyBitmap;
            g.Dispose();
            imageBox.Refresh();
        }

        private void updateImage(Point mousePoint)
        {
            Point? appPoint = approxPoint(mousePoint);
            mousePoint = appPoint != null ? (Point)appPoint : mousePoint;
            Bitmap copyBitmap = new Bitmap(this.bitmap);
            Graphics g = Graphics.FromImage(copyBitmap);

            Pen plPen = new Pen(Color.Blue);

            foreach (MapInterface.Line line in lines)
            {
                g.DrawRectangle(plPen, line.x1 - sqW / 2, line.y1 - sqW / 2, sqW, sqW);
                g.DrawRectangle(plPen, line.x2 - sqW / 2, line.y2 - sqW / 2, sqW, sqW);
                g.DrawLine(plPen, line.x1, line.y1, line.x2, line.y2);
            }
            Pen dotPen = new Pen(Color.Red);
            if (mode == "line2")
            {
                g.DrawRectangle(dotPen, new Rectangle(x1 - sqW / 2, y1 - sqW / 2, sqW, sqW));
                g.DrawLine(dotPen, x1, y1, mousePoint.X, mousePoint.Y);
            }
            if (appPoint != null)
            {
                Pen hoverPen = new Pen(Color.Green);
                g.DrawRectangle(hoverPen, ((Point)appPoint).X - sqW / 2, ((Point)appPoint).Y - sqW / 2, sqW, sqW);
            }
            imageBox.Image = copyBitmap;
            g.Dispose();
            imageBox.Refresh();
        }

        private void imageBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = e.Location;
            updateImage(mousePoint);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Checked = deleteButton.Checked ? false : true;
            mode = deleteButton.Checked ? "delete" : "";
            resetButtons(sender);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void removeAllButton_Click(object sender, EventArgs e)
        {
            lines.Clear();
            updateImage();
        }
    }
}
