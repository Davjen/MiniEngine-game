using Graph02;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    class World
    {
        
        private int[,] grid;
        private int Rows;
        private int Cols;

        private GridGraph graph;
        private Layer layer;
        private int busyWeight = 100;



        //public void Init(int r, int c)
        public void Init(Layer aLayer)
        {
            layer = aLayer;
            grid = new int[layer.Grid.Rows, layer.Grid.Cols];

            Rows = layer.Grid.Rows;
            Cols = layer.Grid.Cols;

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    grid[y, x] = 1;
                    TileInstance inst = layer.Grid.At(y * Cols + x);
                    if (inst != null)
                    {
                        grid[y, x] = 100;
                    }
                }
            }

            graph = new GridGraph(grid);

        }

        public Vector2 ScanLimits(Vector2 position, int direction = 1)
        {
            Node startingPosition = GetNodeAtPosition(position);

            int startR = startingPosition.Position.row;
            int startC = startingPosition.Position.col;
            int r = startingPosition.Position.row;
            int c = startingPosition.Position.col;
            do
            {
                r += 1 * direction;
            } while (grid[r, startC] != 100);

            do
            {
                c += 1 * direction;
            } while (grid[startR, c] != 100);

            return new Vector2(c, r);

        }

        private void GetRandomFreeNodeCoord(out int r, out int c)
        {
            do
            {
                r = RandomGenerator.GetRandomInt(0, Rows);
                c = RandomGenerator.GetRandomInt(0, Cols);
            } while (grid[r, c] != 1);
        }

        private void GetRandomFreeNodeCoord(out int r, out int c, Vector2 min, Vector2 max)
        {
            r = RandomGenerator.GetRandomInt((int)min.Y + 1, (int)max.Y);
            c = RandomGenerator.GetRandomInt((int)min.X + 1, (int)max.X);
        }
        public Node GetRandomFreeNode(Vector2 min, Vector2 max)
        {
            int randR;
            int randC;
            GetRandomFreeNodeCoord(out randR, out randC, min, max);

            return graph.NodeAt(randR, randC);
        }

        public void ChangeWeight(Node n,int weight)
        {
            grid[n.Position.row, n.Position.col] = weight;
        }
        public bool CheckIfEmpty(Vector2 pos, out Node goTo)
        {
            //RISCRIVERE MEGLIO STA CANATA
            Node selectedNode = GetNodeAtPosition(pos);
            goTo = selectedNode;
            if (selectedNode != null)
            {

                if (grid[selectedNode.Position.row, selectedNode.Position.col] != 1)
                {
                    
                    goTo = null;
                    return false;
                }
            }

            if (goTo == null)
            {
                return false;
            }

            return true;
        }
        public bool CheckIfEmpty(Vector2 pos)
        {

            Node selectedNode = GetNodeAtPosition(pos);
            if (selectedNode != null)
            {
                if (grid[selectedNode.Position.row, selectedNode.Position.col] != 1)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
            return true;
        }

        public Node GetRandomFreeNode()
        {
            int randR;
            int randC;
            GetRandomFreeNodeCoord(out randR, out randC);

            return graph.NodeAt(randR, randC);
        }

        public Node GetNodeAtPosition(Vector2 position)
        {

            int x = (int)position.X;
            int y = (int)position.Y;

            if (x < 0 || x >= Cols || y < 0 || y >= Rows)
                return null;

            Node n = graph.NodeAt(y, x);

            return n;
        }

        public static Vector2 GetNodePosition(Node n)
        {
            return new Vector2(n.Position.col + 0.5f, n.Position.row + 0.5f);
        }
    }
}
