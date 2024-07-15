using Metroidvania.Src.Physics.Spatial.Shapes;
using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial
{
    internal class CollisionBox: Spatial
    {
        Shape Shape;
        bool Active = true;

        public CollisionBox(Shape shape)
        {
            Shape = shape;
        }

        public override void SetPosition(PointXY position)
        {
            base.SetPosition(position);
            Shape.SetPosition(position);
        }
    }
}
