using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Statek.Zawartość
{
    static class Dźwięk
    {
        public static Song muzyka { get; private set; }

        private static readonly Random losowo = new Random(); 

        private static SoundEffect[] eksplozje;
        public static SoundEffect Eksplozje { get { return eksplozje[losowo.Next(eksplozje.Length)]; } }

        private static SoundEffect[] strzały;
        public static SoundEffect Strzał { get { return strzały[losowo.Next(strzały.Length)]; } }

        private static SoundEffect[] pojawienie_się;
        public static SoundEffect Pojawienie_się { get { return pojawienie_się[losowo.Next(pojawienie_się.Length)]; } }

        public static void Load(ContentManager content)
        {
            muzyka = content.Load<Song>("Dźwięki/Muzyka");

            eksplozje = Enumerable.Range(1, 8).Select(x => content.Load<SoundEffect>("Dźwięki/eksplozja_0" + x)).ToArray();
            strzały = Enumerable.Range(1, 4).Select(x => content.Load<SoundEffect>("Dźwięki/strzał_0" + x)).ToArray();
            pojawienie_się = Enumerable.Range(1, 8).Select(x => content.Load<SoundEffect>("Dźwięki/pojawienie_się_0" + x)).ToArray();
        }
    }
}
