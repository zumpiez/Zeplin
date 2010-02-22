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
            //don't set XNASafe at all here since we can't be sure where the tex came from.
        }

        public void Load(Image source)
        {
            throw new NotImplementedException();
            //todo do we want to COPY the texture and rendertarget, or reference them?
            //what is the best way to copy a texture and render target?
            //(if we copy, xnasafe = false. if we reference, xnasafe = source.xnasafe)
        }

        public void Load(string resource)
        {
            this.texture = ZeplinGame.ContentManager.Load<Texture2D>(resource);
            Width = texture.Width;
            Height = texture.Height;
            renderTarget = GraphicsHelper.CreateRenderTarget(Width, Height);
            operatingMode = OperatingMode.Safe; //XNA will re-load this for us.
        }

        /// <summary>
        /// Draws an image into another image.
        /// </summary>
        /// <param name="otherImage"></param>
        public void Draw(Image destinationImage)
        {
            this.Draw(destinationImage, Transformation.Identity);
        }

        /// <summary>
        /// Draws an image into another image.
        /// </summary>
        /// <param name="otherImage"></param>
        /// <param name="transformation"></param>
        public void Draw(Image destinationImage, Transformation transformation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws an image to the screen
        /// </summary>
        /// <param name="transformation"></param>
        public void Draw(Transformation transformation)
        {
            ZeplinGame.drawQueue.AddCommand(new DrawCommand(this.Texture, transformation));
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

        void RestoreFromCache(Object sender, EventArgs e)
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

        void FlushCache()
        {
            if (cache != null)
            {
                cache.Dispose();
                cache = null;
            }
        }

        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get
            {
                if (operatingMode == OperatingMode.Volatile)
                    texture = renderTarget.GetTexture();

                return texture;
            }
        }
        
        Texture2D texture;
        RenderTarget2D renderTarget;
        Texture2D cache;

        OperatingMode operatingMode; 

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

    enum OperatingMode
    {
        Safe, Volatile
    }
}
