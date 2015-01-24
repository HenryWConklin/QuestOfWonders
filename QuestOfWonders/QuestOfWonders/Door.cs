using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    public class Door
    {
        public bool opened;
        private Point pos;
        private Bitmap open;
        private Bitmap closed;
        
        private int width;
        private int height;

        public Door(Point p)
        {
            pos = p;
            opened = false;
            open = new Bitmap(Bitmap.FromFile("Resources/door open.png"));
            closed = new Bitmap(Bitmap.FromFile("Resources/door cloosed.png"));
            width = open.Width;
            height = open.Height;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage((opened ? open : closed), pos);
        }

        public Rectangle GetCollisionRectangle()
        {
            return (opened?new Rectangle(pos.X, pos.Y, width, height):new Rectangle());
        }
    }
}
