using System;

using ForgeLightToolkit.Editor;

namespace ForgeLightToolkit
{
    [Serializable]
    public class FloraDefinition
    {
        public string Name;
        public string Texture;

        public bool Unknown;

        public void Deserialize(Reader reader)
        {
            Name = reader.ReadNullTerminatedString();
            Texture = reader.ReadNullTerminatedString();

            Unknown = reader.ReadBool();
        }
    }
}