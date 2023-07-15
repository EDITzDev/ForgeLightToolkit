using System;
using System.Collections.Generic;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Gcnk
{
    [Serializable]
    public class Tile
    {
        private int _version;

        public Vector2Int Coords;

        public Vector4 Position;

        public int Unknown7;

        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;

        public float Unknown12;

        public List<int> EcoDataList = new();

        public List<RuntimeObject> RuntimeObjects = new();

        public List<RawLight> RawLights = new();

        public List<RawArea> RawAreas = new();

        public List<RawGroup> RawGroups = new();

        public int Index;

        public Tile(int version)
        {
            _version = version;
        }

        public void Deserialize(Reader reader)
        {
            Coords.x = reader.ReadInt32();
            Coords.y = reader.ReadInt32();

            Position = reader.ReadVector4();

            Unknown7 = reader.ReadInt32();

            if (Unknown7 > 0)
            {
                Unknown8 = reader.ReadInt32();
                Unknown9 = reader.ReadInt32();
                Unknown10 = reader.ReadInt32();
                Unknown11 = reader.ReadInt32();
            }

            Unknown12 = reader.ReadSingle();

            var ecoDataCount = reader.ReadInt32();

            for (var i = 0; i < ecoDataCount; i++)
            {
                EcoDataList.Add(reader.ReadInt32());

                if(!reader.IsLittleEndian)
                {
                    var unknown = reader.ReadInt32();

                    if(unknown > 0)
                        reader.Skip(12 * unknown);
                }
            }

            var runtimeObjectCount = reader.ReadInt32();

            for (var i = 0; i < runtimeObjectCount; i++)
            {
                var runtimeObject = new RuntimeObject(_version);

                runtimeObject.Deserialize(reader);

                RuntimeObjects.Add(runtimeObject);
            }

            var rawLightCount = reader.ReadInt32();

            for (var i = 0; i < rawLightCount; i++)
            {
                var rawLight = new RawLight();

                rawLight.Deserialize(reader);

                RawLights.Add(rawLight);
            }

            var rawAreaCount = reader.ReadInt32();

            for (var i = 0; i < rawAreaCount; i++)
            {
                var rawArea = new RawArea();

                rawArea.Deserialize(reader);

                RawAreas.Add(rawArea);
            }

            var rawGroupCount = reader.ReadInt32();

            for (var i = 0; i < rawGroupCount; i++)
            {
                var rawGroup = new RawGroup();

                rawGroup.Deserialize(reader);

                RawGroups.Add(rawGroup);
            }

            Index = reader.ReadInt32();

            reader.Skip(4);
        }
    }
}
