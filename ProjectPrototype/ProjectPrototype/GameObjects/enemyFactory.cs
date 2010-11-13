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

namespace ProjectPrototype.GameObjects
{
    static class enemyFactory
    {
        static public Enemy buildEnemy(int enemy,ContentManager content)
        {
            switch (enemy)
            {
                case 0:
                    return buildHelix(content);
                case 1:
                    return buildLokust(content);
                case 2:
                    return buildFiral(content);
                case 3:
                    return buildAeraol(content);
                default:
                    break;
            }
            return null;
        }

        static private Enemy buildHelix(ContentManager content)
        {
            Enemy helix = new Enemy(content.Load<Texture2D>("Sprites\\DevilHead")); //Change sprite
            helix.alive = false;
            helix.health = 100;

            return helix;
        }

        static private Enemy buildLokust(ContentManager content)
        {
            Enemy lokust = new Enemy(content.Load<Texture2D>("Sprites\\DevilHead")); //Change sprite
            lokust.alive = false;
            lokust.health = 100;

            return lokust;
        }

        static private Enemy buildFiral(ContentManager content)
        {
            Enemy firal = new Enemy(content.Load<Texture2D>("Sprites\\DevilHead")); //Change sprite
            firal.alive = false;
            firal.health = 100;

            return firal;
        }

        static private Enemy buildAeraol(ContentManager content)
        {
            Enemy aeraol = new Enemy(content.Load<Texture2D>("Sprites\\DevilHead")); //Change sprite
            aeraol.alive = false;
            aeraol.health = 100;

            return aeraol;
        }
    }
}
