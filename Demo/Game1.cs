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
            Random r = new Random();
            
            World.worldDimensions = new Vector2(10000, 10000);

            Engine.Camera.Dimensions = new Vector2(1280, 720);
            Engine.Camera.Mode = CameraCropMode.MaintainHeight;

            Layer skybox = Engine.CurrentMap.NewLayer();
            skybox.Parallax = new Vector2(0.25f);

            Layer world = Engine.CurrentMap.NewLayer();
            Layer near = Engine.CurrentMap.NewLayer();
            near.Parallax = new Vector2(1.75f);

            HeadsUpDisplay hud = new HeadsUpDisplay();
            Engine.CurrentMap.PutLayer(hud, 1000);
            hud.Pinned = true;

            Tiles.GrassBrick gb = new Tiles.GrassBrick(new Vector2(100, 200));
            gb.Transformation.Scale = new Vector2(0.75f);
            world.Add(gb);

            Tiles.GrassBrick behindgb2 = new Tiles.GrassBrick(new Vector2(550, 250));
            behindgb2.Transformation.Scale = new Vector2(0.75f);
            world.Add(behindgb2);

            Tiles.GrassBrick gb2 = new Tiles.GrassBrick(new Vector2(550, 450));
            gb2.Transformation.Scale = new Vector2(1.25f);
            near.Add(gb2);

            Actors.AnimationTestGuy animationGuy = new Actors.AnimationTestGuy();
            animationGuy.Transformation.Position = new Vector2(550, 670);
            animationGuy.Transformation.Scale = new Vector2(3);
            near.Add(animationGuy);

            Tiles.GrassBrick distantGrassBrick = new Tiles.GrassBrick(new Vector2(350, 250));
            distantGrassBrick.Sprite.Color = new Color(Color.White, 150);
            distantGrassBrick.Transformation.Scale = new Vector2(0.25f);
            skybox.Add(distantGrassBrick);

            Actors.StickNinja snactor = new Actors.StickNinja(new Vector2(100f, 600f));
            world.Add(snactor);

            Actors.Logo logo = new Actors.Logo(new Vector2(55,-55));
            logo.Transformation.Scale = new Vector2(0.35f);
            logo.rotationSpeed = 0.05f;
            logo.offset = -0.5f;
            logo.GameObject.OnUpdate += logo.FollowMouse;
            hud.Add(logo);
            
            #region spinnydemo

            for (int i = 0; i < 10; i++)
            {
                Actors.Logo l = new Actors.Logo(new Vector2(r.Next(-800, 800), r.Next(-600, 600)));
                float scale = (float)r.NextDouble();
                l.Transformation.Scale = new Vector2(scale * 2.5f, scale * 2.5f);
                l.rotationSpeed = (float)(r.NextDouble() * 2) + 0.5f;
                if (r.Next(0, 2) == 1) l.rotationSpeed *= -1;
                l.offset = (float)(r.NextDouble() * Math.PI * 2);
                skybox.Add(l);
            }

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
