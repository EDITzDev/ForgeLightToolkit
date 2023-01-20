using System.IO;
using System.Linq;

using UnityEditor;
using UnityEngine;

using ForgeLightToolkit.Editor.FileTypes;
using ForgeLightToolkit.Editor.FileTypes.Dma;

namespace ForgeLightToolkit.Editor
{
    public class LoadWorldWindow : EditorWindow
    {
        private string _worldName = "FabledRealms";
        private string _assetsPath = "Assets/ForgeLight/FreeRealms";

        private bool _loadLights = true;
        private bool _loadObjects = true;
        private bool _loadRoadMap = true;

        [MenuItem("ForgeLight/Load World")]
        public static void ShowWindow()
        {
            GetWindow<LoadWorldWindow>("Load World");
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width / EditorGUIUtility.pixelsPerPoint, Screen.height / EditorGUIUtility.pixelsPerPoint));

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Assets Path", EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Example: Assets/ForgeLight/FreeRealms", EditorStyles.miniBoldLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            _assetsPath = EditorGUILayout.TextField(_assetsPath, GUILayout.ExpandWidth(false));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("World Name", EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Example: FabledRealms", EditorStyles.miniBoldLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            _worldName = EditorGUILayout.TextField(_worldName, GUILayout.ExpandWidth(false));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            _loadLights = GUILayout.Toggle(_loadLights, "Lights");

            GUILayout.Space(10);

            _loadObjects = GUILayout.Toggle(_loadObjects, "Objects");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            _loadRoadMap = GUILayout.Toggle(_loadRoadMap, "Road Map (FR Only)");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Load World", GUILayout.ExpandWidth(false))
                && !string.IsNullOrEmpty(_worldName) && !string.IsNullOrEmpty(_assetsPath))
                LoadWorld(_worldName, _assetsPath, _loadObjects, _loadLights, _loadRoadMap);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void LoadWorld(string worldName, string assetsPath, bool loadObjects, bool loadLights, bool loadRoadMap)
        {
            var worldObject = new GameObject($"World ({worldName})");

            var gcnkFileAssetGuids = AssetDatabase.FindAssets($"{worldName} t:GcnkFile", new[] { assetsPath });

            foreach (var gcnkFileAssetGuid in gcnkFileAssetGuids)
            {
                var gcnkFileAssetPath = AssetDatabase.GUIDToAssetPath(gcnkFileAssetGuid);

                var gcnkFile = AssetDatabase.LoadAssetAtPath<GcnkFile>(gcnkFileAssetPath);

                if (gcnkFile is null)
                    continue;

                var chunkObject = new GameObject($"Chunk ({gcnkFile.Coords.x}, {gcnkFile.Coords.y})")
                {
                    transform =
                    {
                        parent = worldObject.transform
                    }
                };

                var chunkMeshFilter = chunkObject.AddComponent<MeshFilter>();

                chunkMeshFilter.sharedMesh = gcnkFile.Mesh;

                var gck2FileAssetPath = Path.ChangeExtension(gcnkFileAssetPath, "gck2");

                var gck2File = AssetDatabase.LoadAssetAtPath<Gck2File>(gck2FileAssetPath);

                var chunkMaterial = gck2File is null ? new Material(Shader.Find("Custom/ForgeLight")) : gck2File.Material;

                var chunkMeshRenderer = chunkObject.AddComponent<MeshRenderer>();

                var chunkMaterials = new Material[gcnkFile.Mesh.subMeshCount];

                for (var i = 0; i < chunkMaterials.Length; i++)
                    chunkMaterials[i] = chunkMaterial;

                chunkMeshRenderer.materials = chunkMaterials;

                if (!loadObjects)
                    continue;

                var runtimeObjects = gcnkFile.Tiles.SelectMany(x => x.RuntimeObjects).GroupBy(x => x.ObjectId)
                    .Select(x => x.First());

                foreach (var tileRuntimeObject in runtimeObjects)
                {
                    var adrFilePath = Path.Combine(assetsPath, tileRuntimeObject.FileName);

                    var adrFile = AssetDatabase.LoadAssetAtPath<AdrFile>(adrFilePath);

                    if (adrFile is null)
                    {
                        Debug.LogError($"Failed to load file. {adrFilePath}");
                        continue;
                    }

                    if (adrFile.ModelFileName is null)
                    {
                        Debug.LogError($"Adr has no model file name. {adrFilePath}");
                        continue;
                    }

                    var dmeFilePath = Path.Combine(assetsPath, adrFile.ModelFileName);

                    var dmeFile = AssetDatabase.LoadAssetAtPath<DmeFile>(dmeFilePath);

                    if (dmeFile is null)
                    {
                        Debug.LogError($"Failed to load file. {dmeFilePath}");
                        continue;
                    }

                    var runtimeObject = new GameObject($"Object ({tileRuntimeObject.FileName})")
                    {
                        transform =
                        {
                            parent = chunkObject.transform,
                            localPosition = tileRuntimeObject.Position,
                            localScale = new Vector3(tileRuntimeObject.Scale, tileRuntimeObject.Scale, tileRuntimeObject.Scale),
                            localRotation = Quaternion.Euler(tileRuntimeObject.Rotation.y * Mathf.Rad2Deg, tileRuntimeObject.Rotation.x * Mathf.Rad2Deg, tileRuntimeObject.Rotation.z * Mathf.Rad2Deg)
                        }
                    };

                    var objectMeshFilter = runtimeObject.AddComponent<MeshFilter>();

                    objectMeshFilter.sharedMesh = dmeFile.Mesh;

                    var objectMeshRenderer = runtimeObject.AddComponent<MeshRenderer>();

                    var objectMaterials = new Material[dmeFile.ModelCount];

                    for (var i = 0; i < objectMaterials.Length; i++)
                    {
                        var materialEntry = dmeFile.DmaFile.MaterialEntries[i];

                        foreach (var parameterEntry in materialEntry.ParameterEntries)
                        {
                            if (parameterEntry.Class != D3DXPARAMETER_CLASS.D3DXPC_OBJECT ||
                                parameterEntry.Type != D3DXPARAMETER_TYPE.D3DXPT_TEXTURE)
                                continue;

                            var textureHash = parameterEntry.Uint;

                            foreach (var textureName in dmeFile.DmaFile.Textures)
                            {
                                if (JenkinsHelper.JenkinsOneAtATimeHash(textureName.ToUpper()) != textureHash)
                                    continue;

                                var textureFilePath = Path.Combine(assetsPath, textureName);

                                var texture2d = AssetDatabase.LoadAssetAtPath<Texture2D>(textureFilePath);

                                if (texture2d is null)
                                {
                                    Debug.LogError($"Failed to find texture. {textureFilePath}");
                                    continue;
                                }

                                var objectMaterial = new Material(Shader.Find("Custom/ForgeLight"))
                                {
                                    name = textureName,
                                    mainTexture = texture2d,
                                    mainTextureScale = Vector2.right + Vector2.down
                                };

                                objectMaterials[i] = objectMaterial;

                                // TODO: Use only one texture for now.

                                break;
                            }
                        }
                    }

                    objectMeshRenderer.materials = objectMaterials;
                }

                if (!loadLights)
                    continue;

                foreach (var tile in gcnkFile.Tiles)
                {
                    foreach (var rawLight in tile.RawLights)
                    {
                        var lightObject = new GameObject($"Light ({rawLight.Name})")
                        {
                            transform =
                            {
                                parent = chunkObject.transform,
                                position = rawLight.Position
                            }
                        };

                        var lightComp = lightObject.AddComponent<Light>();

                        lightComp.range = rawLight.Range;
                        lightComp.color = rawLight.Color;
                        lightComp.intensity = rawLight.Intensity;
                    }
                }
            }

            if (loadRoadMap)
            {
                var mapFilePath = Path.Combine(assetsPath, $"{worldName}.map");

                var mapFile = AssetDatabase.LoadAssetAtPath<MapFile>(mapFilePath);

                if (mapFile is not null)
                {
                    var roadMapObject = new GameObject("Road Map")
                    {
                        transform =
                        {
                            parent = worldObject.transform
                        }
                    };

                    foreach (var node in mapFile.Nodes)
                    {
                        var nodeSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                        nodeSphere.transform.position = node.Position;
                        nodeSphere.transform.parent = roadMapObject.transform;
                    }
                }
            }

            worldObject.transform.localScale = new Vector3(1, 1, -1);

            Debug.Log($"Loaded {worldName} world.");
        }
    }
}