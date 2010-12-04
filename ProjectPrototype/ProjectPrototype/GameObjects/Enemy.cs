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
        const int MAX_BULLETS = 200;
        public float MoveHeightVariation { set; private get; }
        public float MoveWidthVariation { set; private get; }
        public List<Bullet> bullets = new List<Bullet>();
        TimeSpan timeSinceLastShot;
        TimeSpan timeBetweenShots;
        public bool hasActiveBullets;

        public Enemy(Texture2D loadedTexture, int path, ContentManager content, Element el, int hp)
            : base(loadedTexture,el,hp)
        {
            Texture2D bulletSprite;
            Random random = new Random();

            this.velocity.X = 1;
            this.velocity.Y = 1;
            this.typeOfPath = path;
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 200);
            timeSinceLastShot = new TimeSpan(0);
            this.hasActiveBullets = false;

            switch (el)
            {
                case Element.Earth:
                    bulletSprite = BulletManager.RockSprite;
                    break;

                case Element.Fire:
                    bulletSprite = BulletManager.FireSprite;
                    break;

                case Element.Ice:
                    bulletSprite = BulletManager.IceSprite;
                    break;

                case Element.Lightning:
                    bulletSprite = BulletManager.LightningSprite;
                    break;

                default:
                    bulletSprite = BulletManager.RockSprite;
                    break;
            }

            for (int i = 0; i < MAX_BULLETS; ++i)
            {
                bullets.Add(new Bullet(bulletSprite));
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
                bullet.Update(ref viewportRect, gameTime);
                foreach (Player player in players)
                {
                    bullet.Collide(player);
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
                bullet.Draw(spritebatch);
            }
        }

        private void Fire()
        {

            // shoot 5 bullets at once
            if (bullets[0].type == bulletType.spread)
            {
                int bulletsFired = 0;

                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.alive)
                    {
                        bullet.alive = true;
                        bullet.element = this.element;
                        bullet.position.X = this.boundingRectangle.Center.X - bullet.boundingRectangle.Width / 2;
                        bullet.position.Y = this.boundingRectangle.Bottom - bullet.boundingRectangle.Height / 2;
                        bullet.velocity.Y = 4.0f;

                        switch (bulletsFired)
                        {
                            case 0:
                                bullet.velocity.X = -3;
                                break;
                            case 1:
                                bullet.velocity.X = -1;
                                break;
                            case 2:
                                bullet.velocity.X = 0;
                                break;
                            case 3:
                                bullet.velocity.X = 1;
                                break;
                            case 4:
                                bullet.velocity.X = 3;
                                break;
                            default:
                                break;
                        }

                        ++bulletsFired;
                    }
                    if (bulletsFired >= 5)
                    {
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

        public override void Kill()
        {
            this.alive = false;
            GameObject.ExplosionManager.play(this.position,
                new Vector2(this.spriteWidth, this.spriteHeight));
        }
    }
}
