using System;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Gcnk
{
    [Serializable]
    public class RawArea
    {
        public string Unknown;

        public int Unknown2;

        public string Unknown3;

        public Vector4 Unknown4;
        public Vector4 Unknown5;

        public int Unknown6;

        public Vector3 Unknown7;

        public void Deserialize(Reader reader)
        {
            Unknown = reader.ReadNullTerminatedString();

            Unknown2 = reader.ReadInt32();

            Unknown3 = reader.ReadNullTerminatedString();

            Unknown4 = reader.ReadVector4();

            Unknown5 = reader.ReadVector4();

            Unknown6 = reader.ReadInt32();

            Unknown7 = reader.ReadVector3();
        }
    }
}
