using Raylib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Raylib.Raylib;
using rl = Raylib;

namespace Tank_Game
{
    using Math_Library;

    public class Game
    {
        #region ---'GameObject' DECLARATIONS---
        GameObject world;
        Vehicle tankBody;
        GameObject tankBarrel;
        #endregion

        #region ---'Game' VARIABLES---
        #region ---FPS AND DELTATIME VARIABLES---
        Stopwatch stopwatch = new Stopwatch();
        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        public int fps = 1;
        private int frames;
        private float deltaTime = 0.005f;
        #endregion
        
        #region ---INIT VARIABLES---
        public static int screenWidth = 0;
        public static int screenHeight = 0;
        #endregion

        #region ---PLACEHOLDER VARIABLES---
        public static Matrix3 placeHolder = new Matrix3();
        Vector3 mouse = new Vector3(0, 0, 1);
        #endregion

        float barrelVelocity = 0;
        float barrelFireTimer = 0;
        Sound tankFire = LoadSound("../Images/tankFiring.wav");
        #endregion

        #region ---'Game' CONSTRUCTORS---
        public Game()
        {
            screenWidth = 2500;
            screenHeight = 1500;
        }

        public Game(int screenWidth, int screenHeight)
        {
            Game.screenWidth = screenWidth;
            Game.screenHeight = screenHeight;
        }
        #endregion

        #region ---'Game' SUBCLASSES---
        public class GameObject
        {
            //These parameters and methods are the backbone of every GameObject in the game
            #region GameObject base parameters and methods
            public Texture2D sprite;
            public Vector2 origin = new Vector2(0, 0);
            #region Local Matrix parameters
            public Matrix3 position = new Matrix3();
            public Matrix3 rotation = new Matrix3();
            public Matrix3 scale = new Matrix3();
            #endregion
            public float rotationAngle = 0;
            public float RotationAngle
            {
                get => rotationAngle;
                set
                {
                    rotationAngle = value;
                    rotation.i1 = (float)Math.Cos(value);
                    rotation.j1 = -(float)Math.Sin(value);
                    rotation.i2 = -rotation.j1;
                    rotation.j2 = rotation.i1;
                }
            }
            public Matrix3 localMatrix = new Matrix3();
            public Matrix3 globalMatrix = new Matrix3();
            public GameObject parent;
            public List<GameObject> children = new List<GameObject>();
            #endregion

            //These are the contructors for GameObjects
            #region  GameObject Constructors
            public GameObject() { }
            public GameObject(string imageLocation, GameObject parent, Vector2 initialPosition, float initialRotation, Vector2 initialScale)
            {
                sprite = LoadTextureFromImage(LoadImage(imageLocation));
                position.i3 = initialPosition.x;
                position.j3 = initialPosition.y;
                RotationAngle = initialRotation;
                rotation.i1 = (float)Math.Cos(rotationAngle);
                rotation.j1 = -(float)Math.Sin(rotationAngle);
                rotation.i2 = -rotation.j1;
                rotation.j2 = rotation.i1;
                scale.i1 = initialScale.x;
                scale.j2 = initialScale.y;
                origin = new Vector2(sprite.width * scale.i1 / 2, sprite.height * scale.j2 / 2);

                this.parent = parent;
                if (parent != null)
                    parent.children.Add(this);
                localMatrix = scale * (rotation * position);
            }

            public GameObject(string imageLocation, GameObject parent, Vector2 initialPosition, float initialRotation, Vector2 initialScale, Vector2 origin)
            {
                sprite = LoadTextureFromImage(LoadImage(imageLocation));
                position.i3 = initialPosition.x;
                position.j3 = initialPosition.y;
                RotationAngle = initialRotation;
                rotation.i1 = (float)Math.Cos(RotationAngle);
                rotation.j1 = -(float)Math.Sin(RotationAngle);
                rotation.i2 = -rotation.j1;
                rotation.j2 = rotation.i1;
                scale.i1 = initialScale.x;
                scale.j2 = initialScale.y;
                this.origin.x = origin.x * scale.i1;
                this.origin.y = origin.y * scale.j2;

                this.parent = parent;
                if (parent != null)
                    parent.children.Add(this);
                localMatrix = scale * (rotation * position);
            }
            #endregion

            //This Vector2 allows the setting of a GameObjects position relative to it's direction
            public Vector2 relativePosition
            {
                set
                {
                    placeHolder.Reset();
                    placeHolder.i1 = 0; placeHolder.j1 = 0; placeHolder.k1 = 0;
                    placeHolder.i2 = 0; placeHolder.j2 = 0; placeHolder.k2 = 0;
                    placeHolder.i3 = value.x; placeHolder.j3 = value.y; placeHolder.k3 = 0;

                    position += (placeHolder * rotation);
                }
            }

            //This function controls Raylib drawing function and translates the games coordinate system to the Raylib coordinate system
            public void DrawObject(Color color)
            {
                DrawTexturePro(sprite, new Rectangle(0, 0, sprite.width, sprite.height), new Rectangle(globalMatrix.i3, screenHeight - globalMatrix.j3, sprite.width * scale.i1, sprite.height * scale.j2), origin, (float)Math.Atan2(globalMatrix.i2, globalMatrix.j2).ConvertToDegrees(), color);
            }

            //This function updates a GameObjects globalMatrix upon being called, and cascades the update down to all of it's children
            public void RecalculateGlobal()
            {
                if (parent == null)
                    globalMatrix = localMatrix;
                else
                    globalMatrix = scale * (rotation * position) * parent.globalMatrix;
                foreach (GameObject child in children)
                    child.RecalculateGlobal();
            }
        }

        public class Vehicle : GameObject
        {
            public Vehicle(string imageLocation, GameObject parent, Vector2 initialPosition, float initialRotation, Vector2 initialScale) :
                base(imageLocation, parent, initialPosition, initialRotation, initialScale)
            { }
            public float longitudinalVelocity = 0;
        }
        #endregion

        #region ---'Game' FUNCTIONS---
        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("Stopwatch high-resolution frequency: {0} ticks per second", Stopwatch.Frequency);
            }

            InitWindow(Game.screenWidth, Game.screenHeight, "Hello World");
            InitAudioDevice();

            //This is where all GameObjects are defined.
            # region ---GAME OBJECTS---
            world = new GameObject("", null, new Vector2(0, 0), 0, new Vector2(1, 1));

            tankBody = new Vehicle("../Images/tankBlue_outline.png", world, new Vector2(0, 0), 0, new Vector2(3, 3));

            tankBarrel = new GameObject("../Images/barrelBlue.png", tankBody, new Vector2(0, 0), 2, new Vector2(3, 5), new Vector2(8, 40));
            #endregion
        }
        
        //Is called when exit button on window is clicked
        public void Shutdown()
        {
            //Shutdown code
        }

        //Is called once per frame to update GameObjects
        public void Update()
        {
            #region ---FPS AND DELTA TIME CALCULATION---
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
            #endregion

            #region ---GAME LOGIC---

            #region Tank Firing controls
            if (IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                Vector2 mousePos = GetMousePosition();
                mouse.i = mousePos.x;
                mouse.j = screenHeight - mousePos.y;
                mouse = tankBarrel.globalMatrix.Inverse().Transpose() * mouse;
                float relativeAngle = (float)Math.Atan2(mouse.i, mouse.j);
                barrelVelocity += relativeAngle * deltaTime * 3;
            }
            barrelVelocity *= (float)Math.Pow(0.02f, deltaTime);
            tankBarrel.RotationAngle += barrelVelocity * deltaTime;

            if (barrelFireTimer > 0.1)
                barrelFireTimer -= deltaTime;
            if (tankBarrel.origin.y < 40 * tankBarrel.scale.j2)
                tankBarrel.origin.y += 100 * deltaTime;

            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && barrelFireTimer < 0.1f)
            {
                PlaySound(tankFire);
                tankBarrel.origin.y = 30 * tankBarrel.scale.j2;
                barrelFireTimer = 3;
            }
            #endregion

            #region Tank Movement controls
            //Rotation of tankBody
            if (IsKeyDown(KeyboardKey.KEY_A))
                tankBody.RotationAngle -= 2 * deltaTime;

            if (IsKeyDown(KeyboardKey.KEY_D))
                tankBody.RotationAngle += 2 * deltaTime;

            //Forward and back controls (with momentum)
            if (IsKeyDown(KeyboardKey.KEY_W))
            {
                tankBody.relativePosition = new Vector2(0, 200 * tankBody.longitudinalVelocity * deltaTime);
                if (tankBody.longitudinalVelocity < 1)
                    tankBody.longitudinalVelocity += 0.5f * deltaTime;
            }

            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                tankBody.relativePosition = new Vector2(0, 100 * tankBody.longitudinalVelocity * deltaTime);
                if (tankBody.longitudinalVelocity > -1)
                    tankBody.longitudinalVelocity -= 0.5f * deltaTime;
            }

            //Momentum decay of longitudinal velocity
            if (!IsKeyDown(KeyboardKey.KEY_W) && !IsKeyDown(KeyboardKey.KEY_S) && Math.Abs(tankBody.longitudinalVelocity) > 0)
            {
                tankBody.longitudinalVelocity -= Math.Sign(tankBody.longitudinalVelocity) * deltaTime;
                tankBody.relativePosition = new Vector2(0, (50 * (float)Math.Sign(tankBody.longitudinalVelocity) + 150) * tankBody.longitudinalVelocity * deltaTime);
            }
            #endregion

            world.RecalculateGlobal();
            #endregion
        }

        //Draws GameObjects in their current states
        public void Draw()
        {
            BeginDrawing();

            ClearBackground(rl.Color.WHITE);

            DrawText(fps.ToString(), 100, 100, 30, Color.RED);

            tankBody.DrawObject(Color.WHITE);
            tankBarrel.DrawObject(Color.LIGHTGRAY);

            EndDrawing();
        }
        #endregion
    }
}
