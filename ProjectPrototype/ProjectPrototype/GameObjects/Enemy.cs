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
        private int typeOfPath; //0->Sine 1->Parabola

        public float MoveHeightVariation { set; private get; }
        public float MoveWidthVariation { set; private get; }

        public Enemy(Texture2D loadedTexture,int path)
            : base(loadedTexture)
        {
            Random random = new Random();

            this.velocity.X = 1;
            this.velocity.Y = 1;
            this.typeOfPath = path;
        }

        public void Update(ref Rectangle viewportRect)
        {
            if (this.typeOfPath == 0)
            {
                this.position = MovementPath.sineWave(this, this.MoveHeightVariation, this.MoveWidthVariation);
            }
            else
            {
                this.position = MovementPath.parabola(this, 0.003f, 800f);
            }
            
            
            this.boundingRectangle.X = (int)this.position.X;
            this.boundingRectangle.Y = (int)this.position.Y;
        }
    }
}
