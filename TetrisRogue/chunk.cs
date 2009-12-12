using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisRogue
{
    class Chunk
    {
        public Chunk(int w, int h)
        {
            _tiles = new DungeonTile[w, h];
        }

        public DungeonTile this[int x, int y]
        {
            get
            {
                return _tiles[x, y];
            }
            set 
            {
                _tiles[x, y] = value;
            }
        }

        public int Width { get { return _tiles.GetLength(0); } }
        public int Height { get { return _tiles.GetLength(1); } }

        private DungeonTile[,] _tiles;
    }
}
