using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace HJEngine.ui
{
    class TextField : Component
    {
        private List<Label> textLines;
        public Pane cursorPane;
        public Color fontColor;
        public int fontSize;
        public string fontType;

        public TextField(gfx.Graphics graphics, string text,
            int fontSize, string fontType, Color fontColor, prim.Point point)
            : base(graphics, "text field", "", point, new prim.Size() )
        {
            this.text = text;
            this.fontColor = fontColor;
            this.fontSize = fontSize;
            this.fontType = fontType;
            UpdateText();
            prim.Size cSize = new prim.Size(textLines[0].size);
            cSize.w = graphics.getWSquareSize(cSize.h)/8;
            prim.Point cPoint = new prim.Point(point.x + textLines[0].size.w, textLines[0].point.y);
            this.cursorPane = new Pane(graphics, fontColor, Color.Transparent,
                new prim.Size(), cPoint, cSize);
        }

        public void UpdateText(string appendText="", gfx.Graphics.KEYCODE keycode=0)
        {
            if(keycode == gfx.Graphics.KEYCODE.BACKSPACE && this.text.Length - 1 >= 0)
            {
                this.text = this.text.Substring(0, this.text.Length - 1);
            }
            else
                this.text += appendText;
            textLines = new List<Label>();
            Label newLabel = new Label(graphics, text, fontSize,
                fontType, fontColor, point, size);
            textLines.Add(newLabel);
        }

        public override void Update()
        {
            base.Update();
            if (graphics.keyBuffer != "" || graphics.keyCode != gfx.Graphics.KEYCODE.NONE)
            { 
                UpdateText(graphics.keyBuffer, graphics.keyCode);
                ChangeCursor();
                this.cursorPane.Update();
            }
        }

        public void ChangeCursor()
        {
            cursorPane.point.x = point.x + textLines[0].size.w;
        }

        public override void Draw()
        {
            base.Draw();
            foreach(Label textLine in textLines)
            {
                textLine.Draw();
            }
            this.cursorPane.Draw();
        }
    }
}
