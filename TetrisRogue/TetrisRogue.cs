using System;
using System.Collections.Generic;
using System.Linq;
using Zeplin;
using Zeplin.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TetrisRogue
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TetrisRogue
    {
        ZeplinGame game;

        public TetrisRogue()
        {
            game = new ZeplinGame();
            game.OnLoad += Load;
            game.OnUpdate += Update;
            game.Run();
        }

        void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            characters.Image = PointScale(3, Engine.Content.Load<Texture2D>(@"characters"));
            environment.Image = PointScale(3, Engine.Content.Load<Texture2D>(@"environment"));
        }

        //doing this lazy-style to get it working. we can engineer something if we care to.
        Sprite characters;
        Sprite environment;
        void Load()
        {
            game.GraphicsDeviceManager.DeviceReset += new EventHandler(GraphicsDevice_DeviceReset);

            characters = new Sprite(PointScale(3, Engine.Content.Load<Texture2D>(@"characters")));
            environment = new Sprite(PointScale(3, Engine.Content.Load<Texture2D>(@"environment")));            
            MetaFont za = new MetaFont("Zaratustra Assemblee", game.Content.RootDirectory);
            
            Layer l = Engine.CurrentMap.NewLayer();
            Layer hud = Engine.CurrentMap.NewLayer(100);
            
            Engine.Camera.Dimensions = new Vector2(1280, 720);
            Engine.Camera.Center = new Vector2(640, -360);
            Engine.Camera.Mode = CameraCropMode.MaintainWidth;

            Tile debug = new Tile(new Sprite(@"debug"));

            l.Add(debug);

            DungeonTile[] tiles = 
            {
                new DungeonTile(environment, new Rectangle(120, 0, 24, 24), Navigability.Navigable),
                new DungeonTile(environment, new Rectangle(144, 0, 24, 24), Navigability.Navigable),
            };

            TextWidget tw = new TextWidget("whoa I'm some text");
            tw.Position = new Vector2(100, 100);
            tw.HorizontalAlignment = Alignment.Near;
            tw.VerticalAlignment = Alignment.Near;
            tw.FontFace = za;
            tw.FontSize = 32;
            tw.Foreground = Color.White;
            hud.Add(tw);

            activeChunk = new StupidChunkGenerator().GenerateChunk(tiles, 9999);

            //l.Add(activeChunk);

            activeChunk.Position = Vector2.Zero;
        }

        Chunk activeChunk;

        void Update(GameTime time)
        {
            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
                activeChunk.Rotate(Direction.Clockwise);

            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
            {
                if(!game.GraphicsDeviceManager.IsFullScreen)
                {
                    if (World.gameResolution.X == 800) game.ChangeResolution(1024, 768, false);
                    else game.ChangeResolution(800, 600, false);
                }
            }

            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.N))
            {
                Engine.Camera.Mode = (CameraCropMode)((int)(Engine.Camera.Mode + 1) % 3);
            }

            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter) && Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
            {
                if (game.GraphicsDeviceManager.IsFullScreen) game.ChangeResolution(800, 600, false);
                else game.SetDefaultResolution();
            }
            
        }

        public static Tile GetTileFromSpritesheet(Sprite sourceArt, Rectangle rect)
        {
            Tile result = new Tile(sourceArt, new Transformation(new Vector2(40, 60), new Vector2(1, 1), 0));

            result.SubRect = rect;

            return result;
        }

        public Texture2D PointScale(int scale, Texture2D sourceImage)
        {
            RenderTarget2D scaled = GraphicsHelper.CreateRenderTarget(game.GraphicsDevice, sourceImage.Width * scale, sourceImage.Height * scale);
            DepthStencilBuffer dsb = new DepthStencilBuffer(scaled.GraphicsDevice, scaled.Width, scaled.Height, scaled.GraphicsDevice.DepthStencilBuffer.Format);

            //stash the original graphics settings
            DepthStencilBuffer stashedDepthStencilBuffer = game.GraphicsDevice.DepthStencilBuffer;
            TextureFilter stashedFilter = game.GraphicsDevice.SamplerStates[0].MagFilter;

            //cram in the new settings
            game.GraphicsDevice.SetRenderTarget(0, scaled);
            game.GraphicsDevice.DepthStencilBuffer = dsb;
            game.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.None;

            //draw to the render surface
            SpriteBatch batch = new SpriteBatch(game.GraphicsDevice);
            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            game.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.None;
            batch.Draw(sourceImage, new Rectangle(0, 0, scaled.Width, scaled.Height), Color.White);
            batch.End();

            //restore original settings
            game.GraphicsDevice.SetRenderTarget(0, null);
            game.GraphicsDevice.DepthStencilBuffer = stashedDepthStencilBuffer;
            game.GraphicsDevice.SamplerStates[0].MagFilter = stashedFilter;

            return scaled.GetTexture();
        }
    }
}
