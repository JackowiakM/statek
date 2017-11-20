using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Statek
{
    class Pocisk : Jednostka
    {
        private static Random losowe = new Random();

        public Pocisk(Vector2 position, Vector2 velocity)
        {
            obraz = Tekstury.Pocisk;
            pozycja = position;
            Prędkość = velocity;
            orientacja = Dodatki.do_kąta(Prędkość);
            promień = 8;
        }

        public override void Update()
        {
            if (Prędkość.LengthSquared() > 0)
            {
                orientacja = Dodatki.do_kąta(Prędkość);
            }

            pozycja += Prędkość;

            if (!Start.rzutnia.Bounds.Contains(Dodatki.do_punktu(pozycja)))
            {
                czy_brak = true;
            }

            if (!Start.rzutnia.Bounds.Contains(pozycja.do_punktu()))
            {
                czy_brak = true;

                for (int i = 0; i < 30; i++)
                {
                    Start.Cząstki_menadżer.stwórz_cząstkę(Tekstury.laser, pozycja, Color.LightBlue,50, 1, new Cząstki_stan()
                                                            {
                                                                Prędkość = losowe.następny_wektor(0, 9),
                                                                Typ = cząstki_typ.Pocisk,
                                                                Mnożnik_długość = 1
                                                            }
                                                            );
                }

            }
        }
    }
}
