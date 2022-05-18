
#region[References]
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#endregion

namespace Trainer.Menu
{
    public class TrainerMenu : MonoBehaviour
    {
        #region[Declarations]

        #region[Trainer]

        public static TrainerMenu instance = null;
        public static BepInEx.Logging.ManualLogSource log = Trainer.BepInLoader.log;

        private Rect MainWindow;
        private bool MainWindowVisible = true;

        private int origHeight = 0;
        private int origWidth = 0;

        #endregion

        #region[Winform]

        private static Thread winformThread = null;
        private static Designer.Form1 winForm = null;
        private static bool isGameQuiting = false;


        #endregion



        #endregion


        public void Awake()
        {
            InitWinForm();
        }

        private void Start()
        {
            #region[Window Definitions - Don't Touch]
            MainWindow = new Rect(UnityEngine.Screen.width / 2 - 100, UnityEngine.Screen.height / 2 - 350, 250f, 50f);

            origHeight = UnityEngine.Screen.height;
            origWidth = UnityEngine.Screen.width;
            #endregion
           
            instance = this;
        }

        private void Update()
        {
            #region[Trainer]
            
            if (Input.GetKeyDown(KeyCode.Backspace)) { MainWindowVisible = !MainWindowVisible; }

            #endregion

        }
        
        private void OnGUI()
        {
            if (!MainWindowVisible) { return; }
                
            if (UnityEngine.Event.current.type == UnityEngine.EventType.Layout)
            {
                #region[For IMGUI Windows]
                                
                GUI.backgroundColor = Color.black;
                GUIStyle titleStyle = new GUIStyle(GUI.skin.window);
                titleStyle.normal.textColor = Color.green;

                //MAIN WINDOW #0
                MainWindow = new Rect(MainWindow.x, MainWindow.y, 250f, 50f);
                MainWindow = GUILayout.Window(777, MainWindow, new GUI.WindowFunction(RenderUI), " Trainer v1", titleStyle, new GUILayoutOption[0]);

                #endregion
            }

        }

        private void RenderUI(int id)
        {
            #region[Styles]

            GUIStyle textStyle = new GUIStyle();
            textStyle.normal.background = Texture2D.whiteTexture;
            textStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle labelStyleGreen = new GUIStyle();
            labelStyleGreen.normal.textColor = Color.green;
            labelStyleGreen.alignment = TextAnchor.MiddleCenter;

            GUIStyle labelStyleCyan = new GUIStyle();
            labelStyleCyan.normal.textColor = Color.cyan;
            labelStyleCyan.alignment = TextAnchor.MiddleCenter;

            GUIStyle labelStyleWhite = new GUIStyle();
            labelStyleWhite.normal.textColor = Color.white;
            labelStyleWhite.alignment = TextAnchor.MiddleCenter;

            GUIStyle labelStyleYellow = new GUIStyle();
            labelStyleYellow.normal.textColor = Color.yellow;
            labelStyleYellow.alignment = TextAnchor.MiddleCenter;

            GUIStyle labelStyleRed = new GUIStyle();
            labelStyleRed.normal.textColor = Color.red;
            labelStyleRed.alignment = TextAnchor.MiddleCenter;


            #endregion

            #region[GameLoaded Checking]
            bool check = true;
            #endregion
                       
            switch (id)
            {
                #region[Main Window]

                case 777:
                    #region[Windows Header]
                    GUILayout.Label("Show/Hide: Backspace", labelStyleWhite, new GUILayoutOption[0]);
                    GUILayout.Space(10f);
                    #endregion

                    if (check)
                    {
                        #region[Main Window Buttons]

                        GUI.color = Color.white;
                        if (GUILayout.Button("Toggle Winform", new GUILayoutOption[0]))
                        {
                            try
                            {
                                if (winForm != null)
                                {
                                    if (winForm.Visible) { winForm.Hide(); } else { winForm.Show(); }
                                }
                                else { InitWinForm(); }
                            }
                            catch (Exception e) { log.LogError("[Trainer]: ERROR: " + e.Message); }
                        }


                        #endregion


                    }

                    break;

                #endregion

            }
            
            GUI.DragWindow();
        }

        #region[Winform Init/Dispose]

        public static void InitWinForm()
        {
            winformThread = new Thread(OpenWinForm);
            winformThread.IsBackground = true;
            winformThread.Start();
        }
        
        public static void OpenWinForm()
        {
            winForm = new Designer.Form1();
            //winForm.log = BepInExLoader.log;
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


