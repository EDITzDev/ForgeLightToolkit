using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using ForgeLightToolkit.Editor.FileTypes.Dme;

namespace ForgeLightToolkit.Editor.FileTypes
{
    public class DmeFile : ScriptableObject
    {
        [HideInInspector]
        public DmaFile DmaFile;

        public Bounds Bounds;

        [HideInInspector]
        public List<MeshEntry> Meshes = new();

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(File.OpenRead(filePath));

            var magic = new string(reader.ReadChars(4));

            if (magic != "DMOD")
                return false;

            var version = reader.ReadInt32();

            DmaFile = CreateInstance<DmaFile>();

            var dmaFileData = reader.ReadBytes(reader.ReadInt32());

            if (!DmaFile.Load(filePath, dmaFileData))
                return false;

            Bounds.SetMinMax(reader.ReadVector3(), reader.ReadVector3());

            var meshCount = DmaFile.MaterialEntries.Count;

            if (version >= 3)
                meshCount = reader.ReadInt32();

            if (meshCount != DmaFile.MaterialEntries.Count)
                return false;

            for (var i = 0; i < meshCount; i++)
            {
                var meshEntry = new MeshEntry
                {
                    MaterialIndex = reader.ReadInt32(),

                    Unknown2 = reader.ReadInt32(),
                    Unknown3 = reader.ReadInt32(),
                    Unknown4 = reader.ReadInt32(),

                    VertexSize = reader.ReadInt32(),
                    VertexBufferCount = reader.ReadInt32(),

                    IndexSize = reader.ReadInt32(),
                    IndexBufferCount = reader.ReadInt32()
                };

                meshEntry.VertexBuffer = reader.ReadBytes(meshEntry.VertexBufferCount * meshEntry.VertexSize);
                meshEntry.IndexBuffer = reader.ReadBytes(meshEntry.IndexBufferCount * meshEntry.IndexSize);

                if (meshEntry.MaterialIndex < DmaFile.MaterialEntries.Count)
                {
                    var materialEntry = DmaFile.MaterialEntries[meshEntry.MaterialIndex];

                    if (!meshEntry.CreateMesh(name, materialEntry, i))
                        return false;

                    Meshes.Add(meshEntry);
                }
            }

            // TODO: Read the rest of the data.

            return true;
        }
    }
}