using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ProgettoFinale
{
    class CompoundCollider : Collider
    {
        public Collider BoundingCollider;

        protected List<Collider> colliders;

        public CompoundCollider(RigidBody owner, Collider boundingCollider) : base(owner)
        {
            BoundingCollider = boundingCollider;
            colliders = new List<Collider>();
        }
        
        public virtual void AddCollider(Collider collider)
        {
            colliders.Add(collider);
        }

        protected virtual bool InnerCollidersCollide(Collider collider, ref Collision collisionInfo)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (collider.Collides(colliders[i],ref collisionInfo))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Collides(Collider aCollider, ref Collision collisionInfo)
        {
            return aCollider.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CircleCollider circle, ref Collision collisionInfo)
        {
            return (circle.Collides(BoundingCollider, ref collisionInfo) && InnerCollidersCollide(circle, ref collisionInfo));
        }

        public override bool Collides(BoxCollider box, ref Collision collisionInfo)
        {
            return (box.Collides(BoundingCollider, ref collisionInfo) && InnerCollidersCollide(box, ref collisionInfo));
        }

        public override bool Collides(CompoundCollider compound, ref Collision collisionInfo)
        {
            if (BoundingCollider.Collides(compound.BoundingCollider, ref collisionInfo))
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    for (int j = 0; j < compound.colliders.Count; j++)
                    {
                        if (colliders[i].Collides(compound.colliders[j], ref collisionInfo))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override bool Contains(Vector2 point)
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            BoundingCollider.Draw();
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].Draw();
            }
        }

        public override void Destroy()
        {
            BoundingCollider.Destroy();
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].Destroy();
            }
            base.Destroy();
        }
    }
}
