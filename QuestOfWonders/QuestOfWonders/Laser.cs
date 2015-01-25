using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace QuestOfWonders
{
    public class Laser
    {

        public int x, y;
        private int drawWidth = 10*24;
        private int drawHeight = 9*24;
        public int width = 5*24;
        public int height = 9*24;
        public int widthOffset;
        Bitmap img;
        public bool hasLaunched = false;

        Brush firstCol = new SolidBrush(Color.DarkSlateBlue);
        Brush finalCol = new SolidBrush(Color.DarkSlateGray);


        public Laser(int x, int y)
        {
            img = new Bitmap(Bitmap.FromFile("Resources/evil laserBig.png"), drawWidth, drawHeight);
            this.x = x;
            this.y = y;
            this.widthOffset = drawWidth - width;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(img, x - frmMain.viewX, y - frmMain.viewY, drawWidth, drawHeight);
        }

        public void OnCollide()
        {
            if (!hasLaunched)
            {
                //SoundSystem.playSound("QuestOfWonders.Resources.Quest of Wonders Stinger 4.wav", false);
                Textbox text = new Textbox(new List<String>() { "Press Space to use the Laser!", "TORPE! DID YOU REALLY JUST-" });
                text.backColor = firstCol;
                frmMain.text = text;
                hasLaunched = true;
                frmMain.allowPlayerControl = false;
            }
        }

        public void KeyDown(Keys key)
        {
            if (hasLaunched && key == Keys.Space && !frmMain.finalKey)
            {
                frmMain.text.backColor = finalCol;
                frmMain.text.Advance();
                frmMain.finalKey = true;
                frmMain.allowPlayerControl = false;
            }
        }

        public Rectangle GetCollisionRect()
        {
            return new Rectangle(x + widthOffset, y, width, height);
        }

    }
}
