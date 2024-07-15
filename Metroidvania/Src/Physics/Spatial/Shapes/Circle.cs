using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;

namespace Metroidvania.Src.Physics.Spatial.Shapes
{
    internal class Circle : Shape
    {
        public required double Radius { get; set; }

        [SetsRequiredMembers]
        public Circle(PointXY position, double radius)
        {
            Position = position;
        }

        public override string ToString()
        {
            return $"{Position}, R:{Radius}";
        }

        public override bool Contains(PointXY point) => point.DistSquared(Position) <= Math.Pow(Radius, 2);

        public override Collision? GetCollision(Polygon p, VectorXY moveVec)
        {
            if (moveVec.IsZero()) return null;
            VectorXY trans2Origin = Position.VecTo(PointXY.Origin);

            foreach (var edge in p.GetEdges())
            {
                VectorXY edgeVec = edge.P1.VecTo(edge.P2);

                double f1 = moveVec.LengthSquared();
                double vX = moveVec.X, vY = moveVec.Y;
                double pX = Position.X + trans2Origin.X;
                double pY = Position.Y + trans2Origin.Y;
                double dX = edge.P2.X - edge.P1.X;
                double dY = edge.P2.Y - edge.P1.Y;

                double vX2 = vX * vX, vY2 = vY * vY;
                double pX2 = pX * pX, pY2 = pY * pY;
                double dX2 = dX * dX, dY2 = dY * dY;

                double Al = 4 * (
                    vX2 * dX2 +
                    vY2 * dY2 +
                    2 * vX * dX * vY * dY -
                    f1 * (dX2 + dY2)
                );

                double Bl = 8 * (
                    pX * vX2 * dX +
                    pY * vY2 * dY +
                    pX * vX * vY * dY +
                    pY * vX * vY * dX -
                    f1 * (pX * dX + pY * dY)
                );

                double Cl = 4 * (
                    pX2 * vX2 +
                    pY2 * vY2 +
                    2 * (pX * dX * pY * dY) -
                    f1 * (pX2 + pY2 - Radius * Radius)
                );

                double[] l_solutions = GeometryUtil.Quadratic(Al, Bl, Cl).Where(d => d > 0 && d < 1).ToArray();
                double[] solutions = new double[l_solutions.Length];

                foreach (double l in l_solutions)
                {
                    double At = f1;
                    double Bt = 2 * (pX * vX + pY * vY + (vX * dX + vY * dY) * l);

                    double sol = GeometryUtil.SingleSolQuadratic(At, Bt);

                    if (sol > 0 && sol <= 1) solutions.Append(GeometryUtil.SingleSolQuadratic(At, Bt));
                }

                if (solutions.Length == 0) return null;

                double t = solutions.Min();

                return new Collision(t, edge.Plane, edge.Normal);
            }
            
            return null;
        }

        public override Collision? GetCollision(Circle c, VectorXY moveVec)
        {
            if (moveVec.IsZero()) return null;

            VectorXY trans2Origin = Position.VecTo(PointXY.Origin);

            PointXY pos = Position + trans2Origin;

            double A = moveVec.LengthSquared();
            double B = 2 * (pos.X * moveVec.X + pos.Y * moveVec.Y);
            double C = pos.X * pos.X + pos.Y * pos.Y + Math.Pow(Radius + c.Radius, 2);

            double[] solutions = GeometryUtil.Quadratic(A, B, C).Where(t => t > 0 && t <= 1).ToArray();
            if (solutions.Length == 0) return null;
            
            double t = solutions.Min();

            PointXY newPos = Position + t * moveVec;
            VectorXY normal = c.Position.VecTo(newPos);
            VectorXY plane = normal.Normal();

            return new Collision(t, plane, normal);
        }
    }
}
