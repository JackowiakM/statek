using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Statek
{
    abstract class Jednostka
    {
        protected Texture2D obraz;
        protected Color kolor = Color.White;

        public Vector2 pozycja, Prędkość;
        public float orientacja;
        public float promień = 20; 
        public bool czy_brak; 

        public Vector2 rozmiar
        {
            get
            {
                return obraz == null ? Vector2.Zero : new Vector2(obraz.Width, obraz.Height);
            }
        }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(obraz, pozycja, null, kolor, orientacja, rozmiar / 2f, 1f, 0, 0);
        }
    }
}
