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
        

        public bulletType type;

        public Bullet(Texture2D loadedTexture)
            : base(loadedTexture)
        {

        }
        
        
        public void Update(ref Rectangle viewportRect)
        {
            //System.Diagnostics.Debug.Print("Alive: " + this.alive + "\n");
            if (this.alive)
            {
                this.position.X += this.velocity.X;
                this.position.Y += this.velocity.Y;

                this.boundingRectangle.X = (int)this.position.X;
                this.boundingRectangle.Y = (int)this.position.Y;
            }

            if (!viewportRect.Contains(new Point((int)this.boundingRectangle.Center.X, (int)this.boundingRectangle.Center.Y)))
            {
                this.alive = false;
            }
        }
    }
}
