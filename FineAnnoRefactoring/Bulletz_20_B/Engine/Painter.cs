using System;
using Aiv.Fast2D;

namespace ProgettoFinale
{
    class Painter
    {

        private static void SetPoint(byte[] bitmap, int w, int x, int y)
        {
            int v = (y * w + x) * 4;
            bitmap[v + 0] = 255;
            bitmap[v + 1] = 0;
            bitmap[v + 2] = 0;
            bitmap[v + 3] = 255;
        }

        public static void DrawRect(int posX, int posY, int w, int h)
        {
            Texture texture = new Texture(w, h);
            byte[] bitmap = new byte[w * h * 4];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {

                    if (x == 0 || y == 0 || x == texture.Width - 1 || y == texture.Height - 1)
                    {
                        SetPoint(bitmap, w, x, y);
                    }
                }

            }

            texture.Update(bitmap);
            Sprite sprite = new Sprite(w, h);
            sprite.position.X = posX - (w / 2);
            sprite.position.Y = posY - (h / 2);

            sprite.DrawTexture(texture);
        }

        static public void DrawCircle(int centerX, int centerY, int ray)
        {
            int w = ray * 2 + 1; //+1 to avoid circle drawn on the edge of the texture causing array index out of bound on bitmap array
            int h = ray * 2 + 1; //+1 to avoid circle drawn on the edge of the texture causing array index out of bound on bitmap array
            Texture texture = new Texture(w, h);
            byte[] bitmap = new byte[w * h * 4];

            int x = 0, y = ray;
            int d = 3 - 2 * ray;
            int cX = w / 2;
            int cY = h / 2;

            while (y >= x)
            {
                SetPoint(bitmap, w, cX + x, cY + y);
                SetPoint(bitmap, w, cX - x, cY + y);
                SetPoint(bitmap, w, cX + x, cY - y);
                SetPoint(bitmap, w, cX - x, cY - y);
                SetPoint(bitmap, w, cX + y, cY + x);
                SetPoint(bitmap, w, cX - y, cY + x);
                SetPoint(bitmap, w, cX + y, cY - x);
                SetPoint(bitmap, w, cX - y, cY - x);

                if (d > 0)
                {
                    d = d + 4 * (x - y) + 10;
                    y--;
                    x++;
                }
                else
                {
                    d = d + 4 * x + 6;
                    x++;
                }

            }

            texture.Update(bitmap);
            Sprite sprite = new Sprite(w, h);
            sprite.position.X = centerX - (w / 2);
            sprite.position.Y = centerY - (h / 2);

            sprite.DrawTexture(texture);
        }
    }


}
