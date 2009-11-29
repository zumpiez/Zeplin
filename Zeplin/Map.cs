//Zeplin Engine - Map.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    /// <summary>
    /// Defines a Zeplin map as a collection of layers
    /// </summary>
    public class Map
    {
        internal List<Layer> layerList;

        /// <summary>
        /// Creates a new map that is empty and contains no layers.
        /// </summary>
        public Map()
        {
            layerList = new List<Layer>();
            layerList.Add(new Layer());
        }

        /// <summary>
        /// Creates a map with a specific number of layers.
        /// </summary>
        /// <param name="numberOfLayers">The quantity of layers that the map should initialize.</param>
        public Map(int numberOfLayers)
        {
            layerList = new List<Layer>();
            for (int i = 0; i < numberOfLayers; i++)
                layerList.Add(new Layer());
        }

        /// <summary>
        /// Adds a new layer in the foreground.
        /// </summary>
        /// <returns>The index of the created layer</returns>
        public int AddLayer()
        {
            layerList.Add(new Layer());
            return layerList.Count - 1;
        }

        /// <summary>
        /// Adds a new layer at the specified index
        /// </summary>
        /// <param name="index">The index position where the layer is to be added.</param>
        public void AddLayer(int index)
        {
            layerList.Insert(index, new Layer());
        }

        /// <summary>
        /// Removes a layer from the map
        /// </summary>
        /// <param name="index">The index of the layer to be removed.</param>
        /// <returns>True: The layer was successfully removed.</returns>
        public bool RemoveLayer(int index)
        {
            try
            {
                layerList.RemoveAt(index);
                return true;
            }
            catch(ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the parallax of a specified layer
        /// </summary>
        /// <param name="layer">A layer number, as determined </param>
        /// <param name="parallax"></param>
        public void SetLayerParallax(int layer, Vector2 parallax)
        {
            layerList[layer].Parallax = parallax;
        }

        public void PinLayer(int layer, bool pin)
        {
            layerList[layer].Pinned = pin;
        }

        /// <summary>
        /// Adds a GameObject to the specified layer.
        /// </summary>
        /// <param name="addedObject">The object to add</param>
        /// <param name="layerIndex">The index of the layer the object will be added to</param>
        /// <returns>True if successful, false if not.</returns>
        public void AddGameObject(IGameObjectProvider addedObject, int layerIndex)
        {
            try
            {
                layerList[layerIndex].AddToLayer(addedObject);
            }
            catch (ArgumentOutOfRangeException)
            {
                for (int i = layerList.Count - 1; i < layerIndex; i++)
                    layerList.Add(new Layer());

                layerList[layerIndex].AddToLayer(addedObject);
            }
        }

        /// <summary>
        /// Adds an actor to the default (back-most) layer.
        /// </summary>
        /// <param name="addedActor">The actor to be added to the layer.</param>
        public void AddGameObject(IGameObjectProvider addedObject)
        {
            layerList[0].AddToLayer(addedObject);
        }

        /// <summary>
        /// Updates every layer owned by this map
        /// </summary>
        /// <param name="gameTime">The amount of time </param>
        public void Update(GameTime gameTime)
        {
            foreach (Layer l in layerList)
            {
                activeLayer = l;
                l.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws every layer owned by this map
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            foreach (Layer l in layerList)
                l.Draw(gameTime);
        }

        /// <summary>
        /// Gets a reference to the layer currently being updated.
        /// </summary>
        internal Layer ActiveLayer
        {
            get { return activeLayer; }
        }
        Layer activeLayer;
    }
}
