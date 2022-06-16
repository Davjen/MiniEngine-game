using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ProgettoFinale
{
    class BoxCollider : Collider
    {
        protected float halfWidth;
        protected float halfHeight;
        public float Height { get { return halfHeight * 2; } }
        public float Width { get { return halfWidth * 2; } }

        public BoxCollider(RigidBody owner, float w, float h) : base(owner)
        {
            halfWidth = w * 0.5f;
            halfHeight = h * 0.5f;
        }

        public override bool Collides(Collider aCollider, ref Collision collisionInfo)
        {
            return aCollider.Collides(this, ref collisionInfo);
        }

        //Box vs Circle
        public override bool Collides(CircleCollider circle, ref Collision collisionInfo)
        {
            float deltaX = circle.Position.X - Math.Max(
                                                Position.X - halfWidth,
                                                Math.Min(circle.Position.X, Position.X + halfWidth)
                                               );
            float deltaY = circle.Position.Y - Math.Max(
                                                Position.Y - halfHeight,
                                                Math.Min(circle.Position.Y, Position.Y + halfHeight)
                                               );
            // RAD(x2 + y2) < r
            // x2 + y2 < r2
            return (deltaX * deltaX + deltaY * deltaY) < (circle.Radius * circle.Radius);
        }

        //Box vs Box
        public override bool Collides(BoxCollider box, ref Collision collisionInfo)
        {
            collisionInfo.Type = CollisionType.None;

            float deltaX = Math.Abs(box.Position.X - Position.X) - (halfWidth + box.halfWidth);
            if (deltaX > 0)
            {
                return false;
            }
            
            float deltaY = Math.Abs(box.Position.Y - Position.Y) - (halfHeight + box.halfHeight);
            if (deltaY > 0)
            {
                return false;
            }

            collisionInfo.Type = CollisionType.RectsIntersection;
            collisionInfo.Delta = new Vector2(-deltaX, -deltaY);

            return true;
        }

        

        public override bool Contains(Vector2 point)
        {
            return
                point.X >= Position.X - halfWidth &&
                point.X <= Position.X + halfWidth &&
                point.Y >= Position.Y - halfHeight &&
                point.Y <= Position.Y + halfHeight;
        }

        public override void Draw()
        {
            Vector2 pos = Position;//get absolute position (rigidBody + offset)
            Painter.DrawRect((int)pos.X, (int)pos.Y, (int)Width, (int)Height);
        }

        public override bool Collides(CompoundCollider compound, ref Collision collisionInfo)
        {
            return compound.Collides(this, ref collisionInfo);
        }
    }
}
