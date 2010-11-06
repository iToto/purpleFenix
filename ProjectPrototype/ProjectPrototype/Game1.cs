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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        
        SpriteBatch spriteBatch;
        Rectangle viewportRect;

        Player playerOne;
        List<Enemy> enemies = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        

#if !XBOX
        KeyboardState previousKeyboardState;
#endif
        GamePadState previousGamepadState;

        Random random = new Random();

        const int maxAsteroids = 5;
        const int maxBullets = 60;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            screenManager.AddScreen(new PlayPrototypeScreen(), null);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Red);

            base.Draw(gameTime);
        }
    }
}
