using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Statek
{
    static class Jednostka_menadżer
    {
        static List<Jednostka> jednostki = new List<Jednostka>();
        static List<Przeciwnik> przeciwnicy = new List<Przeciwnik>(); 
        static List<Pocisk> pociski = new List<Pocisk>();
        static List<Czarna_dziura> czarne_dziury = new List<Czarna_dziura>();

        public static IEnumerable<Czarna_dziura> Czarne_dziury { get { return czarne_dziury; } }

        static bool czy_update;
        static List<Jednostka> dodane_jednostki = new List<Jednostka>();

        public static int zlicz { get { return jednostki.Count; } }
        public static int Czarne_dziury_zlicz { get { return czarne_dziury.Count; } }


        public static void dodaj(Jednostka jednostka)
        {
            if (!czy_update)
            {
                dodaj_jednostke(jednostka);
            }
            else
            {
                dodane_jednostki.Add(jednostka);
            }
        }
        private static void dodaj_jednostke(Jednostka jednostka)
        {
            jednostki.Add(jednostka);
            if (jednostka is Pocisk)
            {
                pociski.Add(jednostka as Pocisk);
            }
            else if (jednostka is Przeciwnik)
            {
                przeciwnicy.Add(jednostka as Przeciwnik);
            }
            else if (jednostka is Czarna_dziura)
            {
                czarne_dziury.Add(jednostka as Czarna_dziura);
            }
        }
        public static void Update()
        {
            czy_update = true;
            kolizja_manipuluj();

            foreach (var jednostki in jednostki)
            {
                jednostki.Update();
            }
            czy_update = false;

            foreach (var jednostki in dodane_jednostki)
            {
                dodaj_jednostke(jednostki);
            }
            dodane_jednostki.Clear();

            pociski = pociski.Where(x => !x.czy_brak).ToList();
            przeciwnicy = przeciwnicy.Where(x => !x.czy_brak).ToList();
            jednostki = jednostki.Where(x => !x.czy_brak).ToList();
            czarne_dziury = czarne_dziury.Where(x => !x.czy_brak).ToList();
        }

        private static bool czy_koliduje(Jednostka a, Jednostka b)
        {
            float promień = a.promień + b.promień;
            return !a.czy_brak && !b.czy_brak && Vector2.DistanceSquared(a.pozycja, b.pozycja) < promień * promień; 
        }

        public static IEnumerable<Jednostka> pobierz_jednostki(Vector2 Pozycja, float promień)
        {
            return jednostki.Where(x => Vector2.DistanceSquared(Pozycja, x.pozycja) < promień * promień); 
        }
        static void kolizja_manipuluj()
        {
            for (int i = 0; i < przeciwnicy.Count; i++)
                for (int j = i + 1; j < przeciwnicy.Count; j++)
                {
                    if (czy_koliduje(przeciwnicy[i], przeciwnicy[j]))
                    {
                        przeciwnicy[i].manipuluj_kolizją(przeciwnicy[j]);
                        przeciwnicy[j].manipuluj_kolizją(przeciwnicy[i]);
                    }
                }

            for (int i = 0; i < przeciwnicy.Count; i++)
                for (int j = 0; j < pociski.Count; j++)
                {
                    if (czy_koliduje(przeciwnicy[i], pociski[j]))
                    {
                        przeciwnicy[i].czy_zestrzelony();
                        pociski[j].czy_brak = true;
                    }
                }

            for (int i = 0; i < przeciwnicy.Count; i++)
            {
                if (przeciwnicy[i].czy_aktywny && czy_koliduje(Gracz_statek.Stan, przeciwnicy[i]))
                {
                    zabij_gracza();
                    break;
                }
            }

            for (int i = 0; i < czarne_dziury.Count; i++)
            {
                for (int j = 0; j < przeciwnicy.Count; j++)
                {
                    if (przeciwnicy[j].czy_aktywny && czy_koliduje(czarne_dziury[i], przeciwnicy[j]))
                    {
                        przeciwnicy[j].czy_zestrzelony();
                    }
                }
                for (int j = 0; j < pociski.Count; j++)
                {
                    if (czy_koliduje(czarne_dziury[i], pociski[j]))
                    {
                        pociski[j].czy_brak = true;
                        czarne_dziury[i].czy_zestrzelony();
                    }
                }
                if (czy_koliduje(Gracz_statek.Stan, czarne_dziury[i]))
                {
                    zabij_gracza();
                    break;
                }
            }
        }
        private static void zabij_gracza()
        {
            Gracz_statek.Stan.zabij();
            przeciwnicy.ForEach(x => x.czy_zestrzelony());
            czarne_dziury.ForEach(x => x.zabij());
            Przeciwnik_pojawienie_się.Reset();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in jednostki)
            {
                entity.Draw(spriteBatch);
            }
        }
    }
}
