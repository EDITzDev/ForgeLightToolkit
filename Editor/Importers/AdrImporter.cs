using UnityEngine;
using UnityEditor.AssetImporters;

using ForgeLightToolkit.Editor.FileTypes;

namespace ForgeLightToolkit.Editor.Importers
{
    [ScriptedImporter(1, "adr")]
    public class AdrImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.assetPath))
            {
                ctx.LogImportError($"Invalid asset path. ({ctx.assetPath})");
                return;
            }

            var adrFile = ScriptableObject.CreateInstance<AdrFile>();

            if (!adrFile.Load(ctx.assetPath))
            {
                ctx.LogImportError($"Failed to load adr file. ({ctx.assetPath})");
                return;
            }

            ctx.AddObjectToAsset("Adr", adrFile);
            ctx.SetMainObject(adrFile);
        }
    }
}