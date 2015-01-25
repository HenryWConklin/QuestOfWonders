using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace QuestOfWonders
{
	public enum Direction
	{
		Left, Right, Up, Down
	};
	
    public class Player
    {
		private const int SPEED = 300;
		private const int JUMPSPEED = 550;
		private const float GRAVITY = 2000f;
        private const float GRAVITY_CAP = 500;

        private PointF pos;
		private PointF vel;
		private bool onGround;
        private bool isDead;

		private List<Keys> leftKey = new List<Keys>(new Keys[]{Keys.A, Keys.Left});
		private List<Keys> rightKey = new List<Keys>(new Keys[]{Keys.D, Keys.Right});
		private List<Keys> jumpKey = new List<Keys>(new Keys[]{Keys.Space, Keys.Up, Keys.W});
        private Keys restart = Keys.R;

        private bool leftPressed;
        private bool rightPressed;

        private bool facingRight;

        private int animIndex;
        private Animation[] anims;

        private const int ANIM_MOVE = 0;
        private const int ANIM_STILL = 1;
        private const int ANIM_HOLD = 2;

		public Player(int x, int y)
		{
			pos = new PointF(x, y);
			vel = new PointF(0, 0);
			onGround = true;
            isDead = false;
            leftPressed = false;
            rightPressed = false;
            animIndex = ANIM_STILL;
            facingRight = true;
            anims = new Animation[3];
            anims[ANIM_MOVE] = new Animation("walk", 8);
            anims[ANIM_STILL] = new Animation("stand", 1);
            anims[ANIM_HOLD] = new Animation("handsup", 1);
		}

        public void Draw(Graphics g)
        {
            Brush b = new SolidBrush(Color.Red);
			int x = (int)pos.X - frmMain.viewX + (facingRight?0:Map.TILE_SIZE);
			int y = (int)pos.Y - frmMain.viewY;
            //g.FillRectangle(b, x, y, Map.TILE_SIZE, 2 * Map.TILE_SIZE);
            g.DrawImage(anims[animIndex].GetFrame(), x, y, (!facingRight?-1:1) * Map.TILE_SIZE, Map.TILE_SIZE * 2);
        }

        public void Update(float time)
        {
            anims[animIndex].Update(time);
			pos.X += vel.X * time;
            if (pos.X < 0)
            {
                pos.X = 0;
                vel.X = 0;
            }
            if (pos.X + GetCollisionRectangle().Width > frmMain.currentMap.widthInTiles * Map.TILE_SIZE)
            {
                pos.X = frmMain.currentMap.widthInTiles * Map.TILE_SIZE - GetCollisionRectangle().Width;
                vel.X = 0;
            }
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

        public void PickUpItem()
        {
            SetAnim(ANIM_HOLD);
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
            if (leftKey.Contains(key)) {
                vel.X = -SPEED;
                leftPressed = true;
                facingRight = false;
                SetAnim(ANIM_MOVE);
            }
            else if (rightKey.Contains(key)) {
                vel.X = SPEED;
                facingRight = true;
                rightPressed = true;
                SetAnim(ANIM_MOVE);
            }
            else if (jumpKey.Contains(key)) {
                Jump();
            }
            else if (key == restart)
            {
                this.Kill();
            }
        }

        public void OnKeyUp(Keys key)
        {
            if (leftKey.Contains(key))
            {
                leftPressed = false;
                if (rightPressed)
                {
                    vel.X = SPEED;
                    facingRight = true;
                }
                else
                {
                    SetAnim(ANIM_STILL);
                    vel.X = 0;
                }
            }
            else if (rightKey.Contains(key))
            {
                rightPressed = false;
                if (leftPressed)
                {
                    vel.X = -SPEED;
                    facingRight = false;
                }
                else
                {
                    vel.X = 0;
                    SetAnim(ANIM_STILL);
                }
            }
        }

        internal void StopHorizVelocity()
        {
            vel.X = 0;
            this.rightPressed = false;
            this.leftPressed = false;
            SetAnim(ANIM_STILL);
        }
    }
}
