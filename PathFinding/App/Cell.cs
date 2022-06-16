using System;
using System.Collections.Generic;
using System;
using Aiv.Fast2D;

namespace F2DPath
{
    class Cell
    {
        private int posX;
        private int posY;
        private int row;
        private int col;
        private Sprite sprite;
        private Texture texture;

        public Cell(int posX, int posY, int row, int col, string imageName)
        {
            this.posX = posX;
            this.posY = posY;
            this.row = row;
            this.col = col;
            
            SetImage(imageName);
            sprite = new Sprite(texture.Width, texture.Height);
            sprite.position.X = this.posX;
            sprite.position.Y = this.posY;
        }

        public void Draw()
        {
            sprite.DrawTexture(texture);
        }

        public bool IsAtPos(int row, int col)
        {
            return this.row == row && this.col == col;
        }

        public void SetImage(string image)
        {
            texture = TextureCache.Named(image);
        }
    }
}
