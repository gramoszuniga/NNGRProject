using System;

namespace NNGRProject
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (SpaceShooter game = new SpaceShooter())
            {
                game.Run();
            }
        }
    }
#endif
}

