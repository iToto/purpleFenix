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
            this.alive = true;
        }

        public void Update(ref Rectangle viewportRect)
        {
            this.position.X += this.velocity.X;
            this.position.Y += this.velocity.Y;

            this.boundingRectangle.X = (int)this.position.X;
            this.boundingRectangle.Y = (int)this.position.Y;

            if (this.position.X < 0)
            {
                this.position.X = 0;
            }

            if (this.position.X > viewportRect.Width)
            {
                this.position.X = viewportRect.Width;
            }

            if (this.position.Y < 0)
            {
                this.position.Y = 0;
            }

            if (this.position.Y > viewportRect.Height)
            {
                this.position.Y = viewportRect.Height;
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
