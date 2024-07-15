using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial.Shapes
{
    internal abstract class Polygon: Shape
    {
        /// <summary>
        /// Struct representing the edge of a polygon. Edges are directional in a way that affects collision physics.
        /// By default the normal of an Edge (representing a vector from the inside to the outside of the polygon) is defined as
        /// <code>new VectorXY(-Plane.Y, Plane.X);</code> (a 90 degree rotation counter clockwise of the vector from P1 to P2).
        /// </summary>
        public struct Edge
        {
            public required PointXY P1;
            public required PointXY P2;

            public VectorXY Plane { get; init; }
            public VectorXY Normal { get; init; }
            public bool AxisAligned { get; init; }
            public Axis? Alignment { get; init; }
            public double? Gradient { get; init; }

            [SetsRequiredMembers]
            public Edge(PointXY p1, PointXY p2)
            {
                P1 = p1;
                P1 = p2;

                double dX = p1.X - p2.X;
                double dY = p1.Y - p2.Y;

                if (p1.X == p2.X)
                {
                    AxisAligned = true;
                    Alignment = Axis.X;
                    Gradient = 0;
                    Plane = VectorXY.UnitX;
                    Normal = VectorXY.UnitY;
                }
                if (p1.Y == p2.Y)
                {
                    AxisAligned = true;
                    Alignment = Axis.Y;
                    Plane = VectorXY.UnitY;
                    Normal = VectorXY.UnitX;
                }
                else
                {
                    Gradient = dY / dX;
                    Plane = new VectorXY(dX, dY).Normalize();
                    Normal = new VectorXY(-Plane.Y, Plane.X);
                }
            }
        }

        /// <summary>Get's an array of vertices defining the polygon.
        /// Each vertex follows on from it's counterclockwise neighbour, so that
        /// the array describes the vertices as they wind clockwise around the polygon.</summary>
        /// <remarks>The array typically starts at a point on the bottom of the polygon for efficiency reasons.</remarks>
        /// <returns>PointXY[] vertices</returns>
        public abstract PointXY[] GetVertices();

        public Edge[] GetEdges()
        {
            PointXY[] verts = GetVertices();
            Edge[] edges = new Edge[verts.Length];

            for (int i = 1; i < verts.Length; i++)
            {
                edges[0] = new Edge(verts[i - 1], verts[i]);
            }
            edges[^1] = new Edge(verts[^1], verts[0]);

            return edges;
        }

        public override Collision? GetCollision(Polygon p, VectorXY moveVec)
        {
            Collision? collision1 = checkVerticesAgainstEdges(GetVertices(), p.GetEdges(), moveVec, double.PositiveInfinity);
            Collision? collision2 = checkVerticesAgainstEdges(p.GetVertices(), GetEdges(), moveVec, collision1 != null? ((Collision)collision1).Time : double.PositiveInfinity);

            return collision2??collision1;
        }

        public override Collision? GetCollision(Circle c, VectorXY moveVec)
        {
            return c.GetCollision(this, -moveVec);
        }

        private Collision? checkVerticesAgainstEdges(PointXY[] vertices, Edge[] edges, VectorXY moveVec, double minDist)
        {
            double collision_dist = minDist;
            double collision_vel = 0;
            Edge? colliding_edge = null;

            foreach (PointXY vert in GetVertices())
            {
                foreach (Edge edge in edges)
                {
                    double dist;
                    double vel;

                    if (edge.AxisAligned)
                    {
                        if (edge.Alignment == Axis.X)
                        {
                            dist = edge.P1.X - vert.X;
                            vel = moveVec.X;

                        }
                        else
                        {
                            dist = edge.P1.Y - vert.Y;
                            vel = moveVec.Y;
                        }
                    }
                    else
                    {
                        PointXY? intersection = GeometryUtil.FindIntersection(vert, vert + moveVec, edge.P1, edge.P2);
                        if (intersection == null) continue;

                        dist = vert.Dist((PointXY)intersection);
                        vel = moveVec.Length();
                    }

                    if (dist < vel && dist < collision_dist)
                    {
                        collision_dist = dist;
                        collision_vel = vel;
                        colliding_edge = edge;
                    }
                }
            }

            if (colliding_edge != null)
            {
                return new Collision(collision_vel == 0 ? 0 : collision_dist / collision_vel, ((Edge)colliding_edge).Plane, ((Edge)colliding_edge).Normal);
            }

            return null;
        }
    }
}
