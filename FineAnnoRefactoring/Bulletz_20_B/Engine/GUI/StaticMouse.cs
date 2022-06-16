using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{

    static class StaticMouse
    {
        private static Sprite sprite;
        private static Texture texture;
        private static bool IsActive;

        public static void Init()
        {
            texture = new Texture("Assets/Arrow_blue.png");
            sprite = new Sprite(Game.PixelsToUnits(32), Game.PixelsToUnits(32));

            IsActive = true;
            sprite.pivot = Vector2.Zero;
        }

        public static void Update()
        {
            sprite.position = CameraMgr.MainCamera.position - CameraMgr.MainCamera.pivot + Game.Win.mousePosition;
        }
        public static void Draw()
        {
            if (IsActive) sprite.DrawTexture(texture, 0, 0, 48, 48);
        }
        public static void ChangeTexture(bool change)
        {

            if (!change)
            {
                texture = GfxMgr.GetTexture("cursorRed");
            }
            else
            {
                texture = GfxMgr.GetTexture("cursorBlue");
            }
        }
    }
}
