using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace QuestOfWonders
{
    class OrbOfWonder : Wonder
    {
        Bitmap img;
        int x, y;
        float vy;
        int width = 30;
        int height = 30;
        public bool hasBeenHit = false;
        private bool holding = false;
        private bool dropped = false;

        int breakIndex = 3; //Break on text frame 3
        List<int> playerChatTextFrames = new List<int>{6, 8}; //When the player starts

        Color playerCol = Color.DarkSlateBlue;
        Color narratorCol = Color.DarkSlateGray;

        public OrbOfWonder(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.vy = 0;
            img = new Bitmap(Bitmap.FromFile("Resources/orb of wonder.png"), width, height);
        }

        List<string> textboxText = new List<String>()
        {
            "Congratualtions! You found the orb! Press any key to save the world.",
            "NO! Not that key! Press the ‘any’ key.",
            "I SAID… PRESS THE ANY KEY TO SAVE THE WORLD!",
            "Did you just..?  Did you really just drop and destroy the Orb of Wonders? What are you, a buffoon? Now what will we do? How will you stop Dr. Waru!? There’s nothing you can… WAIT!",
            "I know something else that may be able to defeat Dr. Waru! It’s a long shot and it will be far more challenging, but I know you can do it!",
            "…Or rather, since it’s YOUR fault if he wins now, you had better not fail!",
            "But-",
            "DO NOT SASS ME, TORPE! You are a silent protagonist and you will act as such!",
            "...",
            "That’s better… Now, ONWARD TO ADVENTURE!"
        };

        int textIndex = 0;

        public void Draw(Graphics g)
        {
            g.DrawImage(img, x - frmMain.viewX, y - frmMain.viewY, width, height);
        }

        public void Update(float time)
        {
            if (holding)
            {
                x = frmMain.player.GetPos().X;
                y = frmMain.player.GetPos().Y - Map.TILE_SIZE;
            }
            if (dropped)
            {
                x += (int)(150 * time);
                y += (int)(vy * time);
                vy += 2000 * time;
                if (y >= frmMain.player.GetPos().Y + 2 * Map.TILE_SIZE - height)
                {
                    y -= 2;
                    dropped = false;
                    img = new Bitmap(Bitmap.FromFile("Resources/broken orb.png"));
                }
            }
        }

        public void OnKeyDown(Keys key)
        {
            if (hasBeenHit)
            {
                textIndex++;

                if (textIndex == breakIndex)
                {
                    BreakOrb();
                }

                if (textIndex < textboxText.Count)
                {
                    if (playerChatTextFrames.Contains(textIndex))
                    {
                        frmMain.text.backColor = new SolidBrush(playerCol);
                    }
                    else
                    {
                        frmMain.text.backColor = new SolidBrush(narratorCol);
                    }
                    frmMain.text.Advance();
                }
                else
                {
                    FinishLevel();
                }
            }
        }

        public void BreakOrb()
        {
            dropped = true;
            holding = false;
        }

        public void FinishLevel()
        {
            frmMain.NextLevel();
        }

        public void LaunchCollisionEvent()
        {
            if (!hasBeenHit)
            {
                frmMain.allowPlayerControl = false;
                hasBeenHit = true;
                holding = true;
                frmMain.text = new Textbox(textboxText, new Rectangle(150, 50, frmMain.viewWidth - 300, 100));
                frmMain.StopPlayerHoriz();
            }
        }

        public int getX() { return x; }
        public int getY() { return y; }
        public int getWidth() { return width; }
        public int getHeight() { return height; }
        
        
    }
}
