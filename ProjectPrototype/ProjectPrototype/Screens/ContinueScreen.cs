using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ProjectPrototype
{
    class ContinueScreen : MenuScreen
    {
        // Time out limit in ms.
        static private int TimeOutLimit = 10000; // 10 seconds
        // Amount of time that has passed.
        private double timeoutCount = 0;
        MenuEntry continueCountDown;
        GameTime time;
        Levels level;

        SoundBank frontSounds;
        WaveBank frontWaves;

        public ContinueScreen(Levels curentLevel, GameTime gameTime)
            : base("YOU NAUGHT COOKIN'?")
        {
            time = gameTime;
            level = curentLevel;

            // Create our menu entries.
            continueCountDown = new MenuEntry(string.Empty);
            MenuEntry playAgainScreen = new MenuEntry("YEA DUDE!");
            MenuEntry quitMenuEntry = new MenuEntry("NAW DUDE");

            SetMenuEntryText();

            // Hook up menu event handlers.            
            playAgainScreen.Selected += PlayGame;
            quitMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(continueCountDown);
            MenuEntries.Add(playAgainScreen);
            MenuEntries.Add(quitMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            //Load Front-End Music
            frontSounds = new SoundBank(ScreenManager.engine, "Content\\Music\\XACT\\FrontEnd.xsb");
            frontWaves = new WaveBank(ScreenManager.engine, "Content\\Music\\XACT\\FrontEnd.xwb");

            //Play Song
            frontSounds.PlayCue("continue Song");

        }

        /// <summary>
        /// Fills in the latest values for the Number Of Players.
        /// </summary>
        void SetMenuEntryText()
        {
            continueCountDown.Text = "CONTINUE? " + (int)((TimeOutLimit - timeoutCount)/1000);

        }
        #region Handle Input

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "QUIT THIS AWESOME GAME?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void UpdateCountDown()
        {
            if (timeoutCount > TimeOutLimit)
            {
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());                
            }
        }

        public override void  Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        { 	       
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            timeoutCount += time.ElapsedGameTime.Milliseconds;
            UpdateCountDown();
            SetMenuEntryText();
        }

        void PlayGame(object sender, PlayerIndexEventArgs e)
        {
            //Load Correct Screen

            switch (level)
            {
                case Levels.TEST:
                    LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new PlayPrototypeScreen());
                    break;
                case Levels.EARTH:
                    LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new LevelOne());
                    break;
                case Levels.SPACE:
                    //ScreenManager.AddScreen(new LevelTwo(), e.PlayerIndex);
                    break;
                case Levels.BOSS:
                    //ScreenManager.AddScreen(new Boss(), e.PlayerIndex);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }
        #endregion
    }
}
