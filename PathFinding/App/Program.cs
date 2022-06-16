using System;
using Aiv.Fast2D;

namespace F2DPath
{
    class Program
    {
        static void Main(string[] args)
        {
            Window Win = new Window(800, 600, "PathFinding");

            PathFinding pf = new PathFinding(Win);

            while (Win.IsOpened)
            {
                if (Win.GetKey(KeyCode.Esc))
                {
                    break;
                }

                pf.Input();

                pf.Update();

                pf.Draw();
               
                Win.Update();
            }

        }
    }
}
