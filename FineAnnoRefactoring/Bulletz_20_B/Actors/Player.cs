using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph02;
using Aiv.Audio;

namespace ProgettoFinale
{
    class Player : Actor
    {
        protected bool isTransformed;
        protected bool isReversed;


        protected Controller controller;


        public NodePath Path { get { return path; } set { path = value; } }

        protected NodePath path;
        protected NodeInfo nextNode;


        protected Animation animation;
        protected bool mutated;
        public World humanWorld;
        public World dogWorld;
        public bool IsSearching { get { return isSearchPressed; } set { isSearchPressed = value; } }

        protected List<Node> multipleTasks;
        private bool isSearching;
        private bool isSearchPressed;

        public bool PerformSearch;
        private bool click;
        private int counter = 0;
        protected bool isPorted;
        private bool numberPicked;
        private bool keyNotFound = true;
        private Vector2 offSet;

        protected AudioSource source;
        protected AudioSource walkingSource;



        protected Items key; //è un bene che la crei il player e non la play scene?
        public bool CanOpenDoor { get { return canOpenDoor; } set { canOpenDoor = value; } }
        public bool canOpenDoor;

        public bool IsPorted { get { return isPorted; } set { isPorted = value; } }
        public Vector2 Scale { get { return sprite.scale; } set { sprite.scale = value; } }
        public int PlayerID { get; protected set; }



        public Player(Vector2 position, World world, World dogWorld, Controller ctrl, int playerID = 0) : base("player", world, w: Game.PixelsToUnits(32), h: Game.PixelsToUnits(32))
        {

            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType((uint)RigidBodyType.Items);

            animation = new Animation(this, 16, 16, 4, 12, true);
            components.Add("Animation", animation);

            this.dogWorld = dogWorld;
            humanWorld = worldGrid;

            RigidBody.Friction = 20;

            sprite.position = position;

            offSet = new Vector2(Game.PixelsToUnits(33.75f), 0);

            Speed = 5;

            IsActive = true;

            PlayerID = playerID;

            controller = ctrl;
            multipleTasks = new List<Node>();

            //source = new AudioSource();
            //walkingSource = new AudioSource();


        }


        public void Input()
        {
            if (Velocity == Vector2.Zero)
            {
                if (!mutated)
                {

                    if (controller.Transform())
                    {
                        if (!isTransformed)
                        {
                            isTransformed = true;
                            TransformToAnimal();
                        }
                    }
                    else if (isTransformed)
                    {
                        isTransformed = false;
                    }
                }
                else
                {
                    if (controller.Search())
                    {
                        if (!isSearching)
                        {

                            isSearching = true;
                        }
                    }
                    else if (isSearching)
                    {
                        isSearching = false;
                        isSearchPressed = true;//using this to check in playscene if player is allowed to search 
                    }

                    if (controller.ReverseTransformation())
                    {
                        if (!isReversed)
                        {
                            isReversed = true;
                            ReverseToHuman();
                        }
                    }
                    else if (isReversed)
                    {
                        isReversed = false;
                    }
                }
            }
        }

        public bool Research()
        {
            Vector2 maxLimits = dogWorld.ScanLimits(Position, 1);
            Vector2 minLimits = dogWorld.ScanLimits(Position, -1);
            int randomSearch = 4;
            Node randomNode;

            for (int i = 0; i < randomSearch; i++)
            {
                randomNode = dogWorld.GetRandomFreeNode(minLimits, maxLimits);
                multipleTasks.Add(randomNode);
            }
            return true;
        }

        private void TransformToAnimal()
        {
            mutated = true;
            texture = GfxMgr.GetTexture("Dog");
            Speed = 7;
            //make a new GridMap with different obstacle.
            worldGrid = dogWorld;
            //PlaySound("PickUp");
        }


        private void ReverseToHuman()
        {
            mutated = false;
            texture = GfxMgr.GetTexture("player");

            worldGrid = humanWorld;
            Speed = 5;
            //source.Play(AudioMgr.GetClip("PickUp"));
            //PlaySound("PickUp");
            if (!worldGrid.CheckIfEmpty(Position))//avoiding players to transform back to human on place they should not be
            {
                TransformToAnimal();
            }
        }

        public void clickInput()
        {

            Vector2 mousePosition = CameraMgr.MainCamera.position - CameraMgr.MainCamera.pivot + Game.Win.mousePosition;
            StaticMouse.ChangeTexture(worldGrid.CheckIfEmpty(mousePosition));
            if (Game.Win.mouseLeft && !click)
            {
                click = true;
                Node selectedNode;

                if (worldGrid.CheckIfEmpty(mousePosition, out selectedNode))
                {
                    isPorted = false;
                    BuildPath(selectedNode);
                }
            }
            if (!Game.Win.mouseLeft && click)
            {
                click = false;
            }

        }


        protected void PerformMoving()
        {
            FollowPath(Speed);
        }

        public virtual bool BuildPath(Node endNode)
        {
            Node startNode = worldGrid.GetNodeAtPosition(Position);



            path = GreedyAlgo.AStar_ShortestPath(startNode, endNode);


            if (path == null || path.Length() == 0)
            {
                return false;
            }
            
            if (path.Length() > 1)
            {//go to the second node
                nextNode.SetNode(path.At(1), 1);
            }
            else
            {//just one node
                nextNode.SetNode(path.At(0), 0);
            }

            return true;
        }

        public virtual bool FollowPath(float followSpeed)
        {
            if (path != null && path.Length() > 0)
            {
                Vector2 nodeDist = nextNode.Position - Position;


                float deltaMovX = nextNode.Position.X - Position.X;
                float deltaMovY = nextNode.Position.Y - Position.Y;

                if (isPorted)
                {
                    return true;
                }

                if (nodeDist.Length <= 0.2f)
                {
                    //it's arrived to the node
                    if (nextNode.Index < path.Length() - 1)
                    {

                        //get new next node
                        int newIndex = nextNode.Index + 1;
                        nextNode.SetNode(path.At(newIndex), newIndex);
                        nodeDist = nextNode.Position - Position;
                        deltaMovX = nextNode.Position.X - Position.X;
                        deltaMovY = nextNode.Position.Y - Position.Y;
                        numberPicked = false;
                    }
                    else
                    {

                        FindingKey();
                        //it's the last node
                        Velocity = Vector2.Zero;
                        TextureOffsetX = 0;
                        animation.Stop();
                        animation.IsActive = false;
                        return true;
                    }
                }
                //PlaySound("Land");
                PlayAnimation();
                WalkingTextureSelection(deltaMovX, deltaMovY);
                Velocity = nodeDist.Normalized() * followSpeed;
            }
            return false;
        }

        private void PlaySound(string name)
        {

            if (!walkingSource.IsPlaying)
            {
                walkingSource.Play(AudioMgr.GetClip(name));
            }
            else if(name != "Land") //GESTISCO IL CASO CAMMINARE PERCHè è L'UNICO SUONO IN LOOP CHE HA IL PLAYER
            {      
                source.Play(AudioMgr.GetClip(name));
            }
        }

        private void FindingKey()
        {
            if (PerformSearch && !numberPicked)
            {
                int random = RandomGenerator.GetRandomInt(0, 3);
                if (random == 2 && keyNotFound)
                {
                    PerformSearch = false;
                    multipleTasks.Clear();
                    counter = 0;
                    numberPicked = true;
                    Vector2 posCheck = Position + offSet;

                    PlaySound("keyFound");
                    if (!worldGrid.CheckIfEmpty(posCheck))
                    {
                        //SAREBBE MEGLIO CHE UN MGR DI OGGETTI GESTISSE QUESTA COSA, MA HO UN SOLO OGGETTO. IDEM LA CLASSE ITEMS PROBABILMENTE DOVREBBE AVERE FIGLI "KEY" PIUTTOSTO CHE "ALTRO"
                        posCheck = Position - offSet;
                        key = new Items("skullKey", posCheck);
                    }
                    else
                    {
                        key = new Items("skullKey", posCheck);
                    }
                    keyNotFound = false;
                }
            }
        }


        private void PlayAnimation()
        {
            if (!animation.IsPlaying)
            {
                animation.Play();
                animation.IsActive = true;
            }
        }

        private void WalkingTextureSelection(float deltaMovX, float deltaMovY)
        {
            if (Math.Abs(deltaMovX) > Math.Abs(deltaMovY))
            {
                if (!mutated)
                {
                    texture = GfxMgr.GetTexture("WalkR");
                }
                else
                {
                    texture = GfxMgr.GetTexture("DogWalkR");
                }
                //Is Moving left or right
                if (deltaMovX > 0)
                {//moving right
                    sprite.FlipX = false;
                }
                else
                {
                    sprite.FlipX = true;
                }
            }
            else
            {
                //is moving up or down
                if (deltaMovY > 0)
                {//moving down
                    if (!mutated) texture = GfxMgr.GetTexture("WalkD");
                    else texture = GfxMgr.GetTexture("DogWalkD");
                }
                else
                {
                    if (!mutated) texture = GfxMgr.GetTexture("WalkU");
                    else texture = GfxMgr.GetTexture("DogWalkU");
                }

            }
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();

                if (path == null)
                {
                    animation.Stop();
                }

                if (sprite.position.X - sprite.pivot.X < 0)
                {//left edge
                    sprite.position.X = sprite.pivot.X;
                }
                else if (sprite.position.X + sprite.pivot.X >= Game.Win.OrthoWidth * 1.95f)
                {//right edge
                    sprite.position.X = Game.Win.OrthoWidth * 1.95f - sprite.pivot.X;
                }

                if (PerformSearch)
                {
                    if (Velocity == Vector2.Zero)
                    {
                        if (counter <= multipleTasks.Count - 1)
                        {

                            BuildPath(multipleTasks[counter]);
                            counter++;
                        }
                        else
                        {
                            multipleTasks.Clear();
                            counter = 0;
                            PerformSearch = false;
                        }
                    }
                }
                PerformMoving();
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Items)
            {
                CollectItem((Items)collisionInfo.Collider);
                canOpenDoor = true;
            }
        }

        private void CollectItem(Items item)
        {
            item.IsActive = false;
        }

        public override void Draw()
        {
            if (IsActive) sprite.DrawTexture(texture, TextureOffsetX, TextureOffsetY, 16, 16);//ricorda 16 16 è la dimensione della texture che rimane piccola, 
        }

        public override void OnDie()
        {
            //Game.IsRunning = false;
            IsActive = false;
        }
    }
}
