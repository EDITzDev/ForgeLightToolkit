using UnityEngine;
using UnityEditor.AssetImporters;

using ForgeLightToolkit.Editor.FileTypes;

namespace ForgeLightToolkit.Editor.Importers
{
    [ScriptedImporter(1, "dma")]
    public class DmaImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.assetPath))
            {
                ctx.LogImportError($"Invalid asset path. ({ctx.assetPath})");
                return;
            }

            var dmaFile = ScriptableObject.CreateInstance<DmaFile>();

            if (!dmaFile.Load(ctx.assetPath))
            {
                ctx.LogImportError($"Failed to load dma file. ({ctx.assetPath})");
                return;
            }

            ctx.AddObjectToAsset("Dma", dmaFile);
            ctx.SetMainObject(dmaFile);
        }
    }
}