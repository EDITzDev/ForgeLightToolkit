﻿namespace ForgeLightToolkit.Editor.FileTypes.Dma
{
    public enum D3DXPARAMETER_CLASS : uint
    {
        D3DXPC_SCALAR,
        D3DXPC_VECTOR,
        D3DXPC_MATRIX_ROWS,
        D3DXPC_MATRIX_COLUMNS,
        D3DXPC_OBJECT,
        D3DXPC_STRUCT
    }

    public enum D3DXPARAMETER_TYPE : uint
    {
        D3DXPT_VOID,
        D3DXPT_BOOL,
        D3DXPT_INT,
        D3DXPT_FLOAT,
        D3DXPT_STRING,
        D3DXPT_TEXTURE,
        D3DXPT_TEXTURE1D,
        D3DXPT_TEXTURE2D,
        D3DXPT_TEXTURE3D,
        D3DXPT_TEXTURECUBE,
        D3DXPT_SAMPLER,
        D3DXPT_SAMPLER1D,
        D3DXPT_SAMPLER2D,
        D3DXPT_SAMPLER3D,
        D3DXPT_SAMPLERCUBE,
        D3DXPT_PIXELSHADER,
        D3DXPT_VERTEXSHADER,
        D3DXPT_PIXELFRAGMENT,
        D3DXPT_VERTEXFRAGMENT,
        D3DXPT_UNSUPPORTED
    }
}