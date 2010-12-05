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
    class LevelOne : GameScreen
    {
        ContentManager content;

        Player playerOne;
        Player playerTwo;
        Player playerThree;
        Player playerFour;

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
        public LevelOne()
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
            levelOne = new Map("Content\\Maps\\TestMap2.xml", content, "Sprites\\zelda", ScreenManager.GraphicsDevice);

            //Load Music
            soundBank = new SoundBank(ScreenManager.engine, "Content\\Music\\XACT\\Level1.xsb");
            waveBank = new WaveBank(ScreenManager.engine, "Content\\Music\\XACT\\Level1.xwb");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //Initialize First Player
            playerOne = new Player(content.Load<Texture2D>("Sprites\\fireShip"), content, Element.Fire, PlayerIndex.One);

            playerOne.position = new Vector2(viewport.Width / 2, viewport.Height - 90);
            playerOne.boundingRectangle.X = (int)playerOne.position.X;
            playerOne.boundingRectangle.Y = (int)playerOne.position.Y;

            //Initialize Second Player
            playerTwo = new Player(content.Load<Texture2D>("Sprites\\waterShip"), content, Element.Ice, PlayerIndex.Two);

            playerTwo.position = new Vector2(viewport.Width / 2 + 65, viewport.Height - 90);
            playerTwo.boundingRectangle.X = (int)playerTwo.position.X;
            playerTwo.boundingRectangle.Y = (int)playerTwo.position.Y;

            //Initialize Third Player
            playerThree = new Player(content.Load<Texture2D>("Sprites\\earthShip"), content, Element.Earth, PlayerIndex.Three);

            playerThree.position = new Vector2(viewport.Width / 2 + 130, viewport.Height - 90);
            playerThree.boundingRectangle.X = (int)playerThree.position.X;
            playerThree.boundingRectangle.Y = (int)playerThree.position.Y;

            //Initialize Fourth Player
            playerFour = new Player(content.Load<Texture2D>("Sprites\\elecShip"), content, Element.Lightning, PlayerIndex.Four);

            playerFour.position = new Vector2(viewport.Width / 2 + 195, viewport.Height - 90);
            playerFour.boundingRectangle.X = (int)playerFour.position.X;
            playerFour.boundingRectangle.Y = (int)playerFour.position.Y;

            ShootingPattern.shootStraight(playerOne.bullets);
            ShootingPattern.shootStraight(playerTwo.bullets);
            ShootingPattern.shootStraight(playerThree.bullets);
            ShootingPattern.shootStraight(playerFour.bullets);
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
                playerTwo.Update(ref viewportRect, gameTime, enemies);
                playerThree.Update(ref viewportRect, gameTime, enemies);
                playerFour.Update(ref viewportRect, gameTime, enemies);

                // Add playerOne to the alive players array.
                if (playerOne.alive)
                {
                    players.Add(playerOne);
                }

                if (playerTwo.alive)
                {
                    players.Add(playerTwo);
                }

                if (playerThree.alive)
                {
                    players.Add(playerThree);
                }

                if (playerFour.alive)
                {
                    players.Add(playerFour);
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

            playerOne.Draw(ScreenManager.SpriteBatch, ScreenManager.Font);
            playerTwo.Draw(ScreenManager.SpriteBatch, ScreenManager.Font);
            playerThree.Draw(ScreenManager.SpriteBatch, ScreenManager.Font);
            playerFour.Draw(ScreenManager.SpriteBatch, ScreenManager.Font);

            explosionManager.Draw(ScreenManager.SpriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(ScreenManager.SpriteBatch);
            }

            ScreenManager.SpriteBatch.End();
        }


        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            playerOne.HandleInput(input, PlayerIndex.One, ScreenManager);
            playerTwo.HandleInput(input, PlayerIndex.Two, ScreenManager);
            playerThree.HandleInput(input, PlayerIndex.Three, ScreenManager);
            playerFour.HandleInput(input, PlayerIndex.Four, ScreenManager);

        }
    }
}
