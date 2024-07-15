using Metroidvania.Src.Game;
using Metroidvania.Src.Physics.Spatial.Environment;
using Metroidvania.Src.Physics.Spatial.Shapes;
using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using Metroidvania.Src.Physics.Spatial.Types.SpatialGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial
{
    internal class SpatialObject : Spatial, IUpdatable
    {
        public EnvironmentalCollisionBox? ECB { get; set; } // TODO move whenever SpatialObject moves (make child).
        public PointXY LastPosition { get; private set; }
        public VectorXY Motion { get; private set; }
        public VectorXY LastMotion { get; private set; }
        
        public SpatialObject(bool spatialTracking = true)
        {
            SpatialTracking = spatialTracking;
        }

        public void Update(double dTime)
        {
            UpdateMotion(dTime);
        }

        public override void SetPosition(PointXY position)
        {
            LastPosition = Position;
            AABB newBounds = Bounds; // TODO
            if (SpatialTracking) _world.SpatialGrid?.UpdateMoved(this, Bounds, newBounds);
            base.SetPosition(position);
        }

        public void UpdateMotion(double dTime)
        {
            VectorXY moveVec = Motion * dTime;

            // Find bounding box containing aabb of start position and end position
            // Broad phase lookup on bounding box
            // Check each collision box of mover against each collision box of potential colliders.
            // No medium phase pass needed on bounding boxes for environement.
            // Only doing environment for this class anyway.

            AABB newBounds = Bounds.Translated(moveVec);
            AABB SearchArea = AABB.BoundsOf(Bounds, newBounds);

            HashSet<Spatial> spatials = _world.SpatialGrid.GetWithinBounds(SearchArea);


        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
