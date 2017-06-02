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
        int score;

        public PlayerIndex playerNumber;
        public Vector2 position;

        int pointsUntilHeal;

        const int HEAL_SCORE = 120;

        Player owner;

        public ScoreManager(Vector2 pos, PlayerIndex player, Player owner)
        {
            score = 0;
            this.position = pos;
            this.playerNumber = player;
            this.owner = owner;
            this.pointsUntilHeal = HEAL_SCORE;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            string scoreString = string.Empty;

            scoreString = "" + this.score;

            spriteBatch.DrawString(font, scoreString,this.position, Color.White);
        }

        public void AddPoints(int points)
        {
            this.score += points;
            this.pointsUntilHeal -= points;
            if (pointsUntilHeal <= 0)
            {
                pointsUntilHeal = HEAL_SCORE;
                this.owner.Health = 100;
            }
        }
    }
}
