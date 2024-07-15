using Metroidvania.Src.Physics.Spatial.Shapes;
using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using  static Metroidvania.Src.Physics.PhysicsConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial.Environment
{
    internal class EnvironmentalCollisionBox: CollisionBox
    {
        public Collision[] collisions = new Collision[MAX_COLLISIONS]; // Collison history for last tick

        public EnvironmentalCollisionBox(Shape shape) : base(shape) { }
    }
}
