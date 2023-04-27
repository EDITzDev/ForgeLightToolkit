using System;
using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Map
{
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
}