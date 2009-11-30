using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    public delegate void Update(GameTime time);
    public delegate void Draw(GameTime time);

    public class GameObject
    {
        public Update OnUpdate { get; set; }

        public Draw OnDraw { get; set; }

        public ICollisionVolume CollisionVolume { get; set; }
    }
}
