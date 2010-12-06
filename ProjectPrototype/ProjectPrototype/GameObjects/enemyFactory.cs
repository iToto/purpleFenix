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
    static class EnemyFactory
    {
        static public ContentManager content;
        static public SoundBank sfxBank;

        static public Enemy buildEnemy(int enemy, ContentManager content, float moveHeight, float moveWidth)
        {
            switch (enemy)
            {
                case 0:
                    return buildHelix(moveHeight, moveWidth);
                case 1:
                    return buildLokust(content, moveHeight, moveWidth);
                case 2:
                    return buildFiral(content, moveHeight, moveWidth);
                case 3:
                    return buildAeraol(content, moveHeight, moveWidth);
                default:
                    break;
            }
            return null;
        }

        static public Enemy buildEnemy(int enemy)
        {
            switch (enemy)
            {
                case 0:
                    return buildHelix(10, 10);
                case 1:
                    return buildLokust(content, 10, 10);
                case 2:
                    return buildFiral(content, 10, 10);
                case 3:
                    return buildAeraol(content, 10, 10);
                default:
                    break;
            }
            return null;
        }

        static private Enemy buildHelix(float heightVariation, float widthVariation)
        {
            Enemy helix = new Enemy(content.Load<Texture2D>("Sprites\\enemy"), 0, content, Element.Earth, 10, sfxBank); //Change sprite
            helix.MoveHeightVariation = heightVariation;
            helix.MoveWidthVariation = widthVariation;
            helix.alive = true;

            return helix;
        }

        static private Enemy buildLokust(ContentManager content, float heightVariation, float widthVariation)
        {
            Enemy lokust = new Enemy(content.Load<Texture2D>("Sprites\\enemy2"), 1, content, Element.Lightning, 10, sfxBank); //Change sprite
            lokust.MoveHeightVariation = heightVariation;
            lokust.MoveWidthVariation = widthVariation;
            lokust.alive = true;

            return lokust;
        }

        static private Enemy buildFiral(ContentManager content, float heightVariation, float widthVariation)
        {
            Enemy firal = new Enemy(content.Load<Texture2D>("Sprites\\enemy3"), 2, content, Element.Fire, 10, sfxBank); //Change sprite
            firal.MoveHeightVariation = heightVariation;
            firal.MoveWidthVariation = widthVariation;
            firal.alive = true;

            return firal;
        }

        static private Enemy buildAeraol(ContentManager content, float heightVariation, float widthVariation)
        {
            Enemy aeraol = new Enemy(content.Load<Texture2D>("Sprites\\enemy4"), 2, content, Element.Ice, 10, sfxBank); //Change sprite
            aeraol.MoveHeightVariation = heightVariation;
            aeraol.MoveWidthVariation = widthVariation;
            aeraol.alive = true;

            return aeraol;
        }
    }
}
