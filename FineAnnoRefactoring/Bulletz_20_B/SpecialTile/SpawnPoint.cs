using Aiv.Audio;
using OpenTK;
using System;

namespace ProgettoFinale
{
    class SpawnPoint
    {
        public Vector2 Position;
        protected bool isPortal;
        protected PlayScene owner;
        protected string use;
        protected bool changeScene;
        protected AudioSource source;
        public bool ChangeScene { get { return changeScene; } set { changeScene = value; } }
        public SpawnPoint(int posX, int posY, PlayScene anOwner, string use = "entrance", bool portal = false)
        {
            this.use = use;
            owner = anOwner;
            isPortal = portal;
            Position = new Vector2(Game.PixelsToUnits(posX * 2), Game.PixelsToUnits(posY * 2));
            //source = new AudioSource();
        }

        public void Update()
        {
            if (isPortal)
            {
                Vector2 dist = owner.Player.Position - Position;

                if (dist.Length <= 0.2f)
                {
                    if (use == "entrance")
                    {
                        //FALLING ANIMATION
                        owner.Player.Scale *= 0.9f;
                        if (owner.Player.Scale.Length <= 0.1f)
                        {
                            //source.Play(AudioMgr.GetClip("Fall"));
                            owner.Player.Position = owner.portals["exit"].Position;
                            owner.Player.IsPorted = true;
                            owner.Player.Scale = Vector2.One;
                            CameraMgr.CameraShake = true;
                        }
                    }
                    if (use == "changeScene" && owner.Player.CanOpenDoor)
                    {
                        //source.Play(AudioMgr.GetClip("DoorOpen"));
                        changeScene = true;
                        if (owner.mainScene)
                        {//AGGIUSTO LA POSIZIONE DEL PLAYER QUANDO CAMBIO SCENA IN MODO DA NON AVERLO SOPRA IL TRIGGER DI CAMBIO SCENA.
                            owner.Player.Position = Position + new Vector2(0, +Game.PixelsToUnits(33.75f));
                        }
                        else
                        {
                            owner.Player.Position = Position + new Vector2(0, -Game.PixelsToUnits(33.75f));
                        }
                        owner.Player.Velocity = Vector2.Zero;
                        owner.Player.Path = null;
                        
                    }
                }
            }

        }
    }
}