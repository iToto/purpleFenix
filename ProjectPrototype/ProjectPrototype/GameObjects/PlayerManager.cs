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
    class PlayerManager
    {
        public int NumberOfPlayers { private set; get; }

        Player playerOne;
        Player playerTwo;
        Player playerThree;
        Player playerFour;

        List<Player> players;

        public bool AllPlayersAreDead { private set; get; }

        public PlayerManager(int numberOfPlayers, ContentManager content, Vector2 initialPosition, SoundBank sfxSounds)
        {
            this.NumberOfPlayers = numberOfPlayers;

            players = new List<Player>();

            //Initializes Players:
            if (numberOfPlayers > 0)
            {
                //Initialize First Player
                playerOne = new Player(content.Load<Texture2D>("Sprites\\fireShip"), 
                    content, Element.Fire, PlayerIndex.One, sfxSounds);

                playerOne.position = initialPosition;
                playerOne.boundingRectangle.X = (int)playerOne.position.X;
                playerOne.boundingRectangle.Y = (int)playerOne.position.Y;

                ShootingPattern.shootStraight(playerOne.bullets);
                players.Add(playerOne);
            }

            if (numberOfPlayers > 1)
            {
                //Initialize Second Player
                playerTwo = new Player(content.Load<Texture2D>("Sprites\\waterShip"), 
                    content, Element.Ice, PlayerIndex.Two, sfxSounds);

                playerTwo.position = new Vector2(initialPosition.X + 65, initialPosition.Y);
                playerTwo.boundingRectangle.X = (int)playerTwo.position.X;
                playerTwo.boundingRectangle.Y = (int)playerTwo.position.Y;

                ShootingPattern.shootStraight(playerTwo.bullets);
                players.Add(playerTwo);
            }

            if (numberOfPlayers > 2)
            {
                //Initialize Third Player
                playerThree = new Player(content.Load<Texture2D>("Sprites\\earthShip"), 
                    content, Element.Earth, PlayerIndex.Three, sfxSounds);

                playerThree.position = new Vector2(initialPosition.X + 130, initialPosition.Y);
                playerThree.boundingRectangle.X = (int)playerThree.position.X;
                playerThree.boundingRectangle.Y = (int)playerThree.position.Y;

                ShootingPattern.shootStraight(playerThree.bullets);
                players.Add(playerThree);
            }

            if (numberOfPlayers > 3)
            {
                //Initialize Fourth Player
                playerFour = new Player(content.Load<Texture2D>("Sprites\\elecShip"), 
                    content, Element.Lightning, PlayerIndex.Four, sfxSounds);

                playerFour.position = new Vector2(initialPosition.X + 195, initialPosition.Y);
                playerFour.boundingRectangle.X = (int)playerFour.position.X;
                playerFour.boundingRectangle.Y = (int)playerFour.position.Y;

                ShootingPattern.shootStraight(playerFour.bullets);
                players.Add(playerFour);
            }

            this.AllPlayersAreDead = false;
        }

        public void Update(GameTime gametime, Rectangle viewportRect, List<Enemy> enemies)
        {
            foreach (Player player in players)
            {
                player.Update(ref viewportRect, gametime, enemies);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach (Player player in players)
            {
                player.Draw(spriteBatch, font);
            }
        }

        public void HandleInput(InputState input, ScreenManager screenManager)
        {
            if (playerOne != null)
            {
                playerOne.HandleInput(input, PlayerIndex.One, screenManager);
            }
            
            if (playerTwo != null)
            {
                playerTwo.HandleInput(input, PlayerIndex.Two, screenManager);
            }

            if (playerThree != null)
            {
                playerThree.HandleInput(input, PlayerIndex.Three, screenManager);
            }

            if (playerFour != null)
            {
                playerFour.HandleInput(input, PlayerIndex.Four, screenManager);
            }
        }

        public List<Player> GetLivingPlayers()
        {
            List<Player> livingPlayers = new List<Player>();

            foreach (Player player in players)
            {
                if (player.alive)
                {
                    livingPlayers.Add(player);
                }
            }

            if (livingPlayers.Count < 1)
            {
                this.AllPlayersAreDead = true;
            }

            return livingPlayers;
        }
    }
}