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
        int x, y;
        int width = 32;
        int height = 64;
        public bool hasBeenHit = false;
        public bool cntrlClicked = false;

        int breakIndex = 2; //Break on text frame 2

        Color playerCol = Color.DarkSlateBlue;
        Color narratorCol = Color.DarkSlateGray;

        public static string dirRepl = "<DIR>";
        public static string otherDirRepl = "<OTHERDIR>";

        public StaffOfWonder(int x, int y)
        {
            this.x = x;
            this.y = y;
            img = new Bitmap(Bitmap.FromFile("Resources/staff.png"), width, height);
        }

        List<string> textboxText = new List<String>()
        {
            "Congratualtions. You found the staff... Press control to finally defeat Dr. Waru.",
            "No! You… you moron! Did you just press the " + dirRepl + " control? Now the staff is broken too! What were you thinking? It's never the " + dirRepl + " control! It's always " + otherDirRepl + " control!" +
            " Now you've broken the Ancient Staff of Wondrous Wonders. Now what will happen to this world!? All is lost! There’s nothing we can- WAIT!",
            "There’s one thing we can try! It’s the last resort that the people of the Ghyathanti tribe left this world long ago! The Secret Sword of Wonderfully" +
            " Wonderful Wonders! The world doesn’t have much time! *clears throat*  And thus, the brilliant...",
            "Hmm...",
            "Well. Torpe ran as quickly as possible to the Red Fields of Horrendously Hideous Trials to retrieve the sword and humanity’s LAST hope!!!!!"
        };

        int textIndex = 0;

        public void Draw(Graphics g)
        {
            if(!hasBeenHit) g.DrawImage(img, x - frmMain.viewX, y - frmMain.viewY, width, height);
        }
        public void Update(float time)
        {

        }

        public void OnKeyDown(Keys key)
        {
            if (hasBeenHit)
            {
                if (!cntrlClicked)
                {
                    if (key == Keys.LControlKey)
                    {

                    }
                    else if (key == Keys.RControlKey)
                    {

                    }
                    cntrlClicked = true;
                }
                else
                {
                    textIndex++;

                    if (textIndex == breakIndex)
                    {
                        BreakOrb();
                    }

                    if (textIndex < textboxText.Count)
                    {
                        frmMain.text.Advance();
                    }
                    else
                    {
                        FinishLevel();
                    }
                }
            }
        }

        public void BreakOrb()
        {
            Console.WriteLine("You Broke It!");
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
