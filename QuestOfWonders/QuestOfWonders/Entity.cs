using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    public class Entity
    {
        Point pos;
        Bitmap img;

        int width, height;

        public Entity(Point p, String path)
        {
            pos = p;
            img = new Bitmap(Bitmap.FromFile(path));
            this.width = img.Width;
            this.height = img.Height;
        }
        public Entity(Point p, String path, int width, int height)
        {
            pos = p;
            img = new Bitmap(Bitmap.FromFile(path));
            this.width = width;
            this.height = height;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(img, pos.X - frmMain.viewX, pos.Y - frmMain.viewY, width, height);
        }
    }
}