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
            menuItemCollection.Add(item);
        }

        /// <summary>
        /// Adds a MenuItem to the Menu using a specified name.
        /// </summary>
        /// <param name="name">The MenuItem can be referenced by this name.</param>
        /// <param name="item">A MenuItem that will belong to this menu.</param>
        public void Add(String name, MenuItem item)
        {
            item.Name = name;
            menuItemCollection.Add(item);
        }

        /// <summary>
        /// Removes a MenuItem from the Menu by object reference
        /// </summary>
        /// <param name="item">A MenuItem that currently belongs to the Menu.</param>
        public void Remove(MenuItem item)
        {
            menuItemCollection.Remove(item);
        }

        /// <summary>
        /// Removes a MenuItem from the Menu by name. If there are several name matches, it removes the first one found.
        /// </summary>
        /// <param name="name">the name of a MenuItem</param>
        public void Remove(String name)
        {
            MenuItem result; //menuitem with a matching name
            foreach (MenuItem m in menuItemCollection)
            {
                if (m.Name == name)
                {
                    result = m;
                    break;
                }
            }

            if(result != null) menuItemCollection.Remove(result);
        }

        /// <summary>
        /// Draws all MenuItems associated with the Menu.
        /// </summary>
        /// <param name="time"></param>
        internal void Draw(GameTime time)
        {
            foreach (MenuItem m in menuItemCollection.Values)
            {
                m.Draw(time);
            }
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

        List<MenuItem> menuItemCollection;
    }
}