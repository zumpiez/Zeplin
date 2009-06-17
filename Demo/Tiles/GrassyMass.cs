using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeplin;
using Microsoft.Xna.Framework;

namespace Demo.Tiles
{
    class GrassyMass : Tile
    {
        public GrassyMass(Vector2 position) : base(new Sprite(@"Images/grassymass"), new Transformation(position, Vector2.One, 0), new SATCollisionVolume())
        {
            this.Scale = new Vector2(0.50f);
            this.Pivot = Sprite.GetCenter();
        }

        //static Sprite grassyMassSprite = new Sprite(@"Images/grassymass");
    }

    class GrassBrick : Tile
    {
        public GrassBrick(Vector2 position) : base(new Sprite(@"Images/grassbrick3"), new Transformation(position, Vector2.One, 0), new SATCollisionVolume(new Vector2(5,18), new Vector2(392, 163)))
        {
            this.Pivot = Sprite.GetCenter();
        }
    }
}
