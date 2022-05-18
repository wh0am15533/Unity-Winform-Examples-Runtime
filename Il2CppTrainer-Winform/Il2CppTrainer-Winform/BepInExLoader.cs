
using System;
using BepInEx;
using UnhollowerRuntimeLib;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading;

namespace Trainer
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class BepInExLoader : BepInEx.IL2CPP.BasePlugin
    {
        #region[Declarations]
        
        public const string
            MODNAME = "Trainer",
            AUTHOR = "wh0am15533",
            GUID = "com." + AUTHOR + "." + MODNAME,
            VERSION = "1.0.0.0";
        
        public static BepInEx.Logging.ManualLogSource log;

        private static bool consoleInitialized = false;
        public static GameObject consoleLogger = null;

        #endregion

        public BepInExLoader()
        {
            log = Log;
        }

        public override void Load()
        {
            InitThreading();
        }

        public static void Init()
        {
            #region[Register Custom Types in Il2Cpp]

            try
            {
                // Trainer
                ClassInjector.RegisterTypeInIl2Cpp<Bootstrapper>();
                ClassInjector.RegisterTypeInIl2Cpp<TrainerComponent>();
            }
            catch
            {
                log.LogError("FAILED to Register Il2Cpp Type!");
            }

            #endregion

            #region[Bootstrap The Main Trainer GameObject]

            #region[DevNote]
            // If you create your main object here, only Awake(), OnEnabled() get fired. But if you try to create the trainer in either of 
            // those it doesn't get created properly as the object get's destroyed right away. Bootstrapping the GameObject like this allows
            // for it to inherit Unity MonoBehavior Events like OnGUI(), Update(), etc without a Harmony Patch. The only patch needed 
            // is for the Bootstrapper. You'll see. The Trainer has an EventTest function, Press 'Tab', and watch the BepInEx Console.
            #endregion
            GameObject bootStrapper = Bootstrapper.Create("BootStrapperGO");

            #endregion

            #region[Harmony Patching]

            if (bootStrapper != null)
            {
                try
                {
                    var harmony = new Harmony("wh0am15533.trainer.il2cpp");

                    #region[Update() Hook - Only Needed for Bootstrapper]

                    var originalUpdate = AccessTools.Method(typeof(EventSystem), "Update"); //EventSystem //UnityEngine.UI.CanvasScaler   ///GameManager, LoadingManager
                    var postUpdate = AccessTools.Method(typeof(Bootstrapper), "Update");
                    //log.LogMessage("   Original Method: " + originalUpdate.DeclaringType.Name + "." + originalUpdate.Name);
                    //log.LogMessage("   Postfix Method: " + postUpdate.DeclaringType.Name + "." + postUpdate.Name);
                    harmony.Patch(originalUpdate, postfix: new HarmonyMethod(postUpdate));

                    #endregion

                }
                catch { log.LogError("FAILED to Apply Hooks's!"); }                
            }

            #endregion

        }

        public static void InitThreading()
        {
            new Thread(() =>
            {
                Thread.Sleep(5000); // 5 second sleep as initialization occurs *really* early

                Init();

            }).Start();
        }



    }
}
