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

        public Door(Point p)
        {
            pos = p;
            opened = false;
            open = new Bitmap(Bitmap.FromFile("Resources/door open.png"));
            closed = new Bitmap(Bitmap.FromFile("Resources/door cloosed.png"));
        }

        public void Draw(Graphics g)
        {
            g.DrawImage((opened ? open : closed), pos.X - frmMain.viewX, pos.Y -frmMain.viewY, Map.TILE_SIZE, Map.TILE_SIZE * 2);
        }

        public Rectangle GetCollisionRectangle()
        {
            return (!opened?new Rectangle(pos.X, pos.Y, Map.TILE_SIZE, 2 * Map.TILE_SIZE):new Rectangle());
        }
    }
}
