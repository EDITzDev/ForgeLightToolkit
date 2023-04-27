using ForgeLightToolkit.Editor.FileTypes.Dma;
using System;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Dme
{
    [Serializable]
    public class MeshEntry
    {
        public int MaterialIndex;

        public int Unknown2;
        public int Unknown3;
        public int Unknown4;

        public int VertexSize;
        public int VertexBufferCount;

        public int IndexSize;
        public int IndexBufferCount;

        public byte[] VertexBuffer;
        public byte[] IndexBuffer;

        public Mesh Mesh;

        public bool CreateMesh(string name, MaterialEntry materialEntry)
        {
            Mesh = new Mesh();

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

            // Vertices

            var vertices = new Vector3[VertexBufferCount];

            var positionEntry = inputLayout.Entries.SingleOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.Position);

            if (positionEntry is null)
            {
                Debug.LogError("Model doesn't have a position entry.");
                return false;
            }

            if (positionEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float3)
            {
                for (var i = 0; i < VertexBufferCount; i++)
                {
                    var x = BitConverter.ToSingle(VertexBuffer, positionEntry.Offset + i * VertexSize + 0);
                    var y = BitConverter.ToSingle(VertexBuffer, positionEntry.Offset + i * VertexSize + 4);
                    var z = BitConverter.ToSingle(VertexBuffer, positionEntry.Offset + i * VertexSize + 8);

                    vertices[i] = new Vector3(x, y, z);
                }
            }
            else
            {
                Debug.LogError($"Unimplemented Input Layout Type \"{positionEntry.Type}\" for Normal.");
                return false;
            }

            Mesh.SetVertices(vertices);

            // Normals

            var normals = new Vector3[VertexBufferCount];

            var normalEntry = inputLayout.Entries.SingleOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.Normal);

            if (normalEntry is not null)
            {
                if (normalEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float3)
                {
                    for (var i = 0; i < VertexBufferCount; i++)
                    {
                        var startIndex = normalEntry.Offset + i * VertexSize;

                        var x = BitConverter.ToSingle(VertexBuffer, startIndex + 0);
                        var y = BitConverter.ToSingle(VertexBuffer, startIndex + 4);
                        var z = BitConverter.ToSingle(VertexBuffer, startIndex + 8);

                        normals[i] = new Vector3(x, y, z);
                    }
                }
                else
                {
                    Debug.LogError($"Unimplemented Input Layout Type \"{normalEntry.Type}\" for Normal.");
                    return false;
                }
            }

            Mesh.SetNormals(normals);

            // UVs

            foreach (var texCoordEntry in inputLayout.Entries.Where(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.TexCoord))
            {
                var uvs = new Vector2[VertexBufferCount];

                if (texCoordEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float2)
                {
                    for (var i = 0; i < VertexBufferCount; i++)
                    {
                        var x = BitConverter.ToSingle(VertexBuffer, texCoordEntry.Offset + i * VertexSize + 0);
                        var y = BitConverter.ToSingle(VertexBuffer, texCoordEntry.Offset + i * VertexSize + 4);

                        uvs[i] = new Vector2(x, y);
                    }
                }
                else
                {
                    Debug.LogError($"Unimplemented Input Layout Type \"{texCoordEntry.Type}\" for TexCoord, UsageIndex {texCoordEntry.UsageIndex}.");
                    return false;
                }

                Mesh.SetUVs(texCoordEntry.UsageIndex, uvs);
            }

            // Indices

            var indices = new int[IndexBufferCount];

            for (var i = 0; i < IndexBufferCount; i++)
            {
                indices[i] = IndexSize switch
                {
                    2 => BitConverter.ToInt16(IndexBuffer, i * IndexSize),
                    4 => BitConverter.ToInt32(IndexBuffer, i * IndexSize),
                    _ => throw new NotImplementedException()
                };
            }

            Mesh.SetIndices(indices, MeshTopology.Triangles, 0);

            return true;
        }
    }
}