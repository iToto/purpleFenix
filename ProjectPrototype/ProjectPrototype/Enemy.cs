using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Diagnostics;

namespace ProjectPrototype
{
    class Enemy : GameObject
    {
        public Enemy(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            Random random = new Random();

            this.velocity.X = 1;
            this.velocity.Y = 1;
        }

        public void Update(ref Rectangle viewportRect)
        {
            this.position.X = this.position.X + ((float)Math.Sin(this.position.Y/10) * 5);
            this.position.Y += this.velocity.Y;

            this.boundingRectangle.X = (int)this.position.X;
            this.boundingRectangle.Y = (int)this.position.Y;
        }
    }
}
