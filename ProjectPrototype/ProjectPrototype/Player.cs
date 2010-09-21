using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class Player : GameObject
    {
        public float speed = 4.0f;

        public Player(Texture2D loadedTexture)
            : base(loadedTexture)
        {

        }

        public void Update(ref Rectangle viewportRect)
        {
            this.rectangle.X += (int)this.velocity.X;
            this.rectangle.Y += (int)this.velocity.Y;

            if (this.rectangle.X < 0)
            {
                this.rectangle.X = 0;
            }

            if (this.rectangle.X > viewportRect.Width)
            {
                this.rectangle.X = viewportRect.Width;
            }

            if (this.rectangle.Y < 0)
            {
                this.rectangle.Y = 0;
            }

            if (this.rectangle.Y > viewportRect.Height)
            {
                this.rectangle.Y = viewportRect.Height;
            }
        }

        public void HandleInput(ref GamePadState gamepadState, ref GamePadState previousGamepadState)
        {

        }

        public void HandleInput(ref KeyboardState keyboardState, ref KeyboardState previousKeyboardState)
        {
            this.velocity.X = 0;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                this.velocity.X = -this.speed;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.velocity.X = this.speed;
            }

            this.velocity.Y = 0;
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.velocity.Y = -this.speed;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.velocity.Y = this.speed;
            }
        }

    }
}
