using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;

using ForgeLightToolkit.Editor;

namespace ForgeLightToolkit
{
    public class GzneFile : ScriptableObject
    {
        public bool HideTerrain;

        public int ChunkSize;
        public int TileSize;

        public int TilePerChunkAxis => ChunkSize / TileSize;

        public float WorldSize;
        public int Unknown5;

        public int StartX;
        public int StartY;

        public int Unknown8;
        public int Unknown9;
        public int Unknown10;

        [HideInInspector]
        public List<EcoData> EcoData = new();

        [HideInInspector]
        public List<FloraDefinition> FloraDefinitions = new();

        [HideInInspector]
        public List<List<Vector3>> InvisibleWalls = new();

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(File.OpenRead(filePath));

            var magic = new string(reader.ReadChars(4));

            if (magic != "GZNE")
                return false;

            var version = reader.ReadInt32();

            if (version > 3)
                throw new NotSupportedException($"Cannot process file with version {version}");

            if(version >= 3)
                HideTerrain = (reader.ReadInt32() & 1) == 1;

            ChunkSize = reader.ReadInt32();
            TileSize = reader.ReadInt32();

            WorldSize = reader.ReadSingle();
            Unknown5 = reader.ReadInt32();
            StartX = reader.ReadInt32();
            StartY = reader.ReadInt32();
            Unknown8 = reader.ReadInt32();
            Unknown9 = reader.ReadInt32();
            Unknown10 = reader.ReadInt32();

            var ecoDataCount = reader.ReadInt32();

            for (var i = 0; i < ecoDataCount; i++)
            {
                var ecoData = new EcoData();

                ecoData.Deserialize(reader);

                EcoData.Add(ecoData);
            }

            var floraDefinitionCount = reader.ReadInt32();

            for (var i = 0; i < floraDefinitionCount; i++)
            {
                var floraDefinition = new FloraDefinition();

                floraDefinition.Deserialize(reader);

                FloraDefinitions.Add(floraDefinition);
            }

            if (version >= 2)
            {
                var invisibleWallCount = reader.ReadInt32();

                if (invisibleWallCount > 10000)
                    throw new IndexOutOfRangeException($"Invalid number of Invisible Walls: {invisibleWallCount}");

                for (var i = 0; i < invisibleWallCount; i++)
                {
                    var invisibleWallVertexCount = reader.ReadInt32();

                    var invisibleWallVertices = new List<Vector3>();

                    for (var j = 0; j < invisibleWallVertexCount; j++)
                        invisibleWallVertices.Add(reader.ReadVector3());

                    InvisibleWalls.Add(invisibleWallVertices);
                }
            }

            return true;
        }
    }
}