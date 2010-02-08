using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Zeplin.Utilities;

namespace Zeplin
{
    public class Image : IDisposable
    {
        public Image()
        {
            ContentLostEventHandler = new EventHandler(RestoreFromCache);
        }

        public void Load(Texture2D source)
        {
            this.texture = source;
            Width = source.Width;
            Height = source.Height;
            renderTarget = GraphicsHelper.CreateRenderTarget(Width, Height);
        }

        public void Load(Image source)
        {
            throw new NotImplementedException();
            //todo do we want to COPY the texture and rendertarget, or reference them?
            //what is the best way to copy a texture and render target?
        }

        public void Load(string resource)
        {
            this.texture = ZeplinGame.ResourceContent.Load<Texture2D>(resource);
            Width = texture.Width;
            Height = texture.Height;
            renderTarget = GraphicsHelper.CreateRenderTarget(Width, Height);
        }

        /// <summary>
        /// Draws an image into another image.
        /// </summary>
        /// <param name="otherImage"></param>
        public void Draw(Image otherImage)
        {
            this.Draw(otherImage, Transformation.Identity);
        }

        /// <summary>
        /// Draws an image into another image.
        /// </summary>
        /// <param name="otherImage"></param>
        /// <param name="transformation"></param>
        public void Draw(Image otherImage, Transformation transformation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws an image to the screen
        /// </summary>
        /// <param name="transformation"></param>
        public void Draw(Transformation transformation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws an image to the screen
        /// </summary>
        public void Draw()
        {
            this.Draw(Transformation.Identity);
        }

        public void Cache()
        {
#if !XBOX360
            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(textureData);
            cache.SetData<Color>(textureData);
            renderTarget.ContentLost += ContentLostEventHandler;
#endif
        }

        public void RestoreFromCache(Object sender, EventArgs e)
        {
#if !XBOX360
            if (cache != null)
            {
                Color[] textureData = new Color[Width * Height];
                cache.SetData<Color>(textureData);
                texture.GetData<Color>(textureData);
            }
            if (cache == null)
                renderTarget.ContentLost -= ContentLostEventHandler;
#endif
        }

        internal void FlushCache()
        {
            if (cache != null)
            {
                cache.Dispose();
                cache = null;
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }



        internal Texture2D texture;
        internal RenderTarget2D renderTarget;
        internal Texture2D cache;

        bool XNASafe; //todo do something with this to make sure that we don't lost textures that XNA should
                      //be able to fix for us.

        EventHandler ContentLostEventHandler;

        #region IDisposable Members

        public void Dispose()
        {
            texture.Dispose();
            renderTarget.Dispose();
            cache.Dispose();
        }

        #endregion
    }
}
