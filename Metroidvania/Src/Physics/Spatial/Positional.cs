using Metroidvania.Src.Physics.Spatial.Shapes;
using Metroidvania.Src.Physics.Spatial.Types.PhysicsTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Physics.Spatial
{
    internal class Positional
    {
        public PointXY Position { get => GetPosition(); protected set => SetPosition(value); }
        public PointXY Anchor { get => GetAnchor(); protected set => SetAnchor(value); }

        public AABB Bounds { get; private set; } //TODO make child
        public List<Positional> Children = new();

        private PointXY? _anchor;
        private VectorXY _offset = new(0, 0);
        private Positional? _parent;

        public PointXY GetPosition()
        {
            return GetAnchor() + GetAbsoluteOffset();
        }

        public virtual void SetPosition(PointXY position)
        {
            if (_parent == null)
            {
                SetAnchor(position - _offset);
            }
            else
            {
                SetOffset((GetAnchor() + _parent.GetAbsoluteOffset()).VecTo(position));
            }
        }

        public PointXY GetAnchor()
        {
            if (_parent != null) return _parent.GetAnchor();
            Debug.Assert(_anchor != null);
            return (PointXY) _anchor;
        }

        public void SetAnchor(PointXY anchor)
        {
            Debug.Assert(_parent == null);
            _anchor = anchor;
        }

        public VectorXY GetOffset()
        {
            return _offset;
        }

        public VectorXY GetAbsoluteOffset()
        {
            if (_parent != null) return _parent.GetAbsoluteOffset() + _offset;
            return _offset;
        }

        public virtual void SetOffset(VectorXY offset)
        {
            _offset = offset;
        }

        public virtual void Move(VectorXY moveVec)
        {
            if (_parent != null)
            {
                Debug.Assert(_anchor != null);
                SetAnchor((PointXY) _anchor + moveVec);
            }
            else
            {
                SetOffset(_offset + moveVec);
            }
        }
    }
}
