using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectPrototype
{
    class Explosion : GameObject
    {
        public bool DoneAnimating { protected set; get; }

        public Explosion(Texture2D loadedTexture)
            : base(loadedTexture, new Vector2(32, 32))
        {
            AddAnimation("explode", new int[5] {0, 1, 2, 3, 4}, 10, false);
            play("explode");
        }

        public void Update(GameTime gametime)
        {
            this.frameRectangle = updateAnimation(gametime);
            this.DoneAnimating = this.CurrentAnimation.Done;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(this.sprite, this.position, this.frameRectangle, Color.White);
        }
    }
}
