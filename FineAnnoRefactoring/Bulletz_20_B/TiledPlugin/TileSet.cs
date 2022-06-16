using System;

namespace ProgettoFinale
{
    class TileSet
    {
        private int rows;
        private int cols;
        private TileType[] types;
        public string ImgPath { get; }

        public int TileWidth { get; }
        public int TileHeight { get; }

        public TileSet(int rows, int cols, int tileW, int tileH, string tsImgPath)
        {
            this.rows = rows;
            this.cols = cols;
            types = new TileType[rows * cols];
            TileWidth = tileW;
            TileHeight = tileH;
            ImgPath = tsImgPath;
        }

     
        public TileType At(int index)
        {
            return types[index];
        }

        public void Set(int row, int col, TileType t)
        {
            types[row * cols + col] = t;
        }
    }
}