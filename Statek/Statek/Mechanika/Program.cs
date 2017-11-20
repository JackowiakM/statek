using System;

namespace Statek
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Start game = new Start())
            {
                game.Run();
            }
        }
    }
#endif
}

