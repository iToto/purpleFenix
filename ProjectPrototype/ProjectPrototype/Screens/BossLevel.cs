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
    class BossLevel : GameScreen
    {
        ContentManager content;
        Texture2D backgroundTexture;

        PlayerManager playerManager;

        DarkFyre boss;
        List<Enemy> enemies;
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

        int numberOfPlayers;


        /// <summary>
        /// Constructor.
        /// </summary>
        public BossLevel(int numberOfPlayers)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.numberOfPlayers = numberOfPlayers;
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

            //Load Level
            levelOne = new Map("Content\\Maps\\Boss.xml", content, "Sprites\\lavaTileset", ScreenManager.GraphicsDevice);

            //Load Music
            soundBank = new SoundBank(ScreenManager.engine, "Content\\Music\\XACT\\Boss.xsb");
            waveBank = new WaveBank(ScreenManager.engine, "Content\\Music\\XACT\\Boss.xwb");

            //Load SFX
            sfxSounds = new SoundBank(ScreenManager.engine, "Content\\Music\\XACT\\SoundFX.xsb");
            sfxWaves = new WaveBank(ScreenManager.engine, "Content\\Music\\XACT\\SoundFX.xwb");

            //Load Boss
            boss = new DarkFyre(content.Load<Texture2D>("Sprites\\bossMaybe"), content, Element.Fire, 50000, sfxSounds);
            boss.position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - boss.sprite.Width / 2, -(boss.sprite.Height));
            boss.alive = true;

            for(int i = 0; i < boss.miniBosses.Count; ++i)
            {
                boss.miniBosses[i].velocity.Y = boss.velocity.Y;
                boss.miniBosses[i].alive = true;
                if (i==0)
                {
                    boss.miniBosses[i].position = new Vector2(boss.position.X + boss.sprite.Width + boss.miniBosses[i].sprite.Width, boss.position.Y - boss.miniBosses[i].sprite.Height/2);
                }
                if (i == 1)
                {
                    boss.miniBosses[i].position = new Vector2(boss.position.X - boss.miniBosses[i].sprite.Width*2, boss.position.Y - boss.miniBosses[i].sprite.Height/2);
                }
                if (i == 2)
                {
                    boss.miniBosses[i].position = new Vector2(boss.position.X - boss.miniBosses[i].sprite.Width*2, boss.position.Y + boss.sprite.Height + boss.miniBosses[i].sprite.Height*2);
                }
                if (i == 3)
                {
                    boss.miniBosses[i].position = new Vector2(boss.position.X + boss.sprite.Width + boss.miniBosses[i].sprite.Width, boss.position.Y + boss.sprite.Height + boss.miniBosses[i].sprite.Height*2);
                }
                
            }
            enemies = new List<Enemy>();
            enemies.AddRange(boss.miniBosses);

            enemies.Add(boss);        

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //Init a Content Manager for the enemy factory
            EnemyFactory.content = content;
            EnemyFactory.sfxBank = sfxSounds;

            //Initialize Players:
            playerManager = new PlayerManager(numberOfPlayers, content, 
                new Vector2(viewport.Width / 2, viewport.Height - 90), sfxSounds);


            //Play Song
            music = soundBank.GetCue("bossSong");
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

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Rectangle viewportRect = new Rectangle(0, 0, viewport.Width, viewport.Height);

                //Update Players
                playerManager.Update(gameTime, viewportRect, enemies);

                levelOne.Update(enemies);


                List<Player> players = playerManager.GetLivingPlayers();

                //Update Boss
                boss.Update(ref viewportRect, gameTime, players);
                
                explosionManager.Update(gameTime);


                //Check if all players are dead.
                if (playerManager.AllPlayersAreDead)
                {
                    LoadingScreen.Load(ScreenManager, false, null, 
                        new ContinueScreen(Levels.EARTH, gameTime, 
                            playerManager.NumberOfPlayers));
                }

                //Check if level has ended.
                if (levelOne.HasReachedEnd && enemies.Count < 1)
                {
                    GoToNextLevel();
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

            levelOne.Draw(ScreenManager.SpriteBatch);

            playerManager.Draw(ScreenManager.SpriteBatch, ScreenManager.Font);
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

            playerManager.HandleInput(input, ScreenManager);

        }

        private void GoToNextLevel()
        {
            music.Stop(AudioStopOptions.Immediate);
            LoadingScreen.Load(ScreenManager, false, null, new CreditsScreen());
        }
    }
}
