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
    class GameObject
    {
        public Texture2D sprite;
        public float rotation;
        public Vector2 velocity;
        public Rectangle rectangle;
        public bool alive;

        public GameObject(Texture2D loadedTexture)
        {
            rotation = 0.0f;
            rectangle = Rectangle.Empty;
            rectangle.Width = loadedTexture.Width;
            rectangle.Height = loadedTexture.Height;
            sprite = loadedTexture;
            velocity = Vector2.Zero;
            alive = false;
        }
    }
}
