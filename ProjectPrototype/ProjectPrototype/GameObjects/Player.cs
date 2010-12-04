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
        const int MAX_BULLETS = 60;

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

        public Player(Texture2D loadedTexture, ContentManager content)
            : base(loadedTexture, new Vector2(32, 32))
        {
            this.alive = true;
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 150);
            timeSinceLastShot = new TimeSpan(0);
            this.health = 100;


            this.AddAnimation("normal", new int[1] {0}, 4, true);
            this.AddAnimation("colors", new int[2] { 0, 1 }, 4, false);
            this.play("colors");

            healthBar = new HealthBar(content.Load<Texture2D>("Sprites\\healthBar"), this.health);
            healthBar.position = new Vector2(10, 20);
            
            //Initialize Bullets
            for (int i = 0; i < MAX_BULLETS; ++i)
            {
                bullets.Add(new Bullet(content.Load<Texture2D>("Sprites\\Bullet")));
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
                if (bullet.alive)
                {
                    bullet.Update(ref viewportRect);
                    foreach (Enemy enemy in enemies)
                    {
                        if (enemy.alive)
                        {
                            if (bullet.boundingRectangle.Intersects(enemy.boundingRectangle))
                            {
                                switch (bullet.CompareElements(enemy))
                                {
                                    case Defense.Standard:
                                        enemy.Health -= 2;
                                        break;
                                    case Defense.Strong:
                                        enemy.Health -= 1;
                                        break;
                                    case Defense.Weak:
                                        enemy.Health -= 4;
                                        break;
                                    default:
                                        break;
                                }
                                System.Diagnostics.Debug.Print("Enemy Health: " + enemy.Health + "\n");

                                bullet.alive = false;

                                if(enemy.Health <= 0)
                                    enemy.alive = false;

                                break;
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            //spritebatch.Draw(this.sprite, this.position, Color.White);
            if (this.alive)
            {
                spritebatch.Draw(this.sprite, this.position, this.frameRectangle, Color.White);
            }

            healthBar.Draw(spritebatch);
            
            foreach (Bullet bullet in bullets)
            {
                if (bullet.alive)
                {
                    spritebatch.Draw(bullet.sprite, bullet.position, Color.White);
                }
            }
        }

        public void HandleInput(InputState input, PlayerIndex playerIndex)
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
                        this.alive = false;
                        return true;
                    }
                }
            }

            return false;
        }

        private void Fire()
        {
            if (this.bullets[0].type == bulletType.spread)
            {
                //Shoot 5 bullets at a time
                for (int i = 0; i < ShootingPattern.MAX_BULLETS; i+=5)
                {
                    if (!this.bullets[i].alive)
                    {
                        for (int j = i; j < i+5; j++)
                        {
                            this.bullets[j].element = this.element;
                            this.bullets[j].alive = true;
                            this.bullets[j].position = this.position;
                            this.bullets[j].velocity.Y = -4.0f;

                            if (j % 5 == 0)
                            {
                                this.bullets[j].velocity.X = -3.0f;
                            }
                            else if (j % 5 == 1)
                            {
                                this.bullets[j].velocity.X = -1.0f;
                            }
                            else if (j % 5 == 2)
                            {
                                this.bullets[j].velocity.X = 0.0f;
                            }
                            else if (j % 5 == 3)
                            {
                                this.bullets[j].velocity.X = 1.0f;
                            }
                            else
                            {
                                this.bullets[j].velocity.X = 3.0f;
                            }
                        }
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
    }
}
