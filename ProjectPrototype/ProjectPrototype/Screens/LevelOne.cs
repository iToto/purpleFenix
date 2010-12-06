﻿using System;
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
        Texture2D backgroundTexture;

        Player playerOne;
        Player playerTwo;
        Player playerThree;
        Player playerFour;

        List<Enemy> enemies = new List<Enemy>();
        SoundBank soundBank;
        ExplosionManager explosionManager;
        BulletManager bulletManager;
        WaveBank waveBank;

        SoundBank sfxSounds;
        WaveBank sfxWaves;

        Cue music;

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

            //Load Background
            backgroundTexture = content.Load<Texture2D>("Sprites\\Lava");

            //Load Level
            levelOne = new Map("Content\\Maps\\TestMap2.xml", content, "Sprites\\lavaTileset", ScreenManager.GraphicsDevice);

            //Load Music
            soundBank = new SoundBank(ScreenManager.engine, "Content\\Music\\XACT\\Level1.xsb");
            waveBank = new WaveBank(ScreenManager.engine, "Content\\Music\\XACT\\Level1.xwb");

            //Load SFX
            sfxSounds = new SoundBank(ScreenManager.engine, "Content\\Music\\XACT\\SoundFX.xsb");
            sfxWaves = new WaveBank(ScreenManager.engine, "Content\\Music\\XACT\\SoundFX.xwb");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //Init a Content Manager for the enemy factory
            EnemyFactory.content = content;
            EnemyFactory.sfxBank = sfxSounds;

            //Initialize First Player
            playerOne = new Player(content.Load<Texture2D>("Sprites\\fireShip"), content, Element.Fire, PlayerIndex.One, sfxSounds);

            playerOne.position = new Vector2(viewport.Width / 2, viewport.Height - 90);
            playerOne.boundingRectangle.X = (int)playerOne.position.X;
            playerOne.boundingRectangle.Y = (int)playerOne.position.Y;

            //Initialize Second Player
            playerTwo = new Player(content.Load<Texture2D>("Sprites\\waterShip"), content, Element.Ice, PlayerIndex.Two, sfxSounds);

            playerTwo.position = new Vector2(viewport.Width / 2 + 65, viewport.Height - 90);
            playerTwo.boundingRectangle.X = (int)playerTwo.position.X;
            playerTwo.boundingRectangle.Y = (int)playerTwo.position.Y;

            //Initialize Third Player
            playerThree = new Player(content.Load<Texture2D>("Sprites\\earthShip"), content, Element.Earth, PlayerIndex.Three, sfxSounds);

            playerThree.position = new Vector2(viewport.Width / 2 + 130, viewport.Height - 90);
            playerThree.boundingRectangle.X = (int)playerThree.position.X;
            playerThree.boundingRectangle.Y = (int)playerThree.position.Y;

            //Initialize Fourth Player
            playerFour = new Player(content.Load<Texture2D>("Sprites\\elecShip"), content, Element.Lightning, PlayerIndex.Four, sfxSounds);

            playerFour.position = new Vector2(viewport.Width / 2 + 195, viewport.Height - 90);
            playerFour.boundingRectangle.X = (int)playerFour.position.X;
            playerFour.boundingRectangle.Y = (int)playerFour.position.Y;

            ShootingPattern.shootStraight(playerOne.bullets);
            ShootingPattern.shootStraight(playerTwo.bullets);
            ShootingPattern.shootStraight(playerThree.bullets);
            ShootingPattern.shootStraight(playerFour.bullets);

            //Play Song
            music = soundBank.GetCue("Level Song 1");
            music.Play();
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!otherScreenHasFocus)
            {
                if (music.IsPaused)
                {
                    music.Resume();
                }

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
                List<Enemy> enemiesToRemove = new List<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    if (!enemy.alive && !enemy.hasActiveBullets)
                    {
                        enemiesToRemove.Add(enemy);
                    }
                    enemy.Update(ref viewportRect, gameTime, players);
                }

                enemies.RemoveAll(enemiesToRemove.Contains);

                explosionManager.Update(gameTime);


                //Check if all players are dead
                if (players.Count < 1)
                {
                    LoadingScreen.Load(ScreenManager, false, null, new ContinueScreen(Levels.EARTH, gameTime));
                }
            }
            else
            {
                music.Pause();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.Draw(backgroundTexture, 
                new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height),  
                Color.White);

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
