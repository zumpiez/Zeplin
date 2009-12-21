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

    class DungeonTile : Tile
    {
        public DungeonTile(Sprite sprite, Rectangle extent, Navigability nav) : base(sprite, (AnimationScript)null)
        {
            SubRect = extent;
            _extent = extent;
            _navigability = nav;
        }

        public DungeonTile(DungeonTile copy) : base(copy.Sprite, copy.AnimationScript)
        {
            SubRect = copy._extent;
            this._extent = copy._extent;
            this._navigability = copy._navigability;
        }

        public Rectangle Extent { get { return _extent; } }
        public Navigability Navigability { get { return _navigability; } }

        private readonly Sprite _sprite;
        private readonly Rectangle _extent;
        private readonly Navigability _navigability;
    }
}
