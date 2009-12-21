using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    public enum ItemState
    {
        active, inactive
    }
    
    public class MenuItem
    {
        public MenuItem()
        {
        }

        /// <summary>
        /// Creates a MenuItem with a Sprite and Transformation
        /// </summary>
        /// <param name="spriteResource">A path to an art resource</param>
        /// <param name="transformation"></param>
        public MenuItem(String spriteResource, Transformation transformation)
        {
            this.Sprite = new Sprite(spriteResource);
            this.Transformation = transformation;
        }

        /// <summary>
        /// Creates a MenuItem with a Sprite and Transformation
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="transformation"></param>
        public MenuItem(Sprite sprite, Transformation transformation)
        {
            this.Sprite = sprite;
            this.Transformation = transformation;
        }

        /// <summary>
        /// Draws the MenuItem.
        /// </summary>
        /// <param name="time"></param>
        internal void Draw(GameTime time)
        {
            Sprite.Draw(Transformation);
        }

        /// <summary>
        /// Gets or sets the state of the MenuItem. This may affect its draw behavior.
        /// </summary>
        internal ItemState State { get; set; }

        //public member until struct properties are figured out
        public Transformation Transformation;
        public Sprite Sprite { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        String name = null;
        public String Name
        {
            get
            {
                if (name == null) return DefaultName;
                else return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// The default name of the MenuItem, based on the type and hash value
        /// </summary>
        internal String DefaultName
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}