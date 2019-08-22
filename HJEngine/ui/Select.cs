using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;

namespace HJEngine.ui
{
    class Choice
    {
        public string name;
        public string value;

        public Choice(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }

    class Select : Component
    {
        private List<Choice> choices;
        private List<Label> choiceLabels;
        private Pane selectPane;
        private ArrowButton leftArrow;
        private ArrowButton rightArrow;
        private prim.MouseOverStateMachine LAmouseOverState;
        private prim.MouseOverStateMachine RAmouseOverState;
        public int selectIndex;

        public Select(gfx.Graphics graphics, string bind, string value,
            Color borderColor, prim.Size borderSize,
             int fontSize, string fontType, Color fontColor, prim.Point point,
             prim.Size size, XmlNodeList choiceNodes)
                : base(graphics, "select", "", point, size)
        {
            this.bind = bind;

            choices = new List<Choice>();
            choiceLabels = new List<Label>();

            selectIndex = 0;

            LAmouseOverState = new prim.MouseOverStateMachine();
            RAmouseOverState = new prim.MouseOverStateMachine();


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



            foreach (XmlNode choiceNode in choiceNodes)
            {
                Choice choice = new Choice(choiceNode.Attributes["text"].Value, 
                    choiceNode.Attributes["value"].Value);
                choices.Add(choice);
                Label choiceLabel = new Label(graphics, choice.name, fontSize, fontType, fontColor, new prim.Point(point), new prim.Size(0,0));
                choiceLabel.point.x = triW + (point.x + ( (size.w-triW)/2)) - (choiceLabel.size.w / 2);
                choiceLabel.point.y = (point.y + (size.h/2)) - (choiceLabel.size.h / 2);
                choiceLabel.Update();
                choiceLabels.Add(choiceLabel);
            }

            SetValue(value);

            selectPane = new Pane(graphics, Color.FromArgb(0, 0, 0, 0), borderColor, borderSize, new prim.Point(point.x + triW, point.y), new prim.Size(size.w-triW, size.h));
        }

        private void SetValue(string value)
        {
            this.value = value;
            for(int i = 0; i < choices.Count; i++ )
            {
                Choice curChoice = choices[i];
                if(this.value == curChoice.value)
                {
                    selectIndex = i;
                }
            }
        }

        private void GotoChoice(int delta)
        {
            if (selectIndex + delta < 0)
                selectIndex = choices.Count - 1;
            else if (selectIndex + delta >= choices.Count)
                selectIndex = 0;
            else
                selectIndex = selectIndex + delta;
            this.value = choices[selectIndex].value;
        }

        public override void Update()
        {
            base.Update();
            leftArrow.Update();
            rightArrow.Update();
            if(leftArrow.activated)
            {
                GotoChoice(-1);
            }
            else if(rightArrow.activated)
            {
                GotoChoice(1);
            }
        }

        public override void Draw()
        {
            base.Draw();
            selectPane.Draw();
            choiceLabels[selectIndex].Draw();
            leftArrow.Draw();
            rightArrow.Draw();
        }
    }
}
