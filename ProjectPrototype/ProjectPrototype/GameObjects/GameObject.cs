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
        public Vector2 position;
        public Rectangle boundingRectangle;
        public bool alive;
        public Element element;

        public GameObject(Texture2D loadedTexture)
        {
            rotation = 0.0f;
            sprite = loadedTexture;

            boundingRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);

            velocity = Vector2.Zero;
            position = Vector2.Zero;

            alive = false;
        }
    }
}
