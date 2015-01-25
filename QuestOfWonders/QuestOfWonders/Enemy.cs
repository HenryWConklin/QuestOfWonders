using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    public class Enemy
    {
        public const float VEL = 50;

        public const int LEFT = -1;
        public const int STATIONARY = 0;
        public const int RIGHT = 1;

        private PointF pos;
        private int dir;
        private Bitmap img;

        public Enemy(PointF p, int direction)
        {
            pos = p;
            dir = direction;
            img = new Bitmap(Bitmap.FromFile("Resources/enemy.png"));
        }

        public Point GetPos()
        {
            return new Point((int)pos.X, (int)pos.Y);
        }

        public int GetDir()
        {
            return dir;
        }

        public void Update(float time)
        {
            pos.X += time * VEL * dir;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(img, (int)pos.X - frmMain.viewX + (dir==-1?Map.TILE_SIZE:0), (int)pos.Y - frmMain.viewY, (dir!=0?dir:1)*Map.TILE_SIZE, Map.TILE_SIZE * 2);
        }

        public void Turn()
        {
            dir = -dir;
        }

        public Rectangle GetCollisionBounds()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, Map.TILE_SIZE, Map.TILE_SIZE * 2);
        }
    }

    
    public class Projectile
    {
        public const int WIDTH = 10;
        public const int HEIGHT = 10;

        private PointF pos;
        private PointF vel;
        private Brush brush;

        Image img;

        public Projectile(PointF p, PointF v)
        {
            img = new Bitmap(Bitmap.FromFile("Resources/bullet.png"), WIDTH, HEIGHT);
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
            g.DrawImage(img, (int)pos.X - frmMain.viewX, (int)pos.Y - frmMain.viewY, WIDTH, HEIGHT);
        }

        public Point getPos()
        {
            return new Point((int)pos.X + WIDTH/2, (int)pos.Y+HEIGHT/2);
        }
    }
}
