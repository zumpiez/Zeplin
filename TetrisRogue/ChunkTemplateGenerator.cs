using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t = TetrisRogue.TileType;

namespace TetrisRogue
{
    class ChunkTemplateGenerator : IChunkGenerator
    {
        public ChunkTemplateGenerator(IList<DungeonTile> tileDatabase)
        {
            TileDatabase = tileDatabase;
        }

        public Chunk GenerateChunk(long seed)
        {
            Chunk c = new Chunk();
            Random r = new Random((int)seed);

            //int templateIdx = r.Next(TEMPLATE_DB.GetLength(0));
            int templateIdx = 0; //for debugging

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    c[x, y] = new DungeonTile(GetTileWithType(TEMPLATE_DB[templateIdx, x, y], r));
                }
            }

            return c;
        }

        private DungeonTile GetTileWithType(TileType type, Random rng)
        {
            IList<DungeonTile> tilesWithType = new List<DungeonTile>(TileDatabase.Where(tile => (tile.Type == type)));
            if (tilesWithType.Count == 0) tilesWithType = TileDatabase;

            return tilesWithType[rng.Next(tilesWithType.Count)];
        }

        public IList<DungeonTile> TileDatabase { get; private set; }
        bool SanchoMode { get; set; }

        private readonly TileType[,,] TEMPLATE_DB = new TileType[,,] { 
            { // corner
              { t.Rock, t.Wall,  t.Wall,  t.Wall  },
              { t.Wall, t.Floor, t.Floor, t.Floor },
              { t.Wall, t.Floor, t.Floor, t.Floor },
              { t.Wall, t.Floor, t.Floor, t.Floor }
            },

            { // wall
              { t.Wall, t.Floor, t.Floor, t.Floor },
              { t.Wall, t.Floor, t.Floor, t.Floor },
              { t.Wall, t.Floor, t.Floor, t.Floor },
              { t.Wall, t.Floor, t.Floor, t.Floor }
            },

            { // entryway
              { t.Wall, t.Floor, t.Floor, t.Wall },
              { t.Wall, t.Floor, t.Floor, t.Wall },
              { t.Wall, t.Floor, t.Floor, t.Wall },
              { t.Rock, t.Wall,  t.Wall,  t.Rock }
            },

            { // entryway 2: electric boogaloo
              { t.Wall, t.Floor,     t.Floor,     t.Wall },
              { t.Wall, t.Floor,     t.Floor,     t.Wall },
              { t.Wall, t.Threshold, t.Threshold, t.Wall },
              { t.Rock, t.Wall,      t.Wall,      t.Rock }
            },

            { // hall
              { t.Wall, t.Floor, t.Floor, t.Wall },
              { t.Wall, t.Floor, t.Floor, t.Wall },
              { t.Wall, t.Floor, t.Floor, t.Wall },
              { t.Wall, t.Floor, t.Floor, t.Wall }
            },

            { // tunnel
              { t.Rock, t.Wall,  t.Floor, t.Wall },
              { t.Rock, t.Wall,  t.Floor, t.Wall },
              { t.Rock, t.Wall,  t.Floor, t.Wall },
              { t.Rock, t.Wall,  t.Floor, t.Wall }
            },

            { // diag
              { t.Wall, t.Floor, t.Floor, t.Floor },
              { t.Rock, t.Wall,  t.Floor, t.Floor },
              { t.Rock, t.Rock,  t.Wall,  t.Floor },
              { t.Rock, t.Rock,  t.Rock,  t.Wall  }
            },

            { // void
              { t.Floor, t.Floor, t.Floor, t.Floor },
              { t.Floor, t.Floor, t.Floor, t.Floor },
              { t.Floor, t.Floor, t.Floor, t.Floor },
              { t.Floor, t.Floor, t.Floor, t.Floor }
            }
        };
    }
}
