#region[References]
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UnityEngine;
#endregion


namespace Designer
{
    public partial class Form1 : Form
    {
        #region[Declarations]

        public BepInEx.Logging.ManualLogSource log;




        #endregion


        public Form1()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GameObject t = new GameObject("Test GameObject Created");
            //log.LogWarning("Test from Winform: " + t.name);
            //Debug.LogWarning("Test from Winform: " + t.name); // Will cause crash in IL2CPP!

            GameObject[] tmp = GameObject.FindObjectsOfType<GameObject>();
            log.LogWarning("Test from Winform: GameObject CNT: " + tmp.Length.ToString());
        }
    }
}
