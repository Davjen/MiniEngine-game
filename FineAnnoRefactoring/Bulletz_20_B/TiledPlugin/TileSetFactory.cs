using System;

namespace ProgettoFinale
{
    class TileSetFactory
    {
        public static TileSet Create(int tileW, int tileH, string tsImgPath, int tsImgWidth, int tsImgHeigh)
        {
            int rows = tsImgHeigh / tileH;
            int cols = tsImgWidth / tileW;

            TileSet result = new TileSet(rows, cols, tileW, tileH, tsImgPath);

            int offX = 0;
            int offY = 0;
            int id = 1;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    TileType t = new TileType(id, tsImgPath, tileW, tileH, offX, offY);
                    result.Set(row, col, t);
                    offX += tileW;
                    id++;
                }
                offX = 0;
                offY += tileH;
            }
            
            return result;
        }
    }
}
 