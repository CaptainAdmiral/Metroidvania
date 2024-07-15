using Metroidvania.Src.Physics.Spatial.Shapes;
using Metroidvania.Src.Physics.Spatial.Types.SpatialGroup;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial
{
    using SpatialBucket = HashSet<Spatial>;
    internal class SpatialGrid
    {
        public required AABB BoundingBox { get; init; }
        public required double CellSize { get; init; }

        private SpatialBucket[,] _spatialGrid;
        private SpatialBucket _outside = new();

        private readonly int _rows;
        private readonly int _cols;


        /// <param name="aabb">Bounding box of the spatial grid. Anything outside of the bounding box will be bucketed together</param>
        [SetsRequiredMembers]
        public SpatialGrid(AABB aabb, double cellSize)
        {
            BoundingBox = aabb;
            CellSize = cellSize;

            _rows = (int) Math.Ceiling(aabb.Height / cellSize);
            _cols = (int) Math.Ceiling(aabb.Width / cellSize);

            _spatialGrid = new SpatialBucket[_rows, _cols];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _spatialGrid[i, j] = new SpatialBucket();
                }
            }
        }

        /// <returns>A hashset of all of the buckets that are at least partially within the specified bounds</returns>
        private SpatialBucket[] _getBuckets(AABB bounds)
        {
            int minRow = (int) Math.Abs(Math.Floor((bounds.MinY - BoundingBox.MinY) / CellSize));
            int maxRow = (int) Math.Abs(Math.Floor((bounds.MaxY - BoundingBox.MaxY) / CellSize));
            int minCol = (int) Math.Abs(Math.Floor((bounds.MinX - BoundingBox.MinX) / CellSize));
            int maxCol = (int) Math.Abs(Math.Floor((bounds.MaxX - BoundingBox.MaxX) / CellSize));

            List<SpatialBucket> buckets = new List<SpatialBucket>();

            for (int i = Math.Max(minRow, 0); i <= Math.Min(maxRow, _rows); i++)
            {
                for (int j = Math.Max(minCol, 0); j <= Math.Min(maxCol, _cols); j++)
                {
                    buckets.Add(_spatialGrid[i, j]);
                }
            }

            if (minRow < 0 || minCol < 0 || maxRow >= _rows || maxCol >= _cols) buckets.Add(_outside);

            return buckets.ToArray();
        }

        public void Register(Spatial obj)
        {
            foreach (SpatialBucket bucket in _getBuckets(obj.Bounds))
            {
                bucket.Add(obj);
            }
        }

        public void Unregister(Spatial obj)
        {
            foreach (SpatialBucket bucket in _getBuckets(obj.Bounds))
            {
                bucket.Remove(obj);
            }
        }

        public HashSet<Spatial> GetWithinBounds(AABB bounds)
        {
            var buckets = _getBuckets(bounds);

            var spatialSet = new HashSet<Spatial>();
            foreach (var bucket in buckets)
            {
                spatialSet.UnionWith(bucket);
            }

            return spatialSet;
        }

        /// <summary>
        /// Must be called whenever a registered object's bounding box changes to update the bucketing withing the spacial hash.
        /// </summary>
        /// <param name="obj">The spatial objects who's bounding box has changed</param>
        /// <param name="oldBounds">The old bounds of the object</param>
        /// <param name="newBounds">The new bounds of the object</param>
        public void UpdateMoved(Spatial obj, AABB oldBounds, AABB newBounds)
        {
            HashSet<SpatialBucket> oldBuckets = _getBuckets(oldBounds).ToHashSet();
            HashSet<SpatialBucket> newBuckets = _getBuckets(newBounds).ToHashSet();

            IEnumerable<SpatialBucket> toRemove = oldBuckets.Except(newBuckets);
            IEnumerable<SpatialBucket> toAdd = newBuckets.Except(oldBuckets);

            foreach (var bucket in toRemove)
            {
                bucket.Remove(obj);
            }

            foreach (var bucket in toAdd)
            {
                bucket.Add(obj);
            }
        }
    }
}