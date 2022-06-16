using System;

namespace ProgettoFinale
{
    class TileGridFactory
    {
        public static TileGrid Create(int rows, int cols, string csvString, TileSet ts)
        {
            TileGrid result = new TileGrid(rows, cols);
            string[] tileIds = csvString.Split(',');

            int xPos = 0;
            int yPos = 0;
            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++)
                {
                    int id = int.Parse(tileIds[row * cols + col]);
                    if (id != 0)
                    {
                        TileType type = ts.At(id - 1);
                        TileInstance inst = new TileInstance(type, xPos, yPos);
                        result.Set(row, col, inst);
                    }
                    xPos += ts.TileWidth;
                }
                xPos = 0;
                yPos += ts.TileHeight;
            }
            return result;
        }
    }
}