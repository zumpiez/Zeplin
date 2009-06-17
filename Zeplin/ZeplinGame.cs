﻿//Zeplin Engine - ZeplinGame.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Zeplin
{
    /// <summary>
    /// Encapsulates the XNA framework's Game object
    /// </summary>
    public class ZeplinGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static ResourceContentManager ResourceContent;

        /// <summary>
        /// A ZeplinGame encapsulates the XNA Framework.
        /// </summary>
        /// <remarks>When a ZeplinGame is created, it will initialize the engine. Therefore, only one ZeplinGame should be created.</remarks>
        public ZeplinGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ResourceContent = new ResourceContentManager(this.Services, EngineResources.ResourceManager);
            //this.IsFixedTimeStep = false;
        }
        
        Map testMap;
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //testMap is obviously going to be replaced by a map loading function.
            testMap = new Map();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Engine.Initialize(Content, spriteBatch, graphics, testMap);
            if (OnLoad != null) OnLoad();
        }

        public delegate void LoadHook();
        public LoadHook OnLoad = null;

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.UpdateInput();
            testMap.Update(gameTime);
            if (OnUpdate != null) OnUpdate(gameTime);
            base.Update(gameTime);
        }

        public delegate void UpdateHook(GameTime gameTime);
        public UpdateHook OnUpdate = null;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            testMap.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}