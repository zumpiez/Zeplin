//Zeplin Engine - World.cs
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
    /// Defines the world space
    /// </summary>
    /// <remarks>This turned out to not really be used for anything. I might roll this into Utilities or Engine.</remarks>
    public static class World
    {
        public static Vector2 lowerLeftCorner;
        public static Vector2 worldDimensions;
        public static Vector2 gameResolution;

        /// <summary>
        /// Converts a point in local object coordinate space into world coordinates
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="objectPoint">A point in object coordinate space. 0,0 = lower left corner of object</param>
        /// <param name="worldPosition">The object's pivot position in world space</param>
        /// <returns></returns>
        public static Vector2 ObjectToWorld(Transformation transformation, Vector2 objectPoint, bool horizontalFlip)
        {
            if (horizontalFlip == true)
                transformation.Scale *= new Vector2(-1, 1);
            Matrix rotation = Matrix.CreateRotationZ(-transformation.Rotation);
            Matrix scale = Matrix.CreateScale(new Vector3(transformation.Scale, 1));
            if (horizontalFlip == true)
                transformation.Scale *= new Vector2(-1, 1);

            Matrix compositeTransformationMatrix = rotation * scale;

            Vector2 offset = objectPoint - transformation.Pivot;
            offset = Vector2.Transform(offset, compositeTransformationMatrix);

            Vector2 worldPoint = transformation.Position + offset;

            return worldPoint;
        }
    }
}
