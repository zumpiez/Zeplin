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
            game.GraphicsDevice.DeviceReset += new EventHandler(GraphicsDevice_DeviceReset);
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
            //Engine.ChangeResolution(1280, 1024, true);
            //Engine.SetDefaultResolution();

            characters = new Sprite(PointScale(3, Engine.Content.Load<Texture2D>(@"characters")));
            environment = new Sprite(PointScale(3, Engine.Content.Load<Texture2D>(@"environment")));            
            
            Layer l = Engine.CurrentMap.NewLayer();
            
            Engine.Camera.Dimensions = new Vector2(1280, 720);
            Engine.Camera.Center = new Vector2(600, -400);
            Engine.Camera.Mode = CameraCropMode.MaintainWidth;

            DungeonTile[] tiles = 
            {
                new DungeonTile(environment, new Rectangle(120, 0, 24, 24), Navigability.Navigable),
                new DungeonTile(environment, new Rectangle(144, 0, 24, 24), Navigability.Navigable),
            };

            activeChunk = new StupidChunkGenerator().GenerateChunk(tiles, 9999);

            l.Add(activeChunk);

            activeChunk.Position = Vector2.Zero;
        }

        Chunk activeChunk;

        void Update(GameTime time)
        {
            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
                activeChunk.Rotate(Direction.Clockwise);

            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
                Engine.ChangeResolution(1024, 768, false);

            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.N))
                Engine.ChangeResolution(800, 600, false);
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
