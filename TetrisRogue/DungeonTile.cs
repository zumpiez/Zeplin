﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Zeplin;

namespace TetrisRogue {
    enum TileType 
    {
        Floor,
        Wall,
        Rock,
        StairsUp,
        StairsDown,
        Pit,
        TrapDoorClosed,
        TrapDoorOpen,
        Threshold,
    }

    class DungeonTile : Tile
    {
        public DungeonTile(Sprite sprite, Rectangle extent, TileType type)
            : this(sprite, extent, type, (AnimationScript)null) { }

        public DungeonTile(Sprite sprite, Rectangle extent, TileType type, AnimationScript script) : base(sprite, script)
        {
            SubRect = extent;
            _extent = extent;
            _type = type;
            if (script != null) script.Loop = true; // hack: this really shouldn't beee heeeeere
        }

        public DungeonTile(DungeonTile copy) : base(copy)
        {
            this._extent = copy._extent;
            this._type = copy._type;
        }

        public Rectangle Extent { get { return _extent; } }
        public TileType Type { get { return _type; } }

        private readonly Rectangle _extent;
        private readonly TileType _type;
    }
}
