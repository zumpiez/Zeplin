//Zeplin Engine - Layer.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zeplin
{
    /// <summary>
    /// Defines a logical collection of actors and tiles
    /// </summary>
    /// <remarks>This should be changed to have a collection of IRenderable and IThinkables</remarks>
    public class Layer
    {
        /// <summary>
        /// Constructs a layer with empty actor and tile collections
        /// </summary>
        public Layer()
        {
            gameObjectList = new List<GameObject>();
            Parallax = Vector2.One;
        }

        /// <summary>
        /// Adds a GameObject to this layer
        /// </summary>
        /// <param name="addedObject">The added GameObject</param>
        public virtual void Add(GameObject addedObject)
        {
            gameObjectList.Add(addedObject);
        }

        /// <summary>
        /// Removes an object from this layer
        /// </summary>
        /// <param name="removedObject">The IGameObjectProvider to be removed</param>
        /// <returns>True if the object was removed, false if it was not found</returns>
        public virtual bool Remove(GameObject removedObject)
        {
            return gameObjectList.Remove(removedObject);
        }

        /// <summary>
        /// Moves an actor to another layer
        /// </summary>
        /// <param name="movedActor">The actor to be moved</param>
        /// <param name="destinationLayer">The layer to move the actor to</param>
        /// <returns>True if the operation was successful, otherwise false</returns>
        public bool MoveTo(GameObject movedObject, Layer destinationLayer)
        {
            bool result = Remove(movedObject);
            if (result) destinationLayer.Add(movedObject);
            return result;
        }

        /// <summary>
        /// Causes the tiles and actors owned by this layer to update themselves
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update</param>
        internal void Update(GameTime gameTime)
        {
            foreach (GameObject o in GameObjects)
            {
                if(o.OnUpdate != null)
                    o.OnUpdate(gameTime);
            }
        }

        /// <summary>
        /// Causes the game objects on this layer to draw themselves.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update</param>
        internal void Draw(GameTime gameTime)
        {
            Matrix viewMatrix = Matrix.Identity;
 
            if (!Pinned)
            {
                viewMatrix = Engine.Camera.ComputeViewMatrix(Parallax);
            }

            Engine.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, viewMatrix);

            foreach (GameObject o in GameObjects)
            {
                if (o.OnDraw != null)
                    o.OnDraw(gameTime);

            }

            Engine.spriteBatch.End();
        }

        /// <summary>
        /// Gets the list of GameObjects owned by this layer
        /// </summary>
        internal virtual IEnumerable<GameObject> GameObjects { get { return gameObjectList; } }
        private List<GameObject> gameObjectList;

        /// <summary>
        /// The parallax factor for the layer.
        /// </summary>
        public Vector2 Parallax { get; set; }

        public bool Pinned { get; set; }
    }
}
