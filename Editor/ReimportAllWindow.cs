using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ForgeLightToolkit
{
    public class ReimportAllWindow : EditorWindow
    {
        [MenuItem("ForgeLight/Reimport All ForgeLight Assets", priority = 11)]
        public static void ReimportAll()
        {
            Debug.Log("Running reimport");
            // TODO: search for all assets matching the file types, reimport them
        }

        [MenuItem("ForgeLight/Reimport ForgeLight Assets by type", priority = 12)]
        public static void OpenReimportWindow()
        {
            GetWindow<ReimportAllWindow>("Reimport Wizzard");
        }

        private void OnGUI()
        {
            // TODO: custom window to allow for selecting which types to reimport
        }
    }
}
