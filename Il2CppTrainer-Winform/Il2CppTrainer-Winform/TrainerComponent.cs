
#region[Refs]
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using Input = BepInEx.IL2CPP.UnityEngine.Input;
using UnityEngine;
#endregion


namespace Trainer
{
    public class TrainerComponent : MonoBehaviour
    {
        #region[Declarations]

        #region[Trainer]

        // Trainer Base
        public static GameObject obj = null;
        public static TrainerComponent instance;
        private static bool initialized = false;
        private static BepInEx.Logging.ManualLogSource log;
        private static bool updateFired = false;

        #endregion

        #region[Winform]

        private static Thread winformThread = null;
        private static Designer.Form1 winForm = null;
        private static bool isGameQuiting = false;


        #endregion

        public static string GAME_NAME = "";
        public static string TRAINER_VER = "v1";




        #endregion

        internal static GameObject Create(string name)
        {
            obj = new GameObject(name);
            DontDestroyOnLoad(obj);

            var component = new TrainerComponent(obj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<TrainerComponent>()).Pointer);
            return obj;
        }

        public TrainerComponent(IntPtr ptr) : base(ptr)
        {
            log = BepInExLoader.log;

            instance = this;
            
        }

        private static void Initialize()
        {
            log.LogWarning(GAME_NAME + " IL2CPP Trainer " + TRAINER_VER + " - wh0am15533");

            log.LogWarning(" ");
            log.LogWarning("HotKeys:");
            log.LogWarning("   F1              = Reprint Hotkeys");
            log.LogWarning("   F12             = Toggle Trainer UI");


            log.LogWarning(" ");

            InitWinForm();

            initialized = true;
        }

        public void Update()
        {
            if (!updateFired) { updateFired = true; }
            
            if (!initialized) { Initialize(); }

            // Toggle Trainer/Winform UI
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F12) && Event.current.type == EventType.KeyDown)
            {
                try
                {
                    if (winForm != null)
                    {
                        if (winForm.Visible) { winForm.Hide(); } else { winForm.Show(); }
                    }
                    else { InitWinForm(); }

                    log.LogMessage("[Trainer]: Toggled Trainer UI Shown: " + winForm.Visible.ToString());
                    Event.current.Use();
                }
                catch (Exception e) { log.LogMessage("ERROR: " + e.Message); }
            }

        }


        #region[Winform Init/Dispose]

        public static void InitWinForm()
        {
            if (winformThread == null)
            {
                winformThread = new Thread(OpenWinForm);
                winformThread.IsBackground = true;
                winformThread.Start();
            }
        }

        public static void OpenWinForm()
        {
            winForm = new Designer.Form1();            
            winForm.log = BepInExLoader.log;
            winForm.ShowDialog();
        }

        // Clean up before Quiting
        public void OnApplicationQuit()
        {
            isGameQuiting = true;
            try
            {
                winForm.Close();
                winForm = null;
                log.LogWarning("[Trainer]: Winform Disposed...");
                (System.Diagnostics.Process.GetCurrentProcess()).Kill();
            }
            catch { }
        }

        #endregion

    }
}
