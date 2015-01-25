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
        int songEntranceIndex = 8;
        int songStop = 13;
        public bool playSong = true;
        public bool hasBeenHit = false;

        List<int> torpeChat = new List<int> { 3, 14 };
        List<int> evilChatTextFrames = new List<int> { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15 }; //When the player starts

        Color evilCol = Color.DarkRed;
        Color narratorCol = Color.DarkSlateGray;
        Color torpeCol = Color.DarkSlateBlue;

        public FinalSceneLauncher(int x, int y)
        {
            this.x = x;
            this.y = y;
            //img = new Bitmap(Bitmap.FromFile("Resources/orb of wonder.png"), width, height);
        }

        List<string> textboxText = new List<String>()
        {
            "That's him! That's Dr. Waru!",
            "Uh-oh - it looks like he's preparing for a monologue! Get out while you still can!",
            "...",
            "...",
            "Nyahahahahahahaha! So, you discovered my lair! Here to stop me?",
            "Well, you are too late! My Ultron-Beam Death Machine of Doom (patent pending) is already prepped to fire! I shall have my revenge!",
            "Ah-em",
            "Why do I need to exact my vengence? Well, allow me to explain in the form of...",
            "THIS SONG!",
            "When I was little, I was tiny and small!",
            "When I was little, I swore that one day I would be tall!",
            "But I'm still little, I am tiny and small!",
            "So now, I'll burn the world instead! ",
            "...",
            "...",
            "Should I sing it again?\n\n>>Yes             No"
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
                    if (playSong && textIndex == songEntranceIndex)
                    {
                        SoundSystem.playSound("QuestOfWonders.Resources.Quest of Wonders2.wav", true);
                    }
                    if (playSong && textIndex == songStop)
                    {
                        SoundSystem.stopAllSounds();
                    }
                    if (torpeChat.Contains(textIndex))
                    {
                        frmMain.text.backColor = new SolidBrush(torpeCol);
                    }
                    else if (evilChatTextFrames.Contains(textIndex))
                    {
                        frmMain.text.backColor = new SolidBrush(evilCol);
                    }
                    else
                    {
                        frmMain.text.backColor = new SolidBrush(narratorCol);
                    }
                    frmMain.text.Advance();
                    if (frmMain.text.done)
                    {
                        frmMain.allowPlayerControl = true;
                    }
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
                SoundSystem.stopAllSounds();
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
