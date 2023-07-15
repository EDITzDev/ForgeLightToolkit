using UnityEngine;
using UnityEditor.AssetImporters;

using ForgeLightToolkit.Editor.FileTypes;

namespace ForgeLightToolkit.Editor.Importers
{
    [ScriptedImporter(1, "gck2")]
    public class Gck2Importer : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.assetPath))
            {
                ctx.LogImportError($"Invalid asset path. ({ctx.assetPath})");
                return;
            }

            var gck2File = ScriptableObject.CreateInstance<Gck2File>();

            if(!gck2File.Load(ctx.assetPath))
            {
                ctx.LogImportError($"Failed to load gck2 file. ({ctx.assetPath})");
                return;
            }

            ctx.AddObjectToAsset("Gck2", gck2File);
            ctx.SetMainObject(gck2File);

            ctx.AddObjectToAsset("Texture", gck2File.Texture);
        }
    }
}