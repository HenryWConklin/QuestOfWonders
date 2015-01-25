using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace QuestOfWonders
{
    public interface Wonder
    {
        int getX();
        int getY();
        int getWidth();
        int getHeight();
        void LaunchCollisionEvent();
        void Update(float time);
        void Draw(Graphics g);
        bool OnKeyDown(Keys key);
    }
}
