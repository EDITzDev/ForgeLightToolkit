using UnityEngine;
using UnityEditor.AssetImporters;

using ForgeLightToolkit.Editor.FileTypes;

namespace ForgeLightToolkit.Editor.Importers
{
    [ScriptedImporter(1, "agr")]
    public class AgrImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.assetPath))
            {
                ctx.LogImportError($"Invalid asset path. ({ctx.assetPath})");
                return;
            }

            var agrFile = ScriptableObject.CreateInstance<AgrFile>();

            if (!agrFile.Load(ctx.assetPath))
            {
                ctx.LogImportError($"Failed to load agr file. ({ctx.assetPath})");
                return;
            }

            ctx.AddObjectToAsset("Agr", agrFile);
            ctx.SetMainObject(agrFile);
        }
    }
}