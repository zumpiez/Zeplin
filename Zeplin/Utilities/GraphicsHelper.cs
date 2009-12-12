using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zeplin;

namespace Zeplin.Utilities
{
    public static class GraphicsHelper
    {
        public static RenderTarget2D CreateRenderTarget(GraphicsDevice device, int width, int height)
        {
            SurfaceFormat outputFormat;
            outputFormat = SurfaceFormat.Color;

            if (device.GraphicsDeviceCapabilities.TextureCapabilities.RequiresPower2)
            {
                double exponent = Math.Ceiling(Math.Log(width) / Math.Log(2));
                width = (int)Math.Pow(2, exponent);

                exponent = Math.Ceiling(Math.Log(height) / Math.Log(2));
                height = (int)Math.Pow(2, exponent);
            }

            return new RenderTarget2D(device, width, height, 0, outputFormat);
        }
    }
}
