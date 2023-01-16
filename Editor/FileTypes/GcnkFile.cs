using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Ionic.Zlib;

using ForgeLightToolkit.Editor.FileTypes.Gcnk;

namespace ForgeLightToolkit.Editor.FileTypes
{
    public class GcnkFile : ScriptableObject
    {
        public int Version;

        public Vector2Int Coords;

        [HideInInspector]
        public List<Tile> Tiles = new();

        [HideInInspector]
        public Texture2D HeightMap;

        [HideInInspector]
        public List<ExportRenderBatch> ExportRenderBatches = new();

        [HideInInspector]
        public List<ushort> IndexBuffer = new();

        [HideInInspector]
        public List<Vertex> VertexBuffer = new();

        [HideInInspector]
        public Mesh Mesh;

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(File.OpenRead(filePath));

            var magic = new string(reader.ReadChars(4));

            if (magic != "GCNK")
                return false;

            Version = reader.ReadInt32();

            if (Version is < 0 or > 6)
                return false;

            var chunkUncompressedLength = reader.ReadInt32();
            var chunkCompressedLength = reader.ReadInt32();

            var chunkCompressedData = reader.ReadBytes(chunkCompressedLength);

            var chunkDecompressedStream = new MemoryStream();

            using (var compressedStream = new MemoryStream(chunkCompressedData))
            using (var zlibStream = new ZlibStream(compressedStream, CompressionMode.Decompress))
                zlibStream.CopyTo(chunkDecompressedStream);

            if (chunkDecompressedStream.Position != chunkUncompressedLength)
                return false;

            chunkDecompressedStream.Position = 0;

            if (!LoadChunk(chunkDecompressedStream))
                return false;

            var collisionUncompressedLength = reader.ReadInt32();
            var collisionCompressedLength = reader.ReadInt32();

            var collisionCompressedData = reader.ReadBytes(collisionCompressedLength);

            var collisionDecompressedStream = new MemoryStream();

            using (var compressedStream = new MemoryStream(collisionCompressedData))
            using (var zlibStream = new ZlibStream(compressedStream, CompressionMode.Decompress))
                zlibStream.CopyTo(collisionDecompressedStream);

            if (collisionDecompressedStream.Position != collisionUncompressedLength)
                return false;

            collisionDecompressedStream.Position = 0;

            if (!LoadCollision(chunkDecompressedStream))
                return false;

            if (!CreateChunkMesh())
                return false;

            return true;
        }

        private bool LoadChunk(Stream chunkStream)
        {
            var reader = new Reader(chunkStream);

            var tileCount = reader.ReadInt32();

            for (var i = 0; i < tileCount; i++)
            {
                var tile = new Tile(Version);

                tile.Deserialize(reader);

                if (i == 0)
                    Coords = tile.Coords;

                Tiles.Add(tile);
            }

            var exportRenderBatchCount = reader.ReadInt32();

            for (var i = 0; i < exportRenderBatchCount; i++)
            {
                var exportRenderBatch = new ExportRenderBatch();

                exportRenderBatch.Deserialize(reader);

                ExportRenderBatches.Add(exportRenderBatch);
            }

            var heightMapBbp = reader.ReadInt32();

            if (heightMapBbp > 0)
            {
                if (Version > 4)
                {
                    var heightMapSize = reader.ReadInt32();

                    var data = new byte[heightMapSize * heightMapSize * heightMapBbp];

                    for (var i = 0; i < data.Length; i += 2)
                    {
                        data[i] = reader.ReadByte();
                        data[i + 1] = 0;
                    }

                    HeightMap = new Texture2D(heightMapSize, heightMapSize, TextureFormat.ARGB4444, true)
                    {
                        name = name,
                        requestedMipmapLevel = 1
                    };

                    HeightMap.LoadRawTextureData(data);
                    HeightMap.Apply();
                }
            }

            var indexBufferCount = reader.ReadInt32();

            for (var i = 0; i < indexBufferCount; i++)
                IndexBuffer.Add(reader.ReadUInt16());

            // TODO: FreeRealms PS3

            var vertexBufferCount = reader.ReadInt32();

            for (var i = 0; i < vertexBufferCount; i++)
            {
                var vertexBuffer = new Vertex();

                vertexBuffer.Deserialize(reader);

                VertexBuffer.Add(vertexBuffer);
            }

            return true;
        }

        private bool LoadCollision(Stream collisionStream)
        {
            // TODO: Bullet Physics

            return true;
        }

        private bool CreateChunkMesh()
        {
            Mesh = new Mesh
            {
                name = name
            };

            var vertices = VertexBuffer.Select(x => x.Position).ToList();
            Mesh.SetVertices(vertices);

            var normals = VertexBuffer.Select(x => x.Normal).ToList();
            Mesh.SetNormals(normals);

            var uvs = VertexBuffer.Select(x => x.TexCoord / 1024).ToList();
            Mesh.SetUVs(0, uvs);

            Mesh.subMeshCount = ExportRenderBatches.Count;

            for (var i = 0; i < ExportRenderBatches.Count; i++)
            {
                var exportRenderBatch = ExportRenderBatches[i];

                var indices = IndexBuffer.Skip(exportRenderBatch.IndexOffset).Take(exportRenderBatch.IndexCount).ToList();

                Mesh.SetIndices(indices, MeshTopology.Triangles, i, baseVertex: exportRenderBatch.VertexOffset);
            }

            return true;
        }
    }
}