using Graph02;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    struct NodeInfo
    {
        public Node Node;
        public Vector2 Position;
        public int Index;

        public void SetNode(Node node, int index)
        {
            Node = node;
            Index = index;
            Position = World.GetNodePosition(node);
        }

    }
}
