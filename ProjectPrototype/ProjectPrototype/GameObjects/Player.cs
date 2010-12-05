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

        PlayerIndex playerIndex;
        
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

        public Player(Texture2D loadedTexture, ContentManager content, Element element, PlayerIndex playerIndex)
            : base(loadedTexture, new Vector2(64, 64))
        {
            Texture2D bulletSprite;

            this.alive = true;
            timeBetweenShots = new TimeSpan(0, 0, 0, 0, 150);
            timeSinceLastShot = new TimeSpan(0);
            this.health = 100;

            this.element = element;

            this.AddAnimation("normal", new int[1] {0}, 4, false);
            this.play("normal");

            this.playerIndex = playerIndex;

            if (playerIndex == PlayerIndex.One)
            {
                healthBar = new HealthBar(content.Load<Texture2D>("Sprites\\healthBar"), this.health);
                healthBar.position = new Vector2(50, 30);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                healthBar = new HealthBar(content.Load<Texture2D>("Sprites\\healthBar"), this.health);
                healthBar.position = new Vector2(170, 30);
            }
            else if (playerIndex == PlayerIndex.Three)
            {
                healthBar = new HealthBar(content.Load<Texture2D>("Sprites\\healthBar"), this.health);
                healthBar.position = new Vector2(290, 30);
            }
            else if (playerIndex == PlayerIndex.Four)
            {
                healthBar = new HealthBar(content.Load<Texture2D>("Sprites\\healthBar"), this.health);
                healthBar.position = new Vector2(410, 30);
            }

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

        public void Update(ref Rectangle viewportRect, GameTime gameTime, List<Enemy> enemies,SoundBank sfx)
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
                        this.Fire(sfx);
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

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            //spritebatch.Draw(this.sprite, this.position, Color.White);
            if (this.alive)
            {
                spritebatch.Draw(this.sprite, 
                    new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight), 
                    this.frameRectangle, Color.White);
            }

            healthBar.Draw(spritebatch, this.playerIndex, font);
            
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
#endif
            //Check for Pause
            if (input.IsPauseGame(playerIndex))
            {
                screenManager.AddScreen(new PauseMenuScreen(), null);
            }
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

        private void Fire(SoundBank sfx)
        {
            BulletManager.Fire(this.bullets, this, -1);
            sfx.PlayCue("choot");
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
