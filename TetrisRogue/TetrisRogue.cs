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

            tileFallState = new StateManager(game);
            tileFallState.AddState("spawning");

            lateralTileMoveState = new StateManager(game);
            lateralTileMoveState.AddState("nada");

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
        StateManager tileFallState;
        StateManager lateralTileMoveState;
        void Update(GameTime time)
        {
            //Untested! This is currently probably horribly broken and is also currently 50% imaginary.
            //Console.WriteLine(tileFallState.CurrentState);
            #region tile falling logic (I wish outlining would let you collapse switch blocks)
            switch (tileFallState.CurrentState.Name)
            {
                case "fallingToNextSpot":
                    //Set up timed transition to changedSpot, to simulate the chunk "falling" between spaces
                    //over a period of time.
                    tileFallState.AddState("changedSpot", TimeSpan.FromSeconds(2));
                    break;

                case "transition fallingToNextSpot to changedSpot":
                    if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
                    {
                        tileFallState.ForceState("fastfall");
                        break;
                    }
                
                    //lerp chunk position for drawin'
                    Vector2 lastPosition = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition);
                    Vector2 nextPosition = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition.X, chunkLogicalPosition.Y + 1);                    
                    activeChunk.Position.Y = Vector2.Lerp(lastPosition, nextPosition, tileFallState.TransitionPercentComplete).Y;
                    //Console.WriteLine(tileFallState.TransitionPercentComplete);
                    break;

                case "changedSpot":
                    //we have made it to the next space!
                    chunkLogicalPosition.Y++;
                    activeChunk.Position.Y = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition.X, chunkLogicalPosition.Y).Y;

                    //make sure we haven't reached the bottom of the board
                    if (chunkLogicalPosition.Y + 1 != gameboard.Size.Y)
                    {
                        //check to see if the spot below is full or not
                        if (gameboard[chunkLogicalPosition.X, chunkLogicalPosition.Y + 1] == null)
                        {
                            tileFallState.AddState("fallingToNextSpot");
                            break;
                        }
                    }
                    
                    //Default case: bottom of board, or spot below is filled
                    tileFallState.AddState("landed");
                    break;

                case "landed":
                    //add the piece to the gameboard at the current logcal position
                    //boardLayer.Remove(activeChunk);
                    gameboard[chunkLogicalPosition] = activeChunk;
                    //prepare to spawn a new tile next update
                    tileFallState.AddState("spawning");
                    break;

                case "spawning":
                    activeChunk = generator.GenerateChunk(rng.Next());
                    //piece will spawn in the top-center of the game board
                    chunkLogicalPosition = new Point(gameboard.Size.X / 2, -1);
                    boardLayer.Add(activeChunk);
                    activeChunk.Position = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition);

                    
                    if (gameboard[chunkLogicalPosition.X, chunkLogicalPosition.Y+1] != null) 
                    {
                        //there is a tile here! player dies.
                        //todo: game state = failure
                        tileFallState.AddState("ur ded");
                    }
                    else
                    {
                        //perform the usual check next update before the piece starts falling
                        tileFallState.AddState("changedSpot");
                    }
                    break;

                case "fastfall":
                    for (int y = chunkLogicalPosition.Y; y < gameboard.Size.Y; y++)
                    {
                        //probe for a piece until bottom of board reached
                        if (gameboard[chunkLogicalPosition.X, y] != null) //piece found
                        {
                            tileFallState.AddState("landed"); //after this, it's touching something or something is fucked up.
                            chunkLogicalPosition.Y = y - 1;
                            break;
                        }
                    }

                    //looped until bottom of board reached. land chunk on bottom.
                    if (!tileFallState.Transitioning) //this will be true if "landed" was added above.
                    {
                        tileFallState.AddState("landed");
                        chunkLogicalPosition.Y = gameboard.Size.Y - 1;
                    }
                    break;
            }
            #endregion

            #region lateral tile movement logic
            if(Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                if (chunkLogicalPosition.X != 0) //not at left edge of board
                {
                    //chunk space to left is open
                    if (gameboard[chunkLogicalPosition.X - 1, chunkLogicalPosition.Y+1] == null)
                    {
                        //no obstruction! move left.
                        lateralTileMoveState.AddState("left", TimeSpan.FromSeconds(0.05));
                    }
                }
            }

            if(Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                if (chunkLogicalPosition.X != gameboard.Size.X - 1) //not at right edge of board
                {
                    //chunk space to right is open
                    if (gameboard[chunkLogicalPosition.X + 1, chunkLogicalPosition.Y+1] == null)
                    {
                        //no obstruction! move right.
                        lateralTileMoveState.AddState("right", TimeSpan.FromSeconds(0.05));
                    }
                }
            }

            
            switch (lateralTileMoveState.CurrentState.Name)
            {
                    
                case "transition nada to left":
                    Vector2 lastPosition = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition);
                    Vector2 nextPosition = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition.X - 1, chunkLogicalPosition.Y);
                    activeChunk.Position.X = Vector2.Lerp(lastPosition, nextPosition, lateralTileMoveState.TransitionPercentComplete).X;
                    break;
                case "transition nada to right":
                    lastPosition = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition);
                    nextPosition = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition.X + 1, chunkLogicalPosition.Y);
                    activeChunk.Position.X = Vector2.Lerp(lastPosition, nextPosition, lateralTileMoveState.TransitionPercentComplete).X;
                    break;
                case "right":
                    chunkLogicalPosition.X++;
                    activeChunk.Position.X = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition).X;
                    //return to not doing anything.
                    lateralTileMoveState.AddState("nada");
                    break;
                case "left":
                    chunkLogicalPosition.X--;
                    activeChunk.Position.X = gameboard.GetLogicalChunkCoordinate(chunkLogicalPosition).X;
                    //return to not doing anything.
                    lateralTileMoveState.AddState("nada");
                    break;
            }

            #endregion

            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                activeChunk.Rotate(Direction.Clockwise);
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
