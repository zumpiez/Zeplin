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
        /// Draws the MenuItem.
        /// </summary>
        /// <param name="time"></param>
        internal void Draw(GameTime time)
        {
            throw new NotImplementedException();
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
