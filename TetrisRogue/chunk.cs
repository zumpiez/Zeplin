using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeplin;

namespace TetrisRogue
{
    class Chunk : GameObject
    {
        public Chunk()
        {
            _tiles = new DungeonTile[4, 4];
        }

        public DungeonTile this[int x, int y]
        {
            get
            {
                RotateCoordinates(ref x, ref y);
                return _tiles[x, y];
            }
            set 
            {
                RotateCoordinates(ref x, ref y);
                _tiles[x, y] = value;
            }
        }

        public void Rotate(Direction direction)
        {
            switch(direction)
            {
                case Direction.Clockwise:
                    _rotation = (Rotation)(((int)_rotation + 1) % 4);
                    break;

                case Direction.Counterclockwise:
                    _rotation = (Rotation)(((int)_rotation - 1) % 4);
                    break;
            }
        }

        public void RotateCoordinates(ref int x, ref int y)
        {
            switch (_rotation)
            {
                case Rotation.Cw90:
                    y = x;
                    x = 3 - y;
                    break;

                case Rotation.Cw180:
                    x = 3 - x;
                    y = 3 - y;
                    break;

                case Rotation.Cw270:
                    x = y;
                    y = 3 - x;
                    break;
                    
                default:
                    break;
            }
        }

        private DungeonTile[,] _tiles;
        private Rotation _rotation;
    }

    public enum Direction
    {
        Clockwise, Counterclockwise
    }

    public enum Rotation
    {
        None, Cw90, Cw180, Cw270
    }
}
