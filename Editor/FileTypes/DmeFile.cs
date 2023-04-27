using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using ForgeLightToolkit.Editor.FileTypes.Dme;

namespace ForgeLightToolkit.Editor.FileTypes
{
    public class DmeFile : ScriptableObject
    {
        [HideInInspector]
        public DmaFile DmaFile;

        public Bounds Bounds;

        public int MeshCount;

        [HideInInspector]
        public List<MeshEntry> Meshes = new();

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(File.OpenRead(filePath));

            var magic = new string(reader.ReadChars(4));

            if (magic != "DMOD")
                return false;

            var version = reader.ReadInt32();

            DmaFile = CreateInstance<DmaFile>();

            var dmaFileData = reader.ReadBytes(reader.ReadInt32());

            if (!DmaFile.Load(filePath, dmaFileData))
                return false;

            Bounds.SetMinMax(reader.ReadVector3(), reader.ReadVector3());

            MeshCount = DmaFile.MaterialEntries.Count;

            if (version >= 3)
                MeshCount = reader.ReadInt32();

            if (MeshCount != DmaFile.MaterialEntries.Count)
                return false;

            for (var i = 0; i < MeshCount; i++)
            {
                var meshEntry = new MeshEntry
                {
                    MaterialIndex = reader.ReadInt32(),

                    Unknown2 = reader.ReadInt32(),
                    Unknown3 = reader.ReadInt32(),
                    Unknown4 = reader.ReadInt32(),

                    VertexSize = reader.ReadInt32(),
                    VertexBufferCount = reader.ReadInt32(),

                    IndexSize = reader.ReadInt32(),
                    IndexBufferCount = reader.ReadInt32()
                };

                meshEntry.VertexBuffer = reader.ReadBytes(meshEntry.VertexBufferCount * meshEntry.VertexSize);
                meshEntry.IndexBuffer = reader.ReadBytes(meshEntry.IndexBufferCount * meshEntry.IndexSize);

                if (meshEntry.MaterialIndex >= DmaFile.MaterialEntries.Count)
                {
                    Debug.LogError("Invalid Material Index.");
                    return false;
                }

                var materialEntry = DmaFile.MaterialEntries[meshEntry.MaterialIndex];

                if (!meshEntry.CreateMesh(name, materialEntry))
                    return false;

                Meshes.Add(meshEntry);
            }

            // TODO: Read the rest of the data.

            /* if (!CreateMesh())
                return false; */

            return true;
        }

        /* private bool CreateMesh()
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var uvs2 = new List<Vector2>();
            var indices = new List<int>();

            foreach (var meshEntry in Meshes)
            {
                if (meshEntry.MaterialIndex >= DmaFile.MaterialEntries.Count)
                {
                    Debug.LogError("Invalid Material Index.");
                    return false;
                }

                var materialEntry = DmaFile.MaterialEntries[meshEntry.MaterialIndex];

                var materialDefinition = MaterialInfo.Instance.MaterialDefinitions.SingleOrDefault(x => x.NameHash == materialEntry.Hash);

                if (materialDefinition is null)
                    return false;

                var drawStyle = materialDefinition.DrawStyles.First();

                if (drawStyle is null)
                {
                    Debug.LogError("Material Definition doesn't contain a DrawStyle.");
                    return false;
                }

                var inputLayout = MaterialInfo.Instance.InputLayouts.SingleOrDefault(x => x.NameHash == drawStyle.InputLayoutHash);

                if (inputLayout is null)
                {
                    Debug.LogError("Failed to find Input Layout.");
                    return false;
                }

                var positionEntry = inputLayout.Entries.SingleOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.Position);

                if (positionEntry is null)
                {
                    Debug.LogError("Model doesn't have a position entry.");
                    return false;
                }

                if (positionEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float3)
                {
                    for (var j = 0; j < meshEntry.VertexBufferCount; j++)
                    {
                        var x = BitConverter.ToSingle(meshEntry.VertexBuffer, positionEntry.Offset + j * meshEntry.VertexSize + 0);
                        var y = BitConverter.ToSingle(meshEntry.VertexBuffer, positionEntry.Offset + j * meshEntry.VertexSize + 4);
                        var z = BitConverter.ToSingle(meshEntry.VertexBuffer, positionEntry.Offset + j * meshEntry.VertexSize + 8);

                        vertices.Add(new Vector3(x, y, z));
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                var normalEntry = inputLayout.Entries.SingleOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.Normal);

                if (normalEntry is not null)
                {
                    if (normalEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float3)
                    {
                        for (var j = 0; j < meshEntry.VertexBufferCount; j++)
                        {
                            var startIndex = normalEntry.Offset + j * meshEntry.VertexSize;

                            var x = BitConverter.ToSingle(meshEntry.VertexBuffer, startIndex + 0);
                            var y = BitConverter.ToSingle(meshEntry.VertexBuffer, startIndex + 4);
                            var z = BitConverter.ToSingle(meshEntry.VertexBuffer, startIndex + 8);

                            normals.Add(new Vector3(x, y, z));
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                var texCoordEntry = inputLayout.Entries.FirstOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.TexCoord && x.UsageIndex == 0);

                if (texCoordEntry is not null)
                {
                    if (texCoordEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float2)
                    {
                        for (var j = 0; j < meshEntry.VertexBufferCount; j++)
                        {
                            var x = BitConverter.ToSingle(meshEntry.VertexBuffer, texCoordEntry.Offset + j * meshEntry.VertexSize + 0);
                            var y = BitConverter.ToSingle(meshEntry.VertexBuffer, texCoordEntry.Offset + j * meshEntry.VertexSize + 4);

                            uvs.Add(new Vector2(x, y));
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                var texCoord2Entry = inputLayout.Entries.FirstOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.TexCoord && x.UsageIndex == 1);

                if (texCoord2Entry is not null)
                {
                    if (texCoord2Entry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float2)
                    {
                        for (var j = 0; j < meshEntry.VertexBufferCount; j++)
                        {
                            var x = BitConverter.ToSingle(meshEntry.VertexBuffer, texCoord2Entry.Offset + j * meshEntry.VertexSize + 0);
                            var y = BitConverter.ToSingle(meshEntry.VertexBuffer, texCoord2Entry.Offset + j * meshEntry.VertexSize + 4);

                            uvs2.Add(new Vector2(x, y));
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                for (var j = 0; j < meshEntry.IndexBufferCount; j++)
                {
                    indices.Add(meshEntry.IndexSize switch
                    {
                        2 => BitConverter.ToInt16(meshEntry.IndexBuffer, j * meshEntry.IndexSize),
                        4 => BitConverter.ToInt32(meshEntry.IndexBuffer, j * meshEntry.IndexSize),
                        _ => throw new NotImplementedException()
                    });
                }
            }

            Mesh.SetVertices(vertices);
            Mesh.SetNormals(normals);

            Mesh.SetUVs(0, uvs);

            if (uvs2.Count > 0)
                Mesh.SetUVs(1, uvs2);

            Mesh.subMeshCount = MeshCount;

            var indexOffset = 0;
            var vertexOffset = 0;

            for (var i = 0; i < MeshCount; i++)
            {
                var meshEntry = Meshes[i];

                var subIndices = indices.Skip(indexOffset).Take(meshEntry.IndexBufferCount).ToList();

                Mesh.SetIndices(subIndices, MeshTopology.Triangles, i, baseVertex: vertexOffset);

                indexOffset += meshEntry.IndexBufferCount;
                vertexOffset += meshEntry.VertexBufferCount;
            }

            return true;
        } */
    }
}