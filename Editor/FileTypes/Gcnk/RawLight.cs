using ForgeLightToolkit.Settings;
using System;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Gcnk
{
    [Serializable]
    public class RawLight
    {
        public string Name;
        public string ColorName;

        public byte Type;

        public Vector4 Position;

        public float Range;
        public float Intensity;
        public Color32 Color;

        public void Deserialize(Reader reader)
        {
            Name = reader.ReadNullTerminatedString();
            ColorName = reader.ReadNullTerminatedString();

            Type = reader.ReadByte();

            Position = reader.ReadVector4();
            
            if (FLTKSettings.Instance.InvertZ)
            {
                Position.z = -Position.z;
            }

            Range = reader.ReadSingle();
            Intensity = reader.ReadSingle();
            Color = reader.ReadColor32();
        }
    }
}