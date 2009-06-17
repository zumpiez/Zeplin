//Zeplin Engine - Transformation.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    /// <summary>
    /// Defines spatial properties for art assets
    /// </summary>
    public struct Transformation
    {
        /// <summary>
        /// Constructs a transformation with the most common elements (position, scale, rotation)
        /// </summary>
        /// <param name="position">The object's position in world space</param>
        /// <param name="scale">The object's scale factors</param>
        /// <param name="rotation">Degrees rotated, in radians</param>
        public Transformation(Vector2 position, Vector2 scale, float rotation)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.pivot = Vector2.Zero;
        }

        /// <summary>
        /// Constructs a transformation with position, scale, rotation and custom pivot
        /// </summary>
        /// <remarks>There are some problems that occur with non-centered pivots. Looking into this.</remarks>
        /// <param name="position">The object's position in world space</param>
        /// <param name="scale">The object's scale factors</param>
        /// <param name="rotation">Degrees rotated, in radians</param>
        /// <param name="pivot">The pivot point, in object space</param>
        public Transformation(Vector2 position, Vector2 scale, float rotation, Vector2 pivot)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.pivot = pivot;
        }

        /// <summary>
        /// Gets or sets the object's position in world coordinates
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Gets or sets the object's scale factors
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        /// <summary>
        /// Gets or sets the object's rotaton in radians
        /// </summary>
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        /// <summary>
        /// Gets or sets the object's pivot point in object space.
        /// </summary>
        public Vector2 Pivot
        {
            get
            {
                return pivot;
            }
            set
            {
                pivot = value;
            }
        }
        
        Vector2 position, scale, pivot;
        float rotation;
    }
}
