using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    class Menu : GameObject
    {
        public Menu()
        {
            OnUpdate += Update;
            OnDraw += Draw;
        }

        /// <summary>
        /// Adds a MenuItem to the Menu using a default name.
        /// </summary>
        /// <param name="item">A MenuItem that will belong to this menu.</param>
        public void Add(MenuItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a MenuItem to the Menu using a specified name.
        /// </summary>
        /// <param name="item">A MenuItem that will belong to this menu.</param>
        /// <param name="name">The MenuItem can be referenced by this name.</param>
        public void Add(MenuItem item, String name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a MenuItem from the Menu by object reference
        /// </summary>
        /// <param name="item">A MenuItem that currently belongs to the Menu.</param>
        public void Remove(MenuItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a MenuItem from the Menu by name
        /// </summary>
        /// <param name="name">the name of a MenuItem</param>
        public void Remove(String name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws all MenuItems associated with the Menu.
        /// </summary>
        /// <param name="time"></param>
        internal void Draw(GameTime time)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets keyboard input and modifies MenuItem states.
        /// </summary>
        /// <param name="time"></param>
        internal void Update(GameTime time)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MenuItem> MenuItems { get; set; }

        KeyValuePair<String, MenuItem> menuItemCollection;
    }
}