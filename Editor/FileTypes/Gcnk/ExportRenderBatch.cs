using System;

namespace ForgeLightToolkit.Editor.FileTypes.Gcnk
{
    [Serializable]
    public class ExportRenderBatch
    {
        public int IndexOffset;
        public int IndexCount;
        public int VertexOffset;
        public int VertexCount;

        public void Deserialize(Reader reader)
        {
            IndexOffset = reader.ReadInt32();
            IndexCount = reader.ReadInt32();
            VertexOffset = reader.ReadInt32();
            VertexCount = reader.ReadInt32();
        }
    }
}