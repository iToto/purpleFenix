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
using System.Diagnostics;
using System;

namespace ProjectPrototype
{
    class MovementPath
    {
        static public Vector2 sineWave(Enemy theEnemy, float amplitude, float smt)
        {
            Vector2 position = theEnemy.position;
            position.X = position.X + ((float)Math.Sin(position.Y / smt) * amplitude);
            position.Y += theEnemy.velocity.Y;

            return position;
        }

        /// <summary>
        /// Makes a Parabola Path
        /// </summary>
        /// <param name="theEnemy">Enemy to apply Path to</param>
        /// <param name="rightBound">Where on the screen he exits from</param>
        /// <param name="depth">How far down he goes (between 0.001,0.004)</param>
        /// <returns></returns>
        static public Vector2 parabola(Enemy theEnemy, float depth, float rightBound)
        {
            Vector2 position = theEnemy.position;
            position.Y = (float)(-depth*theEnemy.position.X)*(theEnemy.position.X-rightBound);
            position.X += theEnemy.velocity.X;

            return position;

        }

        
    }
}
