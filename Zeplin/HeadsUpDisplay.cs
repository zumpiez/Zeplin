using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    public class HeadsUpDisplay : Layer
    {
        public HeadsUpDisplay() : base() 
        {
            Parallax = Vector2.Zero;
            Pinned = true;
            widgets = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// Adds a GameObject to this layer
        /// </summary>
        /// <param name="addedObject">The added IGameObjectProvider</param>
        public override void Add(GameObject addedObject)
        {
            widgets.Add(GenerateDefaultName(addedObject), addedObject);
        }

        /// <summary>
        /// Removes an object from this layer
        /// </summary>
        /// <param name="removedObject">The IGameObjectProvider to be removed</param>
        /// <returns>True if the object was removed, false if it was not found</returns>
        public override bool Remove(GameObject removedObject)
        {
            bool result = false;

            foreach (KeyValuePair<string, GameObject> kvp in widgets)
            {
                if (kvp.Value == removedObject)
                {
                    widgets.Remove(kvp.Key);
                    result = true;
                }
            }

            return result;
        }

        public GameObject this[string widgetName]
        {
            get
            {
                return widgets[widgetName];
            }
            set
            {
                widgets.Add(widgetName, value);
            }
        }

        private string GenerateDefaultName(GameObject gameObject)
        {
            return String.Format("{0}_{1}", gameObject.GetType().ToString(), gameObject.GetHashCode());
        }

        internal override IEnumerable<GameObject> GameObjects { get { return widgets.Values; } }
        private Dictionary<string, GameObject> widgets;
    }
}
