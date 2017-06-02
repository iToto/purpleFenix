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

        // Direction should be 1 (down) or -1 (up).
        public static void Fire(List<Bullet> bullets, GameObject bulletOwner, int direction, int velocityModifier)
        {
            // shoot 5 bullets at once
            if (bullets[0].type == bulletType.spread)
            {
                int bulletsFired = 0;

                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.alive)
                    {
                        bullet.alive = true;
                        bullet.element = bulletOwner.element;

                        PositionBullet(bullet, bulletOwner.boundingRectangle, direction, velocityModifier);

                        switch (bulletsFired)
                        {
                            case 0:
                                bullet.velocity.X = -3;
                                break;
                            case 1:
                                bullet.velocity.X = -1;
                                break;
                            case 2:
                                bullet.velocity.X = 0;
                                break;
                            case 3:
                                bullet.velocity.X = 1;
                                break;
                            case 4:
                                bullet.velocity.X = 3;
                                break;
                            default:
                                break;
                        }

                        ++bulletsFired;
                    }
                    if (bulletsFired >= 5)
                    {
                        break;
                    }
                }
            }
            else if (bullets[0].type == bulletType.straight)
            {
                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.alive)
                    {
                        bullet.alive = true;
                        bullet.element = bulletOwner.element;

                        PositionBullet(bullet, bulletOwner.boundingRectangle, direction, velocityModifier);

                        break;
                    }
                }
            }
            else if (bullets[0].type == bulletType.helix)
            {
            }
            else if (bullets[0].type == bulletType.doubleShot)
            {
            }
            else if (bullets[0].type == bulletType.speratic)
            {
                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.alive)
                    {
                        bullet.alive = true;
                        bullet.element = bulletOwner.element;

                        PositionBullet(bullet, bulletOwner.boundingRectangle, direction, velocityModifier);

                        Random random = new Random();

                        switch (random.Next(5))
                        {
                            case 0:
                                bullet.velocity.X = -3;
                                break;
                            case 1:
                                bullet.velocity.X = -1;
                                break;
                            case 2:
                                bullet.velocity.X = 0;
                                break;
                            case 3:
                                bullet.velocity.X = 1;
                                break;
                            case 4:
                                bullet.velocity.X = 3;
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                }
            }
        }

        private static Bullet PositionBullet(Bullet bullet, Rectangle boundingRectangle, int direction, int velocityModifier)
        {
            if (direction < 0)
            {
                bullet.position.X = boundingRectangle.Center.X - bullet.boundingRectangle.Width / 2;
                bullet.position.Y = boundingRectangle.Top - bullet.boundingRectangle.Height / 2;
                bullet.velocity.Y = -4.0f;
            }
            else
            {
                bullet.position.X = boundingRectangle.Center.X - bullet.boundingRectangle.Width / 2;
                bullet.position.Y = boundingRectangle.Bottom - bullet.boundingRectangle.Height / 2;
                bullet.velocity.Y = 4.0f;
            }

            bullet.velocity.Y += velocityModifier;

            return bullet;
        }
    }
}
