namespace ProgettoFinale
{
    class TileInstance
    {
        public TileType Type { get; }
        public int PosX { get; }
        public int PosY { get; }

        public TileInstance(TileType type, int xPos, int yPos)
        {
            this.Type = type;
            this.PosX = xPos;
            this.PosY = yPos;
        }

        public override string ToString()
        {
            return Type + " {"+ PosX +","+ PosY + "}";
        }
    }
}