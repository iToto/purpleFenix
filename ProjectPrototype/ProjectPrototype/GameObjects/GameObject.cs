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

        public static ExplosionManager ExplosionManager;

        protected int health;
        public int Health
        {
            set { this.health = value; }
            get { return this.health; }
        }

        protected int spriteWidth;
        protected int spriteHeight;

        Dictionary<string, Animation> Animations;
        public Animation CurrentAnimation { protected set; get; }
        protected Rectangle frameRectangle;

        public GameObject(Texture2D loadedTexture)
        {
            sprite = loadedTexture;
            this.spriteWidth = sprite.Width;
            this.spriteHeight = sprite.Height;

            reset();

            element = Element.None;
        }

        public GameObject(Texture2D loadedTexture, Vector2 size)
        {
            sprite = loadedTexture;
            this.spriteWidth = (int)size.X;
            this.spriteHeight = (int)size.Y;

            reset();

            element = Element.None;
        }

        public GameObject(Texture2D loadedTexture, Element element)
        {
            sprite = loadedTexture;
            this.spriteWidth = sprite.Width;
            this.spriteHeight = sprite.Height;

            reset();

            this.element = element;
        }

        public GameObject(Texture2D loadedTexture, Element element, int health)
        {
            sprite = loadedTexture;
            this.spriteWidth = sprite.Width;
            this.spriteHeight = sprite.Height;

            reset();

            this.element = element;
            this.health = health;
        }

        public GameObject(Texture2D loadedTexture, Element element, int health, Vector2 size)
        {
            sprite = loadedTexture;
            this.spriteWidth = (int)size.X;
            this.spriteHeight = (int)size.Y;

            reset();

            this.element = element;
            this.health = health;
        }

        private void reset()
        {
            rotation = 0.0f;
            boundingRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);

            velocity = Vector2.Zero;
            position = Vector2.Zero;

            alive = false;

            health = 1;
        }

        public Defense CompareElements(GameObject opposingObject)
        {
            Element element1 = this.element;
            Element element2 = opposingObject.element;

            Defense returnValue = Defense.Standard;

            // If they are the same element, they are neutral against each other.
            if (element1 == element2)
            {
                return Defense.Standard;
            }

            switch (element1)
            {
                case Element.None:
                    break;
                case Element.Fire:
                    if (element2 == Element.Ice)
                    {
                        returnValue = Defense.Weak;
                    }
                    break;
                case Element.Ice:
                    if (element2 == Element.Fire)
                    {
                        returnValue = Defense.Weak;
                    }
                    break;
                case Element.Lightning:
                    if (element2 == Element.Earth)
                    {
                        returnValue = Defense.Weak;
                    }
                    break;
                case Element.Earth:
                    if (element2 == Element.Lightning)
                    {
                        returnValue = Defense.Weak;
                    }
                    break;
                default:
                    break;
            }
            return returnValue;
        }

        public void AddAnimation(string name, int[] frames, int frameRate, bool looped)
        {
            if (Animations == null)
            {
                Animations = new Dictionary<string, Animation>();
            }

            Animations.Add(name, new Animation(name, frames, frameRate, looped, spriteWidth, spriteHeight));
        }

        public void play(string animationName)
        {
            CurrentAnimation = Animations[animationName];
            CurrentAnimation.resetFrameIndex();
        }

        protected Rectangle updateAnimation(GameTime gametime)
        {
            return CurrentAnimation.Update(gametime);
        }

        public virtual void Kill()
        {
            this.alive = false;
        }

        public virtual void Damage(int damageTaken)
        {
            this.Health -= damageTaken;
        }
    }
}
