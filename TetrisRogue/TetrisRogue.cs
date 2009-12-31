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

        StateManager tileFallState;


        //doing this lazy-style to get it working. we can engineer something if we care to.
        Sprite characters;
        Sprite environment;
        void Load()
        {
            game.GraphicsDeviceManager.DeviceReset += new EventHandler(GraphicsDevice_DeviceReset);

            characters = new Sprite(PointScale(3, Engine.Content.Load<Texture2D>(@"characters")));
            environment = new Sprite(PointScale(3, Engine.Content.Load<Texture2D>(@"environment")));
            MetaFont za = new MetaFont("Zaratustra Assemblee", game.Content.RootDirectory);

            tileFallState = new StateManager(game);
            tileFallState.AddState("spawning");

            boardLayer = Engine.CurrentMap.NewLayer();
            Layer hud = Engine.CurrentMap.NewLayer(100);
            
            Engine.Camera.Dimensions = new Vector2(1280, 720);
            Engine.Camera.Center = new Vector2(640, -360);
            Engine.Camera.Mode = CameraCropMode.MaintainWidth;

            //Tile debug = new Tile(new Sprite(@"debug"));

            //l.Add(debug);

            DungeonTile[] tiles = 
            {
                new DungeonTile(environment, OryxTile(3, 0), TileType.Rock),
                new DungeonTile(environment, OryxTile(3, 11), TileType.Rock),
                new DungeonTile(environment, OryxTile(7, 11), TileType.Rock),

                // gray unfinished
                new DungeonTile(environment, OryxTile(0, 0), TileType.Wall),
                new DungeonTile(environment, OryxTile(1, 0, 2, 1), TileType.Wall, new AnimationScript(new Point[]{new Point(0, 0), new Point(1,0)}, TimeSpan.FromSeconds(1))),
                new DungeonTile(environment, OryxTile(3, 0), TileType.Wall),
                // gray w/brown box
                new DungeonTile(environment, OryxTile(0, 11), TileType.Wall),
                new DungeonTile(environment, OryxTile(1, 11, 2, 1), TileType.Wall, new AnimationScript(new Point[]{new Point(0,0), new Point(1,0)}, TimeSpan.FromSeconds(1))),
                new DungeonTile(environment, OryxTile(3, 11), TileType.Wall),
                // gray w/gray box
                new DungeonTile(environment, OryxTile(4, 11), TileType.Wall),
                // these tiles look like someone took a bite out of the bottom
                //new DungeonTile(environment, OryxTile(5, 11, 2, 1), TileType.Wall, new AnimationScript(new int[]{0, 1}, 1)),
                new DungeonTile(environment, OryxTile(7, 11), TileType.Wall),

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

            TextWidget tw = new TextWidget("Tetrogue v0.1");
            tw.Position = new Vector2(616, 0);
            tw.HorizontalAlignment = Alignment.Near;
            tw.VerticalAlignment = Alignment.Near;
            tw.FontFace = za;
            tw.FontSize = 32;
            tw.Foreground = Color.Black;
            hud.Add(tw);

            generator = new ChunkTemplateGenerator(tiles);
            gameboard = new GameBoard(6, 8, 4);
            
            /*long seed = DateTime.Now.Ticks;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    gameboard[i, j] = ctg.GenerateChunk(seed);
                    seed <<= 2;
                    seed ^= DateTime.Now.Ticks;
                }
            }*/

            boardLayer.Add(gameboard);
            gameboard.Position = new Vector2(20, 20);
        }

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

        GameBoard gameboard;
        ChunkTemplateGenerator generator;
        Chunk activeChunk;
        Point chunkLogicalPosition = Point.Zero;
        Random rng = new Random();
        Layer boardLayer;
        void Update(GameTime time)
        {
            //Untested! This is currently probably horribly broken and is also currently 50% imaginary.
            //Console.WriteLine(tileFallState.CurrentState);
            switch (tileFallState.CurrentState.Name)
            {
                case "fallingToNextSpot":
                    //Set up timed transition to changedSpot, to simulate the chunk "falling" between spaces
                    //over a period of time.
                    tileFallState.AddState("changedSpot", TimeSpan.FromSeconds(2));
                    break;

                case "transition fallingToNextSpot to changedSpot":
                    //lerp chunk position for drawin'
                    Vector2 lastPosition = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition.X, chunkLogicalPosition.Y);
                    Vector2 nextPosition = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition.X, chunkLogicalPosition.Y + 1);
                    activeChunk.Position = Vector2.Lerp(lastPosition, nextPosition, tileFallState.TransitionPercentComplete);
                    Console.WriteLine(tileFallState.TransitionPercentComplete);
                    break;

                case "changedSpot":
                    //we have made it to the next space!
                    chunkLogicalPosition.Y++;
                    //check to see if the spot below is full or not
                    if (gameboard[chunkLogicalPosition.X, chunkLogicalPosition.Y + 1] == null)
                        tileFallState.AddState("fallingToNextSpot");
                    activeChunk.Position = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition.X, chunkLogicalPosition.Y);
                    break;

                case "landed":
                    //add the piece to the gameboard at the current logcal position
                    boardLayer.Remove(activeChunk);
                    gameboard[chunkLogicalPosition.X, chunkLogicalPosition.Y] = activeChunk;
                    //prepare to spawn a new tile next update
                    tileFallState.AddState("spawning");
                    break;

                case "spawning":
                    activeChunk = generator.GenerateChunk(rng.Next());
                    //piece will spawn in the top-center of the game board
                    chunkLogicalPosition = new Point(gameboard.Size.X / 2, -1);
                    boardLayer.Add(activeChunk);

                    
                    if (gameboard[chunkLogicalPosition.X, chunkLogicalPosition.Y+1] != null) 
                    {
                        //there is a tile here! player dies.
                        //todo: gamestate = "failure"
                    }
                    else
                    {
                        //perform the usual check next update before the piece starts falling
                        tileFallState.AddState("changedSpot");
                    }

                    break;
            }


            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) && Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                if (!game.GraphicsDeviceManager.IsFullScreen) game.SetDefaultResolution();
                else game.ChangeResolution(800, 600, false);
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
            batch.Draw(sourceImage, new Rectangle(0, 0, sourceImage.Width * scale, sourceImage.Height * scale), Color.White);
            batch.End();

            //restore original settings
            game.GraphicsDevice.SetRenderTarget(0, null);
            game.GraphicsDevice.DepthStencilBuffer = stashedDepthStencilBuffer;
            game.GraphicsDevice.SamplerStates[0].MagFilter = stashedFilter;

            return scaled.GetTexture();
        }
    }
}
