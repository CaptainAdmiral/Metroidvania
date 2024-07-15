using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Metroidvania.Src.Physics.Spatial.Shapes
{
    /// <summary>
    /// Axis Aligned Bounding Box
    /// </summary>
    internal class AABB : Polygon
    {
        public double MaxX { get; private set; }
        public double MinX { get; private set; }
        public double MaxY { get; private set; }
        public double MinY { get; private set; }

        public required double Width
        {
            get => _width;
            set
            {
                _width = value;
                SetMinMax();
            }
        }
        public required double Height
        {
            get => _height;
            set
            {
                _height = value;
                SetMinMax();
            }
        }

        private double _width;
        private double _height;


        [SetsRequiredMembers]
        public AABB(PointXY point1, PointXY point2)
        {
            Position = point1.MidpointTo(point2);
            Width = Math.Abs(point1.X - point2.X);
            Height = Math.Abs(point1.X - point2.X);
        }

        [SetsRequiredMembers]
        public AABB(PointXY position, double width, double height)
        {
            Position = position;
            Width = width;
            Height = height;
        }

        /// <returns>The minimum bounding box of an array of points</returns>
        public static AABB BoundsOf(params PointXY[] points)
        {
            Debug.Assert(points.Length > 0);

            double maxX = double.MinValue;
            double maxY = double.MinValue;
            double minX = double.MaxValue;
            double minY = double.MaxValue;

            foreach (var point in points)
            {
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
            }

            return new AABB(new(maxX, maxY), new(minX, minY));
        }

        /// <returns>The minimum bounding box that contains all bounding boxes in <paramref name="bounds"/></returns>
        public static AABB BoundsOf(params AABB[] bounds)
        {
            Debug.Assert(bounds.Length > 0);

            double maxX = double.MinValue;
            double maxY = double.MinValue;
            double minX = double.MaxValue;
            double minY = double.MaxValue;

            foreach (var bound in bounds)
            {
                maxX = Math.Max(maxX, bound.MaxX);
                maxY = Math.Max(maxY, bound.MaxY);
                minX = Math.Min(minX, bound.MinX);
                minY = Math.Min(minY, bound.MinY);
            }

            return new AABB(new(maxX, maxY), new(minX, minY));
        }

        private void SetMinMax()
        {
            MaxX = Position.X + Width * 0.5;
            MinX = Position.X - Width * 0.5;
            MaxY = Position.Y + Height * 0.5;
            MinY = Position.Y - Height * 0.5;
        }
        public PointXY[] Corners()
        {
            return new PointXY[]
            {
                new(Position.X - Width * 0.5, Position.Y + Height * 0.5),
                new(Position.X + Width * 0.5, Position.Y + Height * 0.5),
                new(Position.X - Width * 0.5, Position.Y - Height * 0.5),
                new(Position.X + Width * 0.5, Position.Y - Height * 0.5),
            };
        }

        public override PointXY[] GetVertices()
        {
            return new PointXY[]
            {
                new(Position.X + Width * 0.5, Position.Y - Height * 0.5),
                new(Position.X - Width * 0.5, Position.Y - Height * 0.5),
                new(Position.X - Width * 0.5, Position.Y + Height * 0.5),
                new(Position.X + Width * 0.5, Position.Y + Height * 0.5),
            };
        }

        /// <returns>A new AABB translated by <paramref name="translation"/></returns>
        public AABB Translated(VectorXY translation)
        {
            return new(Position + translation, Width, Height);
        }

        public override bool Contains(PointXY point)
        {
            return MinX < point.X && point.X < MaxX && MinY < point.Y && point.Y < MaxY;
        }

        public override string ToString()
        {
            return $"{Position}, W:{Width}, H:{Height}";
        }

        public override void SetPosition(PointXY position)
        {
            base.SetPosition(position);
            SetMinMax();
        }

        public override Collision? GetCollision(Polygon p, VectorXY moveVec)
        {
            throw new NotImplementedException();
        }

        public override Collision? GetCollision(Circle c, VectorXY moveVec)
        {
            throw new NotImplementedException();
        }
    }
}
