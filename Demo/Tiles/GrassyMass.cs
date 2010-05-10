﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeplin;
using Zeplin.CollisionShapes;
using Microsoft.Xna.Framework;


namespace Demo.Tiles
{
    class GrassyMass : Sprite
    {
        public GrassyMass(Vector2 position) : base(new Image(@"Images/grassymass"), new Transformation(position, Vector2.One, 0), new SATCollisionVolume())
        {
            Transformation.Scale = new Vector2(0.50f);
            Transformation.Pivot = Image.Center;
        }

        //static Sprite grassyMassSprite = new Sprite(@"Images/grassymass");
    }

    class GrassBrick : Sprite
    {
        public GrassBrick(Vector2 position) : base(new Image(@"Images/grassbrick3"), new Transformation(position, Vector2.One, 0), new SATCollisionVolume(new Vector2(5,18), new Vector2(392, 163)))
        {
            Transformation.Pivot = Image.Center;
            (CollisionVolume as SATCollisionVolume).ShowCollisionBoundaries = true;
        }
    }
}
