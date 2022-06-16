using Aiv.Fast2D;
using Graph02;
using System;
using System.Collections.Generic;

namespace F2DPath
{
    class Pos
    {
        public static Pos operator +(Pos a, Pos b)
        {
            return new Pos(a.X + b.X, a.Y + b.Y);
        }

        public int X { get; }
        public int Y { get; }

        public int R { get { return X; } }
        public int C { get { return Y; } }

        public Pos(int XorRow, int YorCol)
        {
            X = XorRow;
            Y = YorCol;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

    }

    class PathFinding
    {
        private Window win;
        private int[,] Grid;
        private int Rows;
        private int Cols;
        private Cell[] Cells;
        private int CellSize;
        private int CellSpace;
        private Pos StartCellsPos;

        public Pos EndCellsPos { get; }

        private GridGraph graph;
        private Node Start;
        private Node End;
        private NodePath Path;
        private float Elapsed;
        private int Index;
        private Node CurrentNode;
        private bool MouseLeftClicked;
        private List<Cell> PathCells;
        private Pos ClickedCell;

        public PathFinding(Window win)
        {
            this.win = win;

            Grid = new int[,]
            {
                { 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1 },
                { 1, 2, 2, 2, 2, 1 },
                { 1, 1, 2, 1, 2, 1 },
                { 1, 2, 2, 2, 2, 1 },
                { 1, 1, 1, 1, 1, 0 },
            };
            Rows = Grid.GetLength(0);
            Cols = Grid.GetLength(1);
            
            
            
            Cells = new Cell[Rows * Cols];

            CellSize = 40;
            CellSpace = 5;
            StartCellsPos = new Pos(10, 10);
            EndCellsPos = StartCellsPos + new Pos(Rows * (CellSize+CellSpace)- CellSpace, Cols * (CellSize + CellSpace) - CellSpace);

            Console.WriteLine(EndCellsPos);

            int posX = StartCellsPos.X;
            int posY = StartCellsPos.Y;
            
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {

                    int pos = r * Cols + c;
                    if (Grid[r, c] == 2)
                    {
                        Cells[pos] = new Cell(posX, posY, r, c, "brown");
                    } else
                    {
                        Cells[pos] = new Cell(posX, posY, r, c, "green");
                    }
                    posX += CellSize + CellSpace;
                }
                posX = 10;
                posY += CellSize + CellSpace;
            }

            PathCells = new List<Cell>();
            CalcGraphAndPath();
        }

        private void ResetGreenCells()
        {
            foreach(Cell Each in PathCells)
            {
                Each.SetImage("green");
            }
            PathCells.Clear();
        }

        private void CalcGraphAndPath()
        {
            graph = new GridGraph(Grid);
            Start = graph.NodeAt(0, 0);
            End = graph.NodeAt(5, 5);
            Path = GreedyAlgo.AStar_ShortestPath(Start, End);

            Elapsed = 0.0f;
            CurrentNode = null;
            Index = 0;
        }

        public void Input()
        {
            if (!MouseLeftClicked && win.mouseLeft)
            {
                MouseLeftClicked = true;
                if (win.mouseX < StartCellsPos.X || win.mouseX > EndCellsPos.X) return;
                if (win.mouseY < StartCellsPos.Y || win.mouseY > EndCellsPos.Y) return;
                int x = (int)win.mouseX - StartCellsPos.X;
                int cellCol = (int)(x / (CellSize+CellSpace));
                int xInCell = (cellCol * (CellSize + CellSpace) + CellSize);
                if (xInCell < x) {
                    ClickedCell = null;
                    return;
                }
                
                int y = (int)win.mouseY - StartCellsPos.Y;
                int cellRow = (int)(y / (CellSize + CellSpace));
                int yInCell = (cellRow * (CellSize + CellSpace) + CellSize);
                if (yInCell < y)
                {
                    ClickedCell = null;
                    return;
                }

                ClickedCell = new Pos(cellRow, cellCol);
            } else if (MouseLeftClicked && !win.mouseLeft)
            {
                MouseLeftClicked = false;
                ClickedCell = null;
            }
        }

        
        public void Update()
        {
            if (ClickedCell != null)
            {
                int current = Grid[ClickedCell.R, ClickedCell.C];
                if (current == 1)
                {
                    Cell cell = Cells[ClickedCell.R * Cols + ClickedCell.C];
                    cell.SetImage("brown");
                    Grid[ClickedCell.R, ClickedCell.C] = 2;
                } else
                {
                    Cell cell = Cells[ClickedCell.R * Cols + ClickedCell.C];
                    cell.SetImage("green");
                    Grid[ClickedCell.R, ClickedCell.C] = 1;
                }
                ClickedCell = null;

                CalcGraphAndPath();
                ResetGreenCells();
                return;
            }

            if (Index >= Path.Length()) return;
            Elapsed += win.deltaTime;
            if (Elapsed < 1) return;
            Elapsed = Elapsed - 1.0f;

            CurrentNode = Path.At(Index++);

            /*
            foreach (Cell Each in Cells)
            {
                if (Each.IsAtPos(CurrentNode.Position.row, CurrentNode.Position.col))
                {
                    Each.SetImage("images/blue.png");
                    return;
                }
            }
            */

            Cell Selected = Cells[CurrentNode.Position.row * Cols + CurrentNode.Position.col];
            Selected.SetImage("blue");
            PathCells.Add(Selected);
        }

        public void Draw()
        {
            foreach (Cell Each in Cells) Each.Draw();
        }
    }
}
