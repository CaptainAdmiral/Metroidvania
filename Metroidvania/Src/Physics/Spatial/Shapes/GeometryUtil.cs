using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial.Shapes
{
    internal static class GeometryUtil
    {
        /// <summary>
        /// Finds the intersection of two lines.
        /// Parallel lines will return null even for segments of the same line.
        /// </summary>
        /// <param name="p1">line 1 point 1</param>
        /// <param name="p2">line 1 point 2</param>
        /// <param name="p3">line 2 point 1</param>
        /// <param name="p4">line 2 point 2</param>
        /// <returns>PointXY of intersection if there is any intersection.</returns>
        public static PointXY? FindIntersection(PointXY p1, PointXY p2, PointXY p3, PointXY p4)
        {
            double x1 = p1.X, y1 = p1.Y;
            double x2 = p2.X, y2 = p2.Y;
            double x3 = p3.X, y3 = p3.Y;
            double x4 = p4.X, y4 = p4.Y;

            double gradDiff = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (gradDiff == 0) return null;

            double d1 = x1 * y2 - y1 * x2;
            double d2 = x3 * y4 - y3 * x4;

            double x = (d1 * (x3 - x4) - (x1 - x2) * d2) / gradDiff;
            double y = (d1 * (y3 - y4) - (y1 - y2) * d2) / gradDiff;
            
            if ((x >= Math.Min(x1, x2) && x <= Math.Max(x1, x2) &&
                y >= Math.Min(y1, y2) && y <= Math.Max(y1, y2)) &&
                (x >= Math.Min(x3, x4) && x <= Math.Max(x3, x4) &&
                y >= Math.Min(y3, y4) && y <= Math.Max(y3, y4)))
            {
                return new PointXY(x, y);
            }

            return null;
        }

        /// <summary>
        /// Quadratic formula
        /// </summary>
        /// <returns>An array of solutions of length 2, 1, or 0 if there are no real solutions</returns>
        public static double[] Quadratic(double A, double B, double C)
        {
            double disc = B * B - 4 * A * C;
            if (disc < 0) return new double[0];

            double rt_disc = Math.Sqrt(disc);
            if (disc == 0) return new double[1] { (-B + rt_disc) / 2 * A };

            return new double[2] {
                (-B + rt_disc) / 2 * A,
                (-B - rt_disc) / 2 * A,
            };
        }

        /// <summary>
        /// Quadriatic formula with the discriminant set to 0
        /// </summary>
        /// <returns></returns>
        public static double SingleSolQuadratic(double A, double B) => -B / 2 * A ;
    }
}
