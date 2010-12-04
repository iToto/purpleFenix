using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectPrototype
{
    class ExplosionManager
    {
        Texture2D explosionSheet;
        List<Explosion> explosions = new List<Explosion>();

        public ExplosionManager(ContentManager content)
        {
            this.explosionSheet = content.Load<Texture2D>("Sprites\\explosion-sheet");
        }

        public void play(Vector2 position)
        {
            Explosion newExplosion = new Explosion(this.explosionSheet);
            newExplosion.position = position;

            explosions.Add(newExplosion);
        }

        public void Update(GameTime gametime)
        {
            foreach (Explosion explosion in explosions)
            {
                explosion.Update(gametime);
                if (explosion.DoneAnimating)
                {
                    explosions.RemoveAt(explosions.IndexOf(explosion));
                    break;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Explosion explosion in explosions)
            {
                explosion.Draw(spritebatch);
            }
        }
    }
}
