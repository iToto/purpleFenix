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
    class ShootingPattern
    {
        public const int MAX_BULLETS = 200;

        static public void shootSperatic(List<Bullet> bullets, ContentManager content)
        {
            for (int i = 0; i < MAX_BULLETS; i++)
            {
                bullets.Add(new Bullet(content.Load<Texture2D>("Sprites\\Bullet")));
                bullets[i].alive = false;
                bullets[i].velocity.Y = -4.0f;
                bullets[i].type = bulletType.speratic;

                if (i % 4 == 0)
                {   
                    bullets[i].velocity.X = -3.0f;
                }
                else if (i % 3 == 0)
                {   
                    bullets[i].velocity.X = -1.0f;
                }
                else if (i % 2 == 0)
                {
                    bullets[i].velocity.X = 0.0f;
                }
                else if (i % 1  == 0)
                {
                    bullets[i].velocity.X = 1.0f;
                }
                else
                {
                    bullets[i].velocity.X = 3.0f;
                }
            }
        }

        static public void shootSpread(List<Bullet> bullets, ContentManager content)
        {
            //Create Five bullets that move at different projections

            for (int i = 0; i < MAX_BULLETS; i++)
            {
                bullets[i].type = bulletType.spread;
            }
        }

        static public void shootStraight(List<Bullet> bullets, ContentManager content)
        {
            for (int i = 0; i < MAX_BULLETS; i++)
            {
                bullets[i].type = bulletType.straight;
                
            }
        }

        static public void shootHelix()
        {

        }

        static public void shootDouble()
        {

        }
    }
}
