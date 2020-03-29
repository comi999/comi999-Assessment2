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
        GameObject tankBody;
        GameObject tankBarrel;
        GameObject world;






        Stopwatch stopwatch = new Stopwatch();

        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;

        private float deltaTime = 0.005f;


        rl.Image logo;
        rl.Texture2D texture;
        Matrix3 logoVector = new Matrix3(1,0,0,0,1,0,0,0,1);


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

            //-------------------------------------GAME OBJECTS------------------------------------------------------------------------------------
            world = new GameObject("", 0);
            
            tankBody = new GameObject("../Images/tankBlue_outline.png", 0);
            
            tankBarrel = new GameObject("../Images/barrelBlue.png", 0);

            world.children.Add(tankBody);

            tankBody.parent = world;

            tankBody.children.Add(tankBarrel);

            tankBarrel.parent = tankBody;

            tankBody.LocalMatrix = new Matrix3(1, 0, 0, 0, 1, 0, 1250, 750, 1);

            tankBarrel.LocalMatrix = new Matrix3(1, 0, 0, 0, 1, 0, 0, -25, 1);

        }

        class GameObject
        {
            public Texture2D sprite;
            public float rotation;

            public GameObject(string imageLocation, float rotation)
            {
                sprite = LoadTextureFromImage(LoadImage(imageLocation));
                this.rotation = rotation;
            }

            public Matrix3 localMatrix = new Matrix3();
            public Matrix3 LocalMatrix
            {
                get
                {
                    return localMatrix;
                }
                set
                {
                    localMatrix = value;
                    RecalculateGlobal();
                }
            }
            public Matrix3 globalMatrix = new Matrix3();

            public GameObject parent;
            public List<GameObject> children = new List<GameObject>();


            public void DrawObject(Color color)
            {
                DrawTexturePro(sprite, new Rectangle(0, 0, sprite.width, sprite.height), new Rectangle(globalMatrix.i3, globalMatrix.j3, sprite.width, sprite.height), new Vector2(sprite.width / 2, sprite.height / 2), rotation.ConvertToDegrees(), color);
            }

            public void RecalculateGlobal()
            {
                if (parent == null)
                    globalMatrix = localMatrix;
                else
                    globalMatrix = localMatrix * parent.globalMatrix;
                rotation = (float)Math.Atan(globalMatrix.j1 / globalMatrix.i1);
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
            if (IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                tankBody.LocalMatrix = new Matrix3(1, 0, 0, 0, 1, 0, -1000*deltaTime, 0, 1) * tankBody.LocalMatrix;
            }
            
            tankBarrel.LocalMatrix = new Matrix3().RotateZ(f += 0.1f) ;
           
        }
        float f = 0;
        public void Draw()
        {
            BeginDrawing();

            ClearBackground(rl.Color.WHITE);

            //world.RecalculateGlobal();
            tankBody.DrawObject(Color.WHITE);
            tankBarrel.DrawObject(Color.GRAY);

            EndDrawing();
        }
    }
}
