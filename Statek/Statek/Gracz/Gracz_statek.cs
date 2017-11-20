using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Statek
{
    class Gracz_statek : Jednostka
    {
        static Random losowo = new Random();
        public Start gra; 
        const int klatki_czas = 6;
        int klatki_pozostało = 0;
        int klatek_do_pojawienia_się = 0; 
        public bool czy_nie_żyje { get { return klatek_do_pojawienia_się > 0; } }
        private static Gracz_statek stan;
        public static Gracz_statek Stan
        {
            get
            {
                if (stan == null)
                {
                    stan = new Gracz_statek();
                }
                return stan;
            }
        }

        private Gracz_statek()
        {
            obraz = Tekstury.gracz;
            pozycja = Start.rozmiar_ekranu / 2;
            promień = 10;
        }
        public void zabij()
        {
            Gracz_status.usuń_życie();
            klatek_do_pojawienia_się = Gracz_status.czy_koniec_gry ? 300 : 120;
            Color zółty = new Color(0.8f, 0.8f, 0.4f);

            for (int i = 0; i < 1200; i++)
            {
                float szykość = 18f * (1f - 1 / losowo.następny_float(1f, 10f));
                Color kolor = Color.Lerp(Color.White, zółty, losowo.następny_float(0, 1));
                var state = new Cząstki_stan
                {
                    Prędkość = losowo.następny_wektor(szykość, szykość),
                    Typ = cząstki_typ.Brak,
                    Mnożnik_długość = 1
                };

                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.laser, pozycja, kolor, 190, 1.5f, state);
            }
        }
        public override void Update()
        {
            if (czy_nie_żyje)
            {
                if (Gracz_status.życia == 0)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.T))
                    {
                        klatek_do_pojawienia_się = 120;
                        Gracz_status.Reset();
                        pozycja = Start.rozmiar_ekranu / 2;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.N))
                    {
                        gra.wyjdź();
                    }
                }
                else
                {
                    --klatek_do_pojawienia_się;
                }

                return;
            }
            const float szybkość = 8;
            Prędkość = szybkość * Sterowanie.pobierz_ruch();
            pozycja += Prędkość;
            pozycja = Vector2.Clamp(pozycja, rozmiar / 2, Start.rozmiar_ekranu - rozmiar / 2);

            if (Prędkość.LengthSquared() > 0)
            {
                orientacja = Dodatki.do_kąta(Prędkość);
            }

            var cel = Sterowanie.pobierz_celownik();
            if (cel.LengthSquared() > 0 && klatki_pozostało <= 0)
            {
                klatki_pozostało = klatki_czas;
                float cel_kąt = Dodatki.do_kąta(cel); 
                Quaternion cel_quat = Quaternion.CreateFromYawPitchRoll(0, 0, cel_kąt); 

                float losowy_spread = Dodatki.następny_float(losowo, -0.04f, 0.04f) + Dodatki.następny_float(losowo, -0.04f, 0.04f);
                Vector2 prędkość_p = Prędkość_math.z_krzywej(cel_kąt + losowy_spread, 11f);

                Vector2 pęd = Vector2.Transform(new Vector2(25, -8), cel_quat);
                Jednostka_menadżer.dodaj(new Pocisk(pozycja + pęd, prędkość_p));

                pęd = Vector2.Transform(new Vector2(25, 8), cel_quat);
                Jednostka_menadżer.dodaj(new Pocisk(pozycja + pęd, prędkość_p));
                Zawartość.Dźwięk.Strzał.Play(0.2f, losowo.następny_float(-0.2f, 0.2f), 0);
            }

            if (klatki_pozostało > 0)
            {
                klatki_pozostało--;
            }
            strzał_powtarzalny();
            Prędkość = Vector2.Zero;
        }
        
        private void strzał_powtarzalny()
        {
            if (Prędkość.LengthSquared() > 0.1f)
            {
                orientacja = Prędkość.do_kąta();
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f, 0f, orientacja);

                double czas_rz = Start.czas.TotalGameTime.TotalSeconds; 
                Vector2 pręd_podstawowa = Prędkość.skaluj_do(-3); 
                Vector2 pręd_przed = new Vector2(pręd_podstawowa.Y, -pręd_podstawowa.X) * (0.6f * (float)Math.Sin(czas_rz * 10));
                Color kolor_bok = new Color(200, 38, 9);    
                Color kolor_środek = new Color(255, 187, 30);   
                Vector2 poz = pozycja + Vector2.Transform(new Vector2(-25, 0), rot); 
                const float alfa = 0.7f;

                Vector2 pręd_śr = pręd_podstawowa + losowo.następny_wektor(0, 1);
                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.laser, poz, Color.White * alfa, 60f, new Vector2(0.5f, 1),
                    new Cząstki_stan(pręd_śr, cząstki_typ.Przeciwnik));
                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.żar, poz, kolor_środek * alfa, 60f, new Vector2(0.5f, 1),
                    new Cząstki_stan(pręd_śr, cząstki_typ.Przeciwnik));

                Vector2 pręd_1 = pręd_podstawowa + pręd_przed + losowo.następny_wektor(0, 0.3f);
                Vector2 pręd_2 = pręd_podstawowa - pręd_przed + losowo.następny_wektor(0, 0.3f);
                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.laser, poz, Color.White * alfa, 60f, new Vector2(0.5f, 1),
                    new Cząstki_stan(pręd_1, cząstki_typ.Przeciwnik));
                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.laser, poz, Color.White * alfa, 60f, new Vector2(0.5f, 1),
                    new Cząstki_stan(pręd_2, cząstki_typ.Przeciwnik));

                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.żar, poz, kolor_bok * alfa, 60f, new Vector2(0.5f, 1),
                    new Cząstki_stan(pręd_1, cząstki_typ.Przeciwnik));
                Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.żar, poz, kolor_bok * alfa, 60f, new Vector2(0.5f, 1),
                    new Cząstki_stan(pręd_2, cząstki_typ.Przeciwnik));
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!czy_nie_żyje)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
