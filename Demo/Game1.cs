using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Zeplin;

namespace Demo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Demo
    {
        ZeplinGame game;

        public Demo()
        {
            game = new ZeplinGame();
            game.OnUpdate += DemoUpdateEveryFrame; //optionally inject custom code into the update call
            game.OnLoad += LoadContent; //optionally inject custom code into the content load
            game.Run();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        void LoadContent()
        {   
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            //Zeplin.Engine.Initialize(Content, spriteBatch, graphics, testMap);
            

            Random r = new Random();
            
            World.worldDimensions = new Vector2(10000, 10000);

            Engine.Camera.SetDimensions(1280, 720);
            Engine.Camera.Mode = CameraCropMode.MaintainHeight;
            
            #region random grassymass tiles
            /*for (int i = 0; i < 10; i++)
            {
                Tiles.GrassyMass gm = new Tiles.GrassyMass(new Vector2(100 + i * 75, 300f));
                gm.SetRotation(((float)r.NextDouble() / 4) - ((float)r.NextDouble() / 4));

                float Transformation.ScaleFactor = (float)(1.0 + (r.NextDouble() * 0.3 - r.NextDouble() * 0.3));
                gm.SetTransformation.Scale(gm.GetTransformation.Scale().X * Transformation.ScaleFactor);

                testMap.AddTile(gm);
            }*/
            #endregion

            Tiles.GrassBrick gb = new Tiles.GrassBrick(new Vector2(100, 200));
            gb.Transformation.Scale = new Vector2(0.75f);
            Engine.AddToMap(gb, 1);

            Tiles.GrassBrick behindgb2 = new Tiles.GrassBrick(new Vector2(550, 250));
            behindgb2.Transformation.Scale = new Vector2(0.75f);
            Engine.AddToMap(behindgb2, 1);

            Tiles.GrassBrick gb2 = new Tiles.GrassBrick(new Vector2(550, 450));
            gb2.Transformation.Scale = new Vector2(1.25f);
            Engine.AddToMap(gb2, 2);

            Actors.AnimationTestGuy animationGuy = new Actors.AnimationTestGuy();
            animationGuy.Transformation.Position = new Vector2(550, 670);
            animationGuy.Transformation.Scale = new Vector2(3);
            Engine.AddToMap(animationGuy, 2);

            Tiles.GrassBrick distantGrassBrick = new Tiles.GrassBrick(new Vector2(350, 250));
            distantGrassBrick.Sprite.Color = new Color(Color.White, 150);
            distantGrassBrick.Transformation.Scale = new Vector2(0.25f);
            Engine.AddToMap(distantGrassBrick, 0);

            Actors.StickNinja snactor = new Actors.StickNinja(new Vector2(100f, 600f));
            Engine.AddToMap(snactor, 1);

            Engine.SetLayerParallax(0, new Vector2(0.25f));
            Engine.SetLayerParallax(2, new Vector2(1.75f));

            Actors.Logo logo = new Actors.Logo(new Vector2(45,45));
            logo.Transformation.Scale = new Vector2(0.35f);
            logo.rotationSpeed = 0.05f;
            logo.offset = -0.5f;
            Engine.AddToMap(logo);
            
            #region spinnydemo

            for (int i = 0; i < 10; i++)
            {
                Actors.Logo l = new Actors.Logo(new Vector2(r.Next(-800, 800), r.Next(-600, 600)));
                float scale = (float)r.NextDouble();
                l.Transformation.Scale = new Vector2(scale * 2.5f, scale * 2.5f);
                l.rotationSpeed = (float)(r.NextDouble() * 2) + 0.5f;
                if (r.Next(0, 2) == 1) l.rotationSpeed *= -1;
                l.offset = (float)(r.NextDouble() * Math.PI * 2);
                Engine.AddToMap(l, 0);
            }

            Engine.PinLayer(0, true);

            #endregion
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DemoUpdateEveryFrame(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                game.Exit();

            //Engine.camera.Center = new Vector2(45, 45);            
        }
    }
}
