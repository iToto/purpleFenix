using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectPrototype
{
    class BulletManager
    {
        public static Texture2D FireSprite { private set; get; }
        public static Texture2D IceSprite { private set; get; }
        public static Texture2D LightningSprite { private set; get; }
        public static Texture2D RockSprite { private set; get; }

        public BulletManager(ContentManager content)
        {
            this.LoadContent(content);
        }

        public void LoadContent(ContentManager content)
        {
            FireSprite = content.Load<Texture2D>("Sprites\\fire-sheet");
            IceSprite = content.Load<Texture2D>("Sprites\\ice-sheet");
            LightningSprite = content.Load<Texture2D>("Sprites\\lightning-sheet");
            RockSprite = content.Load<Texture2D>("Sprites\\rock-sheet");
        }
    }
}
