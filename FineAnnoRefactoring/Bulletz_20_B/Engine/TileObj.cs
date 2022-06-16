using Aiv.Fast2D;
using OpenTK;

namespace ProgettoFinale
{
    class TileObj: GameObject
    {
        private int xOff;
        private int yOff;
        public bool IsSearchable;
        public TileObj(string textureName, 
            int tOffX, int tOffY,
            int posX, int posY,
            int width, int height) : base(textureName)
        {
            sprite = new Sprite(Game.PixelsToUnits(width*2), Game.PixelsToUnits(height*2));
            sprite.position.X = Game.PixelsToUnits(posX*2);
            sprite.position.Y = Game.PixelsToUnits(posY*2);
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            xOff = tOffX;
            yOff = tOffY;
            //TEST
            IsActive = false;
            //IsActive = true;
        }

        public override void Draw()
        {
           if (IsActive)
            {
                sprite.DrawTexture(texture, xOff, yOff,16,16);
            }
        }
    }
}
