using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    class Projectile
    {
        public const int WIDTH = 10;
        public const int HEIGHT = 5;

        private PointF pos;
        private PointF vel;
        private Brush brush;

        public Projectile(PointF p, PointF v)
        {
            pos = p;
            vel = v;
            brush = new SolidBrush(Color.PapayaWhip);
        }

        public void Update(float time)
        {
            pos.X += vel.X * time;
            pos.Y += vel.Y * time;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(brush, (int)pos.X - frmMain.viewX, (int)pos.Y - frmMain.viewY, WIDTH, HEIGHT);
        }

        public Point getPos()
        {
            return new Point((int)pos.X, (int)pos.Y);
        }
    }

    class Enemy
    {
        public const float VEL = 150;

        private PointF pos;
        private bool dirRight;
        private Brush brush;

        public Enemy(PointF p, bool right)
        {
            pos = p;
            dirRight = right;
            brush = new SolidBrush(Color.RosyBrown);
        }

        public void Update(float time)
        {
            if (dirRight)
                pos.X += time * VEL;
            else
                pos.X -= time * VEL;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(brush, (int)pos.X - frmMain.viewX, (int)pos.Y - frmMain.viewY, Map.TILE_SIZE, Map.TILE_SIZE);
        }

        public void Turn()
        {
            dirRight = !dirRight;
        }

        public Rectangle GetCollisionBounds()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, Map.TILE_SIZE, Map.TILE_SIZE);
        }
    }
}
