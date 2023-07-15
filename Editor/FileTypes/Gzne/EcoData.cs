using System;
using System.Collections.Generic;

using ForgeLightToolkit.Editor;

namespace ForgeLightToolkit
{
    [Serializable]
    public class EcoData
    {
        public int Unknown;

        public string Name;
        public string Texture;

        public int Scale;

        public List<FloraData> FloraData = new();

        public void Deserialize(Reader reader)
        {
            Unknown = reader.ReadInt32();

            Name = reader.ReadNullTerminatedString();
            Texture = reader.ReadNullTerminatedString();

            Scale = reader.ReadInt32();

            var floraDataCount = reader.ReadInt32();

            for (var i = 0; i < floraDataCount; i++)
            {
                var floraData = new FloraData();

                floraData.Deserialize(reader);

                FloraData.Add(floraData);
            }
        }
    }
}