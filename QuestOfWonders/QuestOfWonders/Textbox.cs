using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    class Textbox
    {
        string myText;
        Rectangle myPos;
        Brush backColor;
        Brush borderColor;
        Brush textColor;

        private Rectangle textRect;

        int borderSize = 4;
        Font myFont;

        public Textbox(string text, Rectangle rect)
        {
            setup(text, rect, Color.DarkSlateGray, Color.Black, Color.White);
        }

        public Textbox(string text, Rectangle rect, Color backCol, Color borderCol, Color textCol)
        {
            setup(text, rect, backCol, borderCol, textCol);
        }

        private void setup(string text, Rectangle rect, Color backCol, Color borderCol, Color textCol)
        {
            myFont = SystemFonts.DefaultFont;
            this.myText = text;
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
            g.DrawString(myText, myFont, textColor, textRect);
        }

        public void Update(float time)
        {
            
        }


    }
}
