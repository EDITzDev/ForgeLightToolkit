using UnityEngine;
using UnityEditor.AssetImporters;

using ForgeLightToolkit.Editor.FileTypes;

namespace ForgeLightToolkit.Editor.Importers
{
    [ScriptedImporter(1, "gzne")]
    public class GzneImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.assetPath))
            {
                ctx.LogImportError($"Invalid asset path. ({ctx.assetPath})");
                return;
            }

            var gzneFile = ScriptableObject.CreateInstance<GzneFile>();

            if (!gzneFile.Load(ctx.assetPath))
            {
                ctx.LogImportError($"Failed to load gzne file. ({ctx.assetPath})");
                return;
            }

            ctx.AddObjectToAsset("Gzne", gzneFile);
            ctx.SetMainObject(gzneFile);
        }
    }
}