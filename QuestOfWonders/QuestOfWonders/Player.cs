using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace QuestOfWonders
{
	enum Direction
	{
		Left, Right, Up, Down
	};
	
    class Player
    {
		private static int SPEED = 5;
		private static int JUMPSPEED = 10;
		private static int GRAVITY = -1;

		private Point pos;
		private PointF vel;
		private bool onGround;

		private Keys leftKey = Keys.Left;
		private Keys rightKey = Keys.Right;
		private Keys jumpKey = Keys.Space;
		
		//private Animation sprite;

		public Player(int x, int y)
		{
			pos = new Point(x, y);
			vel = new PointF(0, 0);
			onGround = true;
		}

        public void Draw(Graphics g)
        {
			Pen p = new Pen(Color.Blue);
			int x = pos.X - frmMain.viewX;
			int y = pos.Y - frmMain.viewY;
			g.DrawRectangle(p, x, y, Map.TILE_SIZE, 2 * Map.TILE_SIZE);
        }

        public void Update(float time)
        {
			onGround = false;
			pos.X += (int)(vel.X * time);
			pos.Y += (int)(vel.Y * time);
			vel.Y += GRAVITY * time;
        }

		public void Jump()
		{
			if (onGround)
			{
				vel.Y = -JUMPSPEED;
			}
		}

        public Rectangle GetCollisionBox()
        {
            return new Rectangle(pos, new Size(Map.TILE_SIZE, 2 * Map.TILE_SIZE));
        }

		public void OnCollide(Direction dir)
		{
			switch (dir)
			{
				case Direction.Left:
					vel.X = Math.Max(0, vel.X);
					break;
				case Direction.Right:
					vel.X = Math.Min(0, vel.X);
					break;
				case Direction.Up:
					vel.Y = Math.Max(0, vel.Y);
					break;
				case Direction.Down:
					vel.Y = Math.Min(0, vel.Y);
					onGround = true;
					break;
			}
		}

		public void Kill()
		{
			//do stuff
		}

        public void OnKeyDown(Keys key)
        {
            if (key == leftKey) {
                vel.X = -SPEED;
            }
            else if (key == rightKey) {
                vel.X = SPEED;
            }
            else if (key == jumpKey) {
                Jump();
            }

        }

        public void OnKeyUp(Keys key)
        {
            if (key == leftKey)
            {
                vel.X = 0;
            }
            else if (key == rightKey)
            {
                vel.X = 0;
            }
        }
    }
}
