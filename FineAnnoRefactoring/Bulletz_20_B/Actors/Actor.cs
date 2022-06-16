using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    abstract class Actor : GameObject
    {
        
        public float Speed = 3.5f;
        
        protected World worldGrid;


        public bool IsGrounded { get; protected set; }




        public Actor(string textureName,World world,DrawLayer layer = DrawLayer.Playground, float w=0, float h=0) : base(textureName, layer, w,h)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            worldGrid = world;
        }

        public Vector2 Velocity
        {
            get
            {//read
                return RigidBody.Velocity;
            }
            set
            {//write
                RigidBody.Velocity = value;
            }
        }

        public virtual void SetXVelocity(float x)
        {
            RigidBody.Velocity = new Vector2(x, RigidBody.Velocity.Y);
        }

        public virtual void SetYVelocity(float y)
        {
            RigidBody.Velocity = new Vector2(RigidBody.Velocity.X, y);
        }

        protected virtual void OnGroundTouch()
        {
            RigidBody.Velocity.Y = 0;
            IsGrounded = true;
            
        }



        public abstract void OnDie();


        public override void OnCollide(Collision collisionInfo)
        {
           
        }


        protected void OnWallCollide(Collision collisionInfo)
        {
            if(collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                //horizontal collision
                if(Position.X < collisionInfo.Collider.Position.X)
                {
                    //collision by left
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                Position = new Vector2(Position.X + collisionInfo.Delta.X, Position.Y);
                RigidBody.Velocity.X = 0;
            }
            else
            {
                //vertical collision
                if (Position.Y < collisionInfo.Collider.Position.Y)
                {
                    //collision from top
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    OnGroundTouch();
                }
                else
                {
                    RigidBody.Velocity.Y = 1;
                }

                Position = new Vector2(Position.X, Position.Y + collisionInfo.Delta.Y);

            }
        }

        public override void Update()
        {
            base.Update();
        }

    }
}
