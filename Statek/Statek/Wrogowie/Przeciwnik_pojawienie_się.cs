using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Statek
{
    class Przeciwnik_pojawienie_się
    {
        static Random losowo = new Random(); 
        static float szansza = 60;
        static float szansa_czarna_dziura = 600;

        public static void Update()
        {
            if (!Gracz_status.czy_koniec_gry)
            {
                if (!Gracz_statek.Stan.czy_nie_żyje && Jednostka_menadżer.zlicz < 200)
                {
                    if (losowo.Next((int)szansza) == 0)
                    {
                        Jednostka_menadżer.dodaj(Przeciwnik.stworz_wrog1(pozycja_przeciwnika()));
                    }
                    if (losowo.Next((int)szansza) == 0)
                    {
                        Jednostka_menadżer.dodaj(Przeciwnik.stworz_wrog2(pozycja_przeciwnika()));
                    }
                    if (Jednostka_menadżer.Czarne_dziury_zlicz < 2 && losowo.Next((int)szansa_czarna_dziura) == 0)
                    {
                        Jednostka_menadżer.dodaj(new Czarna_dziura(pozycja_przeciwnika()));
                    }
                }

                if (szansza > 20)
                {
                    szansza -= 0.005f;
                }
            }
        }
        
        private static Vector2 pozycja_przeciwnika()
        {
            Vector2 poz;
            do
            {
                poz = new Vector2(losowo.Next((int)Start.rozmiar_ekranu.X), losowo.Next((int)Start.rozmiar_ekranu.Y));
            }

            while (Vector2.DistanceSquared(poz, Gracz_statek.Stan.pozycja) < 250 * 250);

            return poz;
        }

        public static void Reset()
        {
            szansza = 60;
        }
    }
}
