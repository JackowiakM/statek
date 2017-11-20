using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Statek
{
    static class Tekstury
    {
        public static Texture2D gracz { get; private set; }
        public static Texture2D wrog_1 { get; private set; }
        public static Texture2D Czarna_dziura { get; private set; }
        public static Texture2D wrog_2 { get; private set; }
        public static Texture2D Pocisk { get; private set; }
        public static Texture2D wskaźnik { get; private set; }
        public static SpriteFont Czcionka { get; private set; }

        public static Texture2D żar { get; private set; }
        public static Texture2D piksel { get; private set; }
        public static Texture2D laser { get; private set; }

        public static void wczytaj(ContentManager zawartość)
        {
            gracz = zawartość.Load<Texture2D>("Tekstury/gracz");
            wrog_1 = zawartość.Load<Texture2D>("Tekstury/wrog_1");
            Czarna_dziura = zawartość.Load<Texture2D>("Tekstury/czarna_dziura");
            wrog_2 = zawartość.Load<Texture2D>("Tekstury/wrog_2");
            Pocisk = zawartość.Load<Texture2D>("Tekstury/pocisk");
            wskaźnik = zawartość.Load<Texture2D>("Tekstury/wskaźnik");

            Czcionka = zawartość.Load<SpriteFont>("czcionka");

            żar = zawartość.Load<Texture2D>("Tekstury/żar");
            laser = zawartość.Load<Texture2D>("Tekstury/laser");
            piksel = new Texture2D(gracz.GraphicsDevice, 1, 1);
            piksel.SetData(new[] { Color.White });
        }
    }
}
