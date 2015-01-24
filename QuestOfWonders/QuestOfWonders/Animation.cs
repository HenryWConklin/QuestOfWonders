using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QuestOfWonders
{
    class Animation
    {
        private const float FRAME_TIME = 1f / 10;

        private Bitmap[] frames;

        private float time;
        private int frame;

        public Animation(String name, int numFrames)
        {
            frames = new Bitmap[numFrames];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = new Bitmap(Bitmap.FromFile("Resources/" + name + i + ".png"));
                
            }

            frame = 0;
            time = 0;
        }

        public void Update(float timePassed)
        {
            time += timePassed;
            while (time > FRAME_TIME)
            {
                frame++;
                frame %= frames.Length;
                time -= FRAME_TIME;
            }
        }

        public Bitmap GetFrame()
        {
            return frames[frame];
        }

        public void Reset()
        {
            frame = 0;
        }

    
    }
}
