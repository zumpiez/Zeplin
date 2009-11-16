 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeplin
{
    public interface ICollisionVolume
    {
        bool TestCollisionCompatibility(ICollisionVolume other);
        bool TestCollision(ICollisionVolume other);
    }
}
