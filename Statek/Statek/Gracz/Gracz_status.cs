using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Statek
{
    class Gracz_status
    {
        private const float mnożnik_czas_zniknięcia = 0.8f;
        private const int mnożnik_max = 20;
        
        public static int życia { get; private set; }
        public static int wynik { get; private set; }
        public static int mnożnik { get; private set; }
        public static int najlepszy_wynik { get; private set; }
        public static bool czy_koniec_gry { get { return życia == 0; } }
        
        private static float mnożnik_czas_pozostały;   
        private static int punkty_za_życia;      
        
        private const string najwyższy_wynik_plik = "Najwyższy wynik.txt";

        static Gracz_status()
        {
            najlepszy_wynik = załaduj_najwyższy_wynik();
            Reset();
        }

        public static void Reset()
        {
            if (wynik > najlepszy_wynik)
            {
                zapisz_najwyższy_wynik(najlepszy_wynik = wynik);
            }
            wynik = 0;
            mnożnik = 1;
            życia = 4;
            punkty_za_życia = 2000;
            mnożnik_czas_pozostały = 0;
        }

        public static void Update()
        {
            if (mnożnik > 1)
            {
                if ((mnożnik_czas_pozostały -= (float)Start.czas.ElapsedGameTime.TotalSeconds) <= 0)
                {
                    mnożnik_czas_pozostały = mnożnik_czas_zniknięcia;
                    zresetuj_mnożnik();
                }
            }
        }
        
        public static void dodaj_punkty(int punkty_podstawowe)
        {
            if (Gracz_statek.Stan.czy_nie_żyje)
            {
                return;
            }
            wynik += punkty_podstawowe * mnożnik;
            while (wynik >= punkty_za_życia)
            {
                punkty_za_życia += 2000;
                życia++;
            }
        }
        
        public static void zwiększ_mnożnik()
        {
            if (Gracz_statek.Stan.czy_nie_żyje)
            {
                return;
            }
            mnożnik_czas_pozostały = mnożnik_czas_zniknięcia;
            if (mnożnik < mnożnik_max)
                mnożnik++;
        }
        
        public static void zresetuj_mnożnik()
        {
            mnożnik = 1;
        }

        public static void usuń_życie()
        {
            życia--;
        }
        
        private static int załaduj_najwyższy_wynik() 
        {
            int wynik_najlepszy;
            return File.Exists(najwyższy_wynik_plik) && int.TryParse(File.ReadAllText(najwyższy_wynik_plik), out wynik_najlepszy) ? wynik_najlepszy : 0;
        }
        
        private static void zapisz_najwyższy_wynik(int wynik)
        {
            File.WriteAllText(najwyższy_wynik_plik, wynik.ToString());
        }
    }
}
