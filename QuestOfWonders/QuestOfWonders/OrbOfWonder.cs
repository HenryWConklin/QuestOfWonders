using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace QuestOfWonders
{
    class OrbOfWonder
    {
        Bitmap img;
        int x, y, width, height;
        public bool hasBeenHit = false;

        public void Draw(Graphics g)
        {

        }
        public void Update(float time)
        {

        }

        public void KeyDown(Keys key)
        {
            if (hasBeenHit)
            {

            }
        }

        public void LaunchCollisionEvent()
        {
            if (!hasBeenHit)
            {

            }
        }
    }
}
