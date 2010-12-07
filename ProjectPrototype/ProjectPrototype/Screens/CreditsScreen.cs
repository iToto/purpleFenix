using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace ProjectPrototype
{
    class CreditsScreen : GameScreen
    {
        ContentManager content;

        SoundBank soundBank;
        WaveBank waveBank;
        Cue music;

        List<String> credits = new List<string>();

        TimeSpan timePerCreditString = new TimeSpan(0, 0, 13);
        TimeSpan timeOnCurrentString = new TimeSpan(0);

        public CreditsScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");


            //Load Music
            soundBank = new SoundBank(ScreenManager.engine, "Content\\Music\\XACT\\Credits.xsb");
            waveBank = new WaveBank(ScreenManager.engine, "Content\\Music\\XACT\\Credits.xwb");

            //Play Song
            music = soundBank.GetCue("CreditsMusic");
            music.Play();

            credits.Add("PROGRAMMING BY: \n BRONSON ZGEB \n ROBBIE CAPUTO \n SALVATORE D'AGOSTINO");
            credits.Add("MUSIC BY: SALVATORE D'AGOSTINO");
            credits.Add("CREDITS SONG BY: \n STATIKZ FT. J. ARTHUR KEENES");
            credits.Add("SOUND CREATED USING: \n         AS3SFXR");
            credits.Add("ART BY: \n ROBBIE CAPUTO \n BRONSON ZGEB");
            credits.Add("ANIMATION BY: BRONSON ZGEB \n\n\n      FONT BY: BRONSON ZGEB");
            credits.Add("LAVA TILESET PROVIDED BY: \n       L0RDPH0ENIX");
            credits.Add("MAP EDITOR PROVIDED BY: \n    NANDOSOFT");
            credits.Add("THANKS FOR PLAYING!                     ");
        }

        public override void Draw(GameTime gametime)
        {
            base.Draw(gametime);

            ScreenManager.SpriteBatch.Begin();

            string nextCredit = credits.First();

            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font,
                nextCredit, 
                new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 -  nextCredit.Length * 3, 
                    ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);

            ScreenManager.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            timeOnCurrentString += gameTime.ElapsedGameTime;

            if (timeOnCurrentString >= timePerCreditString)
            {
                timeOnCurrentString = new TimeSpan(0);
                if (credits.Count > 1)
                {
                    credits.RemoveAt(0);
                }
            }
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (input.IsPauseGame(null))
            {
                music.Stop(AudioStopOptions.Immediate);
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
            }
        }


    }
}
