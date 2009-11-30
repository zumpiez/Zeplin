//Zeplin Engine - Engine.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zeplin
{
    /// <summary>
    /// Provides utility functions that can be called by game objects.
    /// </summary>
    /// <remarks>Some of this stuff will probably get rolled in to utilities or something</remarks>
    public static class Engine
    {
        /// <summary>
        /// Internal helper function that initializes the Zeplin engine with references to necessary XNA components.
        /// </summary>
        internal static void Initialize(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Map map)
        {
            Engine.Content = contentManager;
            Engine.spriteBatch = spriteBatch;
            World.gameResolution = new Vector2(gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
            
            //camera defaults to the size of the world. User can set this to the desired dimensions.
            Engine.Camera = new Camera(World.worldDimensions);

            CurrentMap = map;
        }

        /// <summary>
        /// Draws a line in world coordinate space.
        /// </summary>
        /// <param name="p1">The tail of the line (in world coords)</param>
        /// <param name="p2">The tip of the line (in world coords)</param>
        /// <param name="width">The thickness of the line (in pixels)</param>
        /// <param name="color">The color of the line</param>
        /// <remarks>Precondition: call this from inside a sprite batch draw sequence, i.e. inside actor.draw or tile.draw</remarks>
        internal static void DrawLine(Vector2 p1, Vector2 p2, float width, Color color)
        {
            Vector2 scaleFactor = new Vector2();
            float rotation;

            Vector2 tweenVector = p2 -p1;

            Texture2D square = ZeplinGame.ResourceContent.Load<Texture2D>("whitesquare");

            scaleFactor.X = (float)tweenVector.Length() / square.Height;
            scaleFactor.Y = width / square.Width;
            rotation = (float)Math.Atan2((double)tweenVector.Y, (double)tweenVector.X);

            //Some of these values were inverted to deal with the fact that world coordinate's y-growth is the opposite of the screen's. Without it, the lines were drawing upside-down.
            spriteBatch.Draw(square, new Vector2(p1.X, -p1.Y), null, color, -rotation, Vector2.Zero, scaleFactor, SpriteEffects.None, 0);
        }


        /// <summary>
        /// Tests the specified game object instance against all objects of the specified type on the same map layer.
        /// </summary>
        /// <typeparam name="ColliderType">Any game object that implements ICollidable.</typeparam>
        /// <param name="testedObject">The game object that is being tested</param>
        /// <returns>A reference to the first positive collided object, or null</returns>
        public static IGameObjectProvider TestCollision<TGameObjectProviderType>(IGameObjectProvider tester) where TGameObjectProviderType : IGameObjectProvider
        {
            ICollisionVolume testerCV = tester.GameObject.CollisionVolume;
            if (testerCV == null)
                return null;

            foreach (IGameObjectProvider o in CurrentMap.ActiveLayer.GameObjectProviders)
            {
                if (o is TGameObjectProviderType && testerCV.TestCollision(o.GameObject.CollisionVolume) == true)
                {
                    return o;
                }
            }

            return null;
        }

        /// <summary>

        /// There will be a mechanism for switching between maps.
        /// </summary>
        public static Map CurrentMap { get; private set; }

        internal static ContentManager Content;
        internal static SpriteBatch spriteBatch;


        public static Camera Camera { get; private set; }
    }
}
