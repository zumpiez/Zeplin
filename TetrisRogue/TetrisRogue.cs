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

        void Load()
        {
            Sprite characters = new Sprite(PointScale(3, game.Content.Load<Texture2D>(@"characters")));
            Sprite environment = new Sprite(PointScale(3, game.Content.Load<Texture2D>(@"environment")));

            MetaFont za = new MetaFont("Zaratustra Assemblee", game.Content.RootDirectory);

            Layer l = Engine.CurrentMap.NewLayer();
            Layer hud = Engine.CurrentMap.NewLayer(100);
            
            Engine.Camera.Dimensions = new Vector2(800, 600);
            Engine.Camera.Center = new Vector2(400, -300);

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

            //Chunk will be responsible for drawing its own DungeonTile set.
            l.Add(activeChunk);

            //this will go away after Chunk has an OnDraw implemented.
            /*for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    c[x, y].Transformation.Position = new Vector2(40 + 24 * x, -60 - 24 * y);
                    l.Add(c[x, y]);
                }
            }*/

            activeChunk.Position = new Vector2(100, -100);
        }

        Chunk activeChunk;

        void Update(GameTime time)
        {
            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
                activeChunk.Rotate(Direction.Clockwise);
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
