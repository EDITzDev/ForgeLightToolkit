using System;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Gcnk
{
    [Serializable]
    public class RawGroup
    {
        public string Unknown;

        public Vector4 Unknown2;
        public Vector4 Unknown3;

        public int Unknown4;

        public void Deserialize(Reader reader)
        {
            Unknown = reader.ReadNullTerminatedString();

            Unknown2 = reader.ReadVector4();

            Unknown3 = reader.ReadVector4();

            Unknown4 = reader.ReadInt32();
        }
    }
}
