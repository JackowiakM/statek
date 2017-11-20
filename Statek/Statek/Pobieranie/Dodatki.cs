using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Statek
{
    public static class Dodatki
    {
        public static Point do_punktu(this Vector2 wektor)
        {
            return new Point((int)wektor.X, (int)wektor.Y);
        }
        public static float do_kąta(this Vector2 wektor)
        {
            return (float)Math.Atan2(wektor.Y, wektor.X); 
        }
        public static float następny_float(this Random rand, float minValue, float maxValue)
        {
            return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
        }
        public static Vector2 skaluj_do(this Vector2 wektor, float długość)
        {
            return wektor * (długość / wektor.Length());
        }
        public static Vector2 następny_wektor(this Random losowe, float długość_min, float długość_max)
        {
            double theta = losowe.NextDouble() * 2 * Math.PI;
            float length = losowe.następny_float(długość_min, długość_max);
            return new Vector2(length * (float)Math.Cos(theta), length * (float)Math.Sin(theta));
        }
    }
}
