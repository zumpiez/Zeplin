using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    public class GameObject
    {
        public delegate void Update(GameTime time);
        public Update OnUpdate;

        public delegate void Draw(GameTime time);
        public Draw OnDraw;

        public ICollisionVolume CollisionVolume;
    }
}
