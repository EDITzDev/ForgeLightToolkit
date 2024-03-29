﻿using System;
using System.IO;
using System.Buffers.Binary;

using UnityEngine;

namespace ForgeLightToolkit.Editor
{
    public class Reader
    {
        private readonly Stream _stream;
        private readonly BinaryReader _br;

        public Reader(string path)
        {
            _stream = File.OpenRead(path);

            if (_stream is null)
                throw new ArgumentNullException(nameof(_stream));

            _br = new BinaryReader(_stream);
        }

        public Reader(Stream stream)
        {
            _stream = stream;

            _br = new BinaryReader(_stream);
        }

        public Reader(byte[] data) : this(new MemoryStream(data))
        {
        }

        public bool ReachedEnd => _stream.Position == _stream.Length;
        public int Remaining => Convert.ToInt32(_stream.Length - _stream.Position);

        public void Skip(int count) => _stream.Position += count;
        public void Seek(int offset) => _stream.Seek(offset, SeekOrigin.Begin);

        public byte ReadByte() => _br.ReadByte();
        public char[] ReadChars(int count) => _br.ReadChars(count);
        public byte[] ReadBytes(int count) => _br.ReadBytes(count);

        public short ReadInt16() => _br.ReadInt16();
        public ushort ReadUInt16() => _br.ReadUInt16();

        public int ReadInt32() => _br.ReadInt32();
        public uint ReadUInt32() => _br.ReadUInt32();

        public float ReadSingle() => _br.ReadSingle();

        public string ReadNullTerminatedString()
        {
            var tempString = string.Empty;

            char tempChar;

            while ((tempChar = _br.ReadChar()) != '\0')
                tempString += tempChar;

            return tempString;
        }

        public int ReadCompressedLength()
        {
            int uncompressedLength;

            int compressedLength = _br.ReadByte();

            if (compressedLength < 128)
            {
                uncompressedLength = compressedLength;
            }
            else if (compressedLength != 0xff)
            {
                uncompressedLength = (compressedLength & 0x7F) << 8;
                uncompressedLength |= _br.ReadByte();
            }
            else
            {
                uncompressedLength = BinaryPrimitives.ReverseEndianness(ReadInt32());
            }

            return uncompressedLength;
        }

        public Color32 ReadColor32()
        {
            var a = ReadByte();
            var r = ReadByte();
            var g = ReadByte();
            var b = ReadByte();

            return new Color32(r, g, b, a);
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector4 ReadVector4()
        {
            return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Matrix4x4 ReadMatrix4x4()
        {
            return new Matrix4x4(ReadVector4(), ReadVector4(), ReadVector4(), ReadVector4());
        }
    }
}