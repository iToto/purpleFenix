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

        public Bullet(Texture2D loadedTexture, Element element)
            : base(loadedTexture, new Vector2(16, 16))
        {
            AddAnimation("normal", new int[3] { 0, 1, 2 }, 15, true);
            play("normal");

            this.element = element;
        }

        public void Update(ref Rectangle viewportRect, GameTime gametime)
        {
            this.frameRectangle = updateAnimation(gametime);

            if (this.alive)
            {
                this.position.X += this.velocity.X;
                this.position.Y += this.velocity.Y;

                this.boundingRectangle.X = (int)this.position.X;
                this.boundingRectangle.Y = (int)this.position.Y;

                if (!viewportRect.Contains(new Point((int)this.boundingRectangle.Center.X, (int)this.boundingRectangle.Center.Y)))
                {
                    this.alive = false;
                }
            }

 
        }

        public void Collide(GameObject opponent,ScoreManager score)
        {
            if (opponent.alive && this.alive)
            {
                if (this.boundingRectangle.Intersects(opponent.boundingRectangle))
                {
                    switch (opponent.CompareElements(this))
                    {
                        case Defense.Standard:
                            opponent.Damage(2, Defense.Standard);
                            break;
                        case Defense.Strong:
                            opponent.Damage(1, Defense.Strong);
                            break;
                        case Defense.Weak:
                            opponent.Damage(4, Defense.Weak);
                            break;
                        default:
                            break;
                    }
                    this.alive = false;
                    if (opponent.Health <= 0)
                    {
                        opponent.Kill();
                        if (this.element == Element.Fire && opponent.element == Element.Earth)
                            score.AddPoints(10);
                        else if (this.element == Element.Earth && opponent.element == Element.Lightning)
                            score.AddPoints(10);
                        else if (this.element == Element.Ice && opponent.element == Element.Fire)
                            score.AddPoints(10);
                        else if (this.element == Element.Lightning && opponent.element == Element.Ice)
                            score.AddPoints(10);
                        else
                            score.AddPoints(5);

                            
                    }
                }
            }
        }

        public void Collide(GameObject opponent)
        {
            if (opponent.alive && this.alive)
            {
                if (this.boundingRectangle.Intersects(opponent.boundingRectangle))
                {
                    switch (opponent.CompareElements(this))
                    {
                        case Defense.Standard:
                            opponent.Damage(2, Defense.Standard);
                            break;
                        case Defense.Strong:
                            opponent.Damage(1, Defense.Strong);
                            break;
                        case Defense.Weak:
                            opponent.Damage(4, Defense.Weak);
                            break;
                        default:
                            break;
                    }
                    this.alive = false;
                    if (opponent.Health <= 0)
                    {
                        opponent.Kill();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (this.alive)
            {
                spritebatch.Draw(this.sprite,
                    new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight),
                    this.frameRectangle, Color.White);
            }
        }
    }
}
