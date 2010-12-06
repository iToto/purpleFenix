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

        public Enemy(Texture2D loadedTexture, int path, ContentManager content, Element el, int hp, SoundBank sfx)
            : base(loadedTexture,el,hp)
        {
            Texture2D bulletSprite;
            Random random = new Random();

            this.sfx = sfx;

            this.velocity.X = 1;
            if (path == 2)
            {
                this.velocity.Y = 2;
            }
            else
            {
                this.velocity.Y = 1;
            }

            this.typeOfPath = path;
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 800);
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
                bullets.Add(new Bullet(bulletSprite, this.element));
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
                else if (this.typeOfPath == 1)
                {
                    this.position = MovementPath.parabola(this, 0.003f, 800f);
                }
                else
                {
                    this.position.Y += this.velocity.Y;
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

                if (CheckIfTravelledOffscreen(viewportRect))
                {
                    this.alive = false;
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

            stillHasActiveBullets();
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
            BulletManager.Fire(this.bullets, this, 1);
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
            this.sfx.PlayCue("explode");
        }

        private bool CheckIfTravelledOffscreen(Rectangle viewportRect)
        {
            if (this.boundingRectangle.Top > viewportRect.Bottom)
            {
                return true;
            }

            if (this.boundingRectangle.Left > viewportRect.Right)
            {
                return true;
            }

            return false;
        }
    }
}
