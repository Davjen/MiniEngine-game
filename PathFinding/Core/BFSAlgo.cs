using System;
using System.Collections.Generic;
using System.Text;

namespace Graph02
{
    class BFSAlgo
    {
        public static NodePath Iterative(Node root)
        {
            NodePath visited = new NodePath();            //
            Queue<Node> toVisit = new Queue<Node>();      //
            visited.Add(root);                            //
            toVisit.Enqueue(root);                        //
            while (toVisit.Count > 0)                     // |N|
            {
                Node parent = toVisit.Dequeue();              // 
                foreach(Node eachChild in parent.Edges)       // |E|
                {
                    if (!visited.Has(eachChild))              // 
                    {
                        visited.Add(eachChild);               // 
                        toVisit.Enqueue(eachChild);           // 
                    }
                }
            }
            return visited; //O(N + E)
        }
    }
}
