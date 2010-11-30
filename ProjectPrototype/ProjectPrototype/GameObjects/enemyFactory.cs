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
    static class enemyFactory
    {
        static public Enemy buildEnemy(int enemy,ContentManager content,float moveHeigh, float moveWidth)
        {
            switch (enemy)
            {
                case 0:
                    return buildHelix(content,moveHeigh,moveWidth);
                case 1:
                    return buildLokust(content, moveHeigh, moveWidth);
                case 2:
                    return buildFiral(content, moveHeigh, moveWidth);
                case 3:
                    return buildAeraol(content, moveHeigh, moveWidth);
                default:
                    break;
            }
            return null;
        }

        static private Enemy buildHelix(ContentManager content,float moveHeight, float moveWidth)
        {
            Enemy helix = new Enemy(content.Load<Texture2D>("Sprites\\enemy"),0,content); //Change sprite
            helix.MoveHeightVariation = moveHeight;
            helix.MoveWidthVariation = moveWidth;
            helix.alive = false;
            helix.health = 100;

            return helix;
        }

        static private Enemy buildLokust(ContentManager content, float moveHeight, float moveWidth)
        {
            Enemy lokust = new Enemy(content.Load<Texture2D>("Sprites\\enemy2"), 1, content); //Change sprite
            lokust.MoveHeightVariation = moveHeight;
            lokust.MoveWidthVariation = moveWidth;
            lokust.alive = false;
            lokust.health = 100;

            return lokust;
        }

        static private Enemy buildFiral(ContentManager content, float moveHeight, float moveWidth)
        {
            Enemy firal = new Enemy(content.Load<Texture2D>("Sprites\\enemy3"), 1, content); //Change sprite
            firal.MoveHeightVariation = moveHeight;
            firal.MoveWidthVariation = moveWidth;
            firal.alive = false;
            firal.health = 100;

            return firal;
        }

        static private Enemy buildAeraol(ContentManager content, float moveHeight, float moveWidth)
        {
            Enemy aeraol = new Enemy(content.Load<Texture2D>("Sprites\\enemy4"), 0, content); //Change sprite
            aeraol.MoveHeightVariation = moveHeight;
            aeraol.MoveWidthVariation = moveWidth;
            aeraol.alive = false;
            aeraol.health = 100;

            return aeraol;
        }
    }
}
