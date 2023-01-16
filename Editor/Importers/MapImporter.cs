using UnityEngine;
using UnityEditor.AssetImporters;

using ForgeLightToolkit.Editor.FileTypes;

namespace ForgeLightToolkit.Editor.Importers
{
    [ScriptedImporter(1, "map")]
    public class MapImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.assetPath))
            {
                ctx.LogImportError("Invalid asset path.");
                return;
            }

            var mapFile = ScriptableObject.CreateInstance<MapFile>();

            if (!mapFile.Load(ctx.assetPath))
            {
                ctx.LogImportError("Failed to load map file.");
                return;
            }

            ctx.AddObjectToAsset("Map", mapFile);
            ctx.SetMainObject(mapFile);
        }
    }
}