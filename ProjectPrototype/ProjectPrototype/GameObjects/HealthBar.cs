using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPrototype
{
    class HealthBar : GameObject
    {
        public int CurrentHealth { set; private get; }
        public int maxHealth { protected set; get; }

        public HealthBar(Texture2D loadedTexture, int maxHealth)
            : base(loadedTexture)
        {
            this.maxHealth = maxHealth;
            this.CurrentHealth = maxHealth;
        }

        public void Draw(SpriteBatch spritebatch, PlayerIndex playerIndex, SpriteFont font)
        {
            spritebatch.Draw(this.sprite, new Rectangle((int)this.position.X, (int)this.position.Y, sprite.Width, sprite.Height), Color.Gray);
            spritebatch.Draw(this.sprite, new Rectangle((int)this.position.X, (int)this.position.Y, sprite.Width * CurrentHealth / maxHealth, sprite.Height), Color.Red);

            string playerText;
            switch(playerIndex)
            {
                case PlayerIndex.One:
                    playerText = "P1";
                    break;
                case PlayerIndex.Two:
                    playerText = "P2";
                    break;
                case PlayerIndex.Three:
                    playerText = "P3";
                    break;
                case PlayerIndex.Four:
                    playerText = "P4";
                    break;
                default:
                    playerText = "P1";
                    break;
            }

            spritebatch.DrawString(font, playerText, this.position, Color.White);
        }
    }
}
