using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Statek
{
    public static class Prędkość_math
    {
        public static Vector2 z_krzywej(float kąt, float wielkość) 
        {
            return wielkość * new Vector2((float)Math.Cos(kąt), (float)Math.Sin(kąt));
        }
    }
}
