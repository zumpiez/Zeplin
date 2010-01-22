using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Zeplin;
using Zeplin.Utilities;
using Microsoft.Xna.Framework.Graphics;
using TetrisRogue.Entities;

namespace TetrisRogue
{
    class GameBoard : GameObject
    {
        public GameBoard(int width, int height, int chunkSize)
        {
            if (width <= 1 || height <= 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            chunks = new Chunk[width, height];
            this.OnDraw += Draw;
            this.OnUpdate += Update;
        }

        public void Draw(GameTime time)
        {
            foreach (Chunk d in chunks)
            {
                if (d != null)
                    d.Draw(time);
            }
        }

        public Vector2 Position { get; set; }

        public Chunk this[int x, int y]
        {
            get
            {
                return chunks[x, y];
            }
            set
            {
                chunks[x, y] = value;
            }
        }
        public Chunk this[Point point]
        {
            get
            {
                return chunks[point.X, point.Y];
            }
            set
            {
                chunks[point.X, point.Y] = value;
            }
        }

        public DungeonTile GetDungeonTile(int x, int y)
        {
            return chunks[x / 4, y / 4][x % 4, y % 4];
        }
        public DungeonTile GetDungeonTile(Point p)
        {
            return GetDungeonTile(p.X, p.Y);
        }

        /// <summary>
        /// Locates a specific chunk instance on the board and returns its GameBoard coordinates.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Point? FindChunk(Chunk c)
        {
            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                //start from bottom: most likely to have chunks
                for (int y = chunks.GetLength(1) - 1; y >= 0; y--)
                {
                    if (chunks[x, y] == c) return new Point(x, y);
                }
            }

            //nothing found
            return null;
        }

        public void Update(GameTime time)
        {
            //todo: put dirty flag here so we aren't doing 5million translations a second
            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                for (int y = 0; y < chunks.GetLength(1); y++)
                {
                    //Console.WriteLine("chunk {0}, {1} is at {2}, {3}", x, y, Position.X + 24 * 4 * x, Position.Y + -24 * 4 * y);
                    if (chunks[x, y] != null)
                    {
                        chunks[x, y].Position = this.Position + new Vector2(24 * 4 * x, -24 * 4 * y);
                        chunks[x, y].Update(time);
                    }
                }
            }
        }

        /// <summary>
        /// Gets world coordinates for a chunk in the GameBoard.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal Vector2 GetLogicalChunkCoordinate(int x, int y)
        {
            return this.Position + new Vector2(24 * 4 * x, -24 * 4 * y);
        }
        /// <summary>
        /// Gets world coordinates for a chunk in the GameBoard.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal Vector2 GetLogicalChunkCoordinate(Point point)
        {
            return GetLogicalChunkCoordinate(point.X, point.Y);
        }

        /// <summary>
        /// Segments the gameboard into logical rooms, containing chunks.
        /// </summary>
        internal void Roomify()
        {
            /* DISCLAIMER
             * This is a HELLA naive implementation for the sake of being able to more quickly
             * start implementing things that depend on it. No consideration for speed or memory 
             * usage taken, and liberal copypasta was employed to avoid creating support methods
             * for an implementation that is going to be gutted anyway.
             * I only added classes and class members that I feel will be useful for the final
             * version.
             * todo colin replace this with your fun low complexity version.
             * 
             * yours truly,
             * jeffrey
             */

            Random r = new Random();
            List<Chunk> openSet = new List<Chunk>();
            Point north = new Point(0,-1);
            Point east = new Point(1,0);
            Point south = new Point(0,1);
            Point west = new Point(-1,0);

            rooms.Clear(); //TIME FOR FRESH ROOMS

            foreach (Chunk c in chunks)
            {
                if(c != null && c.Exits != ExitDirection.None)
                    openSet.Add(c);
            }

            Queue<Chunk> exploreQueue = new Queue<Chunk>();

            while (openSet.Count > 0)
            {
                Room room = new Room();
                
                //pick a random chunk from the open set
                Chunk seed = openSet[r.Next(openSet.Count)];
                exploreQueue.Enqueue(seed);

                while (exploreQueue.Count > 0)
                {
                    Chunk exploring = exploreQueue.Dequeue();
                    Point cLocation = FindChunk(exploring).Value; //null crash! fortunately if this is ever null, something is fucking wrong.

                    //assign this to a room and then prevent it from being tested again.
                    room.AddChunk(exploring);
                    openSet.Remove(exploring);

                    //this is the ugliest logic test I have ever written. And then I copy pasted it three more times. #hahawoo
                    if ((exploring.Exits & ExitDirection.North) == ExitDirection.North
                        && cLocation.Y != 0 //top of board: north is out of bounds
                        && this[cLocation.Add(north)] != null //north contains a chunk
                        && openSet.Contains(this[cLocation.Add(north)]) //chunk to north is still in the open set
                        && !exploreQueue.Contains(this[cLocation.Add(north)]) //chunk to north is not already in explore queue
                        && (this[cLocation.Add(north)].Exits & ExitDirection.South) == ExitDirection.South) //chunk opens into this chunk
                    {
                        exploreQueue.Enqueue(this[cLocation.Add(north)]);
                    }

                    if ((exploring.Exits & ExitDirection.East) == ExitDirection.East
                        && cLocation.X != chunks.GetLength(0) - 1 //right edge of board: east is out of bounds
                        && this[cLocation.Add(east)] != null //east contains a chunk
                        && openSet.Contains(this[cLocation.Add(east)]) //chunk to east is still in the open set
                        && !exploreQueue.Contains(this[cLocation.Add(east)]) //chunk to east is not already in explore queue
                        && (this[cLocation.Add(east)].Exits & ExitDirection.West) == ExitDirection.West) //chunk opens into this chunk
                    {
                        exploreQueue.Enqueue(this[cLocation.Add(east)]);
                    }

                    if ((exploring.Exits & ExitDirection.South) == ExitDirection.South
                        && cLocation.Y != chunks.GetLength(1) - 1 //bottom of board: south is out of bounds
                        && this[cLocation.Add(south)] != null //south contains a chunk
                        && openSet.Contains(this[cLocation.Add(south)]) //chunk to south is still in the open set
                        && !exploreQueue.Contains(this[cLocation.Add(south)]) //chunk to south is not already in explore queue
                        && (this[cLocation.Add(south)].Exits & ExitDirection.North) == ExitDirection.North) //chunk opens into this chunk
                    {
                        exploreQueue.Enqueue(this[cLocation.Add(south)]);
                    }

                    if((exploring.Exits & ExitDirection.West) == ExitDirection.West
                        && cLocation.X != 0 //left edge of board: west is out of bounds
                        && this[cLocation.Add(west)] != null //west contains a chunk
                        && openSet.Contains(this[cLocation.Add(west)]) //chunk to west is still in the open set
                        && !exploreQueue.Contains(this[cLocation.Add(west)]) //chunk to west is not already in explore queue
                        && (this[cLocation.Add(west)].Exits & ExitDirection.East) == ExitDirection.East) //chunk opens into this chunk
                    {
                        exploreQueue.Enqueue(this[cLocation.Add(west)]);
                    }
                }
                rooms.Add(room);
            }

            for(int i = 0; i < rooms.Count; i++)
            {
                rooms[i].Tint = new HSVColor(i / (float)rooms.Count, 0.5f, 1, 1);
            }
        }
        
        internal Point Size
        {
            get
            {
                return new Point(chunks.GetLength(0), chunks.GetLength(1));
            }
        }
        
        private List<Room> rooms = new List<Room>();
        private Chunk[,] chunks;

        #region Roguelike stuff
        /// <summary>
        /// Based on a type of target, will return the "most interesting" target from the board.
        /// </summary>
        /// <param name="me">The entity that is seeking a target.</param>
        /// <param name="targetType">The kind of entity the entity is seeking.</param>
        /// <returns>A specific entity reference</returns>
        Entity GetTarget(Entity me, EntityClass targetType)
        {
            throw new NotImplementedException(); //todo implement me
        }

        /// <summary>
        /// performs pathfinding and returns the first step.
        /// </summary>
        /// <param name="me">The entity that defines the starting point of the path</param>
        /// <param name="target">The entity that defines the end of the path</param>
        /// <returns>The first step of the best calculated path, as North, East, South or West.</returns>
        CardinalDirection GetPathToTarget(Entity me, Entity target) 
        {
            throw new NotImplementedException(); //todo implement me
        }

        /// <summary>
        /// Performs pathfinding and returns the complete path.
        /// </summary>
        /// <param name="me">The entity that defines the starting point of the path</param>
        /// <param name="target">The stationary entity that defines the end of the path</param>
        /// <returns>An ordered list of cardinal directions that define the best calculated path</returns>
        /// <remarks>This should only be used on entities with (stationary == true) to avoid logic errors. If used on a target that can move, the returned path is not guaranteed to be accurate after the update in which it was generated.</remarks>
        IList<CardinalDirection> GetPathToStationaryTarget(Entity me, Entity target)
        {
            throw new NotImplementedException(); //todo implement me
        }

        /// <summary>
        /// Gets the EntityClass for a hero in trouble, prioritized by remaining HP.
        /// </summary>
        /// <remarks>Use GetTarget/GetPath methods to get to him</remarks>
        EntityClass FriendInTrouble
        {
            get
            {
                throw new NotImplementedException(); //todo implement me
            }
        }

        #endregion
    }

    public enum CardinalDirection
    {
        North, East, South, West
    }
}
