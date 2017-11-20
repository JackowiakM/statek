using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Statek 
{
    public enum cząstki_typ { Brak, Przeciwnik, Pocisk, Zignoruj_grawitacje }
    public struct Cząstki_stan
    {
        public Vector2 Prędkość;
        public cząstki_typ Typ;
        public float Mnożnik_długość;
        private static Random losowo = new Random(); 

        public Cząstki_stan(Vector2 prędkość, cząstki_typ typ, float mnożnik_długość = 1f)
        {
            Prędkość = prędkość;
            Typ = typ;
            Mnożnik_długość = mnożnik_długość; 
        }

        public static Cząstki_stan pobierz_losowe(float prędkość_min, float prędkość_max)
        {
            var stan = new Cząstki_stan();
            stan.Prędkość = losowo.następny_wektor(prędkość_min, prędkość_max);
            stan.Typ = cząstki_typ.Brak;
            stan.Mnożnik_długość = 1;

            return stan;
        }
        public static void update_cząstka(Cząstki_menadżer<Cząstki_stan>.cząstka cząstka) 
        {
            var pręd = cząstka.stan.Prędkość;
            float szykość = pręd.Length();

            Vector2.Add(ref cząstka.pozycja, ref pręd, out cząstka.pozycja);

            float alfa = Math.Min(1, Math.Min(cząstka.PercentLife * 2, szykość * 1f));
            alfa *= alfa;

            cząstka.kolor.A = (byte)(255 * alfa);

            if (cząstka.stan.Typ == cząstki_typ.Pocisk)
            {
                cząstka.skala.X = cząstka.stan.Mnożnik_długość * Math.Min(Math.Min(1f, 0.1f * szykość + 0.1f), alfa);
            }
            else
            {
                cząstka.skala.X = cząstka.stan.Mnożnik_długość * Math.Min(Math.Min(1f, 0.2f * szykość + 0.1f), alfa);
            }

            cząstka.orientacja = pręd.do_kąta();

            var pos = cząstka.pozycja;
            int width = (int)Start.rozmiar_ekranu.X;
            int height = (int)Start.rozmiar_ekranu.Y;

            if (pos.X < 0)
            {
                pręd.X = Math.Abs(pręd.X);
            }
            else if (pos.X > width)
            {
                pręd.X = -Math.Abs(pręd.X);
            }
            if (pos.Y < 0)
            {
                pręd.Y = Math.Abs(pręd.Y);
            }
            else if (pos.Y > height)
            {
                pręd.Y = -Math.Abs(pręd.Y);
            }

            if (cząstka.stan.Typ != cząstki_typ.Zignoruj_grawitacje)
            {
                foreach (var czarna_dziura in Jednostka_menadżer.Czarne_dziury)
                {
                    var dPos = czarna_dziura.pozycja - pos;
                    float distance = dPos.Length();
                    var n = dPos / distance;
                    pręd += 10000 * n / (distance * distance + 10000);

                    if (distance < 400)
                    {
                        pręd += 45 * new Vector2(n.Y, -n.X) / (distance + 100);
                    }
                }
            }
            if (Math.Abs(pręd.X) + Math.Abs(pręd.Y) < 0.00000000001f)
            {
                pręd = Vector2.Zero;
            }
            else if (cząstka.stan.Typ == cząstki_typ.Przeciwnik)
            {
                pręd *= 0.94f;
            }
            else
            {
                pręd *= 0.96f + Math.Abs(pos.X) % 0.04f;	
            }
            cząstka.stan.Prędkość = pręd;
        }
    }
}
