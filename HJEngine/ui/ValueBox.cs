using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;

namespace HJEngine.ui
{
    class ValueBox : Component
    {
        private List<Pane> boxes;
        private Pane selectPane;
        private ArrowButton leftArrow;
        private ArrowButton rightArrow;
        private prim.MouseOverStateMachine LAmouseOverState;
        private prim.MouseOverStateMachine RAmouseOverState;

        public ValueBox(gfx.Graphics graphics, string bind, string value,
            Color borderColor, prim.Size borderSize,
             int fontSize, string fontType, Color fontColor, prim.Point point,
             prim.Size size)
                : base(graphics, "select", "", point, size)
        {
            this.bind = bind;

            this.value = value;

            LAmouseOverState = new prim.MouseOverStateMachine();
            RAmouseOverState = new prim.MouseOverStateMachine();

            boxes = new List<Pane>();

            float triH = size.h;
            float triW = graphics.getWSquareSize(triH);

            float[] lVertices =
            {
               point.x + triW, point.y + triH, 0.0f, 1.0f, 1.0f,
               point.x + triW, point.y, 0.0f, 1.0f, 0.0f,
               point.x, point.y + (triH/2.0f), 0.0f, 0.0f, 0.5f
            };
            //Remember to add size.x to point.x
            float[] rVertices =
{
               point.x + size.w, point.y, 0.0f, 1.0f, 1.0f,
               point.x + size.w, point.y + triH, 0.0f, 1.0f, 0.0f,
               point.x + triW + size.w, point.y  + (triH/2.0f), 0.0f, 0.0f, 0.5f
            };

            uint[] indices =
            {
                0,1,2
            };

            //leftArrow = new gfx.ColorTexture(graphics, borderColor, lVertices, indices);
            //rightArrow = new gfx.ColorTexture(graphics, borderColor, rVertices, indices);
            leftArrow = new ArrowButton(graphics, "left", fontColor, new prim.Point(point), new prim.Size(triW, triH));
            rightArrow = new ArrowButton(graphics, "right", fontColor, new prim.Point(point.x + size.w, point.y), new prim.Size(triW, triH));

            selectPane = new Pane(graphics, Color.FromArgb(0, 0, 0, 0), borderColor, borderSize, new prim.Point(point.x + triW, point.y), new prim.Size(size.w - triW, size.h));

            for(int i = 0; i < 10; i++)
            {
                prim.Size paneSize = new prim.Size( (this.size.w + this.leftArrow.size.w) * 0.1f, this.size.h);
                prim.Point panePoint = new prim.Point(leftArrow.size.w + point.x + paneSize.w*i, point.y);
                Pane curPane = new Pane(graphics, borderColor, borderColor, borderSize, panePoint, paneSize);
                boxes.Add(curPane);
            }

        }

        private void SetValue(string value)
        {
            //this.value = value;
            //for (int i = 0; i < choices.Count; i++)
            //{
            //    Choice curChoice = choices[i];
            //    if (this.value == curChoice.value)
            //    {
            //        selectIndex = i;
            //    }
            //}
        }

        private void ChangeValue(int delta)
        {
            int intVal = Int32.Parse(value);
            if (intVal + delta < 0)
                return;
            else if (intVal + delta >= 10)
                return;
            else
                this.value = (intVal + delta).ToString();
            //this.value = choices[selectIndex].value;
        }

        public override void Update()
        {
            base.Update();
            leftArrow.Update();
            rightArrow.Update();
            if (leftArrow.activated)
            {
                ChangeValue(-1);
            }
            else if (rightArrow.activated)
            {
                ChangeValue(1);
            }
        }

        public override void Draw()
        {
            base.Draw();
            selectPane.Draw();
            for(int i = 0; i < Int32.Parse(this.value); i++)
            {
                boxes[i].Draw();
            }
            leftArrow.Draw();
            rightArrow.Draw();
        }
    }
}
