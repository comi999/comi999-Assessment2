using Raylib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Raylib.Raylib;
using static Tank_Game.Tank_Game;
using rl = Raylib;

namespace Tank_Game
{
    using Math_Library;
    using System.CodeDom;
    using System.Diagnostics.Tracing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml.Schema;

    public class Game
    {
        #region ---'GameObject' DECLARATIONS---
        //This list stores all GameObjects in the world. Handy for drawing and updating them all in one go.
        static List<GameObject> gameObjects = new List<GameObject>();

        //This list stores all GameObjects that have collision enabled. This is a subset of the above list.
        static List<GameObject> collisionMap = new List<GameObject>();

        //The following Dictionaries store sounds and textures used in the game. A dictionary is used so the 
        //assets can be called with keywords. This way sprites and sounds don't need to keep getting loaded in.
        static Dictionary<string, Sound> soundAssets = new Dictionary<string, Sound>();
        static Dictionary<string, Texture2D> textureAssets = new Dictionary<string, Texture2D>();

        //The following are GameObjects that are made at the start of the game.
        GameObject world;

        //These are the screen edges to keep things inside the screen.
        ObstacleRectangle screenBoundaryTop;
        ObstacleRectangle screenBoundaryRight;
        ObstacleRectangle screenBoundaryBottom;
        ObstacleRectangle screenBoundaryLeft;

        //This is the middle obstacle
        ObstacleRectangle middle;

        //These are the NPC objects that the player can control
        NPCTank tankBody;
        NPCTankBarrel tankBarrel;
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
        //These ints store the dimensions of a created "game" object, so they can be used in calculating things.
        public static int screenWidth = 0;
        public static int screenHeight = 0;
        #endregion

        #region ---PLACEHOLDER VARIABLES---
        //I made these 'placeHolder' Matrices and Vectors just to save on new instance creation. Pointless
        //for todays computers, but I wanted to practice being efficient. They come in handy for writing
        //values to on the fly and for quick calculations without having to create new instances of things.
        public static Matrix3 placeHolderMatrix = new Matrix3();
        public static Vector3 placeHolderVector1 = new Vector3();
        public static Vector3 placeHolderVector2 = new Vector3();
        public static Vector3 placeHolderVector3 = new Vector3();
        public static Vector3 placeHolderVector4 = new Vector3();
        #endregion
        #endregion

        #region ---'Game' CONSTRUCTORS---
        //Constructors for making a new 'game' instance, and controlling how big the screen needs to be.
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
            public float spriteWidth = 1;
            public float spriteHeight = 1;
            public bool drawObject = true;
            public Color objectColour = Color.WHITE;
            public Vector2 origin = new Vector2(0, 0);

            //I store the local scale, rotation and position in the form of Matrices so they can be multiplied on the
            //fly to generate a global matrix.
            #region Local Matrix parameters
            public Matrix3 position = new Matrix3();
            public Matrix3 rotation = new Matrix3();
            public Matrix3 scale = new Matrix3();
            #endregion

            #region Local Rotation paramters
            public float rotationAngle = 0;

            //The following method allows rotation to be set as a float, and it will automatically update the local rotation matrix.
            public float RotationAngle
            {
                get => rotationAngle;
                set
                {
                    
                    rotationAngle = value;
                    if (rotationAngle > 3.142)
                        rotationAngle -= 6.284f;
                    else if (rotationAngle < -3.142)
                        rotationAngle += 6.284f;
                    rotation.i1 = (float)Math.Cos(value);
                    rotation.j1 = -(float)Math.Sin(value);
                    rotation.i2 = -rotation.j1;
                    rotation.j2 = rotation.i1;
                }
            }
            #endregion
            
            #region Local Scale parameters
            //The following method makes sure an items encompassing radius is update when scale is changed.
            public Matrix3 Scale
            {
                get => scale;
                set
                {
                    scale = value;
                    origin = new Vector2(spriteWidth * scale.i1 * 0.5f, spriteHeight * scale.j2 * 0.5f);
                    RecalculateEncompassingRadius();
                }
            }
            #endregion

            #region Collision parameters
            public bool enableCollision = false;
            public bool drawCollisionBox = false;
            public Color collisionBoxColour = Color.BLACK;

            //Represents corners of object bounding box, starting with top left and going clockwise
            public Vector3[] vertices;

            //EncompassingRadius is used to determine if collision needs to be checked or not. If an object
            //that you're checking collision on is not within range of any other objects, it won't check collision
            //against them.
            public float encompassingRadius;

            //collidedObject stores the GameObject that has been collided with. madeContact is a flag that is set to true
            //if contact has been made with another object. contactPoint, contactP1, contactP2 store the contact vertex, and
            //and the line segment described by P1 and P2. This is useful for incidence reflection etc.
            public GameObject collidedObject;
            public bool madeContact = false;
            public Vector2 contactPoint = new Vector2();
            public Vector3 contactP1 = new Vector3();
            public Vector3 contactP2 = new Vector3();

            //possibleCollisions is the list that contains all of the objects whose encompassing radii you are inside of. These
            //are the objects that collision checks need to be performed on.
            public List<GameObject> possibleCollisions = new List<GameObject>();

            //This is a blacklist of all of the objects that your current object shouldn't collide with. For ex: tankBarrel shouldn't
            //collided with tankBody obviously, so they are added to each others blacklists.
            public List<GameObject> dontCollide = new List<GameObject>();

            //collision parameters are empty if a GameObject is made with enableCollision being set to false. If collision is to be changed
            //later on, it gets done through this method, which will then initialise the corner points of that objects OBB.
            public bool EnableCollision
            {
                set
                {
                    enableCollision = value;
                    if (value)
                    {
                        vertices = new Vector3[4] { new Vector3(), new Vector3(), new Vector3(), new Vector3() };

                        placeHolderMatrix.i1 = 1 / (float)Math.Sqrt(globalMatrix.i1 * globalMatrix.i1 + globalMatrix.j1 * globalMatrix.j1);
                        placeHolderMatrix.i2 = 1 / (float)Math.Sqrt(globalMatrix.i2 * globalMatrix.i2 + globalMatrix.j2 * globalMatrix.j2);

                        placeHolderMatrix.j1 = scale.i1 * spriteWidth * 0.5f;
                        placeHolderMatrix.j2 = scale.j2 * spriteHeight * 0.5f;

                        placeHolderMatrix.k1 = (origin.x - placeHolderMatrix.j1) * globalMatrix.i1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.i2 * placeHolderMatrix.i2;
                        placeHolderMatrix.k2 = (origin.x - placeHolderMatrix.j1) * globalMatrix.j1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.j2 * placeHolderMatrix.i2;


                        vertices[0].i = placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                        vertices[0].j = placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                        vertices[3].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                        vertices[3].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                        vertices[1].i = placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                        vertices[1].j = placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                        vertices[2].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                        vertices[2].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                        RecalculateEncompassingRadius();
                    }
                }
            }
            #endregion

            //These are the basic parameters for a GameObject that ties it to the rest of the world.
            #region Worldspace parameters
            public Matrix3 localMatrix = new Matrix3();
            public Matrix3 globalMatrix = new Matrix3();
            public GameObject parent;
            public List<GameObject> children = new List<GameObject>();
            #endregion
            #endregion

            //These are the contructors for GameObject
            #region  GameObject Constructors
            public GameObject()
            {
                gameObjects.Add(this);
            }

            public GameObject(string sprite, GameObject parent, bool enableCollision, Vector2 initialPosition, float initialRotation, Vector2 initialScale)
            {
                gameObjects.Add(this);
                if (enableCollision)
                    collisionMap.Add(this);
                if (sprite != "")
                {
                    this.sprite = textureAssets[sprite];
                    spriteWidth = this.sprite.width;
                    spriteHeight = this.sprite.height;
                }
                position.i3 = initialPosition.x;
                position.j3 = initialPosition.y;
                RotationAngle = initialRotation;
                Scale = new Matrix3(initialScale.x, 0, 0, 0, initialScale.y, 0, 0, 0, 1);
                origin = new Vector2(spriteWidth * scale.i1 * 0.5f, spriteHeight * scale.j2 * 0.5f);
                
                this.parent = parent;
                if (parent != null)
                    parent.children.Add(this);
                localMatrix = scale * (rotation * position);

                this.enableCollision = enableCollision;
                if (enableCollision)
                {
                    vertices = new Vector3[4] { new Vector3(), new Vector3(), new Vector3(), new Vector3() };

                    placeHolderMatrix.i1 = 1 / (float)Math.Sqrt(globalMatrix.i1 * globalMatrix.i1 + globalMatrix.j1 * globalMatrix.j1);
                    placeHolderMatrix.i2 = 1 / (float)Math.Sqrt(globalMatrix.i2 * globalMatrix.i2 + globalMatrix.j2 * globalMatrix.j2);

                    placeHolderMatrix.j1 = scale.i1 * spriteWidth * 0.5f;
                    placeHolderMatrix.j2 = scale.j2 * spriteHeight * 0.5f;

                    placeHolderMatrix.k1 = (origin.x - placeHolderMatrix.j1) * globalMatrix.i1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.i2 * placeHolderMatrix.i2;
                    placeHolderMatrix.k2 = (origin.x - placeHolderMatrix.j1) * globalMatrix.j1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.j2 * placeHolderMatrix.i2;


                    vertices[0].i = placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[0].j = placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[3].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[3].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[1].i = placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[1].j = placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[2].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[2].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    RecalculateEncompassingRadius();
                }
            }

            public GameObject(string sprite, GameObject parent, bool enableCollision, Vector2 initialPosition, float initialRotation, Vector2 initialScale, Vector2 origin)
            {
                gameObjects.Add(this);
                if (enableCollision)
                    collisionMap.Add(this);
                if (sprite != "")
                {
                    this.sprite = textureAssets[sprite];
                    spriteWidth = this.sprite.width;
                    spriteHeight = this.sprite.height;
                }
                position.i3 = initialPosition.x;
                position.j3 = initialPosition.y;
                RotationAngle = initialRotation;
                Scale = new Matrix3(initialScale.x, 0, 0, 0, initialScale.y, 0, 0, 0, 1);
                this.origin.x = origin.x * scale.i1;
                this.origin.y = origin.y * scale.j2;

                this.parent = parent;
                if (parent != null)
                    parent.children.Add(this);
                localMatrix = scale * (rotation * position);

                this.enableCollision = enableCollision;
                if (enableCollision)
                {
                    vertices = new Vector3[4] { new Vector3(), new Vector3(), new Vector3(), new Vector3() };

                    placeHolderMatrix.i1 = 1 / (float)Math.Sqrt(globalMatrix.i1 * globalMatrix.i1 + globalMatrix.j1 * globalMatrix.j1);
                    placeHolderMatrix.i2 = 1 / (float)Math.Sqrt(globalMatrix.i2 * globalMatrix.i2 + globalMatrix.j2 * globalMatrix.j2);

                    placeHolderMatrix.j1 = scale.i1 * spriteWidth * 0.5f;
                    placeHolderMatrix.j2 = scale.j2 * spriteHeight * 0.5f;

                    placeHolderMatrix.k1 = (origin.x - placeHolderMatrix.j1) * globalMatrix.i1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.i2 * placeHolderMatrix.i2;
                    placeHolderMatrix.k2 = (origin.x - placeHolderMatrix.j1) * globalMatrix.j1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.j2 * placeHolderMatrix.i2;


                    vertices[0].i = placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[0].j = placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[3].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[3].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[1].i = placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[1].j = placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[2].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[2].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    RecalculateEncompassingRadius();
                }
            }
            #endregion

            //This Vector2 allows the setting of a GameObjects position relative to it's direction. This comes
            //in handy when making a tank move for example.
            public Vector2 relativePosition
            {
                set
                {
                    placeHolderMatrix.Reset();
                    placeHolderMatrix.i1 = 0; placeHolderMatrix.j1 = 0; placeHolderMatrix.k1 = 0;
                    placeHolderMatrix.i2 = 0; placeHolderMatrix.j2 = 0; placeHolderMatrix.k2 = 0;
                    placeHolderMatrix.i3 = value.x; placeHolderMatrix.j3 = value.y; placeHolderMatrix.k3 = 0;

                    position += (placeHolderMatrix * rotation);
                }
            }

            //UpdateObject is a virtual function. It's base version has no behaviour as behavious is custom to only the child classes of
            //GameObject.
            public virtual void UpdateObject()
            {

            }

            //This function controls Raylib drawing function and translates the games coordinate system to the Raylib coordinate system
            public virtual void DrawObject()
            {
                DrawTexturePro(sprite, new Rectangle(0, 0, spriteWidth, spriteHeight), new Rectangle(globalMatrix.i3, screenHeight - globalMatrix.j3, spriteWidth * scale.i1, spriteHeight * scale.j2), origin, Math.Atan2(globalMatrix.i2, globalMatrix.j2).ConvertToDegrees(), objectColour);

                if (enableCollision && drawCollisionBox)
                {
                    DrawLineEx(new Vector2((int)vertices[0].i,     screenHeight - (int)vertices[0].j),     new Vector2((int)vertices[1].i,    screenHeight - (int)vertices[1].j),    10, collisionBoxColour);
                    DrawLineEx(new Vector2((int)vertices[1].i,    screenHeight - (int)vertices[1].j),    new Vector2((int)vertices[2].i, screenHeight - (int)vertices[2].j), 10, collisionBoxColour);
                    DrawLineEx(new Vector2((int)vertices[2].i, screenHeight - (int)vertices[2].j), new Vector2((int)vertices[3].i,  screenHeight - (int)vertices[3].j),  10, collisionBoxColour);
                    DrawLineEx(new Vector2((int)vertices[3].i,  screenHeight - (int)vertices[3].j),  new Vector2((int)vertices[0].i,     screenHeight - (int)vertices[0].j),     10, collisionBoxColour);
                    DrawCircleLines((int)globalMatrix.i3, screenHeight - (int)globalMatrix.j3, encompassingRadius, Color.RED);
                    if (madeContact)
                        DrawCircle((int)contactPoint.x, screenHeight - (int)contactPoint.y, 100, Color.RED);
                }
            }

            //This function updates a GameObjects globalMatrix upon being called, and cascades the update down to all of it's children.
            //It also recalculates an Objects OBB corner points as well as all of it's childrens ones too.
            public void RecalculateGlobal()
            {
                if (parent == null)
                    globalMatrix = localMatrix;
                else
                    globalMatrix = scale * (rotation * position) * parent.globalMatrix;

                if (enableCollision)
                {
                    placeHolderMatrix.i1 = 1 / (float)Math.Sqrt(globalMatrix.i1 * globalMatrix.i1 + globalMatrix.j1 * globalMatrix.j1);
                    placeHolderMatrix.i2 = 1 / (float)Math.Sqrt(globalMatrix.i2 * globalMatrix.i2 + globalMatrix.j2 * globalMatrix.j2);

                    placeHolderMatrix.j1 = scale.i1 * spriteWidth * 0.5f;
                    placeHolderMatrix.j2 = scale.j2 * spriteHeight * 0.5f;

                    placeHolderMatrix.k1 = (origin.x - placeHolderMatrix.j1) * globalMatrix.i1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.i2 * placeHolderMatrix.i2;
                    placeHolderMatrix.k2 = (origin.x - placeHolderMatrix.j1) * globalMatrix.j1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.j2 * placeHolderMatrix.i2;


                    vertices[0].i =     placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[0].j =     placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;
                    
                    vertices[3].i =  placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[3].j =  placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[1].i =    placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[1].j =    placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[2].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[2].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;
                }
                foreach (GameObject child in children)
                    child.RecalculateGlobal();
            }

            //This function will recalculate the encompassing radius of attached object, and all of it's children. Called upon a scale change.
            public void RecalculateEncompassingRadius()
            {
                if (enableCollision)
                {
                    encompassingRadius = (float)Math.Sqrt(Math.Max(
                        Math.Max((globalMatrix.i3 - vertices[0].i) *     (globalMatrix.i3 - vertices[0].i) +     (globalMatrix.j3 - vertices[0].j) *     (globalMatrix.j3 - vertices[0].j),
                                 (globalMatrix.i3 - vertices[1].i) *    (globalMatrix.i3 - vertices[1].i) +    (globalMatrix.j3 - vertices[1].j) *    (globalMatrix.j3 - vertices[1].j)),
                        Math.Max((globalMatrix.i3 - vertices[3].i) *  (globalMatrix.i3 - vertices[3].i) +  (globalMatrix.j3 - vertices[3].j) *  (globalMatrix.j3 - vertices[3].j),
                                 (globalMatrix.i3 - vertices[2].i) * (globalMatrix.i3 - vertices[2].i) + (globalMatrix.j3 - vertices[2].j) * (globalMatrix.j3 - vertices[2].j))));
                }
                foreach (GameObject child in children)
                    child.RecalculateEncompassingRadius();
            }

            //Returns true if object has collided with an obstacle.
            public bool CheckCollision(ref GameObject collidedObject)
            {
                bool foundCollision = false;
                Vector3 thisPosition = new Vector3(globalMatrix.i3, globalMatrix.j3, 0);
                placeHolderVector1.k = 0;
                possibleCollisions.Clear();
                
                //Function goes through list of all gameObjects and adds the ones which have encompassing regions that overlap this objects own encompassing region
                //to a list called "possibleCollisions"
                foreach (GameObject gameObject in collisionMap)
                {
                    placeHolderVector1.i = gameObject.globalMatrix.i3;
                    placeHolderVector1.j = gameObject.globalMatrix.j3;

                    if ((placeHolderVector1 - thisPosition).MagnitudeSqrd() < (encompassingRadius * encompassingRadius) + (gameObject.encompassingRadius * gameObject.encompassingRadius) + 2 * encompassingRadius * gameObject.encompassingRadius && gameObject != this && !dontCollide.Contains(gameObject))                        
                        possibleCollisions.Add(gameObject);
                }

                //Now that a list of possible collisions has been constructed, it has to be determined whether or not their OBB's infact do intersect.
                foreach (GameObject gameObject in possibleCollisions)
                {
                    //THIS IS SAT COLLISION DETECTION
                    //First, calculate this objects min and max values in it's own vertical and horizontal projection axes.
                    //Because collision boxes are regular rectangles, there is no need to calculate every corner. The other
                    //vertically alligned corners can be ignored because they cast the same shadow as the ones beneath, or
                    //to the left of it.
                    placeHolderVector1.i = (vertices[2] - vertices[3]).i;
                    placeHolderVector1.j = (vertices[2] - vertices[3]).j;
                    placeHolderVector1.k = 0;
                    float thisMinHorizontal = placeHolderVector1 * vertices[3];
                    float thisMaxHorizontal = placeHolderVector1 * vertices[2];

                    placeHolderVector2.i = (vertices[0] - vertices[3]).i;
                    placeHolderVector2.j = (vertices[0] - vertices[3]).j;
                    placeHolderVector2.k = 0;
                    float thisMinVertical = placeHolderVector2 * vertices[3];
                    float thisMaxVertical = placeHolderVector2 * vertices[0];

                    //The other objects shadows can be set at float.Min and float.Max values so that there is an absolute
                    //guarantee that the mins and maxes will be caught.
                    float otherMinHorizontal = float.MaxValue;
                    float otherMaxHorizontal = float.MinValue;
                    float otherMinVertical = float.MaxValue;
                    float otherMaxVertical = float.MinValue;

                    //Find shadows of other object on this objects projection axes
                    foreach (Vector3 vertex in gameObject.vertices)
                    {
                        otherMinHorizontal = Math.Min(otherMinHorizontal, placeHolderVector1 * vertex);
                        otherMaxHorizontal = Math.Max(otherMaxHorizontal, placeHolderVector1 * vertex);
                        otherMinVertical = Math.Min(otherMinVertical, placeHolderVector2 * vertex);
                        otherMaxVertical = Math.Max(otherMaxVertical, placeHolderVector2 * vertex);
                    }

                    //Compare shadows of each object on both axes and see if they overlap. If there are any overlaps, the check
                    //will continue to further verify if the object is indeed intersecting the other object. If there is at
                    //least one gap, a collision is not possible, and the rest of the check will be aborted, as no need to continue.
                    if (!(thisMaxHorizontal > otherMinHorizontal && otherMaxHorizontal > thisMinHorizontal) || !(thisMaxVertical > otherMinVertical && otherMaxVertical > thisMinVertical))
                    {
                        continue;
                    }

                    //If the collision check gets to this point, that's because there was overlaps in the shadows in the previous
                    //check. Now check for shadows against the other objects projection axes.
                    placeHolderVector3.i = (gameObject.vertices[2] - gameObject.vertices[3]).i;
                    placeHolderVector3.j = (gameObject.vertices[2] - gameObject.vertices[3]).j;
                    placeHolderVector3.k = 0;
                    otherMinHorizontal = placeHolderVector3 * gameObject.vertices[3];
                    otherMaxHorizontal = placeHolderVector3 * gameObject.vertices[2];

                    placeHolderVector4.i = (gameObject.vertices[0] - gameObject.vertices[3]).i;
                    placeHolderVector4.j = (gameObject.vertices[0] - gameObject.vertices[3]).j;
                    placeHolderVector4.k = 0;
                    otherMinVertical = placeHolderVector4 * gameObject.vertices[3];
                    otherMaxVertical = placeHolderVector4 * gameObject.vertices[0];

                    thisMinHorizontal = float.MaxValue;
                    thisMaxHorizontal = float.MinValue;
                    thisMinVertical = float.MaxValue;
                    thisMaxVertical = float.MinValue;
                    
                    //Find shadows of this object on other objects projection axes
                    foreach (Vector3 vertex in vertices)
                    {
                        thisMinHorizontal = Math.Min(thisMinHorizontal, placeHolderVector3 * vertex);
                        thisMaxHorizontal = Math.Max(thisMaxHorizontal, placeHolderVector3 * vertex);
                        thisMinVertical = Math.Min(thisMinVertical, placeHolderVector4 * vertex);
                        thisMaxVertical = Math.Max(thisMaxVertical, placeHolderVector4 * vertex);
                    }
                    if (!(otherMaxHorizontal > thisMinHorizontal && thisMaxHorizontal > otherMinHorizontal) || !(otherMaxVertical > thisMinVertical && thisMaxVertical > otherMinVertical))
                    {
                        continue;
                    }

                    foundCollision = true;
                    collidedObject = gameObject;
                }
                //If the function reaches this point, it's because there was overlaps in all shadows in all tests, and therefore a collision
                //has been found on one of the possibleCollision objects.
                if (foundCollision)
                {
                    collisionBoxColour = Color.RED;
                    madeContact = true;
                }
                else
                {
                    collisionBoxColour = Color.BLACK;
                    madeContact = false;
                }
                return foundCollision;
            }

            public Vector2 FindContactPoint(GameObject collidedObject, ref Vector3 contactP1, ref Vector3 contactP2)
            {
                Vector2 collidingPoint = new Vector2();
                float[,] nearestVertex = new float[1,2];
                float previousDistance = float.MaxValue;

                float thisAngle = (float)Math.Atan2(collidedObject.globalMatrix.i3 - globalMatrix.i3, collidedObject.globalMatrix.j3 - globalMatrix.j3);
                float otherAngle = (float)Math.Atan2(globalMatrix.i3 - collidedObject.globalMatrix.i3, globalMatrix.j3 - collidedObject.globalMatrix.j3);
                float thisAdjAngle = thisAngle - RotationAngle;
                float otherAdjAngle = otherAngle - collidedObject.RotationAngle;
                if (thisAdjAngle < -3.142)
                    thisAdjAngle += 6.284f;
                else if (thisAdjAngle > 3.142)
                    thisAdjAngle -= 6.284f;
                if (otherAdjAngle < -3.142)
                    otherAdjAngle += 6.284f;
                else if (otherAdjAngle > 3.142)
                    otherAdjAngle -= 6.284f;
                float thisRelativeAngle = thisAdjAngle % 3.142f;
                float otherRelativeAngle = otherAdjAngle % 3.142f;

                //Find the closest Vertex of this objects to it's relative angle
                //THIS -> COLLIDED
                int thisClosestVertex = 0;
                if (thisRelativeAngle >= 0 && thisRelativeAngle < Math.PI * 0.5f)
                    thisClosestVertex = 1;
                if (thisRelativeAngle >= Math.PI * 0.5f && thisRelativeAngle < Math.PI)
                    thisClosestVertex = 2;
                if (thisRelativeAngle >= -Math.PI * 0.5f && thisRelativeAngle < 0)
                    thisClosestVertex = 0;
                if (thisRelativeAngle >= -Math.PI && thisRelativeAngle < -Math.PI * 0.5f)
                    thisClosestVertex = 3;

                //Find the closest Vertex of other object to it's relative angle
                //COLLIDED -> THIS
                int otherClosestVertex = 0;
                if (otherRelativeAngle >= 0 && otherRelativeAngle < Math.PI * 0.5f)
                    otherClosestVertex = 1;
                if (otherRelativeAngle >= Math.PI * 0.5f && otherRelativeAngle < Math.PI)
                    otherClosestVertex = 2;
                if (otherRelativeAngle >= -Math.PI * 0.5f && otherRelativeAngle < 0)
                    otherClosestVertex = 0;
                if (otherRelativeAngle >= -Math.PI && otherRelativeAngle < -Math.PI * 0.5f)
                    otherClosestVertex = 3;

                //Once the closes vertices and edges have been found on both objects, perpindicular distances between
                //them all are checked to find the edge and point combo with the shortest distance. This will be the
                //collision point and collision edge.
                for (int j = otherClosestVertex - 1; j < otherClosestVertex + 1; j++)
                {
                    int P1 = (j + 4) % 4;
                    int P2 = (j + 1) % 4;
                    for (int k = thisClosestVertex - 1; k < thisClosestVertex + 2; k++)
                    {
                        int P = (k + 4) % 4;
                        float perpindicularDistance =
                            Math.Abs((collidedObject.vertices[P2].j - collidedObject.vertices[P1].j) * vertices[P].i - (collidedObject.vertices[P2].i - collidedObject.vertices[P1].i) * vertices[P].j + collidedObject.vertices[P2].i * collidedObject.vertices[P1].j - collidedObject.vertices[P2].j * collidedObject.vertices[P1].i) / (collidedObject.vertices[P2] - collidedObject.vertices[P1]).Magnitude();
                        
                        if (perpindicularDistance < previousDistance)
                        {
                            previousDistance = perpindicularDistance;
                            nearestVertex[0, 0] = 1;
                            nearestVertex[0, 1] = P;
                            contactP1 = collidedObject.vertices[P1];
                            contactP2 = collidedObject.vertices[P2];
                        }
                    }
                }

                //Checks continue, but on the other way around.
                for (int j = thisClosestVertex - 1; j < thisClosestVertex + 1; j++)
                {
                    int P1 = (j + 4) % 4;
                    int P2 = (j + 1) % 4;
                    for (int k = otherClosestVertex - 1; k < otherClosestVertex + 2; k++)
                    {
                        int P = (k + 4) % 4;
                        float perpindicularDistance =
                            Math.Abs((vertices[P2].j - vertices[P1].j) * collidedObject.vertices[P].i - (vertices[P2].i - vertices[P1].i) * collidedObject.vertices[P].j + vertices[P2].i * vertices[P1].j - vertices[P2].j * vertices[P1].i) / (vertices[P2] - vertices[P1]).Magnitude();

                        if (perpindicularDistance < previousDistance)
                        {
                            previousDistance = perpindicularDistance;
                            nearestVertex[0, 0] = 2;
                            nearestVertex[0, 1] = P;
                            contactP1 = vertices[P1];
                            contactP2 = vertices[P2];
                        }
                    }
                }
                if (nearestVertex[0, 0] == 2)
                    collidingPoint = new Vector2(collidedObject.vertices[(int)nearestVertex[0, 1]].i, collidedObject.vertices[(int)nearestVertex[0, 1]].j);
                else if (nearestVertex[0, 0] == 1)
                    collidingPoint = new Vector2(vertices[(int)nearestVertex[0,1]].i, vertices[(int)nearestVertex[0, 1]].j);
                return collidingPoint;
            }
        }

        //GameObject subclass for the controllable tankBody.
        public class NPCTank : GameObject
        {
            public NPCTank(string sprite, GameObject parent, bool enableCollision, Vector2 initialPosition, float initialRotation, Vector2 initialScale) :
                base(sprite, parent, enableCollision, initialPosition, initialRotation, initialScale)
            { }

            //Longitudinal velocity is used to describe tank momentum. If greater than 1, the tank is moving forward, if less than 1, moving backwards.
            //If 0, of course not moving.
            public float longitudinalVelocity = 0;
            public int lastDirection = 0;

            public override void UpdateObject()
            {
                //Rotation of NPCTank
                if (IsKeyDown(KeyboardKey.KEY_A))
                {
                    RotationAngle -= 2 * game.deltaTime;
                }

                if (IsKeyDown(KeyboardKey.KEY_D))
                {
                    RotationAngle += 2 * game.deltaTime;
                }

                //Forward and back controls (with momentum)
                if (IsKeyDown(KeyboardKey.KEY_W) && !madeContact)
                {
                    relativePosition = new Vector2(0, 200 * longitudinalVelocity * game.deltaTime);
                    
                    //Velocity is added to each time the button is detected down. It's capped at a scalar max of 1.
                    if (longitudinalVelocity < 1)
                        longitudinalVelocity += 0.5f * game.deltaTime;
                }

                if (IsKeyDown(KeyboardKey.KEY_S) && !madeContact)
                {
                    relativePosition = new Vector2(0, 100 * longitudinalVelocity * game.deltaTime);
                    if (longitudinalVelocity > -1)
                        longitudinalVelocity -= 0.5f * game.deltaTime;
                }

                //Momentum decay of longitudinal velocity. If neither back nor forward controls are pressed, and the tank still has velocity
                //then velocity is eaten away from, until it goes to 0.
                if (!IsKeyDown(KeyboardKey.KEY_W) && !IsKeyDown(KeyboardKey.KEY_S) && Math.Abs(longitudinalVelocity) > 0)
                {
                    longitudinalVelocity -= Math.Sign(longitudinalVelocity) * game.deltaTime;
                    relativePosition = new Vector2(0, (50 * (float)Math.Sign(longitudinalVelocity) + 150) * longitudinalVelocity * game.deltaTime);
                }

                //This sets the lastDirection flag to either 1 or -1. That allows determination direction of position correction upon collision.
                //It's a dirty way to correct for collision, but it works.
                if (longitudinalVelocity > 0)
                    lastDirection = 1;
                else if (longitudinalVelocity < 0)
                    lastDirection = -1;
                
                //Collision is checked, and the collidedObject, contactPoint, contactP1, contactP2 are all updated and stored.
                if (CheckCollision(ref collidedObject))
                {
                    madeContact = true;
                    contactPoint = FindContactPoint(collidedObject, ref contactP1, ref contactP2);
                }
                else
                    madeContact = false;

                //Based on which direction the tank was moving before it made contact, will determine which type of position correction is used.
                //If moving forward, it'll move back, and the other way around.
                if (lastDirection == 1 && madeContact)
                {
                    longitudinalVelocity = 0;
                    relativePosition = new Vector2(0, -5);
                }
                else if (lastDirection == -1 && madeContact)
                {
                    longitudinalVelocity = 0;
                    relativePosition = new Vector2(0, 5);
                }
            }
        }

        //GameObject for the tankBarrel
        public class NPCTankBarrel : GameObject
        {
            public NPCTankBarrel(string sprite, GameObject parent, bool enableCollision, Vector2 initialPosition, float initialRotation, Vector2 initialScale, Vector2 origin) :
                base(sprite, parent, enableCollision, initialPosition, initialRotation, initialScale, origin)
            { }

            //barrelVelocity stores the rotation speed of the tankBarrel. The barrelFireTimer is used to prevent tank fire spamming.
            float barrelVelocity = 0;
            float barrelFireTimer = 0;
            public override void UpdateObject()
            {
                //The mouse position is calculated and stored in normal coordinate space, where bottomleft is 0,0.
                Vector2 mousePos = GetMousePosition();
                mousePos.y = screenHeight - mousePos.y;

                //The following code fires upon Right clicking. This causes the barrel to track the mouse. This is done
                //by multipling the mouseVector by the inverse of the tankBarrel's globalMatrix. This brings the mouse into 
                //tankBarrel space, and allows for easy angle calculation.
                if (IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
                {
                    placeHolderVector1.Reset();
                    placeHolderVector1.i = mousePos.x;
                    placeHolderVector1.j = mousePos.y;
                    placeHolderVector1.k = 1;
                    placeHolderVector1 = globalMatrix.Inverse().Transpose() * placeHolderVector1;
                    barrelVelocity += (float)Math.Atan2(placeHolderVector1.i, placeHolderVector1.j) * game.deltaTime * 3;
                }
                //After rotation velocity is calculated, it is applied to the rotation angle of the barrel. It slows down as it gets closer to the mouse.
                barrelVelocity *= (float)Math.Pow(0.02f, game.deltaTime);
                RotationAngle += barrelVelocity * game.deltaTime;

                //The following code resets the barrel position and timer if they are not at their initial values.
                if (barrelFireTimer > 0.1)
                    barrelFireTimer -= game.deltaTime;
                if (origin.y < 40 * scale.j2)
                    origin.y += 100 * game.deltaTime;

                //Upon clicking left mouse button, the tank firing sound is played, the tankbarrel moves backwards, and the timer is set to 3 seconds.
                //Also, a tank shell is spawned at the end of the tankbarrel.
                if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && barrelFireTimer < 0.1f)
                {
                    PlaySound(soundAssets["tankFiring"]);
                    origin.y = 30 * scale.j2;
                    barrelFireTimer = 3;

                    float tankBarrelRotation = (float)Math.Atan2(game.tankBarrel.globalMatrix.i2, game.tankBarrel.globalMatrix.j2);
                    Vector2 tankBarreltip = new Vector2(game.tankBarrel.globalMatrix.i3 + 40 * (float)Math.Sin(tankBarrelRotation), game.tankBarrel.globalMatrix.j3 + 40 * (float)Math.Cos(tankBarrelRotation));
                    TankRound newShell = new TankRound(tankBarreltip, new Vector2(1, 1), 1);
                    newShell.velocity.i = (float)Math.Sin(tankBarrelRotation);
                    newShell.velocity.j = (float)Math.Cos(tankBarrelRotation);
                    newShell.RotationAngle = (float)Math.Atan2(game.tankBarrel.globalMatrix.i2, game.tankBarrel.globalMatrix.j2);
                    newShell.objectColour = Color.GRAY;
                    newShell.dontCollide.Add(game.tankBarrel);
                    newShell.dontCollide.Add(game.tankBody);
                    game.tankBody.dontCollide.Add(newShell);
                    game.tankBarrel.dontCollide.Add(newShell);
                }
            }
        }

        //This class spawns a tankShell. They have a max bounce limit before they spawn an explosion.
        public class TankRound : GameObject
        {
            public TankRound(Vector2 initialPosition, Vector2 initialScale, int maxBounce) :
                base("tankShell", game.world, true, initialPosition, 0, initialScale)
            {
                this.maxBounce = maxBounce;
            }
            public Vector3 velocity = new Vector3();
            public int maxBounce = 0;
            public int bounces = 0;

            public override void UpdateObject()
            {
                CheckCollision(ref collidedObject);
                if (!madeContact)
                {
                    //if no collision has been found, the tankShell continues on it's way with it's current velocity.
                    position += new Matrix3(new Vector3(), new Vector3(), 10000 * game.deltaTime * velocity);
                }
                else if (bounces < maxBounce)
                {
                    //if an impact is detected, and it's below it's bounce limit, it'll find the edge that it impacted, and calculate a reflection vector.
                    //After that, it will change it's velocity to that of the reflection vector and also rotate into that new vector.
                    bounces++;
                    FindContactPoint(collidedObject, ref contactP1, ref contactP2);
                    velocity = velocity - 2 * (new Vector3(-(contactP2 - contactP1).j, (contactP2 - contactP1).i, 0).Normalise() * velocity) * new Vector3(-(contactP2 - contactP1).j, (contactP2 - contactP1).i, 0).Normalise();
                    RotationAngle = (float)Math.Atan2(velocity.i, velocity.j);
                    position += new Matrix3(new Vector3(), new Vector3(), 10000 * game.deltaTime * velocity);
                }
                else
                {
                    //If a tankShell has reached it's bounce limit, it will delete itself from the world children list, the gameObjects list, and also
                    //the don't collide lists. After no points exist anymore, the garbage collection will take care of the rest.
                    //A tankImpact will be spawned at the location of it's last impact.
                    game.world.children.Add(new TankImpact(new Vector2(globalMatrix.i3, globalMatrix.j3)));
                    game.world.children.Remove(this);
                    gameObjects.Remove(this);
                    game.tankBody.dontCollide.Remove(this);
                    game.tankBarrel.dontCollide.Remove(this);
                }
            }
        }

        //TankImpact just spawns an explosion sprite that grows in size, and then decreases in size until it's animation has finished.
        //After that, it'll delete itself from all lists containing it, and vanish from existance.
        public class TankImpact : GameObject
        {
            public TankImpact(Vector2 initialPosition) :
                base("tankImpact", game.world, false, initialPosition, 0, new Vector2(1, 1))
            { }
            public int frame = 0;

            public override void UpdateObject()
            {
                if (frame < 15)
                {
                    frame++;
                    Scale += new Matrix3(2 * game.deltaTime, 0, 0, 0, 2 * game.deltaTime, 0, 0, 0, 0);
                }
                else if (frame >= 15 && frame < 30)
                {
                    frame++;
                    Scale -= new Matrix3(2 * game.deltaTime, 0, 0, 0, 2 * game.deltaTime, 0, 0, 0, 0);

                }
                else
                {
                    game.world.children.Remove(this);
                    gameObjects.Remove(this);
                }

                
            }
        }

        //This was a small obstacle type that didn't get completed. Circular collision is a whole new level.
        public class ObstacleCircle : GameObject
        {
            public ObstacleCircle(GameObject parent, bool enableCollision, Vector2 initialPosition, float initialRadius)
            {
                gameObjects.Add(this);
                if (enableCollision)
                    collisionMap.Add(this);
                position.i3 = initialPosition.x;
                position.j3 = initialPosition.y;
                Scale = new Matrix3(initialRadius, 0, 0, 0, initialRadius, 0, 0, 0, 1);

                this.parent = parent;
                if (parent != null)
                    parent.children.Add(this);
                localMatrix = scale * position;

                this.enableCollision = enableCollision;
                if (enableCollision)
                {
                    //WIP
                }
            }
            public override void DrawObject()
            {
                DrawCircle((int)globalMatrix.i3, screenHeight - (int)globalMatrix.j3, scale.i1, objectColour);
            }
        }

        //This obstacle type is used for gameBoundaries and any other random obstacles.
        public class ObstacleRectangle : GameObject
        {
            public ObstacleRectangle(GameObject parent, bool enableCollision, Vector2 initialPosition, float initialRotation, Vector2 initialScale)
            {
                if (enableCollision)
                    collisionMap.Add(this);
                position.i3 = initialPosition.x;
                position.j3 = initialPosition.y;
                RotationAngle = initialRotation;
                Scale = new Matrix3(initialScale.x, 0, 0, 0, initialScale.y, 0, 0, 0, 1);
                origin.x = 0.5f * scale.i1;
                origin.y = 0.5f * scale.j2;

                this.parent = parent;
                if (parent != null)
                    parent.children.Add(this);
                localMatrix = scale * (rotation * position);
                RecalculateGlobal();

                this.enableCollision = enableCollision;
                if (enableCollision)
                {
                    vertices = new Vector3[4] { new Vector3(), new Vector3(), new Vector3(), new Vector3() };

                    placeHolderMatrix.i1 = 1 / (float)Math.Sqrt(globalMatrix.i1 * globalMatrix.i1 + globalMatrix.j1 * globalMatrix.j1);
                    placeHolderMatrix.i2 = 1 / (float)Math.Sqrt(globalMatrix.i2 * globalMatrix.i2 + globalMatrix.j2 * globalMatrix.j2);

                    placeHolderMatrix.j1 = scale.i1 * spriteWidth * 0.5f;
                    placeHolderMatrix.j2 = scale.j2 * spriteHeight * 0.5f;

                    placeHolderMatrix.k1 = (origin.x - placeHolderMatrix.j1) * globalMatrix.i1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.i2 * placeHolderMatrix.i2;
                    placeHolderMatrix.k2 = (origin.x - placeHolderMatrix.j1) * globalMatrix.j1 * placeHolderMatrix.i1 + (origin.y - placeHolderMatrix.j2) * globalMatrix.j2 * placeHolderMatrix.i2;


                    vertices[0].i = placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[0].j = placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[3].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[3].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 - placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[1].i = placeHolderMatrix.k1 + globalMatrix.i3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[1].j = placeHolderMatrix.k2 + globalMatrix.j3 + placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    vertices[2].i = placeHolderMatrix.k1 + globalMatrix.i3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.i2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.i1;
                    vertices[2].j = placeHolderMatrix.k2 + globalMatrix.j3 - placeHolderMatrix.i2 * placeHolderMatrix.j2 * globalMatrix.j2 + placeHolderMatrix.i1 * placeHolderMatrix.j1 * globalMatrix.j1;

                    RecalculateEncompassingRadius();
                }
            }
            
            public override void DrawObject()
            {
                DrawRectanglePro(new Rectangle(globalMatrix.i3, screenHeight - globalMatrix.j3, scale.i1, scale.j2), new Vector2(scale.i1 * 0.5f, scale.j2 * 0.5f), Math.Atan2(globalMatrix.i2, globalMatrix.j2).ConvertToDegrees(), objectColour);
                if (enableCollision && drawCollisionBox)
                {
                    DrawLineEx(new Vector2((int)vertices[0].i,     screenHeight - (int)vertices[0].j),     new Vector2((int)vertices[1].i,    screenHeight - (int)vertices[1].j),    10, collisionBoxColour);
                    DrawLineEx(new Vector2((int)vertices[1].i,    screenHeight - (int)vertices[1].j),    new Vector2((int)vertices[2].i, screenHeight - (int)vertices[2].j), 10, collisionBoxColour);
                    DrawLineEx(new Vector2((int)vertices[2].i, screenHeight - (int)vertices[2].j), new Vector2((int)vertices[3].i,  screenHeight - (int)vertices[3].j),  10, collisionBoxColour);
                    DrawLineEx(new Vector2((int)vertices[3].i,  screenHeight - (int)vertices[3].j),  new Vector2((int)vertices[0].i,     screenHeight - (int)vertices[0].j),     10, collisionBoxColour);
                    DrawCircleLines((int)globalMatrix.i3, screenHeight - (int)globalMatrix.j3, encompassingRadius, Color.RED);
                }
            }
        }
        #endregion

        #region ---'Game' FUNCTIONS---
        //This function is called once on 'game' creation and sets up all gameObjects and assets and anything else that needs to exist for the running of 'game'.
        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("Stopwatch high-resolution frequency: {0} ticks per second", Stopwatch.Frequency);
            }

            InitWindow(screenWidth, screenHeight, "Tank Game");
            InitAudioDevice();

            //This is where assets are loaded into the Asset libraries
            textureAssets.Add("tankBody", LoadTextureFromImage(LoadImage("../Images/tankBlue_outline.png")));
            textureAssets.Add("tankBarrel", LoadTextureFromImage(LoadImage("../Images/barrelBlue.png")));
            textureAssets.Add("tankShell", LoadTextureFromImage(LoadImage("../Images/tankShell.png")));
            textureAssets.Add("tankImpact", LoadTextureFromImage(LoadImage("../Images/tankImpact.png")));

            soundAssets.Add("tankFiring", LoadSound("../Images/tankFiring.wav"));

            //This is where all GameObjects are defined.
            # region ---GAME OBJECTS---
            world = new GameObject("", null, false, new Vector2(0, 0), 0, new Vector2(1, 1));

            screenBoundaryTop = new ObstacleRectangle(world, true, new Vector2(screenWidth * 0.5f, screenHeight), 0, new Vector2(screenWidth, 1));
            screenBoundaryRight = new ObstacleRectangle(world, true, new Vector2(screenWidth, screenHeight * 0.5f), 0, new Vector2(1, screenHeight));
            screenBoundaryBottom = new ObstacleRectangle(world, true, new Vector2(screenWidth * 0.5f, 0), 0, new Vector2(screenWidth, 1));
            screenBoundaryLeft = new ObstacleRectangle(world, true, new Vector2(0, screenHeight * 0.5f), 0, new Vector2(1, screenHeight));

            middle = new ObstacleRectangle(world, true, new Vector2(screenWidth * 0.5f, screenHeight * 0.5f), 0, new Vector2(400, 400));

            tankBody = new NPCTank("tankBody", world, true, new Vector2(200, 200), 0, new Vector2(3, 3));
            
            tankBarrel = new NPCTankBarrel("tankBarrel", tankBody, true, new Vector2(0, 0), 0, new Vector2(3, 5), new Vector2(8, 40));

            middle.objectColour = Color.BLACK;

            tankBody.dontCollide.Add(tankBarrel);
            tankBarrel.dontCollide.Add(tankBody);
            #endregion
        }

        //Is called when exit button on window is clicked
        public void Shutdown()
        {
            CloseWindow();
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
            deltaTime = Math.Min( 0.16666667f, deltaTime );
            #endregion

            #region ---GAME LOGIC---
            for (int i = gameObjects.Count - 1; i >= 0; i-- )
                gameObjects[i].UpdateObject();

            //F turns on debugging info. It displaced encompassing regions, collision boxes, and contact points.
            if (IsKeyPressed(KeyboardKey.KEY_F))
            {
                foreach (GameObject gameObject in gameObjects)
                    gameObject.drawCollisionBox = !gameObject.drawCollisionBox;
            }
            world.RecalculateGlobal();
            #endregion
        }

        //Draws GameObjects in their current states
        public void Draw()
        {
            BeginDrawing();

            ClearBackground(rl.Color.WHITE);

            DrawText(fps.ToString(), 100, 100, 30, Color.RED);

            foreach (GameObject gameObject in gameObjects)
                gameObject.DrawObject();

            EndDrawing();
        }
        #endregion
    }
}
