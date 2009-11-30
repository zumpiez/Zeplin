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
        internal SortedList<int, Layer> layers;
        int appendZ;

        /// <summary>
        /// Creates a new map that is empty and contains no layers.
        /// </summary>
        public Map()
        {
            layers = new SortedList<int, Layer>();
            appendZ = 0;
        }

        /// <summary>
        /// Adds a new layer in the foreground.
        /// </summary>
        /// <returns>The index of the created layer</returns>
        public Layer NewLayer()
        {
            Layer newLayer = new Layer();
            PutLayer(newLayer, appendZ);

            return newLayer;
        }

        /// <summary>
        /// Adds a new layer at the specified index
        /// </summary>
        /// <param name="z">The z-position where the layer is to be added.</param>
        public Layer NewLayer(int z)
        {
            Layer newLayer = new Layer();
            PutLayer(newLayer, z);

            return newLayer;
        }

        public void PutLayer(Layer layer, int z)
        {
            layers.Add(z, layer);

            if (z >= appendZ)
            {
                appendZ = z + 1;
            }
        }

        /// <summary>
        /// Removes a layer from the map
        /// </summary>
        /// <param name="index">The index of the layer to be removed.</param>
        /// <returns>True: The layer was successfully removed.</returns>
        public bool RemoveLayer(int index)
        {
            return layers.Remove(index);
        }

        /// <summary>
        /// Updates every layer owned by this map
        /// </summary>
        /// <param name="gameTime">The amount of time </param>
        public void Update(GameTime gameTime)
        {
            foreach (Layer l in layers.Values)
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
            foreach (Layer l in layers.Values)
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
