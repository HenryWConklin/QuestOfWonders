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
            on = new Bitmap(Bitmap.FromFile("Resources/lever on.png"));
            off = new Bitmap(Bitmap.FromFile("Resources/lever off.png"));
        }

        public void Draw(Graphics g)
        {
            g.DrawImage((isOn ? on : off), pos.X - frmMain.viewX, pos.Y - frmMain.viewY, Map.TILE_SIZE, Map.TILE_SIZE);
        }

        public Rectangle GetCollisionRectangle()
        {
            return new Rectangle(pos.X, pos.Y, on.Width, on.Height);
        }
    }
}
