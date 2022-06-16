using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    class Background : IDrawable
    {
        protected Sprite Sky;
        protected Sprite Sky2;

        protected Texture[] textures;
        protected Sprite[] sprites;
        protected float[] positionsY;

        //private Sprite tail;
        private Texture texture;

        private DrawLayer layer;

        public DrawLayer Layer { get { return layer; } }


        public Background()
        {
            layer = DrawLayer.Background;

            texture = new Texture("Assets/sky.png");
            Sky = new Sprite(Game.PixelsToUnits(texture.Width), Game.PixelsToUnits(texture.Height));
            Sky.Camera = CameraMgr.GetCamera("Sky");
            Sky.position.Y = 3.8f;

            Sky2 = new Sprite(Game.PixelsToUnits(texture.Width), Game.PixelsToUnits(texture.Height));
            Sky2.Camera = Sky.Camera;
            Sky2.position.Y = Sky.position.Y;
            Sky2.position.X = Sky.Width;

            textures = new Texture[4];
            sprites = new Sprite[textures.Length * 2];

            positionsY = new float[] { 4.5f, 1.5f, 3f, 2.5f };

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = GfxMgr.GetTexture("Bg_" + i);

                sprites[i] = new Sprite(Game.PixelsToUnits(textures[i].Width), Game.PixelsToUnits(textures[i].Height));
                sprites[i].position.Y = positionsY[i];

                int cloneIndex = i + textures.Length;

                sprites[cloneIndex] = new Sprite(Game.PixelsToUnits(textures[i].Width), Game.PixelsToUnits(textures[i].Height));
                sprites[cloneIndex].position.Y = positionsY[i];
                sprites[cloneIndex].position.X = sprites[i].Width;

                if (i < textures.Length - 1)
                {
                    sprites[i].Camera = CameraMgr.GetCamera("Bg_"+i);
                }

                sprites[cloneIndex].Camera = sprites[i].Camera;
            }

            
           

            //tail = new Sprite(Game.Win.Width, Game.Win.Height);
        }

        //public void Update()
        //{
        //    head.position.X += speed * Game.DeltaTime;

        //    if (head.position.X < -head.Width)
        //    {
        //        head.position.X += head.Width;
        //    }

        //    //tail.position.X = head.position.X + head.Width;
        //}


        public void Draw()
        {
            Sky.DrawTexture(texture);
            Sky2.DrawTexture(texture);
            for (int i = 0; i < textures.Length; i++)
            {
                sprites[i].DrawTexture(textures[i]);
                sprites[i + textures.Length].DrawTexture(textures[i]);
            }
            //tail.DrawTexture(texture);
        }
    }
}
