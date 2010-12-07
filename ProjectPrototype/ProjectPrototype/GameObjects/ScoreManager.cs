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
        public int score;
        public PlayerIndex playerNumber;
        public Vector2 position;

        public ScoreManager(Vector2 pos, PlayerIndex player)
        {
            score = 0;
            this.position = pos;
            this.playerNumber = player;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            string scoreString = string.Empty;

            scoreString = "" + this.score;

            spriteBatch.DrawString(font, scoreString,this.position, Color.White);
        }
    }
}
