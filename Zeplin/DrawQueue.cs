using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    internal class DrawQueue
    {
        public DrawQueue()
        {
            drawQueue = new Queue<Queue<DrawCommand>>();
            currentDrawLayer = new Queue<DrawCommand>();
            parallaxQueue = new Queue<Vector2>();
        }
        
        public void AddCommand(DrawCommand cmd)
        {
            currentDrawLayer.Enqueue(cmd);
        }

        public void NextLayer()
        {
            NextLayer(Vector2.One);
        }

        public void NextLayer(Vector2 parallaxFactor)
        {
            drawQueue.Enqueue(currentDrawLayer);
            parallaxQueue.Enqueue(parallaxFactor);
            currentDrawLayer = new Queue<DrawCommand>();
        }

        public void Draw()
        {
            //finalize the current queue and put it in the structure.
            drawQueue.Enqueue(currentDrawLayer);
           
            //draw all queues
            while (drawQueue.Count > 0)
            {
                currentDrawLayer = drawQueue.Dequeue();
                if (parallaxQueue.Count == 0) parallaxQueue.Enqueue(Vector2.One);
                var parallaxFactors = parallaxQueue.Dequeue();
                RenderTarget2D lastDestination = null;
                if(currentDrawLayer.Count > 0) lastDestination = currentDrawLayer.Peek().destination; //initialize to the first RenderTarget in this layer's queue
                
                var parallaxMatrix = ComputeParallax(lastDestination, parallaxFactors);

                //get the initial draw state and initialize the sprite batch for the current layer's settings.
                ZeplinGame.GraphicsDeviceManager.GraphicsDevice.SetRenderTarget(0, lastDestination); //this is the first RenderTarget from the first queue.
                ZeplinGame.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, parallaxMatrix);

                //draw all commands in the queue for this layer
                while (currentDrawLayer.Count > 0)
                {
                    DrawCommand current = currentDrawLayer.Dequeue();
                    if (current.destination != lastDestination) //we must swap render targets
                    {
                        ZeplinGame.spriteBatch.End(); //stop this batch before changing device settings
                        ZeplinGame.GraphicsDeviceManager.GraphicsDevice.SetRenderTarget(0, current.destination);
                        
                        //recompute parallax
                        parallaxMatrix = ComputeParallax(lastDestination, parallaxFactors);
                        
                        ZeplinGame.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState, parallaxMatrix);
                    }
                    ZeplinGame.spriteBatch.Draw(current.source, current.transformation.Position, null, Color.White, current.transformation.Rotation, current.transformation.Pivot, current.transformation.Scale, SpriteEffects.None, current.transformation.Depth);
                    lastDestination = current.destination;
                }
                //this layer is done, loop back to dequeue next layer, compute parallax, etc
            }

            ZeplinGame.spriteBatch.End();
        }

        /// <summary>
        /// Returns the parallax matrix if rendering to the screen (drawing to layer), or a 1,1 parallax factor matrix if drawing to another image.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="parallax"></param>
        /// <returns></returns>
        Matrix ComputeParallax(RenderTarget2D destination, Vector2 parallax)
        {
            if (destination == null)
                return Engine.Camera.ComputeViewMatrix(parallax);
            else
                return Engine.Camera.ComputeViewMatrix(Vector2.One);
        }

        Queue<DrawCommand> currentDrawLayer;
        Queue<Queue<DrawCommand>> drawQueue;
        Queue<Vector2> parallaxQueue;
    }

    struct DrawCommand
    {
        public DrawCommand(Texture2D source, Transformation transformation, RenderTarget2D destination)
        {
            this.source = source;
            this.transformation = transformation;
            this.destination = destination;
        }

        public DrawCommand(Texture2D source, Transformation transformation)
        {
            this.source = source;
            this.transformation = transformation;
            this.destination = null;
        }

        internal Texture2D source;
        internal RenderTarget2D destination;
        internal Transformation transformation;
    }
}
