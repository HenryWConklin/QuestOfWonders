using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    public class Switch
    {
        public bool isOn;
        private Point pos;
        private Bitmap on;
        private Bitmap off;

        public Switch(Point p)
        {
            isOn = false;
            pos = p;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage((isOn ? on : off), pos);
        }

        public Rectangle GetCollisionRectangle()
        {
            return new Rectangle(pos.X, pos.Y, on.Width, on.Height);
        }
    }
}
