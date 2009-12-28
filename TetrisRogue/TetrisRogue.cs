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

            //Tile debug = new Tile(new Sprite(@"debug"));

            //l.Add(debug);

            DungeonTile[] tiles = 
            {
                new DungeonTile(environment, OryxTile(0, 0), TileType.Rock),
                new DungeonTile(environment, OryxTile(3, 0), TileType.Rock),
                new DungeonTile(environment, OryxTile(0, 0), TileType.Wall),
                new DungeonTile(environment, OryxTile(1, 0, 2, 1), TileType.Wall, new AnimationScript(new Point[]{new Point(0,0), new Point(0,1)}, TimeSpan.FromSeconds(1))),
                new DungeonTile(environment, OryxTile(3, 0), TileType.Wall),
                new DungeonTile(environment, OryxTile(5, 0), TileType.Floor),
                new DungeonTile(environment, OryxTile(6, 0), TileType.Floor),
                new DungeonTile(environment, OryxTile(7, 0), TileType.StairsUp),
                new DungeonTile(environment, OryxTile(8, 0), TileType.StairsDown),
                new DungeonTile(environment, OryxTile(9, 0), TileType.Pit),
                new DungeonTile(environment, OryxTile(10, 0), TileType.TrapDoorClosed),
                new DungeonTile(environment, OryxTile(11, 0), TileType.TrapDoorOpen),
                new DungeonTile(environment, OryxTile(12, 0), TileType.Threshold),
                new DungeonTile(environment, OryxTile(0, 5), TileType.Threshold),
                new DungeonTile(environment, OryxTile(1, 5), TileType.Threshold),
                new DungeonTile(environment, OryxTile(1, 6), TileType.Threshold)
            };

            tiles[3].FrameSize = new Point(24, 24);

            TextWidget tw = new TextWidget("whoa I'm some text");
            tw.Position = new Vector2(100, 100);
            tw.HorizontalAlignment = Alignment.Near;
            tw.VerticalAlignment = Alignment.Near;
            tw.FontFace = za;
            tw.FontSize = 48;
            tw.Foreground = Color.White;
            hud.Add(tw);

            activeChunk = new ChunkTemplateGenerator(tiles).GenerateChunk(DateTime.Now.Ticks);

            l.Add(activeChunk);

            activeChunk.Position = Vector2.Zero;
        }

        Chunk activeChunk;

        Rectangle OryxTile(int left, int top) { return OryxTile(left, top, 1, 1, 3); }
        Rectangle OryxTile(int left, int top, int width, int height) { return OryxTile(left, top, width, height, 3); }

        Rectangle OryxTile(int left, int top, int width, int height, int scale)
        {
            Rectangle subrect = new Rectangle();

            subrect.X = left * 8 * scale;
            subrect.Y = top * 8 * scale;
            subrect.Width = width * 8 * scale;
            subrect.Height = height * 8 * scale;

            return subrect;
        }

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
