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
		private const int SPEED = 300;
		private const int JUMPSPEED = 500;
		private const float GRAVITY = 2000f;
        private const float GRAVITY_CAP = 500;

        private PointF pos;
		private PointF vel;
		private bool onGround;
        private bool isDead;

		private Keys leftKey = Keys.Left;
		private Keys rightKey = Keys.Right;
		private Keys jumpKey = Keys.Space;

        private bool leftPressed;
        private bool rightPressed;

        private int animIndex;
        private Animation[] anims;

        private const int ANIM_LEFT = 0;
        private const int ANIM_RIGHT = 1;

		public Player(int x, int y)
		{
			pos = new PointF(x, y);
			vel = new PointF(0, 0);
			onGround = true;
            isDead = false;
            leftPressed = false;
            rightPressed = false;
            animIndex = ANIM_LEFT;
            anims = new Animation[2];
            anims[ANIM_LEFT] = new Animation("linkwalkingleft", 10);
            anims[ANIM_RIGHT] = new Animation("linkwalkingright", 10);
		}

        public void Draw(Graphics g)
        {
            Brush b = new SolidBrush(Color.Red);
			int x = (int)pos.X - frmMain.viewX;
			int y = (int)pos.Y - frmMain.viewY;
            //g.FillRectangle(b, x, y, Map.TILE_SIZE, 2 * Map.TILE_SIZE);
            g.DrawImage(anims[animIndex].GetFrame(), x, y, Map.TILE_SIZE, Map.TILE_SIZE * 2);
        }

        public void Update(float time)
        {
            anims[animIndex].Update(time);
			pos.X += vel.X * time;
			pos.Y += vel.Y * time;
            if (! onGround)
            {
                vel.Y += GRAVITY * time;
                vel.Y = Math.Min(vel.Y, GRAVITY_CAP);
            }
            onGround = false;
        }

		public void Jump()
		{
			if (onGround)
			{
				vel.Y = -JUMPSPEED;
			}
		}

        public Point GetPos()
        {
            return new Point((int)pos.X, (int)pos.Y);
        }

        public void SetPosX(int x)
        {
            this.pos.X = x;
        }

        public void SetPosY(int y)
        {
            this.pos.Y = y;
        }

		public void OnCollide(Direction dir)
		{
			switch (dir)
			{
				case Direction.Left:
					//vel.X = Math.Max(0, vel.X);
					break;
				case Direction.Right:
					//vel.X = Math.Min(0, vel.X);
					break;
				case Direction.Up:
					vel.Y = Math.Max(0, vel.Y);
					break;
				case Direction.Down:
					vel.Y = Math.Min(0, vel.Y);
                    //onGround = true;
					break;
			}
		}

        public Rectangle GetCollisionRectangle()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, Map.TILE_SIZE, 2 * Map.TILE_SIZE);
        }

        public void Ground()
        {
            onGround = true;
        }

		public void Kill()
		{
            isDead = true;
		}

        private void SetAnim(int index)
        {
            anims[animIndex].Reset();
            animIndex = index;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void OnKeyDown(Keys key)
        {
            if (key == leftKey) {
                vel.X = -SPEED;
                leftPressed = true;
                SetAnim(ANIM_LEFT);
            }
            else if (key == rightKey) {
                vel.X = SPEED;
                rightPressed = true;
                SetAnim(ANIM_RIGHT);
            }
            else if (key == jumpKey) {
                Jump();
            }
        }

        public void OnKeyUp(Keys key)
        {
            if (key == leftKey)
            {
                leftPressed = false;
                if (rightPressed)
                {
                    vel.X = SPEED;
                    SetAnim(ANIM_RIGHT);
                }
                else
                {
                    vel.X = 0;
                }
            }
            else if (key == rightKey)
            {
                rightPressed = false;
                if (leftPressed)
                {
                    vel.X = -SPEED;
                    SetAnim(ANIM_LEFT);
                }
                else
                {
                    vel.X = 0;
                }
            }
        }
    }
}
