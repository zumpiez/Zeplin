using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    public class GameObject
    {
        public delegate void update(GameTime time);
        public update OnUpdate;

        public delegate void draw(GameTime time);
        public draw OnDraw;

        public ICollisionVolume CollisionVolume;
    }
}
