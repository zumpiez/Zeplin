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
    public class Transformation
    {
        /// <summary>
        /// Constructs a transformation with zeroed values
        /// </summary>
        public Transformation()
        {
            position = Vector2.Zero;
            scale = Vector2.One;
            rotation = 0;
            pivot = Vector2.Zero;
        }

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
        /// Gets or sets the object's X position value in world coordinates
        /// </summary>
        public float X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }

        /// <summary>
        /// Gets or sets the object's Y position value in world coordinates
        /// </summary>
        public float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
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

        /// <summary>
        /// Gets or sets the object's size in the world
        /// </summary>
        /// <remarks>I don't think this is implemented anywhere, but it is intended to take a 0..1 texture coordinate space and translate it into n..m world coordinate space. Right now there is no 0..1 texture coordinate space, though. It's just measured in pixels.</remarks>
        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }
        
        Vector2 position, scale, pivot, size;
        float rotation;
    }
}
