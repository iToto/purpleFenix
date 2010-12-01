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
        List<Enemy> helixes = new List<Enemy>();
        List<Enemy> lokusts = new List<Enemy>();
        List<Enemy> enemies = new List<Enemy>();

        List<Bullet> bullets = new List<Bullet>();
        TimeSpan timeSinceLastSpawn;
        Map levelOne;

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

            EnemyFactory.content = content;

            //Load Level
            levelOne = new Map("Content\\Maps\\testMap.xml", content, "Sprites\\zelda", ScreenManager.GraphicsDevice);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //Initialize Bullets
            for (int i = 0; i < MAX_BULLETS; ++i)
            {
                bullets.Add(new Bullet(content.Load<Texture2D>("Sprites\\Bullet")));
            }

            //Initialize First Player
            playerOne = new Player(content.Load<Texture2D>("Sprites\\greenShip"), bullets);
            playerOne.position = new Vector2(viewport.Width / 2, viewport.Height - 60);
            playerOne.boundingRectangle.X = (int)playerOne.position.X;
            playerOne.boundingRectangle.Y = (int)playerOne.position.Y;


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
                playerOne.Update(ref viewportRect, gameTime, bullets, enemies);
            }

            levelOne.Update(enemies);


            //Update enemies
            foreach (Enemy enemy in enemies)
            {
                if (enemy.alive)
                {
                    enemy.Update(ref viewportRect);
                }
            }
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();

            levelOne.Draw(ScreenManager.SpriteBatch);
            if (playerOne.alive)
            {
                playerOne.Draw(ScreenManager.SpriteBatch);
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
#if !XBOX
            KeyboardState keyboardState = input.CurrentKeyboardStates[1];
            KeyboardState previousKeyboardState = input.LastKeyboardStates[1];


            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                ScreenManager.Game.Exit();
            }
#endif
            playerOne.HandleInput(input, PlayerIndex.One);
            //playerTwo.HandleInput(input, bullets, PlayerIndex.Two);
            //playerThree.HandleInput(input, bullets, PlayerIndex.Three);
            //playerFour.HandleInput(input, bullets, PlayerIndex.Four);

        }

        public void SpawnEnemies()
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            int i = 0;

            foreach (Enemy enemy in helixes)
            {
                if (!enemy.alive)
                {
                    enemy.alive = true;
                    enemy.position = new Vector2(viewport.Width / 2, 0 - (i * 10));
                    ++i;
                }
            }
            foreach (Enemy enemy in lokusts)
            {
                if (!enemy.alive)
                {
                    enemy.alive = true;
                    enemy.position = new Vector2(0 - (i * 10), 0);
                    ++i;
                }
            }
        }
    }
}