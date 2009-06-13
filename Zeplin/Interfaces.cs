//Zeplin Engine - Interfaces.cs
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
    /// Defines support for being drawn to a Zeplin map layer.
    /// </summary>
    public interface IRenderable
    {
        void Draw(GameTime gameTime);
        Sprite Sprite { get; }
    }

    /// <summary>
    /// Defines support for collision detection with other ICollidable instances.
    /// </summary>
    public interface ICollidable
    {
        bool TestCollision(ICollidable otherCollider);
        CollisionVolume CollisionVolume { get; }
    }

    /// <summary>
    /// Defines support for inclusion in a layer's update phase
    /// </summary>
    public interface IThinkable
    {
         void UpdateBehavior(GameTime time);
    }

    /// <summary>
    /// Defines support for Zeplin's transformation class
    /// </summary>
    public interface ITransformable
    {
        Vector2 Scale { get; set; }
        float Rotation { get; set; }
        Vector2 Pivot { get; set; }
        Vector2 Translation { get; set; }
        Vector2 Size { get; set; }
    }
}
