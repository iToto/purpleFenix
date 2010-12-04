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

        TimeSpan delay;
        TimeSpan timeSinceFrameChange;

        //Texture2D spriteSheet;

        public Animation(string name, int[] frames, int frameDelay, bool looped, int frameWidth, int frameHeight)
        {
            this.name = name;
            this.frames = frames;
            this.delay = new TimeSpan(0, 0, frameDelay);
            this.looped = looped;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

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

            int currentFrame = this.frames[frameIndex];

            return new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        }

        public void resetFrameIndex()
        {
            this.frameIndex = 0;
        }
    }
}
