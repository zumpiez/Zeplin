//Zeplin Engine - Tile.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Zeplin.CollisionShapes;
using Microsoft.Xna.Framework.Graphics;

namespace Zeplin
{
    /// <summary>
    /// Defines a Tile, which can be positioned and drawn in the world and is compatible with collision
    /// </summary>
    public class Tile : GameObject, ICollisionVolumeProvider
    {
        public Tile() : this(null, new Transformation(), null, null) { }
        public Tile(Image sprite) : this(sprite, new Transformation(), null, null) { }

        /// <summary>
        /// Constructs a tile with a sprite and a transformation
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="transformation"></param>
        public Tile(Image sprite, Transformation transformation) : this(sprite, transformation, null, null) {}
        public Tile(Image sprite, AnimationScript animation) : this(sprite, new Transformation(), animation, null) {}
        public Tile(Image sprite, Transformation transformation, SATCollisionVolume collider) : this(sprite, transformation, null, collider) { }

        public Tile(Image sprite, Transformation transformation, AnimationScript animation, SATCollisionVolume collider)
        {
            this.Sprite = sprite;
            SubRect = new Rectangle(0, 0, sprite.Texture.Width, sprite.Texture.Height);

            this.Transformation = new Transformation(transformation);
            this.AnimationScript = animation;
            this.CollisionVolume = collider;

            OnDraw += this.Draw;

            if (CollisionVolume != null)
            {
                OnUpdate += delegate(GameTime time) { collider.TransformCollisionVolume(this.Transformation); };
            }
        }

        public Tile(Tile oldTile) : this(oldTile.Sprite, oldTile.Transformation, oldTile.AnimationScript, oldTile.collider)
        {
            this.FrameSize = oldTile.FrameSize;
            this.SubRect = oldTile.SubRect;

        }

        /// <summary>
        /// Draws the contents of the tile using the tile's transformation
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            if (AnimationScript != null)
            {
                Rectangle sourceRect;
                //Console.WriteLine("going into ProcessAnimation with {0} {1} {2}", gameTime, FrameSize, Sprite);
                sourceRect = AnimationScript.ProcessAnimation(gameTime, FrameSize, SubRect);
                //Console.WriteLine("drawing tile with subrect {0}", sourceRect);

                Sprite.Draw(Transformation, sourceRect);
            }
            else
            {
                Sprite.Draw(Transformation, SubRect);
            }

            if(CollisionVolume != null)
                (CollisionVolume as SATCollisionVolume).Draw();
        }

        public void Draw(GameTime gameTime, Color color)
        {
            Sprite.Color = color;
            Draw(gameTime);
            Sprite.Color = Color.White;
        }

        /// <summary>
        /// Updates the collision volume associated with this tile based on the current transformation.
        /// </summary>
        internal void RefreshCollisionVolume()
        {
            collider.TransformCollisionVolume(Transformation);
        }

        /// <summary>
        /// Gets or sets the sprite asset associated with this tile
        /// </summary>
        public Image Sprite { get; protected set; }
        
        public Transformation Transformation;

        /// <summary>
        /// Gets or sets the current AnimationScript being played by this tile.
        /// </summary>
        protected AnimationScript AnimationScript { get; set; }

        public Point FrameSize { get; set; }

        public Rectangle SubRect { get; set; }


        #region ICollisionVolumeProvider Members

        /// <summary>
        /// Gets the CollisionVolume associated with this tile
        /// </summary>
        public ICollisionVolume CollisionVolume
        {
            get
            {
                return collider;
            }
            set
            {
                collider = (SATCollisionVolume)value;
            }
        }
        SATCollisionVolume collider;

        #endregion
    }
}
