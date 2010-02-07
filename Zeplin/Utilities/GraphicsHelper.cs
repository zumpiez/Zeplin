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
                width = NextBiggestPow2(width);
                height = NextBiggestPow2(height);
            }

            if (device.GraphicsDeviceCapabilities.TextureCapabilities.RequiresSquareOnly)
            {
                if (width > height) height = width;
                if (width < height) width = height;
            }

            //todo maybe don't use PreserveContents here. Might have xbox performance problems.
            return new RenderTarget2D(device, width, height, 0, outputFormat, RenderTargetUsage.PreserveContents);
        }

        public static RenderTarget2D CreateRenderTarget(int width, int height)
        {
            return CreateRenderTarget(ZeplinGame.GraphicsDeviceManager.GraphicsDevice, width, height);
        }

        public static int NextBiggestPow2(int value)
        {
            value--;
            value = (value >> 1) | value;
            value = (value >> 2) | value;
            value = (value >> 4) | value;
            value = (value >> 8) | value;
            value = (value >> 16) | value;
            value++;

            return value;
        }
    }
}
