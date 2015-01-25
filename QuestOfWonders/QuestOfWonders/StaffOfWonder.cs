using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace QuestOfWonders
{
    class StaffOfWonder : Wonder
    {
        Bitmap img;
        Bitmap aura;
        int x, y;
        int width = 32;
        int height = 64;
        float vy;
        public bool hasBeenHit = false;
        public bool cntrlClicked = false;
        private bool holding;
        private bool dropped;

        Color playerCol = Color.DarkSlateBlue;
        Color narratorCol = Color.DarkSlateGray;

        public StaffOfWonder(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.vy = 0;
            holding = false;
            dropped = false;
            img = new Bitmap(Bitmap.FromFile("Resources/staff.png"), width, height);
            aura = new Bitmap(Bitmap.FromFile("Resources/awesomness.png"));
        }

        List<string> textboxText = new List<String>()
        {
            "Congratualtions. You found the staff... Press control to finally defeat Dr. Waru.",
            "No! You… you moron! Not that control! The one one the other side! Who uses the control on THAT side?" +
            " Now you've broken the Ancient Staff of Wondrous Wonders, all because you couldn't use the correct control. Now what will happen to this world!? All is lost! There’s nothing we can - WAIT!",
            "There’s one thing we can try! It’s our last resort, left by the people of the Ghyathanti tribe long ago: The Secret Sword of Wonderfully" +
            " Wonderful Wonders! The world doesn’t have much time! *clears throat*  And thus, the brilliant... er... the fantastic...",
            "Hmm...",
            "Well. Torpe ran as quickly as possible to the Red Fields of Horrendously Hideous Trials to retrieve the sword and humanity’s LAST hope!"
        };

        int textIndex = 0;

        public void Draw(Graphics g)
        {
            if (aura != null)
                g.DrawImage(aura, x - frmMain.viewX - 32, y - frmMain.viewY - 32, width + 64, height + 64);
           g.DrawImage(img, x - frmMain.viewX, y - frmMain.viewY, width, height);
        }
        public void Update(float time)
        {
            if (holding)
            {
                x = frmMain.player.GetPos().X;
                y = frmMain.player.GetPos().Y - 2 * Map.TILE_SIZE - 10;
            }
            if (dropped)
            {
                x += (int)(150 * time * (frmMain.player.facingRight ? 1 : -1));
                y += (int)(vy * time);
                vy += 2000 * time;
                if (y >= frmMain.player.GetPos().Y + 2 * Map.TILE_SIZE - height)
                {
                    SoundSystem.playSound("QuestOfWonders.Resources.Break.wav", false);
                    y -= 2;
                    dropped = false;
                    img = new Bitmap(Bitmap.FromFile("Resources/broken staff.png"));
                    aura = null;
                    width = img.Width;
                }
            }
        }

        public bool OnKeyDown(Keys key)
        {
            if (hasBeenHit)
            {
                if (!cntrlClicked)
                {
                    if (key == Keys.ControlKey)
                    {
                        cntrlClicked = true;
                        frmMain.text.Advance();
                        BreakOrb();
                    }
                }
                else
                {             

                    textIndex++;

                    if (textIndex < textboxText.Count - 1)
                    {
                        frmMain.text.Advance();
                    }
                    else
                    {
                        FinishLevel();
						return true;
                    }
                }
            }
			return false;
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
                frmMain.player.PickUpItem();
                SoundSystem.playSound("QuestOfWonders.Resources.Quest of Wonders Stinger 2.wav", false);
            }
        }

        public int getX() { return x; }
        public int getY() { return y; }
        public int getWidth() { return width; }
        public int getHeight() { return height; }
        
        
    }
}
