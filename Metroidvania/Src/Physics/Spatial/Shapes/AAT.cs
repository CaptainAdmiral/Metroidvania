using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System.Diagnostics.CodeAnalysis;

namespace Metroidvania.Src.Physics.Spatial.Shapes
{
    /// <summary>
    /// Axis Aligned Triangle
    /// </summary>
    internal class AAT : Polygon
    {
        public required double Width { get; set; }
        public required double Height { get; set; }

        [SetsRequiredMembers]
        public AAT(PointXY anchor, double width, double height)
        {
            Position = anchor;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{Position}, W:{Width}, H:{Height}";
        }
    }
}
