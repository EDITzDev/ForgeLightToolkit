using System;
using System.Collections.Generic;

namespace ForgeLightToolkit.Editor.FileTypes.Dma
{
    [Serializable]
    public class MaterialEntry
    {
        public uint Hash;

        public List<ParameterEntry> ParameterEntries = new();

        public void Deserialize(Reader reader)
        {
            Hash = reader.ReadUInt32();

            var parameterCount = reader.ReadInt32();

            for (var i = 0; i < parameterCount; i++)
            {
                var parameterEntry = new ParameterEntry();

                parameterEntry.Deserialize(reader);

                ParameterEntries.Add(parameterEntry);
            }
        }
    }
}
