using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisRogue
{
    class GameBoard
    {
        public GameBoard(int width, int height, int chunkSize)
        {
            if (width <= 1 || height <= 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            chunks = new Chunk[width, height];
        }

        private Chunk nextChunk;


        private Chunk[,] chunks;
    }
}
