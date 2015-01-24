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
        void LaunchCollisionEvent();
        void Update(float time);
        void Draw(Graphics g);
        void OnKeyDown(Keys key);
    }
}
