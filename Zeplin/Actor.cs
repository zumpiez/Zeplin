﻿//Zeplin Engine - Actor.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Zeplin.CollisionShapes;

namespace Zeplin
{
    /// <summary>
    /// Defines an interactive component in the game world
    /// </summary>
    public class Actor : Sprite
    {
        /// <summary>
        /// An Actor can be drawn on screen, translated around arbitrarily, collide with other game objects, and process update logic every frame.
        /// </summary>
        /// <param name="sprite">The artwork the actor will use during Draw.</param>
        /// <param name="transformation">Positioning information.</param>
        /// <param name="collider">A collision shape that mirrors the transformation.</param>
        public Actor(Image sprite, Transformation transformation, SATCollisionVolume collider) : base(sprite, transformation, collider)
        {
        }

        /// <summary>
        /// An Actor can be drawn on screen, translated around arbitrarily, collide with other game objects, and process update logic every frame.
        /// </summary>
        /// <param name="sprite">The artwork the actor will use during Draw.</param>
        /// <param name="transformation">Positioning information.</param>
        public Actor(Image sprite, Transformation transformation) : this(sprite, transformation, new SATCollisionVolume()) { }

        /// <summary>
        /// This is called for you once per map draw by Zeplin. 
        /// </summary>
        /// <param name="gameTime"></param>
        public new void Draw(GameTime gameTime)
        {
            //If there is no animation script defined on the Sprite, just draw the Sprite.
            Rectangle sourceRect;
            if (AnimationScript != null)
            {
                sourceRect = AnimationScript.ProcessAnimation(gameTime, FrameSize);
                Image.Draw(Transformation, sourceRect);
            }
            else
            {
                Image.Draw(Transformation, null);
            }
            ((SATCollisionVolume)CollisionVolume).Draw();
        }
    }
}
