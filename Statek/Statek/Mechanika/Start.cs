using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Statek
{
    public class Start : Microsoft.Xna.Framework.Game 
    {
        public static Start Stan { get; private set; }
        public static Viewport rzutnia { get { return Stan.GraphicsDevice.Viewport; } }
        public static Vector2 rozmiar_ekranu { get { return new Vector2(rzutnia.Width, rzutnia.Height); } }
        public static GameTime czas { get; private set; }
        public static Cz�stki_menad�er<Cz�stki_stan> Cz�stki_menad�er { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool pauza = false;

        public Start()
        {
            Stan = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Jednostka_menad�er.dodaj(Gracz_statek.Stan);
            Cz�stki_menad�er = new Cz�stki_menad�er<Cz�stki_stan>(1024 * 20, Cz�stki_stan.update_cz�stka);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Zawarto��.D�wi�k.muzyka);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Zawarto��.D�wi�k.Load(Content);
            Tekstury.wczytaj(Content);
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            czas = gameTime;
            Sterowanie.Update();

            if (Sterowanie.przycisk_czy_wci�ni�te(Buttons.Back) || Sterowanie.klawiatura_czy_wci�ni�te(Keys.Escape))
            {
                this.Exit();
            }
            if (Sterowanie.klawiatura_czy_wci�ni�te(Keys.P))
            {
                pauza = !pauza;
            }
            if (!pauza)
            {
                Jednostka_menad�er.Update();
                Cz�stki_menad�er.Update();
                Przeciwnik_pojawienie_si�.Update();
                Gracz_status.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
          
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            Jednostka_menad�er.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Cz�stki_menad�er.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            spriteBatch.DrawString(Tekstury.Czcionka, "Zycia: " + Gracz_status.�ycia, new Vector2(5), Color.White);
            narysuj_tekst("Wynik: " + Gracz_status.wynik, 5);
            narysuj_tekst("Mnoznik: " + Gracz_status.mno�nik, 35);

            spriteBatch.Draw(Tekstury.wska�nik, Sterowanie.pozycja_myszki, Color.White);

            if (Gracz_status.czy_koniec_gry)
            {
                string tekst = "Koniec gry\n" +
                    "Twoj wynik: " + Gracz_status.wynik + "\n" +
                    "Najwyzszy wynik: " + Gracz_status.najlepszy_wynik + "\n" +
                    "Powtorz gre T/N";

                Vector2 textSize = Tekstury.Czcionka.MeasureString(tekst);
                spriteBatch.DrawString(Tekstury.Czcionka, tekst, rozmiar_ekranu / 2 - textSize / 2, Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void wyjd�() 
        {
            this.Exit();
        }
        private void narysuj_tekst(string Tekst, float y) 
        {
            var tekst_czcionka = Tekstury.Czcionka.MeasureString(Tekst).X;
            spriteBatch.DrawString(Tekstury.Czcionka, Tekst, new Vector2(rozmiar_ekranu.X - tekst_czcionka - 5, y), Color.White);
        }
    }
}
