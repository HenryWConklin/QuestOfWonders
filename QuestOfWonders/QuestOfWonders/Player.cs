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
		private static const int SPEED = 5;
		private static const int JUMPSPEED = 10;
		private static const int GRAVITY = -1;

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
			int x = pos.X - formMain.veiwX;
			int y = pos.Y - formMain.viewY;
			g.DrawRectangle(p, pos.X, pos.Y, Map.TILE_SIZE, 2 * Map.TILE_SIZE);
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

		public void OnCollide(Direction dir)
		{
			switch (dir)
			{
				case Left:
					vel.X = Math.Max(0, vel.X);
					break;
				case Right:
					vel.X = Math.Min(0, vel.X);
					break;
				case Up:
					vel.Y = Math.Max(0, vel.Y);
					break;
				case Down:
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
			switch (key)
			{
				case leftKey:
					vel.X = -SPEED;
					break;
				case rightKey:
					vel.X = SPEED
					break;
				case jumpKey:
					Jump();
					break;
			}
        }

        public void OnKeyUp(Keys key)
        {
			switch (key)
			{
				case leftKey:
					vel.X = 0;
					break;
				case rightKey:
					vel.X = 0;
					break;
				case jumpKey:
					break;
			}
        }
    }
}
