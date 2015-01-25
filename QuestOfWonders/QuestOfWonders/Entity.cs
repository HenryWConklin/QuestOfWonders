using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    class Entity
    {
        Point pos;
        Bitmap img;

        public Entity(Point p, String path)
        {
            pos = p;
            img = new Bitmap(Bitmap.FromFile(path));
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(img, pos.X - frmMain.viewX, pos.Y - frmMain.viewY);
        }
    }
}
