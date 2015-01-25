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

        private List<String> textboxText = new List<String>()
        {
            "Press % to finally defeat Dr. Waru."
        };

        private List<String> textboxTextShift = new List<String>() 
        {
            "NO not THAT shift key! My God I can't believe you did that AGAIN! I thought you would know better this time. Really. I honestly believed in you. Wow... What a disappointment. I can only imagine how your parents dealt with you all your life. You know what? Forget it. *clears throat* The ignoramus know by few as Torpe decided to simply go and face the awful Dr. Waru without the aid of a mystical object... Because God knows he must be gifted with other magical abilities to mess up this much already. Therefore, Torpe headed to the Doctor's secret hideout hidden inside... The Cave of DOOM!"
        };

        private List<String> textboxText5 = new List<String>()
        {
            "Did you really just click 5? Do you even type? I thought you step up to the plate and save the world this time. Really. I honestly believed in you. Wow... What a disappointment. I can only imagine how your parents dealt with you all your life. You know what? Forget it. *clears throat* The ignoramus know by few as Torpe decided to simply go and face the awful Dr. Waru without the aid of a mystical object... Because God knows he must be gifted with other magical abilities to mess up this much already. Therefore, Torpe headed to the Doctor's secret hideout hidden inside... The Cave of DOOM!"
        };

        public Sword(Point p)
        {
            pos = p;
            img = new Bitmap(Bitmap.FromFile("Resources/sword.png"));
            state = 0;
            textIndex = 0;
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

        }

        public void Draw(Graphics g)
        {
            if (state == 0) g.DrawImage(img, pos.X - frmMain.viewX, pos.Y - frmMain.viewY);
        }

        public void OnKeyDown(Keys key)
        {
            if (state == 1)
            {
                Console.Out.WriteLine(key);
                if (key == Keys.ShiftKey)
                {
                    frmMain.text = new Textbox(textboxTextShift, new Rectangle(150, 50, frmMain.viewWidth - 300, 100));
                    state = 2;
                    BreakSword();
                }
                else if (key == Keys.D5)
                {
                    frmMain.text = new Textbox(textboxText5, new Rectangle(150, 50, frmMain.viewWidth - 300, 100));
                    state = 2;
                    BreakSword();
                }
            }
            else if (state == 2)
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
        private void BreakSword()
        {
            Console.WriteLine("Broke Sword");
        }
    }

  

}
