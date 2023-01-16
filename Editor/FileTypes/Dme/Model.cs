using System;

namespace ForgeLightToolkit.Editor.FileTypes.Dme
{
    [Serializable]
    public class Model
    {
        public int Unknown;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;

        public int VertexSize;
        public int VertexBufferCount;

        public int IndexSize;
        public int IndexBufferCount;

        public byte[] VertexBuffer;
        public byte[] IndexBuffer;
    }
}