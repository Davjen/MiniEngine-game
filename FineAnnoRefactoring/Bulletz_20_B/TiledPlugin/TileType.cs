namespace ProgettoFinale
{
    internal class TileType
    {
        public int Id { get; }
        public string ImagePath { get; }
        public int Width { get; }
        public int Height { get; }
        public int OffX { get; }
        public int OffY { get; }
        public TileProperties Props { get; set; }

        public TileType(int id, string tsImgPath, int tileW, int tileH, int offX, int offY)
        {
            this.Id = id;
            this.ImagePath = tsImgPath;
            this.Width = tileW;
            this.Height = tileH;
            this.OffX = offX;
            this.OffY = offY;
            Props = new TileProperties();
        }

        public override string ToString()
        {
            return "id: " + Id + " (" + OffX + "," + OffY +")";
        }
    }
}