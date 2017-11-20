using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Statek
{
    class Czarna_dziura : Jednostka
    {
        private static Random losowo = new Random();
        private float kąt = 0; 
        private int punkty_uderzenia = 10; 

        public Czarna_dziura(Vector2 Pozycja)
        {
            obraz = Tekstury.Czarna_dziura;
            pozycja = Pozycja;
            promień = obraz.Width / 2f;
        }
        
        public void czy_zestrzelony()
        {
            punkty_uderzenia--;
            if (punkty_uderzenia <= 0)
            {
                czy_brak = true; 
            }

            const int liczba_cząstek = 150;
            float odcień = (float)((3 * Start.czas.TotalGameTime.TotalSeconds) % 6);
            float start = losowo.następny_float(0, MathHelper.TwoPi / liczba_cząstek); 
            Color kolor = Kolor_zmiana.zmiana_do_koloru(odcień, 0.25f, 1);

            for (int i = 0; i < liczba_cząstek; i++)
            {
                Vector2 pręd = Prędkość_math.z_krzywej(MathHelper.TwoPi * i / liczba_cząstek + start, losowo.następny_float(8, 16));
                Vector2 poz = pozycja + 2f * pręd;
                var state = new Cząstki_stan()
                {
                    Prędkość = pręd,
                    Mnożnik_długość = 1,
                    Typ = cząstki_typ.Zignoruj_grawitacje
                };

                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.laser, poz, kolor, 90, 1.5f, state);
            }
        }
        public override void Update() 
        {
            var jednostki = Jednostka_menadżer.pobierz_jednostki(pozycja, 250);

            foreach (var Jednostki in jednostki)
            {
                if (Jednostki is Przeciwnik && !(Jednostki as Przeciwnik).czy_aktywny)
                {
                    Jednostki.Prędkość += (Jednostki.pozycja - pozycja).skaluj_do(0.3f);
                }
                else
                {
                    var Poz = pozycja - Jednostki.pozycja;
                    var długość = Poz.Length();

                    Jednostki.Prędkość += Poz.skaluj_do(MathHelper.Lerp(2, 0, długość / 250f));
                }
            }

            if ((Start.czas.TotalGameTime.Milliseconds / 250) % 2 == 0)
            {
                Vector2 pręd = Prędkość_math.z_krzywej(kąt, losowo.następny_float(12, 15));
                Color kolor = Kolor_zmiana.zmiana_do_koloru(5, 0.5f, 0.8f); 
                Vector2 poz = pozycja + 2f * new Vector2(pręd.Y, -pręd.X) + losowo.następny_wektor(4, 8);
                var stan = new Cząstki_stan()
                {
                    Prędkość = pręd,
                    Mnożnik_długość = 1,
                    Typ = cząstki_typ.Przeciwnik
                };

                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.laser, poz, kolor, 190, 1.5f, stan);
            }

            kąt -= MathHelper.TwoPi / 50f;
        }
        public void zabij()
        {
            punkty_uderzenia = 0;
            czy_zestrzelony();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            float skala = 1 + 0.1f * (float)Math.Sin(10 * Start.czas.TotalGameTime.TotalSeconds);
            spritebatch.Draw(obraz, pozycja, null, kolor, orientacja, rozmiar / 2f, skala, 0, 0);
        }
    }
}
