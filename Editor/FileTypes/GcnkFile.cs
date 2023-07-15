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
        public Texture2D DetailMask;

        [HideInInspector]
        public List<ExportRenderBatch> ExportRenderBatches = new();

        [HideInInspector]
        public ushort[] IndexBuffer;

        [HideInInspector]
        public Vertex[] VertexBuffer;

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

            var detailMaskCount = reader.ReadInt32();

            if (detailMaskCount > 0)
            {
                if (Version < 4)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    var detailMaskSize = reader.ReadInt32();

                    if (detailMaskCount == 1)
                    {
                        var data = new byte[detailMaskSize * detailMaskSize * 2 / 2];

                        for (var i = 0; i < data.Length; i += 2)
                        {
                            var pixel = reader.ReadByte();

                            data[i] = (byte)((pixel >> 4) * 16);
                            data[i + 1] = (byte)((pixel & 15) * 16);
                        }

                        DetailMask = new Texture2D(detailMaskSize, detailMaskSize, TextureFormat.R8, false)
                        {
                            name = name
                        };

                        DetailMask.LoadRawTextureData(data);
                        DetailMask.Apply();
                    }
                    else if (detailMaskCount == 2)
                    {
                        var data = new byte[detailMaskSize * detailMaskSize * 4 / 2];

                        for (var i = 0; i < data.Length; i += 2)
                        {
                            data[i + 1] = reader.ReadByte();
                        }

                        DetailMask = new Texture2D(detailMaskSize, detailMaskSize, TextureFormat.ARGB4444, false)
                        {
                            name = name
                        };

                        DetailMask.LoadRawTextureData(data);
                        DetailMask.Apply();
                    }
                    else if (detailMaskCount == 4)
                    {
                        var data = reader.ReadBytes(detailMaskSize * detailMaskSize * 4 / 2);

                        DetailMask = new Texture2D(detailMaskSize, detailMaskSize, TextureFormat.ARGB4444, false)
                        {
                            name = name
                        };

                        DetailMask.LoadRawTextureData(data);
                        DetailMask.Apply();
                    }
                    else
                    {
                        reader.Skip(detailMaskSize * detailMaskSize * detailMaskCount / 2);

                        Debug.Log($"Unknown Detail Mask Count: {detailMaskCount} File: {name}");
                    }
                }
            }

            var indexBufferCount = reader.ReadInt32();

            IndexBuffer = new ushort[indexBufferCount];

            for (int i = 0; i < IndexBuffer.Length; i++)
                IndexBuffer[i] = reader.ReadUInt16();

            if (!reader.IsLittleEndian)
            {
                var unknown = reader.ReadInt32();

                reader.Skip(unknown);
            }

            var vertexBufferCount = reader.ReadInt32();

            VertexBuffer = new Vertex[vertexBufferCount];

            for (var i = 0; i < VertexBuffer.Length; i++)
            {
                var vertexBuffer = new Vertex();

                vertexBuffer.Deserialize(reader);

                VertexBuffer[i] = vertexBuffer;
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

            var colors = VertexBuffer.Select(x => x.Color).ToList();
            Mesh.SetColors(colors);

            // Hack - Unity doesn't support multiple colors per vertex.
            var colors2 = VertexBuffer.Select(x => x.Color2).ToList();
            Mesh.SetUVs(7, colors2);

            var uvs = VertexBuffer.Select(x => x.TexCoord / 1024).ToList();
            Mesh.SetUVs(0, uvs);

            var uvs2 = VertexBuffer.Select(x => x.TexCoord2 / 256).ToList();
            Mesh.SetUVs(1, uvs2);

            Mesh.subMeshCount = ExportRenderBatches.Count;

            for (var i = 0; i < ExportRenderBatches.Count; i++)
            {
                var exportRenderBatch = ExportRenderBatches[i];

                var indices = IndexBuffer.Skip(exportRenderBatch.IndexOffset).Take(exportRenderBatch.IndexCount).ToArray();
                Mesh.SetIndices(indices, MeshTopology.Triangles, i, baseVertex: exportRenderBatch.VertexOffset);
            }

            return true;
        }
    }
}