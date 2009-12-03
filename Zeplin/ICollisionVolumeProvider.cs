using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeplin
{
    public interface ICollisionVolumeProvider
    {
        ICollisionVolume CollisionVolume { get; set; }
    }
}
