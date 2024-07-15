using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using Metroidvania.Src.Physics.Spatial.Shapes;
using Metroidvania.Src.Game;
using Metroidvania.Src.Physics.Spatial.Environment;

namespace Metroidvania.Src.Physics.Spatial
{
    internal class World : IScene, IUpdatable
    {
        public List <StaticEnvironment> Environment = new(); // TODO: Add lists for the other spatial groups. Add method to retrieve list from spatial group.

        public SpatialGrid SpatialGrid = new(new AABB(new PointXY(0, 0), new PointXY(100, 100)), cellSize: 10);

        public void Update(double dTime)
        {
            // TODO: Update lists of living, projectiles etc
        }
    }
}
