using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes
{
    public enum Axis
    {
        X,
        Y
    }

    /// <summary>
    /// Mutually exclusive group stored in seperate lists in world.
    /// SpatialObjects can only have one SpatialType
    /// </summary>
    public enum SpatialType
    {
        StaticEnv,
        DynamicEnv,
        Projectile,
        Living,
        Particle,
    }

    /// <summary>
    /// SpatialObjects can belong to multiple spatial groups.
    /// Each SpatialGroup is a subtype of a SpatialType.
    /// </summary>
    public enum SpatialGroup
    {
        Enemy,
    }

    public struct PointXY
    {
        public required readonly double X { get; init; }
        public required readonly double Y { get; init; }

        [SetsRequiredMembers]
        public PointXY(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static readonly PointXY Origin = new PointXY(0, 0);

        public static PointXY operator +(PointXY a, PointXY b) => new PointXY(a.X + b.X, a.Y + b.Y);
        public static PointXY operator -(PointXY a, PointXY b) => new PointXY(a.X - b.X, a.Y - b.Y);
        public static PointXY operator +(PointXY a, VectorXY b) => a.AddVec(b);
        public static PointXY operator -(PointXY a, VectorXY b) => a.AddVec(-b);

        public VectorXY VecTo(PointXY to) => new VectorXY(to.X - this.X, to.Y - this.Y);
        public PointXY AddVec(VectorXY vec) => new PointXY(this.X + vec.X, this.Y + vec.Y);
        public double Dist(PointXY to) => Math.Sqrt(Math.Pow(this.X - to.X, 2) + Math.Pow(this.Y - to.Y, 2));
        public double DistSquared(PointXY to) => Math.Pow(this.X - to.X, 2) + Math.Pow(this.Y - to.Y, 2);
        public bool InRange(PointXY to, double dist) => (this.X - to.X) * (this.X - to.X) + (this.Y - to.Y) * (this.Y - to.Y) < dist * dist;
        public PointXY MidpointTo(PointXY to) => new PointXY((X + to.X) / 2, (Y + to.Y) / 2);
        public override string ToString()
        {
            new PointXY { X = 10, Y = 10 };
            return $"({X}, {Y})";
        }
    }

    public struct VectorXY
    {
        public required double X;
        public required double Y;

        [SetsRequiredMembers]
        public VectorXY(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static readonly VectorXY Zero = new VectorXY(0, 0);
        public static readonly VectorXY UnitX = new VectorXY(1, 0);
        public static readonly VectorXY UnitY = new VectorXY(0, 1);

        public static VectorXY operator -(VectorXY a) => new VectorXY(-a.X, -a.Y);
        public static VectorXY operator +(VectorXY a, VectorXY b) => new VectorXY(a.X + b.X, a.Y + b.Y);
        public static VectorXY operator -(VectorXY a, VectorXY b) => new VectorXY(a.X - b.X, a.Y - b.Y);
        public static VectorXY operator *(VectorXY a, double b) => new VectorXY(a.X * b, a.Y * b);
        public static VectorXY operator *(double a, VectorXY b) => b * a;
        public static VectorXY operator /(VectorXY a, double b) => new VectorXY(a.X / b, a.Y / b);

        public bool IsZero() => X == 0 && Y == 0;

        public double Length() => Math.Sqrt(X * X + Y * Y);
        public double LengthSquared() => X * X + Y * Y;
        public VectorXY Normalize()
        {
            double length = Length();
            if (length == 0)
            {
                return Zero;
            }
            return this / length;
        }

        public VectorXY Normal() =>  new VectorXY(-Y, X).Normalize();

        public double DotProduct(VectorXY vec) =>  X * vec.X + Y * vec.Y;

        public double Gradient() => Y/X;

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public struct LineSegment
    {
        public required PointXY P1 { get; init; }
        public required PointXY P2 { get; init; }

        [SetsRequiredMembers]
        public LineSegment(PointXY p1, PointXY p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }

    public struct Collision
    {
        public double Time;
        public VectorXY Plane;
        public VectorXY Normal;

        public Collision(double time, VectorXY plane, VectorXY normal)
        {
            Time = time;
            Plane = plane;
            Normal = normal;
        }
    }
}