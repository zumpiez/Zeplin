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
            : this(position, scale, rotation, Vector2.Zero)
        {
        }

        public static bool operator==(Transformation t1, Transformation t2)
        {
            return (
                t1.Position == t2.Position &&
                t1.Rotation == t2.Rotation &&
                t1.Scale == t2.Scale);
        }

        public static bool operator !=(Transformation t1, Transformation t2)
        {
            return !(t1 == t2);
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
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Pivot = pivot;
        }

        /// <summary>
        /// The object's position in world coordinates
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The object's X and Y scale factors
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// The object's rotaton in radians
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The object's pivot point in object space.
        /// </summary>
        public Vector2 Pivot;
    }
}
