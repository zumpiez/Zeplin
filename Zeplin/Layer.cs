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
    internal class Layer
    {
        /// <summary>
        /// Constructs a layer with empty actor and tile collections
        /// </summary>
        internal Layer()
        {
            actorList = new List<Actor>();
            tileList = new List<Tile>();
            objectList = new List<GameObject>();
        }

        /// <summary>
        /// Adds a GameObject to this layer
        /// </summary>
        /// <param name="addedObject">The added GameObject</param>
        public void AddToLayer(GameObject addedObject)
        {
            objectList.Add(addedObject);
        }

        /*/// <summary>
        /// Adds an actor to this layer
        /// </summary>
        /// <param name="addedActor">The added actor</param>
        public void AddToLayer(Actor addedActor)
        {
            actorList.Add(addedActor);
        }

        /// <summary>
        /// Adds a tile to this layer
        /// </summary>
        /// <param name="addedTile">The tile added</param>
        public void AddToLayer(Tile addedTile)
        {
            tileList.Add(addedTile);
        }*/

        /// <summary>
        /// Removes an object from this layer
        /// </summary>
        /// <param name="removedObject">The object to be removed</param>
        /// <returns>True if the object was removed, false if it was not found</returns>
        public bool RemoveFromLayer(GameObject removedObject)
        {
            return objectList.Remove(removedObject);
        }

        /*/// <summary>
        /// Removes an actor from this layer
        /// </summary>
        /// <param name="removedActor">The actor to be removed</param>
        /// <returns>True if the actor was removed, false if it was not found</returns>
        public bool RemoveFromLayer(Actor removedActor)
        {
            return actorList.Remove(removedActor);
        }

        /// <summary>
        /// Removes a tile from this layer
        /// </summary>
        /// <param name="removedTile">The tile to be removed</param>
        /// <returns>True if the tile was removed, false if it was not found</returns>
        public bool RemoveFromLayer(Tile removedTile)
        {
            return tileList.Remove(removedTile);
        }*/

        /// <summary>
        /// Moves an actor to another layer
        /// </summary>
        /// <param name="movedActor">The actor to be moved</param>
        /// <param name="destinationLayer">The layer to move the actor to</param>
        /// <returns>True if the operation was successful, otherwise false</returns>
        public bool MoveToLayer(GameObject movedObject, Layer destinationLayer)
        {
            bool result = objectList.Remove(movedObject);
            if (result) destinationLayer.objectList.Add(movedObject);
            return result;
        }
        
        /*/// <summary>
        /// Moves an actor to another layer
        /// </summary>
        /// <param name="movedActor">The actor to be moved</param>
        /// <param name="destinationLayer">The layer to move the actor to</param>
        /// <returns>True if the operation was successful, otherwise false</returns>
        public bool MoveToLayer(Actor movedActor, Layer destinationLayer)
        {
            bool result = actorList.Remove(movedActor);
            if (result) destinationLayer.actorList.Add(movedActor);
            return result;
        }

        /// <summary>
        /// Moves a tile to another layer
        /// </summary>
        /// <param name="movedTile">The tile to be moved</param>
        /// <param name="destinationLayer">The layer to move the tile to</param>
        /// <returns>True if the operating was successful, otherwise false</returns>
        public bool MoveToLayer(Tile movedTile, Layer destinationLayer)
        {
            bool result = tileList.Remove(movedTile);
            if (result) destinationLayer.tileList.Add(movedTile);
            return result;
        }*/

        /*/// <summary>
        /// Causes the tiles and actors owned by this layer to update themselves
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update</param>
        internal void Update(GameTime gameTime)
        {
            foreach (Tile t in tileList)
            {
                t.RefreshCollisionVolume();
            }
            foreach (Actor a in actorList)
            {
                a.RefreshCollisionVolume();
                a.UpdateBehavior(gameTime);
            }
        }*/

        /// <summary>
        /// Causes the tiles and actors owned by this layer to update themselves
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update</param>
        internal void Update(GameTime gameTime)
        {
            foreach (GameObject o in objectList)
            {
                o.OnUpdate(gameTime);
            }
        }

        /*/// <summary>
        /// Causes the tiles and actors drawn by this layer to draw themselves
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update</param>
        public void Draw(GameTime gameTime)
        {
            Engine.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, Engine.camera.ComputeViewMatrix(parallax));
            foreach (Tile t in tileList)
                t.Draw(gameTime);

            foreach (Actor a in actorList)
                a.Draw(gameTime);

            Engine.spriteBatch.End();
        }*/

        /// <summary>
        /// Causes the game objects on this layer to draw themselves.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update</param>
        public void Draw(GameTime gameTime)
        {
            Engine.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, Engine.camera.ComputeViewMatrix(parallax));
            
            foreach (GameObject o in objectList)
                o.OnDraw(gameTime);

            Engine.spriteBatch.End();
        }

        /*
        List<Actor> actorList;
        /// <summary>
        /// Returns the list of actors owned by this layer
        /// </summary>
        public List<Actor>ActorList
        {
            get { return actorList; }
        }
        
        List<Tile> tileList;
        /// <summary>
        /// Gets the list of tiles owned by this layer
        /// </summary>
        public List<Tile> TileList
        {
            get { return tileList; }
        }*/

        List<GameObject> objectList;
        /// <summary>
        /// Gets the list of GameObjects owned by this layer
        /// </summary>
        public List<GameObject> ObjectList
        {
            get { return objectList; }
        }

        /// <summary>
        /// The parallax factor for the layer.
        /// </summary>
        public Vector2 Parallax = Vector2.One;
        
        /*/// <summary>
        /// Gets or sets the parallax factor of this matrix
        /// </summary>
        public Vector2 Parallax
        {
            get { return parallax; }
            set { parallax = value; }
        }*/
    }
}
