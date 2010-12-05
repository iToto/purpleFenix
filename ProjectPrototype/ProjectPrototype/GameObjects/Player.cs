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
    
    class Player : GameObject
    {
        const int MAX_BULLETS = 200;

        public float speed = 4.0f;
        TimeSpan timeSinceLastShot;
        TimeSpan timeBetweenShots;
        bool isShooting = false;
        public List<Bullet> bullets = new List<Bullet>();
        
        HealthBar healthBar;

        public new int Health
        {
            set
            {
                this.health = value;
                this.healthBar.CurrentHealth = (int)MathHelper.Clamp(value, 0, this.healthBar.maxHealth);
            }

            get
            {
                return this.health;
            }
                  
        }

        public Player(Texture2D loadedTexture, ContentManager content, Element element)
            : base(loadedTexture, new Vector2(32, 32))
        {
            Texture2D bulletSprite;

            this.alive = true;
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 150);
            timeSinceLastShot = new TimeSpan(0);
            this.health = 100;

            this.element = element;

            this.AddAnimation("normal", new int[1] {0}, 4, false);
            this.play("normal");

            healthBar = new HealthBar(content.Load<Texture2D>("Sprites\\healthBar"), this.health);
            healthBar.position = new Vector2(10, 20);

            switch (this.element)
            {
                case Element.Lightning:
                    bulletSprite = BulletManager.LightningSprite;
                    break;

                case Element.Ice:
                    bulletSprite = BulletManager.IceSprite;
                    break;

                case Element.Fire:
                    bulletSprite = BulletManager.FireSprite;
                    break;

                case Element.Earth:
                    bulletSprite = BulletManager.RockSprite;
                    break;

                default:
                    bulletSprite = BulletManager.IceSprite;
                    break;
            }

            //Initialize Bullets
            for (int i = 0; i < MAX_BULLETS; ++i)
            {
                bullets.Add(new Bullet(bulletSprite, this.element));
            }
        }

        public void Update(ref Rectangle viewportRect, GameTime gameTime, List<Enemy> enemies)
        {
            if (this.alive)
            {
                frameRectangle = this.updateAnimation(gameTime);

                this.position.X += this.velocity.X;
                this.position.Y += this.velocity.Y;

                this.boundingRectangle.X = (int)this.position.X;
                this.boundingRectangle.Y = (int)this.position.Y;

                if (this.position.X < 0)
                {
                    this.position.X = 0;
                }

                if (this.position.X > viewportRect.Width - this.sprite.Width)
                {
                    this.position.X = viewportRect.Width - this.sprite.Width;
                }

                if (this.position.Y < 0)
                {
                    this.position.Y = 0;
                }

                if (this.position.Y > (viewportRect.Height - this.sprite.Height))
                {
                    this.position.Y = viewportRect.Height - this.sprite.Height;
                }

                if (this.isShooting)
                {
                    if (timeSinceLastShot == new TimeSpan(0))
                    {
                        this.Fire();
                    }

                    timeSinceLastShot += gameTime.ElapsedGameTime;
                    if (timeSinceLastShot >= timeBetweenShots)
                    {
                        timeSinceLastShot = new TimeSpan(0);
                    }
                }

                CheckEnemyCollision(enemies);

            }

            //Update Bullets and collide with enemies
            foreach (Bullet bullet in bullets)
            {
                bullet.Update(ref viewportRect, gameTime);
                foreach (Enemy enemy in enemies)
                {
                    bullet.Collide(enemy);
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            //spritebatch.Draw(this.sprite, this.position, Color.White);
            if (this.alive)
            {
                spritebatch.Draw(this.sprite, 
                    new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight), 
                    this.frameRectangle, Color.White);
            }

            healthBar.Draw(spritebatch);
            
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spritebatch);
            }
        }

        public void HandleInput(InputState input, PlayerIndex playerIndex, ScreenManager screenManager)
        {
            if (!this.alive)
            {
                return;
            }

#if !XBOX
            KeyboardState keyboardState = input.CurrentKeyboardStates[(int)playerIndex];
            KeyboardState previousKeyboardState = input.LastKeyboardStates[(int)playerIndex];
#endif

            GamePadState gamepadState = input.CurrentGamePadStates[(int)playerIndex];
            GamePadState previousGamepadState = input.LastGamePadStates[(int)playerIndex];

            this.velocity.X = 0;

            Vector2 thumbstick = gamepadState.ThumbSticks.Left;
            
            this.velocity.X = this.speed * thumbstick.X;
            this.velocity.Y = this.speed * -thumbstick.Y;

            if (gamepadState.IsButtonDown(Buttons.A))
            {
                this.isShooting = true;
            }
            else if (gamepadState.IsButtonUp(Buttons.A))
            {
                this.isShooting = false;
            }

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                this.velocity.X = -this.speed;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.velocity.X = this.speed;
            }

            this.velocity.Y = 0;
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.velocity.Y = -this.speed;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.velocity.Y = this.speed;
            }

            if (keyboardState.IsKeyDown(Keys.C))
            {
                this.isShooting = true;
            }
            else if (keyboardState.IsKeyUp(Keys.C))
            {
                this.isShooting = false;
            }

            //Check for Pause
            if (input.IsPauseGame(playerIndex))
            {
                screenManager.AddScreen(new PauseMenuScreen(), null);
            }


#endif
        }


        private bool CheckEnemyCollision(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.alive)
                {
                    if (enemy.boundingRectangle.Intersects(this.boundingRectangle))
                    {
                        Kill();
                        return true;
                    }
                }
            }

            return false;
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
                        bullet.position.Y = this.boundingRectangle.Top - bullet.boundingRectangle.Height / 2;
                        bullet.velocity.Y = -4.0f;

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
            else if (this.bullets[0].type == bulletType.straight)
            {
                foreach (Bullet bullet in this.bullets)
                {
                    if (!bullet.alive)
                    {
                        bullet.position = this.position;
                        bullet.alive = true;
                        bullet.velocity.Y = -4.0f;
                        break;
                    }
                }                
            }
            else if (this.bullets[0].type == bulletType.helix)
            {
            }
            else if (this.bullets[0].type == bulletType.doubleShot)
            {
            }
            else if (this.bullets[0].type == bulletType.speratic)
            {
                foreach (Bullet bullet in this.bullets)
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

        public override void Kill()
        {
            this.Health = 0;
            this.alive = false;
            GameObject.ExplosionManager.play(this.position, 
                new Vector2(this.spriteWidth, this.spriteHeight));
        }

        public override void Damage(int damageTaken)
        {
            this.Health -= damageTaken;
        }
    }
}
