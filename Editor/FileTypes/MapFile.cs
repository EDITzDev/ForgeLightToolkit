using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes
{
    public class MapFile : ScriptableObject
    {
        [HideInInspector]
        public List<Node> Nodes = new();

        [Serializable]
        public struct Node
        {
            public int Id;

            public Vector3 Position;

            public Edge[] Edges;
        }

        [Serializable]
        public struct Edge
        {
            public int Id;

            public float Distance;
        }

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(File.OpenRead(filePath));

            while (!reader.ReachedEnd)
            {
                var node = new Node
                {
                    Id = reader.ReadInt32(),
                    Position = reader.ReadVector3()
                };

                var edgeCount = reader.ReadUInt32();

                node.Edges = new Edge[edgeCount];

                for (var i = 0; i < node.Edges.Length; i++)
                {
                    node.Edges[i].Id = reader.ReadInt32();
                    node.Edges[i].Distance = reader.ReadSingle();
                }

                Nodes.Add(node);
            }

            return true;
        }
    }
}