using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPrototype
{
    class PlayerSelectScreen : MenuScreen
    {
        int numberOfPlayers;
        MenuEntry numberOfPlayersEntry;

        public PlayerSelectScreen()
            : base("PLAYER SELECT")
        {
            numberOfPlayers = 1;

            // Create our menu entries.
            numberOfPlayersEntry = new MenuEntry(string.Empty);
            MenuEntry backMenuEntry = new MenuEntry("BACK");

            SetMenuEntryText();

            // Hook up menu event handlers.
            numberOfPlayersEntry.Selected += IncrementNumberOfPlayers;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(numberOfPlayersEntry);
            MenuEntries.Add(backMenuEntry);
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            numberOfPlayersEntry.Text = "NUMBER OF PLAYERS: " + this.numberOfPlayers;
            
        }
        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void IncrementNumberOfPlayers(object sender, PlayerIndexEventArgs e)
        {
            this.numberOfPlayers = (this.numberOfPlayers % 4);
            ++this.numberOfPlayers;
            SetMenuEntryText();
        }


        #endregion
    }
}
