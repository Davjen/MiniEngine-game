using System;

namespace ProgettoFinale
{
    class TileGrid
    {
        private int rows;
        private int cols;
        private TileInstance[] tiles;
        public int Rows { get { return rows; } }
        public int Cols { get { return cols; } }

        public TileGrid(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            tiles = new TileInstance[rows * cols];
        }

        public TileInstance At(int index)
        {
            return tiles[index];
        }

        public void Set(int row, int col, TileInstance inst)
        {
            tiles[row * cols + col] = inst;
        }

        public int Size()
        {
            return tiles.Length;
        }
    }
}