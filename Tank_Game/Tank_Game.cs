using Math_Library;
using Raylib;
using static Raylib.Raylib;

namespace Tank_Game
{
    public class Tank_Game
    {
        public static Game game;

        static void Main()
        {
            game = new Game(2500, 1500);
            game.Init();

            while (!WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }

            game.Shutdown();
        }
    }
}
