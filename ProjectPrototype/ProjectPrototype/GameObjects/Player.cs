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
        public float speed = 4.0f;
        TimeSpan timeSinceLastShot;
        TimeSpan timeBetweenShots;
        bool isShooting = false;

        List<Bullet> bullets;
        const int MAX_BULLETS = 100;

        public Player(Texture2D loadedTexture, List<Bullet> bullets)
            : base(loadedTexture)
        {
            this.alive = true;
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 150);
            timeSinceLastShot = new TimeSpan(0);

            this.bullets = bullets;
        }

        public void Update(ref Rectangle viewportRect, GameTime gameTime, List<Enemy> enemies)
        {
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
                    this.Fire(bullets);
                }

                timeSinceLastShot += gameTime.ElapsedGameTime;
                if (timeSinceLastShot >= timeBetweenShots)
                {
                    timeSinceLastShot = new TimeSpan(0);
                }
            }

            CheckEnemyCollision(enemies);


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
                                bullet.alive = false;
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
            spritebatch.Draw(this.sprite, this.position, Color.White);
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

        private void Fire(List<Bullet> bullets)
        {
            foreach (Bullet bullet in bullets)
            {
                if (!bullet.alive)
                {
                    bullet.position = this.position;
                    bullet.velocity.Y = -4.0f;
                    bullet.alive = true;
                    break;
                }
            }
        }

    }
}
