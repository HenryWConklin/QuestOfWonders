using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace QuestOfWonders
{
    class FinalSceneLauncher : Wonder
    {
        //Bitmap img;
        int x, y;
        int width = 32;
        int height = 96;
        public bool hasBeenHit = false;

        List<int> evilChatTextFrames = new List<int> { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }; //When the player starts

        Color evilCol = Color.DarkRed;
        Color narratorCol = Color.DarkSlateGray;

        public FinalSceneLauncher(int x, int y)
        {
            this.x = x;
            this.y = y;
            //img = new Bitmap(Bitmap.FromFile("Resources/orb of wonder.png"), width, height);
        }

        List<string> textboxText = new List<String>()
        {
            "That's him! That's Dr. Waru!",
            "Uh-oh - it looks like he's preparing for a monologue! I'm out!",
            "...",
            "Nyahahahahahahaha! You discovered my lair, did you?",
            "Well, you are too late! My Death Laser will destroy all life outside of this lab and I shall have my revenge!",
            "Why do I need to exact my vengence? Well, allow me to explain in the form of...",
            "THIS SONG!",
            "When I was a child, I never got my way!",
            "My parents wouldn't get me a new toy every day.",
            "I built a robot army and found a nice warm cave",
            "And here I've laid in wait, and I built a giant laser and now I will destroy the world.",
            "...",
            "Would you like me to sing that again?\n       >Yes          No"
        };

        int textIndex = 0;

        public void Draw(Graphics g)
        {
            //Invisible
        }
        public void Update(float time)
        {

        }

        public void OnKeyDown(Keys key)
        {
            if (hasBeenHit)
            {
                textIndex++;

                if (textIndex < textboxText.Count)
                {
                    if (evilChatTextFrames.Contains(textIndex))
                    {
                        frmMain.text.backColor = new SolidBrush(evilCol);
                    }
                    else
                    {
                        frmMain.text.backColor = new SolidBrush(narratorCol);
                    }
                    frmMain.text.Advance();
                }
                else
                {
                    //FinishLevel();
                    frmMain.allowPlayerControl = true;
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
