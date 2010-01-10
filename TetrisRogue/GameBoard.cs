using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Zeplin;

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

        //Segments the gameboard into logical rooms, containing chunks.
        private void Roomify()
        {
            Random r = new Random();
            List<Chunk> openSet = new List<Chunk>();
            foreach (Chunk c in chunks)
            {
                openSet.Add(c);
            }

            while (openSet.Count > 0)
            {
                Chunk c = openSet[r.Next(openSet.Count)];

            }
        }
        
        internal Point Size
        {
            get
            {
                return new Point(chunks.GetLength(0), chunks.GetLength(1));
            }
        }
        
        private List<Room> rooms;
        private Chunk[,] chunks;
    }
}
