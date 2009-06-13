//Zeplin Engine - RectangleF.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin.Utilities
{
    /// <summary>
    /// Defines a rectangle using floating point values.
    /// Eventually this should match the implementation of XNA.Rectangle.
    /// </summary>
    public struct RectangleF
    {
        Vector2 topLeftCorner, bottomRightCorner;

        /// <summary>
        /// Creates a RectangleF
        /// </summary>
        /// <param name="topLeftCorner">The top left corner of the rectangle</param>
        /// <param name="dimensions">The width and height of the rectangle.</param>
        public RectangleF(Vector2 topLeftCorner, Vector2 dimensions)
        {
            this.topLeftCorner = topLeftCorner;
            bottomRightCorner = topLeftCorner + dimensions;
        }

        /// <summary>
        /// Returns the Y-value of the top of the rectangle.
        /// </summary>
        public float Top
        {
            get { return topLeftCorner.Y; }
        }

        /// <summary>
        /// Returns the X-value of the right side of the rectangle.
        /// </summary>
        public float Right
        {
            get { return bottomRightCorner.X; }
        }

        /// <summary>
        /// Returns the Y-value of the bottom of the rectangle.
        /// </summary>
        public float Bottom
        {
            get { return bottomRightCorner.Y; }
        }

        /// <summary>
        /// Returns the X-value of the left side of the rectangle.
        /// </summary>
        public float Left
        {
            get { return topLeftCorner.X; }
        }

        /// <summary>
        /// Gets the width of the RectangleF
        /// </summary>
        public float Width
        {
            get { return bottomRightCorner.X - topLeftCorner.X; }
            //set { bottomRightCorner.X = topLeftCorner.X + value; }
        }

        /// <summary>
        /// Gets the height of the RectangleF
        /// </summary>
        public float Height
        {
            get { return bottomRightCorner.Y - topLeftCorner.Y; }
            //set { bottomRightCorner.Y = topLeftCorner.Y + value; }
        }

        /// <summary>
        /// Determines if two rectangles are intersecting.
        /// </summary>
        /// <param name="otherRectangle"></param>
        /// <returns></returns>
        public bool Intersects(RectangleF otherRectangle)
        {
            if ((Left > otherRectangle.Right) || (Right < otherRectangle.Left) || (Top > otherRectangle.Bottom) || (Bottom < otherRectangle.Top))
                return false;

            else return true;
        }

        /// <summary>
        /// Returns the overlapping area of two rectangles, if any.
        /// </summary>
        /// <param name="otherRectangle">The rectangle to compute against.</param>
        /// <returns>The overlapping area.</returns>
        public RectangleF Intersect(RectangleF otherRectangle)
        {
            if (Intersects(otherRectangle))
            {
                Vector2 resultTopLeftCorner = new Vector2(Math.Max(Left, otherRectangle.Left), Math.Max(Top, otherRectangle.Top));
                Vector2 resultBottomRightCorner = new Vector2(Math.Min(Right, otherRectangle.Right), Math.Min(Bottom, otherRectangle.Bottom));

                Vector2 resultDimensions = resultBottomRightCorner - resultTopLeftCorner;

                return new RectangleF(resultTopLeftCorner, resultDimensions);
            }

            return new RectangleF();
        }
    }
}