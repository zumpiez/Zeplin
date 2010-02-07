using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zeplin
{
    public class TextWidget : GameObject
    {
        public TextWidget() : this("") {}

        public TextWidget(string text)
        {
            Text = text;

            OnDraw += this.Draw;
        }

        private void Draw(GameTime now)
        {
            if (FontFace != null) 
            {
                float scale;
                SpriteFont bestFont = FontFace.GetBestFont(FontSize, out scale);

                // calculate position.
                Vector2 size = bestFont.MeasureString(Text);

                Vector2 topLeft = PinSizeAndAlignmentToTopLeft(Position, size, HorizontalAlignment, VerticalAlignment);
                ZeplinGame.spriteBatch.DrawString(bestFont, Text, topLeft, Foreground, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }

        private Vector2 PinSizeAndAlignmentToTopLeft(Vector2 pin, Vector2 size, Alignment horizAlignment, Alignment vertAlignment)
        {
            Vector2 topLeft = new Vector2();

            switch (horizAlignment) 
            {
                case Alignment.Near: topLeft.X = pin.X; break;
                case Alignment.Center: topLeft.X = pin.X - (size.X / 2); break;
                case Alignment.Far: topLeft.X = pin.X - size.X; break;
            }

            switch (vertAlignment)
            {
                case Alignment.Near: topLeft.Y = pin.Y; break;
                case Alignment.Center: topLeft.Y = pin.Y - (size.Y / 2); break;
                case Alignment.Far: topLeft.Y = pin.Y - size.Y; break;
            }

            return topLeft;
        }

        public string Text { get; set; }

        public Vector2 Position { get; set; }

        public Alignment HorizontalAlignment { get; set; }
        public Alignment VerticalAlignment { get; set; }

        public Color Foreground { get; set; }

        public MetaFont FontFace { get; set; }
        public float FontSize { get; set; }
    }
}
