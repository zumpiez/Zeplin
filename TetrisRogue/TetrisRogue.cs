using System;
using System.Collections.Generic;
using System.Linq;
using Zeplin;
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
            Sprite characters = new Sprite(@"characters");
            Sprite environment = new Sprite(@"environment");
            RenderTarget2D scaledCharacters = new RenderTarget2D(game.GraphicsDevice, characters.Image.Width * 3, characters.Image.Height * 3, 0, characters.Image.Format);
            RenderTarget2D scaledEnvironment = new RenderTarget2D(game.GraphicsDevice, environment.Image.Width * 3, environment.Image.Height * 3, 0, environment.Image.Format);
            DepthStencilBuffer dsb = new DepthStencilBuffer(scaledCharacters.GraphicsDevice, scaledCharacters.Width, scaledCharacters.Height, scaledCharacters.GraphicsDevice.DepthStencilBuffer.Format);
            game.GraphicsDevice.SetRenderTarget(0, scaledCharacters);

            DepthStencilBuffer old = game.GraphicsDevice.DepthStencilBuffer;
            game.GraphicsDevice.DepthStencilBuffer = dsb;

            TextureFilter originalFilter = game.GraphicsDevice.SamplerStates[0].MagFilter;

            //game.GraphicsDevice.Clear(Color.Magenta);
            SpriteBatch batch = new SpriteBatch(game.GraphicsDevice);
            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            game.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.None;
            batch.Draw(characters.Image, new Rectangle(0, 0, scaledCharacters.Width, scaledCharacters.Height), Color.White);
            batch.End();
            
            game.GraphicsDevice.SetRenderTarget(0, null);
            game.GraphicsDevice.DepthStencilBuffer = old;
            game.GraphicsDevice.SamplerStates[0].MagFilter = originalFilter;
            
            characters = new Sprite(scaledCharacters.GetTexture());
            
            game.GraphicsDevice.SetRenderTarget(0, scaledEnvironment);
            batch = new SpriteBatch(game.GraphicsDevice);
            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            game.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.None;
            batch.Draw(environment.Image, new Rectangle(0, 0, scaledEnvironment.Width, scaledEnvironment.Height), Color.White);
            batch.End();
            
            game.GraphicsDevice.SetRenderTarget(0, null);
            game.GraphicsDevice.DepthStencilBuffer = old;
            game.GraphicsDevice.SamplerStates[0].MagFilter = originalFilter;
            
            environment = new Sprite(scaledEnvironment.GetTexture());

            
            Layer l = Engine.CurrentMap.NewLayer();
            
            Engine.Camera.Dimensions = new Vector2(800, 600);
            Engine.Camera.Center = new Vector2(400, -300);

            DungeonTile[] tiles = 
            {
                new DungeonTile(environment, new Rectangle(120, 0, 24, 24), Navigability.Navigable),
                new DungeonTile(environment, new Rectangle(144, 0, 24, 24), Navigability.Navigable),
            };

            Chunk c = new StupidChunkGenerator().GenerateChunk(tiles, 9999);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    c[x, y].Transformation.Position = new Vector2(40 + 24 * x, -60 - 24 * y);
                    l.Add(c[x, y]);
                }
            }
        }

        Tile brick;
        void Update(GameTime time)
        {
            //brick.Transformation.Position = Input.MousePosition;
        }

        public static Tile GetTileFromSpritesheet(Sprite sourceArt, Rectangle rect)
        {
            /*int framex;
            if (rect.X == 0) framex = 0;
            else framex = rect.X / rect.Width; 
            
            int framey;
            if (rect.Y == 0) framey = 0;
            else framey = rect.Y / rect.Height; 

            int frame = framey * (sourceArt.Image.Width / rect.Width) + framex;*/

            Tile result = new Tile(sourceArt, new Transformation(new Vector2(40, 60), new Vector2(1, 1), 0));

            result.SubRect = rect;

            return result;
        }
    }
}
