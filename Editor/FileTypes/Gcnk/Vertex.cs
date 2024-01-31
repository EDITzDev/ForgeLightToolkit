﻿using ForgeLightToolkit.Settings;
using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Gcnk
{
    public struct Vertex
    {
        public Vector3 Position;

        public Vector3 Normal;

        public Color32 Color;
        public Vector4 Color2;

        public Vector2 TexCoord;
        public Vector2 TexCoord2;

        public void Deserialize(Reader reader)
        {
            Position = reader.ReadVector3();

            if (FLTKSettings.Instance.InvertZ)
            {
                Position.z = -Position.z;
            }

            Normal.x = reader.ReadByte() / 127.5f - 1;
            Normal.y = reader.ReadByte() / 127.5f - 1;
            Normal.z = reader.ReadByte() / 127.5f - 1;

            if (FLTKSettings.Instance.InvertZ)
            {
                Normal.z = -Normal.z;
            }

            reader.Skip(1);

            Color.r = reader.ReadByte();
            Color.b = reader.ReadByte();
            Color.g = reader.ReadByte();
            Color.a = reader.ReadByte();

            Color2.x = reader.ReadByte();
            Color2.y = reader.ReadByte();
            Color2.z = reader.ReadByte();
            Color2.w = reader.ReadByte();

            TexCoord.x = reader.ReadUInt16();
            TexCoord.y = reader.ReadUInt16();

            TexCoord2.x = reader.ReadUInt16();
            TexCoord2.y = reader.ReadUInt16();
        }
    }
}