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
        const int MAX_BULLETS = 60;
        public float MoveHeightVariation { set; private get; }
        public float MoveWidthVariation { set; private get; }
        public List<Bullet> bullets = new List<Bullet>();
        TimeSpan timeSinceLastShot;
        TimeSpan timeBetweenShots;
        public bool hasActiveBullets;

        public Enemy(Texture2D loadedTexture, int path, ContentManager content, Element el, int hp)
            : base(loadedTexture,el,hp)
        {
            Random random = new Random();

            this.velocity.X = 1;
            this.velocity.Y = 1;
            this.typeOfPath = path;
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 200);
            timeSinceLastShot = new TimeSpan(0);
            this.hasActiveBullets = false;
            for (int i = 0; i < MAX_BULLETS; ++i)
            {
                bullets.Add(new Bullet(content.Load<Texture2D>("Sprites\\Bullet")));
            }
        }

        public void Update(ref Rectangle viewportRect, GameTime gameTime, List<Player> players)
        {
            if (this.alive)
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

                if (timeSinceLastShot == new TimeSpan(0) && this.alive)
                {
                    this.Fire();
                }

                timeSinceLastShot += gameTime.ElapsedGameTime;
                if (timeSinceLastShot >= timeBetweenShots)
                {
                    timeSinceLastShot = new TimeSpan(0);
                }
            }


                //Update Bullets and collide with enemies
                foreach (Bullet bullet in bullets)
                {
                    if (bullet.alive)
                    {
                        bullet.Update(ref viewportRect);
                        foreach (Player player in players)
                        {
                            if (player.alive)
                            {
                                if (bullet.boundingRectangle.Intersects(player.boundingRectangle))
                                {
                                    switch (bullet.CompareElements(player))
                                    {
                                        case Defense.Standard:
                                            player.Health -= 2;
                                            break;
                                        case Defense.Strong:
                                            player.Health -= 1;
                                            break;
                                        case Defense.Weak:
                                            player.Health -= 4;
                                            break;
                                        default:
                                            break;
                                    }
                                    bullet.alive = false;
                                    if (player.Health <= 0)
                                        player.Kill();

                                    break;
                                }
                            }
                        }
                    }

            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (this.alive)
            {
                spritebatch.Draw(this.sprite, this.position, Color.White);
            }

            foreach (Bullet bullet in bullets)
            {
                if (bullet.alive)
                {
                    spritebatch.Draw(bullet.sprite, bullet.position, Color.White);
                }
            }
        }

        private void Fire()
        {
            if (bullets[0].type == bulletType.spread)
            {
                //Shoot 5 bullets at a time
                for (int i = 0; i < ShootingPattern.MAX_BULLETS; i += 5)
                {
                    if (!bullets[i].alive)
                    {
                        for (int j = i; j < i + 5; j++)
                        {
                            bullets[j].alive = true;
                            bullets[j].element = this.element;
                            bullets[j].position = this.position;
                            bullets[j].velocity.Y = 4.0f;

                            if (j % 5 == 0)
                            {
                                bullets[j].velocity.X = -3.0f;
                            }
                            else if (j % 5 == 1)
                            {
                                bullets[j].velocity.X = -1.0f;
                            }
                            else if (j % 5 == 2)
                            {
                                bullets[j].velocity.X = 0.0f;
                            }
                            else if (j % 5 == 3)
                            {
                                bullets[j].velocity.X = 1.0f;
                            }
                            else
                            {
                                bullets[j].velocity.X = 3.0f;
                            }
                        }
                        break;
                    }
                }
            }
            else if (bullets[0].type == bulletType.straight)
            {
                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.alive)
                    {
                        bullet.position = this.position;
                        bullet.alive = true;
                        bullet.velocity.Y = 4.0f;
                        break;
                    }
                }
            }
            else if (bullets[0].type == bulletType.helix)
            {
            }
            else if (bullets[0].type == bulletType.doubleShot)
            {
            }
            else if (bullets[0].type == bulletType.speratic)
            {
                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.alive)
                    {
                        bullet.position = this.position;
                        bullet.alive = true;
                        break;
                    }
                }
            }
        }
    
        private void stillHasActiveBullets()
        {
            this.hasActiveBullets = false;
            foreach (Bullet bullet in this.bullets)
            {
                if (bullet.alive)
                {
                    this.hasActiveBullets = true;
                    break;
                }
            }
        }

        public void Kill()
        {
            this.alive = false;
            GameObject.ExplosionManager.play(this.position,
                new Vector2(this.spriteWidth, this.spriteHeight));
        }
    }
}
