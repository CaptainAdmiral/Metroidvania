using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial.Shapes
{
    internal abstract class Shape : Positional
    {
        /// <summary>
        /// Point In Polygon
        /// </summary>
        /// <returns>true if a given point is inside this shape</returns>
        public abstract bool Contains(PointXY point);

        /// <summary>
        /// Points In Polygon
        /// </summary>
        /// <returns>true if all points are inside this shape</returns>
        public bool ContainsAll(PointXY[] points)
        {
            return points.All(point => Contains(point));
        }

        /// <returns>A collision object (or null if no collison) representing the earliest collison of this shape with polygon <paramref name="p"/>
        /// when this shape is moved by vector <paramref name="moveVec"/></returns>
        public abstract Collision? GetCollision(Polygon p, VectorXY moveVec);

        /// <returns>A collision object (or null if no collison) representing the earliest collison of this shape with circle <paramref name="c"/>
        /// when this shape is moved by vector <paramref name="moveVec"/></returns>
        public abstract Collision? GetCollision(Circle c, VectorXY moveVec);

        public abstract override string ToString();
    }
}
