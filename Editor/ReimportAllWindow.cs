using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace ForgeLightToolkit
{
    public class ReimportAllWindow : EditorWindow
    {
        [MenuItem("ForgeLight/Reimport All ForgeLight Assets", priority = 11)]
        public static void ReimportAll()
        {
            Debug.Log("Running reimport");
            reimport(EnumFileType.ALL);
        }

        [MenuItem("ForgeLight/Reimport ForgeLight Assets by type", priority = 12)]
        public static void OpenReimportWindow()
        {
            GetWindow<ReimportAllWindow>("Reimport Wizzard");
        }

        private const int BTN_SPACING = 5;

        private bool adr;
        private bool agr;
        private bool dma;
        private bool dme;
        private bool gck2;
        private bool gcnk;
        private bool gzne;
        private bool map;

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width / EditorGUIUtility.pixelsPerPoint, Screen.height / EditorGUIUtility.pixelsPerPoint));

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Files Types to Reimport", EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(BTN_SPACING);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            adr = GUILayout.Toggle(adr, ".adr");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(BTN_SPACING);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            agr = GUILayout.Toggle(agr, ".agr");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(BTN_SPACING);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            dma = GUILayout.Toggle(dma, ".dma");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(BTN_SPACING);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            dme = GUILayout.Toggle(dme, ".dme");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(BTN_SPACING);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            map = GUILayout.Toggle(map, ".map");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(BTN_SPACING);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            gck2 = GUILayout.Toggle(gck2, ".gck2");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(BTN_SPACING);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            gcnk = GUILayout.Toggle(gcnk, ".gcnk");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(BTN_SPACING);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            gzne = GUILayout.Toggle(gzne, ".gzne");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Reimport", GUILayout.ExpandWidth(false)))
            {
                EnumFileType types = EnumFileType.None;
                types |= (adr ? EnumFileType.Adr : EnumFileType.None);
                types |= (agr ? EnumFileType.Agr : EnumFileType.None);
                types |= (dme ? EnumFileType.Dme : EnumFileType.None);
                types |= (dma ? EnumFileType.Dma : EnumFileType.None);
                types |= (map ? EnumFileType.Map : EnumFileType.None);
                types |= (gck2 ? EnumFileType.Gck2 : EnumFileType.None);
                types |= (gcnk ? EnumFileType.Gcnk : EnumFileType.None);
                types |= (gzne ? EnumFileType.Gzne : EnumFileType.None);

                if (types != EnumFileType.None)
                {
                    //Close();
                    reimport(types);
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private static void reimport(EnumFileType typeMask)
        {
            try
            {
                AssetDatabase.StartAssetEditing();

                List<string> filters = new();

                if ((typeMask & EnumFileType.Adr) == EnumFileType.Adr)
                {
                    filters.Add("t:adrFile");
                }

                if ((typeMask & EnumFileType.Agr) == EnumFileType.Agr)
                {
                    filters.Add("t:agrFile");
                }

                if ((typeMask & EnumFileType.Dma) == EnumFileType.Dma)
                {
                    filters.Add("t:dmaFile");
                }

                if ((typeMask & EnumFileType.Dme) == EnumFileType.Dme)
                {
                    filters.Add("t:dmeFile");
                }

                if ((typeMask & EnumFileType.Map) == EnumFileType.Map)
                {
                    filters.Add("t:mapFile");
                }

                if ((typeMask & EnumFileType.Gck2) == EnumFileType.Gck2)
                {
                    filters.Add("t:gck2File");
                }

                if ((typeMask & EnumFileType.Gcnk) == EnumFileType.Gcnk)
                {
                    filters.Add("t:gcnkFile");
                }

                if ((typeMask & EnumFileType.Gzne) == EnumFileType.Gzne)
                {
                    filters.Add("t:gzneFile");
                }

                var guids = AssetDatabase.FindAssets(string.Join(' ', filters));
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }
    }

    public enum EnumFileType 
    {
        None = 0,
        Adr = 1,
        Agr = 2,
        Dma = 4,
        Dme = 8,
        Map = 16,
        Gck2 = 32,
        Gcnk = 64,
        Gzne = 128,
        ALL = 255
    }
}
