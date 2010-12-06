using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPrototype
{
    class LevelSelectScreen : MenuScreen
    {
        int numberOfPlayers;
        MenuEntry selectedLevelEntry;

        static Levels selectedLevel = Levels.EARTH; 

        public LevelSelectScreen(int numOfPlayers)
            : base("SELECT LEVEL")
        {
            numberOfPlayers = numOfPlayers;

            // Create our menu entries.
            selectedLevelEntry = new MenuEntry(string.Empty);
            MenuEntry continueToNextScreen = new MenuEntry("PLAY LEVEL");
            MenuEntry backMenuEntry = new MenuEntry("BACK");

            SetMenuEntryText();

            // Hook up menu event handlers.
            selectedLevelEntry.Selected += IncrementSelectedLevel;
            continueToNextScreen.Selected += PlayGame;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(selectedLevelEntry);
            MenuEntries.Add(continueToNextScreen);
            MenuEntries.Add(backMenuEntry);

        }

        /// <summary>
        /// Fills in the latest values for the Number Of Players.
        /// </summary>
        void SetMenuEntryText()
        {
            selectedLevelEntry.Text = "LEVEL: " + selectedLevel;

        }
        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void IncrementSelectedLevel(object sender, PlayerIndexEventArgs e)
        {
            selectedLevel++;

            if (selectedLevel > Levels.BOSS)
                selectedLevel = 0;

            SetMenuEntryText();
        }

        void PlayGame(object sender, PlayerIndexEventArgs e)
        {
            //Load Correct Screen

            switch (selectedLevel)
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
        #endregion
    }
}
