using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial
{
    internal class Spatial : Positional
    {
        public Guid Id { get; } = Guid.NewGuid();
        protected World _world;
        public List<SpatialType> Groups = new();
        public bool SpatialTracking { get; init; }
    }
}
