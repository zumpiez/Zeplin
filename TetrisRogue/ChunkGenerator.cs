using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisRogue 
{
    interface ChunkGenerator 
    {
        Chunk GenerateChunk(IEnumerable<Chunk> chunkDatabase, long seed);
    }
}
