using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisRogue 
{
    internal interface IChunkGenerator 
    {
        Chunk GenerateChunk(long seed);

        IList<DungeonTile> TileDatabase { get; }
    }
}
