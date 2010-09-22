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
            this.velocity.X = 1;
        }

        public void Update(ref Rectangle viewportRect)
        {
            //Debug.Print( "Y: "+Math.Sin(this.rectangle.X/10));
            this.position.Y = (float)Math.Sin(this.position.X/10) * 10;
            this.position.X += this.velocity.X;
        }
    }
}
