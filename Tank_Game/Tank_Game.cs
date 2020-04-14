using Math_Library;
using Raylib;
using static Raylib.Raylib;

namespace Tank_Game
{
    public class Tank_Game
    {
        static void Main()
        {
            Game game = new Game(2500, 1500);
            
            
            game.Init();

            while (!WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }

            game.Shutdown();

            CloseWindow();
        }
    }
}
