using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPrototype
{
    class ScoreManager
    {
        List<int> playerScores = new List<int>();
        int numberOfPlayers;
        
        public ScoreManager(int numOfPlayers)
        {
            numberOfPlayers = numOfPlayers;
            for(int i = 0; i < numberOfPlayers; ++i)
            {
                playerScores[i] = 0;
            }
        }

        public void updateScore(int player,int increment)
        {
            playerScores[player - 1] += increment;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            string scoreString = string.Empty;

            for(int i = 0; i < numberOfPlayers; ++i)
            {
                scoreString += "P" + (i + 1) + " SCORE: " + playerScores[i] + " ";
            }

            spriteBatch.DrawString(font, scoreString, Vector2.Zero, Color.White);
        }
    }
}
