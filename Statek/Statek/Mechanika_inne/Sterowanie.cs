using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Statek
{
    static class Sterowanie
    {
        private static KeyboardState stan_klawiatury, ostatni_stan_klawiatury;
        private static MouseState stan_myszki, ostatni_stan_myszki; 
        private static GamePadState stan_pada, ostatni_stan_pada;

        private static bool celowanie_myszką = false;
        public static Vector2 pozycja_myszki { get { return new Vector2(stan_myszki.X, stan_myszki.Y); } }

        public static void Update()
        {
            ostatni_stan_klawiatury = stan_klawiatury;
            ostatni_stan_myszki = stan_myszki;
            ostatni_stan_pada = stan_pada;

            stan_klawiatury = Keyboard.GetState();
            stan_myszki = Mouse.GetState();
            stan_pada = GamePad.GetState(PlayerIndex.One);
            if (new[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down }.Any(x => stan_klawiatury.IsKeyDown(x)) || stan_pada.ThumbSticks.Right != Vector2.Zero)
            {
                celowanie_myszką = false;
            }
            else if (pozycja_myszki != new Vector2(ostatni_stan_myszki.X, ostatni_stan_myszki.Y))
            {
                celowanie_myszką = true;
            }
        }
        
        public static bool klawiatura_czy_wciśnięte(Keys klawisz)
        {
            return ostatni_stan_klawiatury.IsKeyUp(klawisz) && stan_klawiatury.IsKeyDown(klawisz);
        }
        
        public static bool przycisk_czy_wciśnięte(Buttons przycisk)
        {
            return ostatni_stan_pada.IsButtonUp(przycisk) && stan_pada.IsButtonDown(przycisk);
        }
        
        public static Vector2 pobierz_ruch()
        {
            Vector2 kierunek = stan_pada.ThumbSticks.Left;
            kierunek.Y *= -1; 

            if (stan_klawiatury.IsKeyDown(Keys.A))
            {
                kierunek.X -= 1;
            }
            if (stan_klawiatury.IsKeyDown(Keys.D))
            {
                kierunek.X += 1;
            }
            if (stan_klawiatury.IsKeyDown(Keys.W))
            {
                kierunek.Y -= 1;
            }
            if (stan_klawiatury.IsKeyDown(Keys.S))
            {
                kierunek.Y += 1;
            }

            if (kierunek.LengthSquared() > 1)
            {
                kierunek.Normalize();
            }

            return kierunek;
        }
        
        public static Vector2 pobierz_celownik()
        {
            if (celowanie_myszką)
                return pobierz_celownik_myszki();

            Vector2 kierunek = stan_pada.ThumbSticks.Right;
            kierunek.Y *= -1;

            if (stan_klawiatury.IsKeyDown(Keys.Left))
                kierunek.X -= 1;
            if (stan_klawiatury.IsKeyDown(Keys.Right))
                kierunek.X += 1;
            if (stan_klawiatury.IsKeyDown(Keys.Up))
                kierunek.Y -= 1;
            if (stan_klawiatury.IsKeyDown(Keys.Down))
                kierunek.Y += 1;

            if (kierunek == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(kierunek);
        }
        
        private static Vector2 pobierz_celownik_myszki()
        {
            Vector2 kierunek = pozycja_myszki - Gracz_statek.Stan.pozycja;

            if (kierunek == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(kierunek);
        }
        
        public static bool bomba_czy()
        {
            return przycisk_czy_wciśnięte(Buttons.LeftTrigger) || przycisk_czy_wciśnięte(Buttons.RightTrigger) || klawiatura_czy_wciśnięte(Keys.Space);
        }
    }
}
