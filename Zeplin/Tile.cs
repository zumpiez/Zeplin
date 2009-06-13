//Zeplin Engine - Tile.cs
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
    /// Defines a Tile, which can be positioned and drawn in the world and is compatible with collision
    /// </summary>
    public class Tile : IRenderable, ICollidable, ITransformable
    {
        /// <summary>
        /// Constructs a tile with a sprite, transformation and collision volume
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="transformation"></param>
        /// <param name="collider"></param>
        public Tile(Sprite sprite, Transformation transformation, CollisionVolume collider)
        {
            this.msprite = sprite;
            this.transformer = transformation;
            this.collider = collider;
        }

        /// <summary>
        /// Constructs a tile with a sprite and a transformation
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="transformation"></param>
        public Tile(Sprite sprite, Transformation transformation) : this(sprite, transformation, new CollisionVolume())
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
                msprite.Draw(transformer, 1, sourceRect);
            }
            else
            {
                msprite.Draw(transformer, 1, null);
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
        public bool TestCollision(ICollidable otherCollider)
        {
            return collider.TestCollision(otherCollider.CollisionVolume);
        }

        CollisionVolume collider;
        /// <summary>
        /// Gets the CollisionVolume associated with this tile
        /// </summary>
        public CollisionVolume CollisionVolume
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
            collider.TransformCollisionVolume(transformer);
        }

        #endregion

        #region ITransformable Members

        /// <summary>
        /// Gets or sets the scale dimensions of the tile
        /// </summary>
        public Vector2 Scale
        {
            get { return transformer.Scale; }
            set
            {
                transformer.Scale = value;
                collider.TransformCollisionVolume(transformer);
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle of the tile
        /// </summary>
        public float Rotation
        {
            get { return transformer.Rotation; }
            set
            {
                transformer.Rotation = value;
                collider.TransformCollisionVolume(transformer);
            }
        }

        /// <summary>
        /// Gets or sets the tile's pivot's object space point
        /// </summary>
        public Vector2 Pivot
        {
            get { return transformer.Pivot; }
            set
            {
                transformer.Pivot = value;
                collider.TransformCollisionVolume(transformer);
            }
        }

        /// <summary>
        /// Gets or sets the position of the tile's pivot point in world space
        /// </summary>
        public Vector2 Translation
        {
            get { return transformer.Position; }
            set
            {
                transformer.Position = value;
                collider.TransformCollisionVolume(transformer);
            }
        }

        /// <summary>
        /// Gets or sets the size of the tile in world coordinates
        /// </summary>
        /// <remarks>I don't think this is actually used. TODO: investigate!</remarks>
        public Vector2 Size
        {
            get { return transformer.Size; }
            set
            {
                transformer.Size = value;
                collider.TransformCollisionVolume(transformer);
            }
        }
        
        #endregion


        Transformation transformer;
        /// <summary>
        /// Gets or sets the tile's transformation
        /// </summary>
        protected Transformation Transformation
        {
            set
            {
                transformer = value;
                collider.TransformCollisionVolume(transformer);
            }
            get
            {
                return transformer;
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
