using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    abstract class Collider
    {
        public Vector2 Offset;

        public Vector2 Position { get { return RigidBody.Position + Offset; } }

        public RigidBody RigidBody;

        public Collider(RigidBody owner)
        {
            Offset = Vector2.Zero;
            RigidBody = owner;
        }

        public abstract bool Collides(Collider aCollider, ref Collision collisionInfo);
        public abstract bool Collides(CircleCollider circle, ref Collision collisionInfo);
        public abstract bool Collides(BoxCollider box, ref Collision collisionInfo);
        public abstract bool Collides(CompoundCollider compound, ref Collision collisionInfo);
        public abstract bool Contains(Vector2 point);

        public abstract void Draw();

        public virtual void Destroy()
        {
            RigidBody = null;
        }

    }
}
