using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisRogue 
{
    class StupidChunkGenerator : ChunkGenerator 
    {
        public Chunk GenerateChunk(IList<DungeonTile> tileDatabase, long seed) 
        {
            Chunk c = new Chunk();
            Random r = new Random((int)seed);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    //c[x,y] = tileDatabase[r.Next(tileDatabase.Count)];
                    c[x, y] = new DungeonTile(tileDatabase[r.Next(tileDatabase.Count)]);
                }
            }

            return c;
        }
    }
}
