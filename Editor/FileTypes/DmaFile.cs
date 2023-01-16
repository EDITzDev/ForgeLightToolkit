using System.IO;
using System.Collections.Generic;

using UnityEngine;

using ForgeLightToolkit.Editor.FileTypes.Dma;

namespace ForgeLightToolkit.Editor.FileTypes
{
    public class DmaFile : ScriptableObject
    {
        public List<string> Textures = new();

        public int MaterialCount;

        public List<MaterialEntry> MaterialEntries = new();

        public bool Load(string filePath, byte[] buffer)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(buffer);

            return LoadInternal(reader);
        }

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(File.OpenRead(filePath));

            return LoadInternal(reader);
        }

        private bool LoadInternal(Reader reader)
        {
            var magic = new string(reader.ReadChars(4));

            if (magic != "DMAT")
                return false;

            var version = reader.ReadUInt32();

            if (version != 1)
                return false;

            var textureDataSize = reader.ReadInt32();

            while (textureDataSize > 0)
            {
                var texture = reader.ReadNullTerminatedString();

                Textures.Add(texture);

                textureDataSize -= texture.Length + 1;
            }

            MaterialCount = reader.ReadInt32();

            for (var i = 0; i < MaterialCount; i++)
            {
                var materialEntry = new MaterialEntry();

                var hash = reader.ReadUInt32();

                var materialData = reader.ReadBytes(reader.ReadInt32());

                materialEntry.Deserialize(new Reader(materialData));

                MaterialEntries.Add(materialEntry);
            }

            return true;
        }
    }
}