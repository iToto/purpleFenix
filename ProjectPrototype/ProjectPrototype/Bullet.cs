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


namespace ProjectPrototype
{
    class Bullet : GameObject
    {
        public Bullet(Texture2D loadedTexture)
            : base(loadedTexture)
        {

        }
        
        
        public void Update(ref Rectangle viewportRect)
        {
            if (this.alive)
            {
                this.rectangle.X += (int)this.velocity.X;
                this.rectangle.Y += (int)this.velocity.Y;
            }

            if (!viewportRect.Contains(new Point((int)this.rectangle.Location.X, (int)this.rectangle.Location.Y)))
            {
                this.alive = false;
            }
        }
    }
}
