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

        public int ModelCount;

        [HideInInspector]
        public List<Model> Models = new();

        [HideInInspector]
        public Mesh Mesh;

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

            ModelCount = DmaFile.MaterialEntries.Count;

            if (version >= 3)
                ModelCount = reader.ReadInt32();

            if (ModelCount != DmaFile.MaterialEntries.Count)
                return false;

            for (var i = 0; i < ModelCount; i++)
            {
                var model = new Model
                {
                    Unknown = reader.ReadInt32(),
                    Unknown2 = reader.ReadInt32(),
                    Unknown3 = reader.ReadInt32(),
                    Unknown4 = reader.ReadInt32(),

                    VertexSize = reader.ReadInt32(),
                    VertexBufferCount = reader.ReadInt32(),

                    IndexSize = reader.ReadInt32(),
                    IndexBufferCount = reader.ReadInt32()
                };

                model.VertexBuffer = reader.ReadBytes(model.VertexBufferCount * model.VertexSize);
                model.IndexBuffer = reader.ReadBytes(model.IndexBufferCount * model.IndexSize);

                Models.Add(model);
            }

            // TODO: Read the rest of the data.

            if (!CreateMeshes())
                return false;

            return true;
        }

        private bool CreateMeshes()
        {
            Mesh = new Mesh
            {
                name = name
            };

            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var indices = new List<int>();

            for (var i = 0; i < ModelCount; i++)
            {
                var model = Models[i];

                var materialData = DmaFile.MaterialEntries[i];

                var materialDefinition = MaterialInfo.Instance.MaterialDefinitions.FirstOrDefault(x => x.NameHash == materialData.Hash);

                if (materialDefinition is null)
                    return false;

                var drawStyle = materialDefinition.DrawStyles.First();

                if (drawStyle is null)
                    return false;

                var inputLayout = MaterialInfo.Instance.InputLayouts.FirstOrDefault(x => x.NameHash == drawStyle.InputLayoutHash);

                if (inputLayout is null)
                    return false;

                var positionEntry = inputLayout.Entries.FirstOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.Position);

                if (positionEntry is null)
                    return false;

                if (positionEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float3)
                {
                    for (var j = 0; j < model.VertexBufferCount; j++)
                    {
                        var x = BitConverter.ToSingle(model.VertexBuffer, positionEntry.Offset + j * model.VertexSize + 0);
                        var y = BitConverter.ToSingle(model.VertexBuffer, positionEntry.Offset + j * model.VertexSize + 4);
                        var z = BitConverter.ToSingle(model.VertexBuffer, positionEntry.Offset + j * model.VertexSize + 8);

                        vertices.Add(new Vector3(x, y, z));
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                var normalEntry = inputLayout.Entries.FirstOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.Normal);

                if (normalEntry is null)
                    return false;

                if (normalEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float3)
                {
                    for (var j = 0; j < model.VertexBufferCount; j++)
                    {
                        var x = BitConverter.ToSingle(model.VertexBuffer, normalEntry.Offset + j * model.VertexSize + 0);
                        var y = BitConverter.ToSingle(model.VertexBuffer, normalEntry.Offset + j * model.VertexSize + 4);
                        var z = BitConverter.ToSingle(model.VertexBuffer, normalEntry.Offset + j * model.VertexSize + 8);

                        normals.Add(new Vector3(x, y, z));
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                var texCoordEntry = inputLayout.Entries.FirstOrDefault(x => x.Usage == MaterialInfo.InputLayout.Entry.EntryUsage.TexCoord);

                if (texCoordEntry is null)
                    return false;

                if (texCoordEntry.Type == MaterialInfo.InputLayout.Entry.EntryType.Float2)
                {
                    for (var j = 0; j < model.VertexBufferCount; j++)
                    {
                        var x = BitConverter.ToSingle(model.VertexBuffer, texCoordEntry.Offset + j * model.VertexSize + 0);
                        var y = BitConverter.ToSingle(model.VertexBuffer, texCoordEntry.Offset + j * model.VertexSize + 4);

                        uvs.Add(new Vector2(x, y));
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                for (var j = 0; j < model.IndexBufferCount; j++)
                {
                    indices.Add(model.IndexSize switch
                    {
                        2 => BitConverter.ToInt16(model.IndexBuffer, j * model.IndexSize),
                        4 => BitConverter.ToInt32(model.IndexBuffer, j * model.IndexSize),
                        _ => throw new NotImplementedException()
                    });
                }
            }

            Mesh.SetVertices(vertices);
            Mesh.SetNormals(normals);
            Mesh.SetUVs(0, uvs);

            Mesh.subMeshCount = ModelCount;

            var indexOffset = 0;
            var vertexOffset = 0;

            for (var i = 0; i < ModelCount; i++)
            {
                var model = Models[i];

                var subIndices = indices.Skip(indexOffset).Take(model.IndexBufferCount).ToList();

                Mesh.SetIndices(subIndices, MeshTopology.Triangles, i, baseVertex: vertexOffset);

                indexOffset += model.IndexBufferCount;
                vertexOffset += model.VertexBufferCount;
            }

            return true;
        }
    }
}