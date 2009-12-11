using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Zeplin;

namespace TetrisRogue {
    enum Navigability 
    {
        Navigable,
        Wall
    }

    struct DungeonTile 
    {
        public DungeonTile(Sprite sprite, Rectangle extent, Navigability nav) 
        {
            _sprite = sprite;
            _extent = extent;
            _navigability = nav;
        }

        public Sprite Sprite { get { return _sprite; } }
        public Rectangle Extent { get { return _extent; } }
        public Navigability Navigability { get { return _navigability; } }

        private readonly Sprite _sprite;
        private readonly Rectangle _extent;
        private readonly Navigability _navigability;
    }
}
