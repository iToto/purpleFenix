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
    class DarkFyre : GameObject
    {
        const int MAX_BULLETS = 200;
        public float MoveHeightVariation { set; private get; }
        public float MoveWidthVariation { set; private get; }
        public List<Bullet> bullets = new List<Bullet>();
        public List<Enemy> bossParts = new List<Enemy>();

        TimeSpan timeSinceLastShot;
        TimeSpan timeBetweenShots;
        public bool hasActiveBullets;

        public DarkFyre(Texture2D loadedTexture, ContentManager content, Element el, int hp, SoundBank sfx)
            : base(loadedTexture,el,hp)
        {
            Texture2D bulletSprite;
            Random random = new Random();

            this.sfx = sfx;

            this.velocity.X = 1;
            
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 800);
            timeSinceLastShot = new TimeSpan(0);
            this.hasActiveBullets = false;

            //for (int i = 0; i < MAX_BULLETS; ++i)
            //{
            //    bullets.Add(new Bullet(bulletSprite, this.element));
            //}
        }

        public void Update(ref Rectangle viewportRect, GameTime gameTime, List<Player> players)
        {
            if (this.alive)
            {
                //TODO Move the boss in some manner

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
            //TODO have different parts fire

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
