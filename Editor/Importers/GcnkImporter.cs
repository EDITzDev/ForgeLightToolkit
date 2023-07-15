using UnityEngine;
using UnityEditor.AssetImporters;

using ForgeLightToolkit.Editor.FileTypes;

namespace ForgeLightToolkit.Editor.Importers
{
    [ScriptedImporter(1, "gcnk")]
    public class GcnkImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.assetPath))
            {
                ctx.LogImportError($"Invalid asset path. ({ctx.assetPath})");
                return;
            }

            var gcnkFile = ScriptableObject.CreateInstance<GcnkFile>();

            if (!gcnkFile.Load(ctx.assetPath))
            {
                ctx.LogImportError($"Failed to load gcnk file. ({ctx.assetPath})");
                return;
            }

            ctx.AddObjectToAsset("Gcnk", gcnkFile);
            ctx.SetMainObject(gcnkFile);

            ctx.AddObjectToAsset("Mesh", gcnkFile.Mesh);

            if(gcnkFile.DetailMask is not null)
                ctx.AddObjectToAsset("DetailMask", gcnkFile.DetailMask);
        }
    }
}