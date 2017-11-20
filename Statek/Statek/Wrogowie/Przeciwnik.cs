using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Statek
{
    class Przeciwnik : Jednostka
    {
        public static Random losowe = new Random(); 
        private int czas_do_startu = 60;
        public bool czy_aktywny { get { return czas_do_startu <= 0; } }
        private List<IEnumerator<int>> zachowania = new List<IEnumerator<int>>();
        public int punkt_wartość { get; private set; }
        
        public Przeciwnik(Texture2D obraz, Vector2 pozycja_p)
        {
            this.obraz = obraz;
            pozycja = pozycja_p;
            promień = obraz.Width / 2f;
            kolor = Color.Transparent;
        }

        public override void Update()
        {
            if (czas_do_startu <= 0)
            {
                zastosuj_zachowania();
            }
            else
            {
                czas_do_startu--;
                kolor = Color.White * (1 - czas_do_startu / 60f);
            }

            pozycja += Prędkość;
            pozycja = Vector2.Clamp(pozycja, rozmiar / 2, Start.rozmiar_ekranu - rozmiar / 2);

            Prędkość *= 0.8f;
        }
        
        public void czy_zestrzelony()
        {
            czy_brak = true;

            float odcień1 = losowe.następny_float(0, 6);
            float odcień2 = (odcień1 + losowe.następny_float(0, 2)) % 6f;
            Color kolor1 = Kolor_zmiana.zmiana_do_koloru(odcień1, 0.5f, 1);
            Color kolor2 = Kolor_zmiana.zmiana_do_koloru(odcień2, 0.5f, 1);

            Zawartość.Dźwięk.Eksplozje.Play(0.5f, losowe.następny_float(-0.2f, 0.2f), 0);
            for (int i = 0; i < 120; i++)
            {
                float speed = 18f * (1f - 1 / losowe.następny_float(1f, 10f));
                var state = new Cząstki_stan()
                {
                    Prędkość = losowe.następny_wektor(speed, speed),
                    Typ = cząstki_typ.Przeciwnik,
                    Mnożnik_długość = 1f
                };

                Color color = Color.Lerp(kolor1, kolor2, losowe.następny_float(0, 1));
                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.laser, pozycja, color, 190, 1.5f, state);
            }
            Gracz_status.dodaj_punkty(punkt_wartość);
            Gracz_status.zwiększ_mnożnik();
        }
        
        private void dodaj_zachowanie(IEnumerable<int> zachowanie)
        {
            zachowania.Add(zachowanie.GetEnumerator());
        }
        
        private void zastosuj_zachowania()
        {
            for (int i = 0; i < zachowania.Count; i++)
            {
                if (!zachowania[i].MoveNext())
                    zachowania.RemoveAt(i--);
            }
        } 
        public void manipuluj_kolizją(Przeciwnik przeciwnik)
        {
            var d = pozycja - przeciwnik.pozycja;
            Prędkość += 10 * d / (d.LengthSquared() + 1);
        }
        IEnumerable<int> ścigaj_gracza(float przyśpieszenie = 1f)
        {
            while (true)
            {
                Prędkość += (Gracz_statek.Stan.pozycja - pozycja).skaluj_do(przyśpieszenie);
                if (Prędkość != Vector2.Zero)
                    orientacja = Prędkość.do_kąta();

                yield return 0;
            } 
        }
        IEnumerable<int> poruszaj_losowo()
        {
            float kierunek = losowe.następny_float(0, MathHelper.TwoPi);

            while (true)
            {
                kierunek += losowe.następny_float(-0.1f, 0.1f);
                kierunek = MathHelper.WrapAngle(kierunek);

                for (int i = 0; i < 6; i++)
                {
                    Prędkość += Prędkość_math.z_krzywej(kierunek, 0.4f);
                    orientacja -= 0.05f;

                    var bounds = Start.rzutnia.Bounds;
                    bounds.Inflate(-obraz.Width, -obraz.Height);

                    if (!bounds.Contains(pozycja.do_punktu()))
                        kierunek = (Start.rozmiar_ekranu / 2 - pozycja).do_kąta() + losowe.następny_float(-MathHelper.PiOver2, MathHelper.PiOver2);

                    yield return 0;
                }
            }
        }
        public static Przeciwnik stworz_wrog1(Vector2 pozycja)
        {
            var przeciwnik = new Przeciwnik(Tekstury.wrog_1, pozycja);
            przeciwnik.dodaj_zachowanie(przeciwnik.ścigaj_gracza());
            przeciwnik.punkt_wartość = 5;
            return przeciwnik;
        }

        public static Przeciwnik stworz_wrog2(Vector2 pozycja)
        {
            var przeciwnik = new Przeciwnik(Tekstury.wrog_2, pozycja);
            przeciwnik.dodaj_zachowanie(przeciwnik.poruszaj_losowo());
            przeciwnik.punkt_wartość = 1;
            return przeciwnik;
        }
    }
}
