//Zeplin Engine - Tile.cs
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
    /// Defines a Tile, which can be positioned and drawn in the world and is compatible with collision
    /// </summary>
    public class Tile : IGameObjectProvider
    {
        GameObject gameobject = new GameObject();
        public GameObject GameObject
        {
            get { return gameobject; }
        }

        /// <summary>
        /// Constructs a tile with a sprite, transformation and collision volume
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="transformation"></param>
        /// <param name="collider"></param>
        public Tile(Sprite sprite, Transformation transformation, SATCollisionVolume collider)
        {
            this.msprite = sprite;
            this.transformation = transformation;
            gameobject.CollisionVolume = collider;

            gameobject.OnDraw += this.Draw;
        }

        /// <summary>
        /// Constructs a tile with a sprite and a transformation
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="transformation"></param>
        public Tile(Sprite sprite, Transformation transformation) : this(sprite, transformation, new SATCollisionVolume())
        {
        }

        /// <summary>
        /// Draws the contents of the tile using the tile's transformation
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            Rectangle sourceRect;
            if (currentAnimation != null)
            {
                sourceRect = currentAnimation.ProcessAnimation(gameTime, msprite);
                msprite.Draw(transformation, 1, sourceRect);
            }
            else
            {
                msprite.Draw(transformation, 1, null);
            }

            if(collider != null)
                collider.Draw();

        }

        Sprite msprite;
        /// <summary>
        /// Gets or sets the sprite asset associated with this tile
        /// </summary>
        public Sprite Sprite
        {
            get
            {
                return msprite;
            }
            protected set
            {
                msprite = value;
            }
        }

        Transformation lastTransformation = new Transformation();
        /// <summary>
        /// Tests for collision between this tile and another ICollidable object
        /// </summary>
        /// <param name="otherCollider"></param>
        /// <returns></returns>
        public ICollisionVolume TestCollision(ICollisionVolume otherCollider)
        {
            //Check to see if this object's transformation has changed and refresh the CV if it has
            if (lastTransformation != transformation)
            {
                //Caches the current transformation
                lastTransformation = transformation;
                RefreshCollisionVolume();
            }
            
            return collider.TestCollision(otherCollider);
        }

        SATCollisionVolume collider;
        /// <summary>
        /// Gets the CollisionVolume associated with this tile
        /// </summary>
        public SATCollisionVolume CollisionVolume
        {
            get
            {
                return collider;
            }
            protected set
            {
                collider = value;
            }
        }

        /// <summary>
        /// Updates the collision volume associated with this tile based on the current transformation.
        /// </summary>
        internal void RefreshCollisionVolume()
        {
            collider.TransformCollisionVolume(transformation);
        }

        public Transformation transformation;

        AnimationScript currentAnimation = null;
        /// <summary>
        /// Gets or sets the current AnimationScript being played by this tile.
        /// </summary>
        protected AnimationScript AnimationScript
        {
            get
            {
                return currentAnimation;
            }
            set
            {
                this.currentAnimation = value;
            }
        }
    }
}
