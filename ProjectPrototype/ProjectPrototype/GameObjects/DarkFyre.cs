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
    class DarkFyre : Enemy
    {
        const int MAX_BULLETS = 200;
        const int NUM_ENEMIES = 4;
        public List<Enemy> miniBosses = new List<Enemy>();

        TimeSpan timeSinceLastShot;
        TimeSpan timeBetweenShots;

        public DarkFyre(Texture2D loadedTexture, ContentManager content, Element el, int hp, SoundBank sfx)
            : base(loadedTexture,content,el,2200,sfx)
        {
            Random random = new Random();

            this.sfx = sfx;

            this.velocity.X = 0;
            this.velocity.Y = 0.5f;
            
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 800);
            timeSinceLastShot = new TimeSpan(0);
            this.hasActiveBullets = false;

            //Add miniEnemies
            for (int i = 0; i < 1; ++i)
            {
                miniBosses.Add(new Enemy(content.Load<Texture2D>("Sprites\\enemy"), content, Element.Earth, 10, sfx));
                miniBosses.Add(new Enemy(content.Load<Texture2D>("Sprites\\enemy2"), content, Element.Lightning, 10, sfx));
                miniBosses.Add(new Enemy(content.Load<Texture2D>("Sprites\\enemy3"), content, Element.Fire, 10, sfx));
                miniBosses.Add(new Enemy(content.Load<Texture2D>("Sprites\\enemy4"), content, Element.Ice, 10, sfx));
            }

            //for (int i = 0; i < MAX_BULLETS; ++i)
            //{
            //    bullets.Add(new Bullet(bulletSprite, this.element));
            //}

            this.boundingRectangle.Width = 400;
            this.boundingRectangle.Height = 120;
        }

        public override void Update(ref Rectangle viewportRect, GameTime gameTime, List<Player> players)
        {
            //Update miniBoss' positions
            foreach (Enemy enemy in this.miniBosses)
            {
                enemy.Update(ref viewportRect, gameTime, players);
                enemy.velocity.Y = this.velocity.Y;
            }

            if (this.alive)
            {
                //TODO Move the boss in some manner
                if (this.position.Y >= viewportRect.Height / 3)
                    this.velocity.Y = 0;

                this.position.Y += this.velocity.Y;

                this.boundingRectangle.X = 
                    (int)this.position.X + this.spriteWidth / 2 - this.boundingRectangle.Width / 2;

                this.boundingRectangle.Y = 
                    (int)this.position.Y + this.spriteHeight / 2 - this.boundingRectangle.Height / 2;

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

        public override void Draw(SpriteBatch spritebatch)
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

            BulletManager.Fire(this.bullets, this, 1, 0);
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

        public bool AllEnemiesAreDead()
        {
            foreach (Enemy enemy in miniBosses)
            {
                if (enemy.alive)
                {
                    return false;
                }
            }

            if (this.alive)
            {
                return false;
            }

            return true;
        }
    }
}
