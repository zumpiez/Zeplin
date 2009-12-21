using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    class TextHudWidget
    {
        private GameObject gameObject = new GameObject();
        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public TextHudWidget(string text)
        {
            Text = text;

            GameObject.OnDraw += this.Draw;
        }

        private void Draw(GameTime now)
        {

        }

        public string Text { get; set; }
    }
}
