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
        SoundBank soundBank;
        ExplosionManager explosionManager;
        BulletManager bulletManager;
        WaveBank waveBank;
        

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
        }


        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
         
            //Initialize Explosion Manager
            explosionManager = new ExplosionManager(content);

            //Initialize Bullet Manager
            bulletManager = new BulletManager(content);

            //Init the explosion manager for all game objects
            GameObject.ExplosionManager = explosionManager;

            //Init a Content Manager for the enemy factory
            EnemyFactory.content = content;

            //Load Level
            levelOne = new Map("Content\\Maps\\testMap.xml", content, "Sprites\\zelda", ScreenManager.GraphicsDevice);

            //Load Music
            soundBank = new SoundBank(ScreenManager.engine, "Content\\Music\\XACT\\Level1.xsb");
            waveBank = new WaveBank(ScreenManager.engine, "Content\\Music\\XACT\\Level1.xwb");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //Initialize First Player
            playerOne = new Player(content.Load<Texture2D>("Sprites\\greenShip"), content);

            playerOne.position = new Vector2(viewport.Width / 2, viewport.Height - 60);
            playerOne.boundingRectangle.X = (int)playerOne.position.X;
            playerOne.boundingRectangle.Y = (int)playerOne.position.Y;
       
            ShootingPattern.shootSpread(playerOne.bullets, content);
            //ShootingPattern.shootStraight(bullets, content);
            //ShootingPattern.shootSperatic(bullets,content);

            //Play Song
            soundBank.PlayCue("Level Song 1");
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!otherScreenHasFocus)
            {
                List<Player> players = new List<Player>();

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Rectangle viewportRect = new Rectangle(0, 0, viewport.Width, viewport.Height);

                //Update player One
                playerOne.Update(ref viewportRect, gameTime, enemies);

                // Add playerOne to the alive players array.
                if (playerOne.alive)
                {
                    players.Add(playerOne);
                }

                levelOne.Update(enemies);

                //Update enemies
                foreach (Enemy enemy in enemies)
                {
                    enemy.Update(ref viewportRect, gameTime, players);
                }

                explosionManager.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();

            levelOne.Draw(ScreenManager.SpriteBatch);
            
            playerOne.Draw(ScreenManager.SpriteBatch);

            explosionManager.Draw(ScreenManager.SpriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(ScreenManager.SpriteBatch);
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
                //ScreenManager.Game.Exit();
            }
#endif
            playerOne.HandleInput(input, PlayerIndex.One,ScreenManager);
            //playerTwo.HandleInput(input, bullets, PlayerIndex.Two);
            //playerThree.HandleInput(input, bullets, PlayerIndex.Three);
            //playerFour.HandleInput(input, bullets, PlayerIndex.Four);

        }
    }
}