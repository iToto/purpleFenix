using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPrototype
{
    class Animation
    {
        int[] frames;
        string name;
        bool looped;

        int frameWidth;
        int frameHeight;
        int frameIndex;
        int frameCount;

        TimeSpan delay;
        TimeSpan timeSinceFrameChange;

        public Animation(string name, int[] frames, int frameRate, bool looped, int frameWidth, int frameHeight)
        {
            this.name = name;
            this.frames = frames;
            this.looped = looped;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameCount = this.frames.Count();

            int miliseconds = 1000 / frameRate;
            this.delay = new TimeSpan(0, 0, 0, 0, miliseconds);

            this.timeSinceFrameChange = new TimeSpan(0);
        }

        public Rectangle Update(GameTime gametime)
        {
            this.timeSinceFrameChange += gametime.ElapsedGameTime;

            if (timeSinceFrameChange >= delay)
            {
                ++frameIndex;
                timeSinceFrameChange = new TimeSpan(0);
            }

            if (frameIndex >= frameCount)
            {
                if (looped)
                {
                    frameIndex = frameIndex % frameCount;
                }
                else
                {
                    frameIndex = frameCount - 1;
                }
            }

            int currentFrame = this.frames[frameIndex];

            int xPosition = currentFrame * frameWidth;

            return new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        }

        public void resetFrameIndex()
        {
            this.frameIndex = 0;
        }
    }
}
