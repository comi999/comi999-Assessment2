using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rl = Raylib;
using static Raylib.Raylib;
using Raylib;
using System.Runtime.CompilerServices;

namespace Tank_Game
{

    using Math_Library;

    class Game
    {

        Stopwatch stopwatch = new Stopwatch();

        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;

        private float deltaTime = 0.005f;


        rl.Image logo;
        rl.Texture2D texture;
        Matrix3 logoVector = new Matrix3();

        public Game()
        {
        }

        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("Stopwatch high-resolution frequency: {0} ticks per second", Stopwatch.Frequency);
            }

            //logo = LoadImage("..\\Images\\aie-logo-dark.jpg");
            //logo = LoadImage(@"..\Images\aie-logo-dark.jpg");
            logo = LoadImage("../Images/barrelBlue.png");
            texture = LoadTextureFromImage(logo);


        }

        class GameObject
        {
            public Matrix3 localMatrix = new Matrix3();
            public Matrix3 globalMatrix = new Matrix3();

            public GameObject parent;
            public List<GameObject> children = new List<GameObject>();

            public void RecalculateGlobal()
            {
                globalMatrix = localMatrix * parent.globalMatrix;
                foreach (GameObject child in children) 
                    child.RecalculateGlobal();
            }
        }

        public void Shutdown()
        {
        }

        public void Update()
        {
            lastTime = currentTime;
            currentTime = stopwatch.ElapsedTicks;
            deltaTime = (float)(currentTime - lastTime) / Stopwatch.Frequency;
            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;

            // insert game logic here

            f += deltaTime;
            Matrix3 Translate = new Matrix3(1, 0, 0, 0, 1, 0, (float)Math.Sin(f) * 800, 0, 1);
            Matrix3 Rotation = new Matrix3(); Rotation.RotateZ(f * 360f.ConvertToRadians());
            logoVector = Rotation * Translate;

        }


        float f = 0;
        


        public void Draw()
        {

            BeginDrawing();

            ClearBackground(rl.Color.WHITE);

            DrawText(fps.ToString(), 10, 10, 500, rl.Color.RED);

            DrawTexturePro(texture, new Rectangle(0, 0, texture.width, texture.height), new Rectangle(2500 / 2 + logoVector.i3, 1500 / 2 + logoVector.j3, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2 ), ((float)Math.Atan(logoVector.j2/logoVector.i2)).ConvertToDegrees(), Color.WHITE) ;
            

            EndDrawing();
        }
    }
}
