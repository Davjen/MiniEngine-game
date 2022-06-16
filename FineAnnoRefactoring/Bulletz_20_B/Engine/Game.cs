using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgettoFinale
{
    static class Game
    {

        public static Window Win;
        public static bool IsRunning;
        public static Scene CurrentScene { get; private set; }

        public static float DeltaTime { get { return Win.deltaTime; } }

        public static int ConnectedJoystics { get { return controllers.Count; } }

        public static float UnitSize { get; private set; }
        public static float OptimalUnitSize { get; private set; }
        public static float OptimalScreenHeight { get; private set; }

        private static List<Controller> controllers;
        private static KeyboardCtrl keyboardController;
        private static Dictionary<string, PlayScene> playScenes;

        private static string mainMap;
        private static string secondMap;
        private static string mainMapTheme;
        private static string secondMapTheme;




        public static void Init()
        {
            Win = new Window(800, 800, "Definetly Not SpaceShooter");
            Win.SetVSync(false);
            Win.SetDefaultOrthographicSize(33.75f);
            OptimalScreenHeight = 1080;

            UnitSize = Win.Height / Win.OrthoHeight;
            OptimalUnitSize = OptimalScreenHeight / Win.OrthoHeight;

            controllers = new List<Controller>();

            keyboardController = new KeyboardCtrl(0);
            playScenes = new Dictionary<string, PlayScene>();

            mainMap = "Assets/InternoDungeon.tmx";
            secondMap = "Assets/Scene2.tmx";

            mainMapTheme = "Assets/audio/World.wav";
            secondMapTheme = "Assets/audio/Cave.wav";
            
            //sicuramente è meglio avere playscene padre di dunegonscene e worldscene e dividere le responsabilità, ma per questa demo non ho ritenuto necessario farlo non avendo grosse particolarità fra una scena e un'altra ne parecchie scene.
            PlayScene playScene = new PlayScene(mainMap, true, mainMapTheme);
            PlayScene dungeon = new PlayScene(secondMap, false, secondMapTheme);
            playScene.NextScene = dungeon;
            dungeon.NextScene = playScene;

            CurrentScene = playScene;

            StaticMouse.Init();
            Cursor.Hide();

        }

        public static float PixelsToUnits(float pixelsSize)
        {
            return pixelsSize / OptimalUnitSize;
        }

        public static Controller GetController(int index)
        {
            Controller ctrl = keyboardController;
            if (controllers.Count - 1 >= index)
            {
                ctrl = controllers[index];
            }
            return ctrl;
        }

        public static void Play()
        {
            //TEST
            CurrentScene.OnEnter();

            IsRunning = true;

            while (Win.IsOpened && IsRunning)
            {
                //input
                if (Win.GetKey(KeyCode.Esc))
                {
                    break;
                }

                if (!CurrentScene.IsPlaying)
                {

                    Scene nextScene = CurrentScene.OnExit();
                    GC.Collect();

                    if (nextScene != null)
                    {
                        CurrentScene = nextScene;
                        CurrentScene.OnEnter();
                        ((PlayScene)CurrentScene).Player.CanOpenDoor = true; //SUPER CANATA LO SO, MA IL CAMBIO SCENA C'è SOLO SE HA LA CHIAVE PER CUI SE STA CAMBIANDO SCENA PER FORZA HA LA CHIAVE
                        //IN QUESTA MANIERA PUò USCIRE DAL DUNGEON SENZA DOVER TROVARE UNA CHIAVE. NELL'OTTICA DI OTTIMIZZAZIONE IL PLAYER SAREBBE MEGLIO PASSSARLO ALLA SCENA IN MODO CHE SIA SEMPRE LO STESSO E SI PRESERVINO LE INFO(INVENTARIO, ECC)
                        //IN QUESTO PROGETTO NON ESSENDOCI QUESTA NECESSITà VOLUTAMENTE NON L'HO SISTEMATO.
                    }
                    else
                    {
                        return;
                    }
                }

                CurrentScene.Input();

                StaticMouse.Update();
                CurrentScene.Update();

                CurrentScene.Draw();
                StaticMouse.Draw();
                Win.Update();
            }
        }

        public static RandomTimer RandomTimer(int timeMin, int timeMax)
        {
            return new RandomTimer(timeMin, timeMax);
        }
    }
}
