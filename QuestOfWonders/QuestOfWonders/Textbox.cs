using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    public class Textbox
    {
        List<string> allText;
        int curTextNum = 0;
        Rectangle myPos;
        public Brush backColor, borderColor, textColor;

        private Rectangle textRect;

        bool done = false;

        int borderSize = 4;
        Font myFont;

        public Textbox(List<string> text, Rectangle rect)
        {
            setup(text, rect, Color.DarkSlateGray, Color.Black, Color.White);
        }

        public Textbox(List<string> text, Rectangle rect, Color backCol, Color borderCol, Color textCol)
        {
            setup(text, rect, backCol, borderCol, textCol);
        }

        private void setup(List<string> text, Rectangle rect, Color backCol, Color borderCol, Color textCol)
        {
            myFont = SystemFonts.DefaultFont;
            this.allText = text;
            this.myPos = rect;
            this.backColor = new SolidBrush(backCol);
            this.borderColor = new SolidBrush(borderCol);
            this.textColor = new SolidBrush(textCol);
            textRect = new Rectangle(rect.X + borderSize, rect.Y + borderSize, rect.Width - borderSize * 2, rect.Height - borderSize * 2);
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(borderColor, myPos);
            g.FillRectangle(backColor, textRect);
            g.DrawString(allText[curTextNum], myFont, textColor, textRect);
        }

        public void Advance()
        {
            if (curTextNum < allText.Count - 1)
            {
                curTextNum++;
            }
            else
            {
                done = true;
            }
        }

        public void Update(float time)
        {
            
        }


    }
}
