using System;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes.Dma
{
    [Serializable]
    public class ParameterEntry
    {
        public uint Hash;

        public D3DXPARAMETER_CLASS Class;

        public D3DXPARAMETER_TYPE Type;

        public int Int;
        public float Float;
        public Vector4 Vector4;
        public Matrix4x4 Matrix4x4;
        public uint Object;

        public void Deserialize(Reader reader)
        {
            Hash = reader.ReadUInt32();

            Class = (D3DXPARAMETER_CLASS)reader.ReadUInt32();

            Type = (D3DXPARAMETER_TYPE)reader.ReadUInt32();

            var parameterData = reader.ReadBytes(reader.ReadInt32());

            var parameterReader = new Reader(parameterData);

            switch (Class)
            {
                case D3DXPARAMETER_CLASS.D3DXPC_SCALAR:

                    if (Type == D3DXPARAMETER_TYPE.D3DXPT_FLOAT)
                        Float = parameterReader.ReadSingle();
                    else
                        Int = parameterReader.ReadInt32();

                    break;

                case D3DXPARAMETER_CLASS.D3DXPC_VECTOR:
                    Vector4 = parameterReader.ReadVector4();
                    break;

                case D3DXPARAMETER_CLASS.D3DXPC_MATRIX_ROWS:
                case D3DXPARAMETER_CLASS.D3DXPC_MATRIX_COLUMNS:
                    Matrix4x4 = parameterReader.ReadMatrix4x4();
                    break;

                case D3DXPARAMETER_CLASS.D3DXPC_OBJECT:
                    Object = parameterReader.ReadUInt32();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
