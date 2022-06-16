using Aiv.Audio;
using Aiv.Fast2D;
using Graph02;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    class PlayScene : Scene
    {

        public bool isActive;
        protected Player player;

        protected List<TileObj> tileObjs;

        public Player Player { get { return player; } set { player = value; } }



        protected Layer collidableLayer;

        protected World worldGrid;
        protected World dogWorldGrid;

        public bool IsAlreadyStarted;
        private SpawnPoint spawn;
        public Dictionary<string, SpawnPoint> portals;
        private bool GameOver;
        public bool mainScene;
        protected AudioSource bgSource;
        protected AudioClip bgClip;
        protected List<SearchAreaTile> zone;
        private bool firstTime;

        public PlayScene(string map, bool mainScene, string mainTheme)
        {

            TmxReader reader = new TmxReader(map);
            TileSet ts = reader.TileSet;
            List<Layer> layers = reader.Layers;
            portals = new Dictionary<string, SpawnPoint>();
            this.mainScene = mainScene;

            GfxMgr.AddTexture(ts.ImgPath, ts.ImgPath);
            tileObjs = new List<TileObj>();
            zone = new List<SearchAreaTile>();
            CameraMgr.Init();//lo inizializzo vuoto per non avere problemi con il mouse statico. Poi lo start gestirà meglio la cosa.

            //bgSource = new AudioSource();
            bgClip = new AudioClip(mainTheme);
            //bgSource.Volume = 0.3f;

            BuildWorlds(layers);

        }

        private void BuildWorlds(List<Layer> layers)
        {
            foreach (Layer each in layers)
            {
                AddTilesFor(each, tileObjs);
                if (each.Props.Has("collidable"))
                {
                    collidableLayer = each;
                    worldGrid = new World();
                    worldGrid.Init(collidableLayer);
                }
                if (each.Props.Has("dogWorld"))
                {
                    dogWorldGrid = new World();
                    dogWorldGrid.Init(each);
                }
            }
        }

        public override void OnEnter()
        {
            //bgSource.Play(bgClip, true);
            if (IsAlreadyStarted)
            {
                for (int i = 0; i < tileObjs.Count; i++)
                {
                    tileObjs[i].IsActive = true;
                }
                player.IsActive = true;
                IsPlaying = true;
                CameraMgr.Target = player; //a rigore dovrei salvarmi il player nel game(probabilmente pure meglio crearlo in game) in modo che al cambio della scena lo estrapolo e lo passo alla scena successiva in modo da non perdere informazioni
            }
            else
            {
                Start();
            }
        }

        public override void Start()
        {
            IsAlreadyStarted = true;
            LoadTextures();
            LoadClip();
            for (int i = 0; i < tileObjs.Count; i++)
            {
                tileObjs[i].IsActive = true;
            }

            Vector2 playerPos = spawn.Position;


            CameraMgr.Init(playerPos, new Vector2(Game.Win.OrthoWidth * 0.5f, Game.Win.OrthoHeight * 0.5f));
            CameraMgr.Behaviour = FollowBehaviour.FollowTarget;

            player = new Player(playerPos, worldGrid, dogWorldGrid, Game.GetController(0), 0);


            CameraMgr.Target = player;

            base.Start();
        }


        protected virtual void LoadTextures()
        {

            GfxMgr.AddTexture("player", "Assets/PlayerAssets/HEROS_PixelPackTOPDOWN8BIT_Adventurer Idle R.gif");

            //GUI
            GfxMgr.AddTexture("cursorBlue", "Assets/Arrow_blue.png");
            GfxMgr.AddTexture("cursorRed", "Assets/Arrow_Red.png");




            GfxMgr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMgr.AddTexture("blueBar", "Assets/loadingBar_bar.png");




            GfxMgr.AddTexture("WalkD", "Assets/PlayerAssets/WalkD.png");
            GfxMgr.AddTexture("WalkR", "Assets/PlayerAssets/WalkR.png");
            GfxMgr.AddTexture("WalkU", "Assets/PlayerAssets/WalkU.png");

            //ITEM
            GfxMgr.AddTexture("Treasure", "Assets/Items/chest.png");
            GfxMgr.AddTexture("skullKey", "Assets/Items/skullKey.png");

            //DOGGO
            GfxMgr.AddTexture("DogWalkD", "Assets/PetAssets/DogWalkD.png");
            GfxMgr.AddTexture("DogWalkU", "Assets/PetAssets/DogWalkU.png");
            GfxMgr.AddTexture("DogWalkR", "Assets/PetAssets/DogWalkR.png");
            GfxMgr.AddTexture("Dog", "Assets/PetAssets/DogIdle.gif");


        }
        protected void LoadClip()
        {
            AudioMgr.AddClip("Fall", "Assets/audio/Fall01.wav");
            AudioMgr.AddClip("DoorOpen", "Assets/audio/DoorOpen.wav");
            AudioMgr.AddClip("keyFound", "Assets/audio/keyFound.wav");
            AudioMgr.AddClip("Land", "Assets/audio/Land01.wav");
            AudioMgr.AddClip("PickUp", "Assets/audio/Pickup01.wav");
        }
        public override void Update()
        {


            PhysicsMgr.Update();
            UpdateMgr.Update();
            PhysicsMgr.CheckCollisions();
            CameraMgr.Update();

            SearchingZone();

            foreach (var portal in portals)
            {
                portal.Value.Update();
            }

            if (player.CanOpenDoor) // CHANGING WEIGHT OF DOOR NODE TO BE CLICKABLE
            {
                if (!firstTime)
                {
                    Node n = worldGrid.GetNodeAtPosition(portals["changeScene"].Position);
                    player.humanWorld.ChangeWeight(n, 1);
                    player.dogWorld.ChangeWeight(n, 1);
                    firstTime = true;
                }
            }

            if (portals.ContainsKey("changeScene"))
            {

                if (portals["changeScene"].ChangeScene)
                {
                    IsPlaying = false;
                    portals["changeScene"].ChangeScene = false;
                }
            }
        }

        private void SearchingZone()
        {
            if (player.IsSearching)
            {
                Node playerNode = dogWorldGrid.GetNodeAtPosition(player.Position);
                Node check;
                for (int i = 0; i < zone.Count; i++)
                {
                    //CONVIENE DICHIARARE FUORI DAL FOR QUESTA VARIABILE IN MODO CHE NON LA CREI AD OGNI FOR? 
                    check = dogWorldGrid.GetNodeAtPosition(zone[i].Position);
                    if ((playerNode.Position) == (check.Position))
                    {
                        player.PerformSearch = player.Research();
                        player.IsSearching = false;
                    }
                    else
                    {
                        player.IsSearching = false; // preventing player pressing search outside and then once is in zone allowed instasearch without input
                    }
                }
            }
        }

        public override void Draw()
        {
            DrawMgr.Draw();
        }

        public override void Input()
        {
            if (player.IsActive)
            {
                player.clickInput();
                player.Input();
            }

        }
        private void AddTilesFor(Layer layer, List<TileObj> tileObjs)
        {

            DrawLayer engineLayer = DrawLayer.Playground;
            if (layer.Props.Has("drawLayer"))
            {
                string drawLayer = layer.Props.GetString("drawLayer");
                engineLayer = (DrawLayer)Enum.Parse(typeof(DrawLayer), drawLayer);
            }
            if (layer.Props.Has("searchArea"))
            {

            }


            for (int i = 0; i < layer.Grid.Size(); i++)
            {
                TileInstance inst = layer.Grid.At(i);
                if (inst == null) continue;
                string texture = inst.Type.ImagePath;
                int tOffX = inst.Type.OffX;
                int tOffY = inst.Type.OffY;
                int width = inst.Type.Width;
                int height = inst.Type.Height;
                int posX = inst.PosX + (int)(width * 0.5f); // metto 8 perchè è la metà dell'altezza del tile a 16X16  che poi diventerà 16 quando si raddoppia nel tileobj.In questa maniera riesco a mantenere i nodi al centro del tile dopo il ridimneisonamento.
                int posY = inst.PosY + (int)(height * 0.5f);
                TileObj obj = new TileObj(texture, tOffX, tOffY, posX, posY, width, height);
                tileObjs.Add(obj);


                if (inst.Type.Props.Has("spawnPoint") && inst.Type.Props.GetBool("spawnPoint"))
                {
                    spawn = new SpawnPoint(posX, posY, this);
                }
                if (inst.Type.Props.Has("portal"))
                {
                    string prop = (inst.Type.Props.GetString("portal"));
                    portals.Add(prop, new SpawnPoint(posX, posY, this, prop, true));
                }
                if (layer.Props.Has("searchArea"))
                {
                    SearchAreaTile t = new SearchAreaTile(texture, tOffX, tOffY, posX, posY, width, height);
                    zone.Add(t);
                }


                obj.Layer = engineLayer;

            }
        }

        public override Scene OnExit()
        {
            if (GameOver) //sistemare con la lista di obj altrimenti disegna tutto
            {
                player = null;
                UpdateMgr.ClearAll();
                DrawMgr.ClearAll();
                PhysicsMgr.ClearAll();
                GfxMgr.ClearAll();

            }
            else
            {
                for (int i = 0; i < tileObjs.Count; i++)
                {
                    tileObjs[i].IsActive = false;
                }
                player.IsActive = false;

                bgSource.Stop();
            }
            return base.OnExit();
        }
    }
}
