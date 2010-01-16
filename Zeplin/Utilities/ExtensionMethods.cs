//Zeplin Engine - ExtensionMethods.cs
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
    /// Contains extension methods for C# types.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Interpolates between two values at a given rate.
        /// </summary>
        /// <param name="value">The value to be modified</param>
        /// <param name="target">Target value that will eventually be reached</param>
        /// <param name="rate">The amount to increment/decrement by.</param>
        /// <returns></returns>
        public static float ApproachTargetValue(this float value, float target, float rate)
        {
            if (value < target)
                return (Math.Min(value + rate, target));
            else if (value > target)
                return (Math.Max(value - rate, target));
            else return target;
        }

        /// <summary>Adds addition to XNA points.</summary>
        /// <remarks>Why the HELL can't you add XNA points</remarks>
        /// <param name="value"></param>
        /// <param name="addend"></param>
        /// <returns></returns>
        public static Point Add(this Point value, Point addend)
        {
            return new Point(value.X + addend.X, value.Y + addend.Y);
        }
        public static Point Subtract(this Point value, Point addend)
        {
            return new Point(value.X - addend.X, value.Y - addend.Y);
        }
    }
}
