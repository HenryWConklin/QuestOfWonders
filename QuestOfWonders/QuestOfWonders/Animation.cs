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
        private bool looping;
        private bool running;

        public Animation(String name, int numFrames, bool loop = true)
        {
            frames = new Bitmap[numFrames];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = new Bitmap(Bitmap.FromFile("Resources/" + name + i + ".png"));
                
            }
            looping = loop;
            frame = 0;
            time = 0;
            running = true;
        }

        public void Update(float timePassed)
        {
            if (running)
            {
                time += timePassed;
                while (time > FRAME_TIME)
                {
                    frame++;
                    if (looping)
                        frame %= frames.Length;
                    else if (frame >= frames.Length)
                        frame = frames.Length - 1;
                    time -= FRAME_TIME;
                }
            }
        }

        public void Pause()
        {
            running = false;
        }

        public void Resume()
        {
            running = true;
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
