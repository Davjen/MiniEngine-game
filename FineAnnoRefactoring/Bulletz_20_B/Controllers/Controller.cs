using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    abstract class Controller
    {
        protected int index;

        public Controller(int controllerIndex)
        {
            index = controllerIndex;
        }

        public abstract bool Transform();
        public abstract bool ReverseTransformation();
        public abstract float GetHorizontal();
        public abstract float GetVertical();

        public abstract bool Search();
    }
}
