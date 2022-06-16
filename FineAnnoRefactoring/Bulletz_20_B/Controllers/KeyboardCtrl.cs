using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    class KeyboardCtrl : Controller
    {
        public KeyboardCtrl(int controllerIndex) : base(controllerIndex)
        {
        }

        public override float GetHorizontal()
        {
            float direction = 0;

            if (Game.Win.GetKey(KeyCode.D))
            {
                direction = 1;
            }
            else if (Game.Win.GetKey(KeyCode.A))
            {
                direction = -1;
            }

            return direction;
        }

        public override float GetVertical()
        {
            float direction = 0;

            if (Game.Win.GetKey(KeyCode.W))
            {
                direction = -1;
            }
            else if (Game.Win.GetKey(KeyCode.S))
            {
                direction = 1;
            }

            return direction;
        }

        public override bool Search()
        {
            return Game.Win.GetKey(KeyCode.S);
        }

        public override bool Transform()
        {
            return Game.Win.GetKey(KeyCode.T);
        }
        public override bool ReverseTransformation()
        {
            return Game.Win.GetKey(KeyCode.R);
        }
    }
}
