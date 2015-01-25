using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace QuestOfWonders
{
    class Sword : Wonder
    {
        private Point pos;
        private Bitmap img;
        private int state;
        private int textIndex;
        private float vy;

        private List<String> textboxText = new List<String>()
        {
            "Press % to finally defeat Dr. Waru."
        };

        private List<String> textboxTextShift = new List<String>() 
        {
            "NO not the shift key! My God I can't believe you did that... AGAIN!",
            "I thought you would know better this time. Really. I honestly believed in you. Wow... What a disappointment. I can only imagine how your parents dealt with you all your life.",
            "You know what? Forget it.",
            "*clears throat*",
            "Well then, Torpe, it truly has come to this. You must go and face the awful Dr. Waru without the aid of ANY of the THREE mystical objects... because GOD KNOWS there's something special about you.",
            "Time to visit the secret hideout of Dr. Waru, hidden inside..."
        };

        private List<String> textboxText5 = new List<String>()
        {
            "Did you really just click 5? Do you even type?",
            "I thought you step up to the plate and save the world this time. Really. I honestly believed in you. Wow... What a disappointment. I can only imagine how your parents dealt with you all your life.",
            "You know what? Forget it.",
            "*clears throat*",
            "Well then, Torpe, it truly has come to this. You must go and face the awful Dr. Waru without the aid of ANY of the THREE mystical objects... because GOD KNOWS there's something special about you.",
            "Time to visit the secret hideout of Dr. Waru hidden inside...",
            "The Cave of DOOM!"
        };

        public Sword(Point p)
        {
            pos = p;
            img = new Bitmap(Bitmap.FromFile("Resources/sword.png"));
            state = 0;
            textIndex = 0;
            vy = 0;
        }

        public int getX()
        {
            return pos.X;
        }

        public int getY()
        {
            return pos.Y;
        }

        public int getWidth()
        {
            return img.Width;
        }

        public int getHeight()
        {
            return img.Height;
        }

        public void LaunchCollisionEvent()
        {
            if (state == 0)
            {
                frmMain.allowPlayerControl = false;
                state = 1;
                frmMain.text = new Textbox(textboxText, new Rectangle(150, 50, frmMain.viewWidth - 300, 100));
                frmMain.StopPlayerHoriz();
            }
        }

        public void Update(float time)
        {
            if (state==1)
            {
                pos.X = frmMain.player.GetPos().X - 16;
                pos.Y = frmMain.player.GetPos().Y - Map.TILE_SIZE;
            }
            if (state==2)
            {
                pos.X += (int)(200 * time);
                pos.Y += (int)(vy * time);
                vy += 2000 * time;
                if (pos.Y >= frmMain.player.GetPos().Y + 2 * Map.TILE_SIZE - getHeight())
                {
                    state = 3;
                    img = new Bitmap(Bitmap.FromFile("Resources/broken sword.png"), getWidth(), getHeight());
                }
            }

        }

        public void Draw(Graphics g)
        {
            g.DrawImage(img, pos.X - frmMain.viewX, pos.Y - frmMain.viewY);
        }

        public void OnKeyDown(Keys key)
        {
            if (state == 1)
            {
                if (key == Keys.ShiftKey)
                {
                    frmMain.text = new Textbox(textboxTextShift, new Rectangle(150, 50, frmMain.viewWidth - 300, 100));
                    state = 2;
                }
                else if (key == Keys.D5)
                {
                    frmMain.text = new Textbox(textboxText5, new Rectangle(150, 50, frmMain.viewWidth - 300, 100));
                    state = 2;
                }
            }
            else if (state >= 2)
            {
                textIndex++;
               
                if (textIndex < textboxText5.Count())
                {
                    frmMain.text.Advance();
                }
                else
                {
                    frmMain.NextLevel();
                }
                
            }
        }
    }

  

}
