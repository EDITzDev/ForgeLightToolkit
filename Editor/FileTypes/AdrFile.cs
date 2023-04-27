#nullable enable

using System.IO;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes
{
    public class AdrFile : ScriptableObject
    {
        public string? ModelFileName;
        public string? MaterialFileName;

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(File.OpenRead(filePath));

            while (!reader.ReachedEnd)
            {
                var definitionType = reader.ReadByte();
                var definitionSize = reader.ReadCompressedLength();
                var definitionData = reader.ReadBytes(definitionSize);

                switch (definitionType)
                {
                    case 2:
                        ParseModelDefinition(definitionData);
                        break;
                }
            }

            return true;
        }

        private void ParseModelDefinition(byte[] data)
        {
            var reader = new Reader(data);

            while (!reader.ReachedEnd)
            {
                var definitionType = reader.ReadByte();
                var definitionSize = reader.ReadCompressedLength();

                switch (definitionType)
                {
                    case 1:
                        ModelFileName = reader.ReadNullTerminatedString();
                        break;

                    case 2:
                        MaterialFileName = reader.ReadNullTerminatedString();
                        break;

                    default:
                        reader.Skip(definitionSize);
                        break;
                }
            }
        }
    }
}