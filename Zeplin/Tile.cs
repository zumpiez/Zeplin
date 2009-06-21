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
    public class Tile
    {
        GameObject gameobject = new GameObject();

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
            this.collider = collider;
        }

        /// <summary>
        /// Constructs a tile with a sprite and a transformation
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="transformation"></param>
        public Tile(Sprite sprite, Transformation transformation) : this(sprite, transformation, new SATCollisionVolume())
        {
        }

        #region IRenderable Members
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
        #endregion

        #region ICollidable Members
        /// <summary>
        /// Tests for collision between this tile and another ICollidable object
        /// </summary>
        /// <param name="otherCollider"></param>
        /// <returns></returns>
        public ICollisionVolume TestCollision(ICollisionVolume otherCollider)
        {
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

        #endregion

        #region ITransformable Members

        /// <summary>
        /// Gets or sets the scale dimensions of the tile
        /// </summary>
        public Vector2 Scale
        {
            get { return transformation.Scale; }
            set
            {
                transformation.Scale = value;
                collider.TransformCollisionVolume(transformation);
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle of the tile
        /// </summary>
        public float Rotation
        {
            get { return transformation.Rotation; }
            set
            {
                transformation.Rotation = value;
                collider.TransformCollisionVolume(transformation);
            }
        }

        /// <summary>
        /// Gets or sets the tile's pivot's object space point
        /// </summary>
        public Vector2 Pivot
        {
            get { return transformation.Pivot; }
            set
            {
                transformation.Pivot = value;
                collider.TransformCollisionVolume(transformation);
            }
        }

        /// <summary>
        /// Gets or sets the position of the tile's pivot point in world space
        /// </summary>
        public Vector2 Translation
        {
            get { return transformation.Position; }
            set
            {
                transformation.Position = value;
                collider.TransformCollisionVolume(transformation);
            }
        }
        
        #endregion


        Transformation transformation;
        /// <summary>
        /// Gets or sets the tile's transformation
        /// </summary>
        protected Transformation Transformation
        {
            set
            {
                transformation = value;
                collider.TransformCollisionVolume(transformation);
            }
            get
            {
                return transformation;
            }
        }

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
