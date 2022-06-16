using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    struct CameraLimits
    {
        public float MaxX;
        public float MinX;
        public float MaxY;
        public float MinY;


        public CameraLimits(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }
    }

    public enum FollowBehaviour { Fixed, FollowTarget, FollowPoint }


    static class CameraMgr
    {
        private static Dictionary<string, Tuple<Camera, float>> cameras;

        public static Camera MainCamera { get; private set; }
        public static GameObject Target;
        public static Vector2 TargetPoint;
        public static FollowBehaviour Behaviour;

        public static CameraLimits Limits;
        public static Vector2 Offset;
        public static bool CameraShake;
        public static float CameraSpeed = 12;
        public static float timer = 0;
        public static float timer1 = 0;

        public static void Init(Vector2 position, Vector2 pivot)
        {
            if (MainCamera == null)
            {
                MainCamera = new Camera(position.X, position.Y);
            }
            else
            {
                MainCamera.position = position;
            }

            MainCamera.pivot = pivot;

            cameras = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void Init()
        {
            Init(Vector2.Zero, Vector2.Zero);

        }

        public static void AddCamera(string cameraName, Camera camera = null, float cameraSpeed = 0.0f)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }

            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                return cameras[cameraName].Item1;
            }
            return null;
        }

        public static void CameraShakeMethod()
        {
            MainCamera.position.X = Target.Position.X + (float)Math.Sin(Game.DeltaTime * 100) * 0.7f;
        }

        public static void Reset()
        {
            MainCamera.position = Vector2.Zero;
            MainCamera.pivot = Vector2.Zero;
            Target = null;
            Behaviour = FollowBehaviour.Fixed;

            Limits.MinX = 0;
            Limits.MinY = 0;
            Limits.MaxX = 0;
            Limits.MaxY = 0;
        }

        public static void Update()
        {

            if (CameraShake)
            {
                timer += Game.DeltaTime;
                CameraShakeMethod();
                if (timer >= 1f)
                {
                    CameraShake = false;
                    timer = 0;
                }
            }
            Vector2 pointToFollow;
            Vector2 oldCameraPos = MainCamera.position;

            if (Behaviour == FollowBehaviour.FollowTarget)
            {
                pointToFollow = Target.Position;
            }
            else if (Behaviour == FollowBehaviour.FollowPoint)
            {
                pointToFollow = TargetPoint;
            }
            else
            {
                return;
            }

            MainCamera.position = Vector2.Lerp(MainCamera.position, pointToFollow + Offset, Game.DeltaTime * CameraSpeed);

            //FixPosition();

            Vector2 cameraDelta = MainCamera.position - oldCameraPos;

            foreach (var cam in cameras)
            {
                //camera.position += delta * camera.speed;
                cam.Value.Item1.position += cameraDelta * cam.Value.Item2;
            }
        }

        private static void FixPosition()
        {
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, Limits.MinY, Limits.MaxY);
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, Limits.MinX, Limits.MaxX);
        }
    }
}
