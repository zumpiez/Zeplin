using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisRogue 
{
    class StupidChunkGenerator : IChunkGenerator 
    {
        public StupidChunkGenerator(IList<DungeonTile> tileDatabase)
        {
            TileDatabase = tileDatabase;
        }

        public Chunk GenerateChunk(long seed) 
        {
            Chunk c = new Chunk();
            Random r = new Random((int)seed);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    c[x, y] = new DungeonTile(TileDatabase[r.Next(TileDatabase.Count)]);
                }
            }

            return c;
        }

        public IList<DungeonTile> TileDatabase { get; private set; }
    }
}
