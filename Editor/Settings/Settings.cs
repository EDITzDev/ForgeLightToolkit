using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ForgeLightToolkit.Settings
{
    public class SettingsEditor
    {
        private static Vector2 scrollPos = Vector2.zero;
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            // ensure availability of the instance
            _ = FLTKSettings.Instance;

            var provider = new SettingsProvider("Project/Forgelight Toolkit Settings", SettingsScope.Project)
            {
                label = "Forgelight Toolkit Settings",
                guiHandler = context =>
                {
                    SerializedObject settings = new(FLTKSettings.Instance);

                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    GUILayout.BeginVertical();

                    var invZProp = settings.FindProperty("invertZ");
                    EditorGUILayout.PropertyField(invZProp, new GUIContent("Invert Z Axis"));
                    EditorGUILayout.PropertyField(settings.FindProperty("shaderOverrides"), new GUIContent("Shader Name Overrides"));
                    EditorGUILayout.PropertyField(settings.FindProperty("modelOverrides"), new GUIContent("Model Name Overrides"));

                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();

                    if (settings.hasModifiedProperties)
                    {
                        bool runReimport = invZProp.boolValue != FLTKSettings.Instance.InvertZ;

                        settings.ApplyModifiedPropertiesWithoutUndo();
                        FLTKSettings.Instance.save();
                        
                        if (runReimport)
                        {
                            ReimportAllWindow.ReimportAll();
                        }
                    }
                },
                keywords = new HashSet<string>() { "Forgelight", "FLTK" }
            };

            return provider;
        }
    }

    public class FLTKSettings : ScriptableObject
    {
        private const string FilePath = "ProjectSettings/ForgeLightToolKitSettings.json";

        private static FLTKSettings _instance;
        public static FLTKSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance<FLTKSettings>();
                    _instance.load();
                }
                return _instance;
            }
        }

        public bool InvertZ => invertZ;
        [SerializeField]
        private bool invertZ;

        public List<ShaderOverride> ShaderOverrides => shaderOverrides;
        [SerializeField]
        private List<ShaderOverride> shaderOverrides;

        public List<ModelOverride> ModelOverrides => modelOverrides;
        [SerializeField]
        private List<ModelOverride> modelOverrides;

        private void initDefaults()
        {
            invertZ = false;
            shaderOverrides = new();
            modelOverrides = new();
        }

        private void initNullable()
        {
            shaderOverrides ??= new();
            modelOverrides ??= new();
        }

        private void load()
        {
            if (!File.Exists(FilePath))
            {
                initDefaults();
                return;
            }

            try
            {
                string jsonText = File.ReadAllText(FilePath);
                EditorJsonUtility.FromJsonOverwrite(jsonText, this);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                initDefaults();
            }

            initNullable();
        }

        internal void save()
        {
            string dirName = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllText(FilePath, EditorJsonUtility.ToJson(this, true));
        }

        public Shader GetShader(string name)
        {
            foreach (var so in shaderOverrides)
            {
                if (so.ShaderName == name)
                {
                    return so.Shader;
                }
            }

            return null;
        }

        public GameObject GetModel(string name)
        {
            foreach (var mo in modelOverrides)
            {
                if (mo.ObjectName == name)
                {
                    return mo.Object;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class ShaderOverride
    {
        public string ShaderName => shaderName;
        [SerializeField]
        private string shaderName;

        public Shader Shader => shader;
        [SerializeField]
        private Shader shader;
    }

    [Serializable]
    public class ModelOverride
    {
        public string ObjectName => objName;
        [SerializeField]
        private string objName;

        public GameObject Object => obj;
        [SerializeField]
        private GameObject obj;
    }
}
