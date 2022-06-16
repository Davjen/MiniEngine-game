using Aiv.Audio;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    class Items : GameObject
    {
        public Items(string textureName,Vector2 pos ,DrawLayer layer = DrawLayer.Playground, float width = 0, float height = 0) : base(textureName, layer, Game.PixelsToUnits(24), Game.PixelsToUnits(24))
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Items;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            Position = pos;

            IsActive = true;            
    }



        public override void Draw()
        {
            if(IsActive)
            sprite.DrawTexture(texture, 0, 0, 16, 16);
        }
    }
}
