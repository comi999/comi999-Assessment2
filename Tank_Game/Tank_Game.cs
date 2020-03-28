using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Math_Library;
using rl = Raylib;
using static Raylib.Raylib;
using System.Threading;
using System.Diagnostics;

namespace Tank_Game
{
    class Tank_Game
    {
        static void Main()
        {
            Game game = new Game();

            InitWindow(2500, 1500, "Hello World");
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
