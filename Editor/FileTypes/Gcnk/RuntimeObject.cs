using System;
using System.Collections.Generic;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Gcnk
{
    [Serializable]
    public class RuntimeObject
    {
        private readonly int _version;

        public int Unknown; // TODO

        public string FileName;
        public string Unknown3;

        public Vector4 Position;
        public Vector4 Rotation;

        public float Scale;

        public string MaterialName;
        public string TintAlias;

        public Vector4 Unknown9;

        public int ObjectId;
        public int Unknown11;

        public List<byte[]> Unknown12 = new();

        public RuntimeObject(int version)
        {
            _version = version;
        }

        public override string ToString()
        {
            return $"{nameof(Unknown)}: {Unknown}, " +
                   $"{nameof(FileName)}: {FileName}, " +
                   $"{nameof(Position)}: ({Position.x}, {Position.y}, {Position.z}), " +
                   $"{nameof(Rotation)}: ({Rotation.x}, {Rotation.y}, {Rotation.z}), " +
                   $"{nameof(Scale)}: {Scale}, " +
                   $"{nameof(MaterialName)}: {MaterialName}, " +
                   $"{nameof(TintAlias)}: {TintAlias}, " +
                   $"{nameof(Unknown9)}: ({Unknown9.x}, {Unknown9.y}, {Unknown9.z}), " +
                   $"{nameof(ObjectId)}: {ObjectId}, " +
                   $"{nameof(Unknown11)}: {Unknown11}";
        }

        public void Deserialize(Reader reader)
        {
            Unknown = reader.ReadInt32();

            FileName = reader.ReadNullTerminatedString();
            Unknown3 = reader.ReadNullTerminatedString();

            Position = reader.ReadVector4();

            Rotation = reader.ReadVector4();

            Scale = reader.ReadSingle();

            if (_version >= 6)
            {
                MaterialName = reader.ReadNullTerminatedString();
                TintAlias = reader.ReadNullTerminatedString();

                if (!string.IsNullOrEmpty(TintAlias))
                {
                    Unknown9 = reader.ReadVector4();
                }
            }

            reader.Skip(4);

            if (_version > 4)
            {
                ObjectId = reader.ReadInt32();
            }
            else
            {
                var unknown = reader.ReadNullTerminatedString();
                Debug.Log($"Unknown RuntimeObject Value: {unknown}");
            }

            Unknown11 = reader.ReadInt32();

            if (_version > 2)
            {
                var unknownCount = reader.ReadInt32();

                if (unknownCount > 0)
                {
                    for (var i = 0; i < unknownCount; i++)
                    {
                        var unknown12Size = reader.ReadInt32();

                        Unknown12.Add(reader.ReadBytes(unknown12Size * 4));
                    }
                }
            }
        }
    }
}