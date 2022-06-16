using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    class CircleCollider : Collider
    {
        
        public float Radius;

        public CircleCollider(RigidBody owner, float radius):base(owner)
        {
            Radius = radius;
        }

        public override bool Collides(Collider aCollider, ref Collision collisionInfo)
        {
            return aCollider.Collides(this, ref collisionInfo);
        }

        //Circle vs Circle
        public override bool Collides(CircleCollider other, ref Collision collisionInfo)
        {
            Vector2 dist = other.Position - Position;
            return dist.LengthSquared <= Math.Pow(Radius + other.Radius,2);
        }

        //Circle vs Box
        public override bool Collides(BoxCollider box, ref Collision collisionInfo)
        {
            return box.Collides(this, ref collisionInfo);
        }

        //Circle vs Compound
        public override bool Collides(CompoundCollider compound, ref Collision collisionInfo)
        {
            return compound.Collides(this, ref collisionInfo);
        }

        public override bool Contains(Vector2 point)
        {
            Vector2 distFromCenter = point - Position;

            return distFromCenter.Length <= Radius;
        }

        public override void Draw()
        {
            Vector2 pos = Position;//get absolute position (rigidBody + offset)
            Painter.DrawCircle((int)pos.X, (int)pos.Y, (int)Radius);
        }

    }
}
