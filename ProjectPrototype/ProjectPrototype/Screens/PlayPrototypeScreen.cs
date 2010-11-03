using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
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

    class PlayPrototypeScreen : GameScreen
    {
        ContentManager content;

        Player playerOne;
        List<Enemy> enemies = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        TimeSpan timeSinceLastSpawn;

        const int MAX_BULLETS = 60;
        const int MAX_ENEMIES = 10;
        const int SPAWN_TIME = 10;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlayPrototypeScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            timeSinceLastSpawn = TimeSpan.Zero;
        }


        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //Initialize First Player
            playerOne = new Player(content.Load<Texture2D>("Sprites\\greenShip"));
         
            playerOne.position = new Vector2(viewport.Width / 2, viewport.Height - 60);
            playerOne.boundingRectangle.X = (int)playerOne.position.X;
            playerOne.boundingRectangle.Y = (int)playerOne.position.Y;


            //Initialize Bullets
            for (int i = 0; i < MAX_BULLETS; ++i)
            {
                bullets.Add(new Bullet(content.Load<Texture2D>("Sprites\\Bullet")));
            }


            //Initialize Enemies
            for (int i = 0; i < MAX_ENEMIES; ++i)
            {
                enemies.Add(new Enemy(content.Load<Texture2D>("Sprites\\DevilHead")));
                enemies[i].alive = true;
                enemies[i].position = new Vector2(viewport.Width / 2, 0 - (i * 10));
            }
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle viewportRect = new Rectangle(0, 0, viewport.Width, viewport.Height);

            timeSinceLastSpawn += gameTime.ElapsedGameTime;

            if (timeSinceLastSpawn >= TimeSpan.FromSeconds(SPAWN_TIME))
            {
                SpawnEnemies();
                timeSinceLastSpawn = TimeSpan.Zero;
            }

            //Update player One
            if (playerOne.alive)
            {
                playerOne.Update(ref viewportRect);
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
                                bullet.alive = false;
                                enemy.alive = false;
                                //enemies.RemoveAt(enemies.IndexOf(enemy));
                                break;
                            }
                        }
                    }
                }
            }

            //Update enemies
            foreach (Enemy enemy in enemies)
            {
                if (enemy.alive)
                {
                    enemy.Update(ref viewportRect);
                }
            }

            //Check collision with enemies
            playerOne.CheckEnemyCollision(ref enemies);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();
            if (playerOne.alive)
            {
                ScreenManager.SpriteBatch.Draw(playerOne.sprite, playerOne.position, Color.White);
            }

            foreach (Bullet bullet in bullets)
            {
                if (bullet.alive)
                {
                    ScreenManager.SpriteBatch.Draw(bullet.sprite, bullet.position, Color.White);
                }
            }

            foreach (Enemy enemy in enemies)
            {
                if (enemy.alive)
                {
                    ScreenManager.SpriteBatch.Draw(enemy.sprite, enemy.position, Color.White);
                }
            }

            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "PROTOTYPE", Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.End();
        }


        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            KeyboardState keyboardState = input.CurrentKeyboardStates[1];
            KeyboardState previousKeyboardState = input.LastKeyboardStates[1];

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                ScreenManager.Game.Exit();
            }

            playerOne.HandleInput(input);

            if (keyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C))
            {
                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.alive)
                    {
                        bullet.position = playerOne.position;
                        bullet.velocity.Y = -4.0f;
                        bullet.alive = true;
                        break;
                    }
                }
            }
        }

        public void SpawnEnemies()
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            int i = 0;

            foreach (Enemy enemy in enemies)
            {
                if (!enemy.alive)
                {
                    enemy.alive = true;
                    enemy.position = new Vector2(viewport.Width / 2, 0 - (i * 10));
                    ++i;
                }
            }
        }
    }
}

/*
#if !XBOX
        KeyboardState previousKeyboardState;
#endif
        GamePadState previousGamepadState;

        Random random = new Random();

        const int maxAsteroids = 5;
        const int maxBullets = 60;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            screenManager.AddScreen(new BackgroundScreen(), null);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerOne = new Player(Content.Load<Texture2D>("Sprites\\greenShip"));

            playerOne.position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                graphics.GraphicsDevice.Viewport.Height - 60);
            playerOne.boundingRectangle.X = (int)playerOne.position.X;
            playerOne.boundingRectangle.Y = (int)playerOne.position.Y;

            for (int i = 0; i < maxAsteroids; ++i)
            {
                enemies.Add(new Enemy(Content.Load<Texture2D>("Sprites\\DevilHead")));

                enemies[i].position = new Vector2(
                    random.Next(graphics.GraphicsDevice.Viewport.Width), 
                    random.Next(-200, 0));
                enemies[i].boundingRectangle.X = (int)enemies[i].position.X;
                enemies[i].boundingRectangle.Y = (int)enemies[i].position.Y;
                enemies[i].alive = true;
            }

            //bullets = new Bullet[maxBullets];
            for (int i = 0; i < maxBullets; ++i)
            {
                bullets.Add(new Bullet(Content.Load<Texture2D>("Sprites\\Bullet")));
            }

            viewportRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, 
                graphics.GraphicsDevice.Viewport.Height);
            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            if (playerOne.alive)
            {
                playerOne.Update(ref viewportRect);
            }

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
                                enemies.RemoveAt(enemies.IndexOf(enemy));
                                break;
                            }
                        }
                    }
                }
            }

            foreach (Enemy enemy in enemies)
            {
                if (enemy.alive)
                {
                    enemy.Update(ref viewportRect);

                    if (enemy.boundingRectangle.Intersects(playerOne.boundingRectangle))
                    {
                        playerOne.alive = false;
                    }
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public void HandleInput()
        {
#if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();
#endif
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (gamepadState.Buttons.Back == ButtonState.Pressed 
                || keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

#if !XBOX
            if (playerOne.alive)
            {
                playerOne.HandleInput(ref keyboardState, ref previousKeyboardState);
                if (keyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C))
                {
                    foreach (Bullet bullet in bullets)
                    {
                        if (!bullet.alive)
                        {
                            bullet.position = playerOne.position;
                            bullet.velocity.Y = -4.0f;
                            bullet.alive = true;
                            break;
                        }
                    }
                }
            }
            previousKeyboardState = keyboardState;
#endif
            previousGamepadState = gamepadState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (playerOne.alive)
            {
                spriteBatch.Draw(playerOne.sprite, playerOne.position, Color.White);
            }

            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    spriteBatch.Draw(enemy.sprite, enemy.position, Color.White);
                }
            }

            foreach (Bullet bullet in bullets)
            {
                if (bullet.alive)
                {
                    spriteBatch.Draw(bullet.sprite, bullet.position, Color.White);
                }
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
*/