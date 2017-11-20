using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;

namespace Statek
{
    public class Cząstki_menadżer<T>
    {
        private Action<cząstka> update_cząstki; 
        private cząstka_tablica lista_cząstek; 

        public Cząstki_menadżer(int pojemność, Action<cząstka> cząstki_Update)
        {
            this.update_cząstki = cząstki_Update;
            lista_cząstek = new cząstka_tablica(pojemność);

            for (int i = 0; i < pojemność; i++)
            {
                lista_cząstek[i] = new cząstka();
            }
        } 
        public void stwórz_cząstkę(Texture2D tekstura, Vector2 pozycja, Color odcień, float czas, float skala, T stan, float theta = 0)
        {
            stwórz_cząstkę(tekstura, pozycja, odcień, czas, new Vector2(skala), stan, theta);
        }
        public void stwórz_cząstkę(Texture2D tekstura, Vector2 pozycja, Color odcień, float czas, Vector2 skala, T stan, float theta = 0)
        {
            cząstka cząstka;

            if (lista_cząstek.zlicz == lista_cząstek.rozmiar)
            {
                cząstka = lista_cząstek[0];
                lista_cząstek.Start++;
            }
            else
            {
                cząstka = lista_cząstek[lista_cząstek.zlicz];
                lista_cząstek.zlicz++;
            }

            cząstka.tekstura = tekstura;
            cząstka.pozycja = pozycja;
            cząstka.kolor = odcień;

            cząstka.czas = czas;
            cząstka.PercentLife = 1f;
            cząstka.skala = skala;
            cząstka.orientacja = theta;
            cząstka.stan = stan;

        }
        public void Update()
        {
            int usunięte_zlicz = 0; 
            for (int i = 0; i < lista_cząstek.zlicz; i++)
            {
                var cząstka = lista_cząstek[i];
                update_cząstki(cząstka);
                cząstka.PercentLife -= 1f / cząstka.czas;

                zamień(lista_cząstek, i - usunięte_zlicz, i);

                if (cząstka.PercentLife < 0)
                {
                    usunięte_zlicz++;
                }
            }
            lista_cząstek.zlicz -= usunięte_zlicz;
        } 
        private static void zamień(cząstka_tablica lista, int index1, int index2)
        {
            var temp = lista[index1];
            lista[index1] = lista[index2];
            lista[index2] = temp;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < lista_cząstek.zlicz; i++)
            {
                var particle = lista_cząstek[i];

                Vector2 origin = new Vector2(particle.tekstura.Width / 2, particle.tekstura.Height / 2);
                spriteBatch.Draw(particle.tekstura, particle.pozycja, null, particle.kolor, particle.orientacja, origin, particle.skala, 0, 0);
            }
        } 
        public class cząstka
        {
            public Texture2D tekstura;
            public Vector2 pozycja;
            public float orientacja;

            public Vector2 skala = Vector2.One;

            public Color kolor;
            public float czas;
            public float PercentLife = 1f; 
            public T stan;
        }
        private class cząstka_tablica
        {
            private int start;
            public int Start
            {
                get { return start; }
                set { start = value % lista.Length; }
            }

            public int zlicz { get; set; }
            public int rozmiar { get { return lista.Length; } }
            private cząstka[] lista;

            public cząstka_tablica() { }  

            public cząstka_tablica(int pojemność)
            {
                lista = new cząstka[pojemność];
            }
            public cząstka this[int i]
            {
                get { return lista[(start + i) % lista.Length]; }
                set { lista[(start + i) % lista.Length] = value; }
            }
        }
    }
}

