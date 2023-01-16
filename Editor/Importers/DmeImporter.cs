using UnityEngine;
using UnityEditor.AssetImporters;

using ForgeLightToolkit.Editor.FileTypes;

namespace ForgeLightToolkit.Editor.Importers
{
    [ScriptedImporter(1, "dme")]
    public class DmeImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.assetPath))
            {
                ctx.LogImportError("Invalid asset path.");
                return;
            }

            var dmeFile = ScriptableObject.CreateInstance<DmeFile>();

            if (!dmeFile.Load(ctx.assetPath))
            {
                ctx.LogImportError("Failed to load dme file.");
                return;
            }

            ctx.AddObjectToAsset("Dme", dmeFile);
            ctx.SetMainObject(dmeFile);

            ctx.AddObjectToAsset("Dma", dmeFile.DmaFile);
            ctx.AddObjectToAsset("Mesh", dmeFile.Mesh);
        }
    }
}