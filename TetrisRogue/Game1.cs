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
    public class Game1
    {
        ZeplinGame game;

        public Game1()
        {
            game = new ZeplinGame();
            game.OnLoad += Load;
            game.OnUpdate += OnUpdate;
            game.Run();
        }

        void Load()
        {
            Sprite characters = new Sprite(@"characters");
            RenderTarget2D scaledCharacters = new RenderTarget2D(game.GraphicsDevice, characters.Image.Width * 3, characters.Image.Height * 3, 0, SurfaceFormat.Color);
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

            Layer l = Engine.CurrentMap.NewLayer();
            
            Tile bigchars = new Tile(characters,new Transformation(Vector2.Zero, Vector2.One, 0.0f));
            l.Add(bigchars);

            Engine.Camera.Dimensions = new Vector2(800, 600);
            Engine.Camera.Center = new Vector2(400, -300);
        }

        void OnUpdate(GameTime time)
        {
        }
    }
}
