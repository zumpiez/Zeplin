using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Zeplin.Utilities;

namespace TetrisRogue
{
    class Room
    {
        public void AddChunk(Chunk c)
        {
            chunks.Add(c);
        }

        public bool Complete { get; internal set; }
        public bool Cleared { get; internal set; } //for debugging. todo: replace this with chunk traversal testing for entities

        public HSVColor Tint //for debugging. todo: remove me.
        {
            get
            {
                return _tint;
            }
            set
            {
                _tint = value;
                foreach (Chunk c in chunks)
                    c.Tint = value;
            }
        }

        HSVColor _tint;
        List<Chunk> chunks = new List<Chunk>();
    }
}
