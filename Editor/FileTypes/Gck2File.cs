using System.IO;

using UnityEngine;

using Ionic.Zlib;

namespace ForgeLightToolkit.Editor.FileTypes
{
    public class Gck2File : ScriptableObject
    {
        public int Version;

        public int TextureSize;

        public Texture2D Texture;

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            var reader = new Reader(File.OpenRead(filePath));

            var magic = new string(reader.ReadChars(4));

            if (magic != "GCK2")
                return false;

            Version = reader.ReadInt32();

            if (Version < 0)
                return false;

            TextureSize = reader.ReadInt32();

            var uncompressedLength = reader.ReadInt32();
            var compressedLength = reader.ReadInt32();

            var compressedData = reader.ReadBytes(compressedLength);

            var decompressedStream = new MemoryStream();

            using (var compressedStream = new MemoryStream(compressedData))
            using (var zlibStream = new ZlibStream(compressedStream, CompressionMode.Decompress))
                zlibStream.CopyTo(decompressedStream);

            if (decompressedStream.Position != uncompressedLength)
                return false;

            decompressedStream.Position = 0;

            if (!LoadTexture(decompressedStream))
                return false;

            return true;
        }

        private bool LoadTexture(Stream textureStream)
        {
            var reader = new Reader(textureStream);

            var magic = new string(reader.ReadChars(4));

            if (magic != "DDS ")
                return false;

            var size = reader.ReadInt32();

            if (size != 124)
                return false;

            // DDS_HEADER
            var flags = reader.ReadInt32();
            var height = reader.ReadInt32();
            var width = reader.ReadInt32();
            var pitchOrLinearSize = reader.ReadInt32();
            var depth = reader.ReadInt32();
            var mipMapCount = reader.ReadInt32();
            var reserved = reader.ReadBytes(11 * sizeof(int));

            // DDS_PIXELFORMAT
            var ddspfSize = reader.ReadUInt32();
            var ddspfFlags = reader.ReadUInt32();
            var ddspfFourCc = new string(reader.ReadChars(4));
            var ddspfRgbBitCount = reader.ReadUInt32();
            var ddspfRBitMask = reader.ReadUInt32();
            var ddspfGBitMask = reader.ReadUInt32();
            var ddspfBBitMask = reader.ReadUInt32();
            var ddspfABitMask = reader.ReadUInt32();

            // DDS_HEADER
            var caps = reader.ReadInt32();
            var caps2 = reader.ReadInt32();
            var caps3 = reader.ReadInt32();
            var caps4 = reader.ReadInt32();
            var reserved2 = reader.ReadInt32();

            var data = reader.ReadBytes(reader.Remaining);

            var alphaPixels = (ddspfFlags & 0x1) != 0;
            var alpha = (ddspfFlags & 0x2) != 0;
            var fourCc = (ddspfFlags & 0x4) != 0;
            var rgb = (ddspfFlags & 0x40) != 0;
            var yuv = (ddspfFlags & 0x200) != 0;
            var luminance = (ddspfFlags & 0x20000) != 0;

            var rgb888 = ddspfRBitMask == 0x000000ff && ddspfGBitMask == 0x0000ff00 && ddspfBBitMask == 0x00ff0000;
            var bgr888 = ddspfRBitMask == 0x00ff0000 && ddspfGBitMask == 0x0000ff00 && ddspfBBitMask == 0x000000ff;
            var rgb565 = ddspfRBitMask == 0x0000f800 && ddspfGBitMask == 0x000007e0 && ddspfBBitMask == 0x0000001f;
            var argb4444 = ddspfABitMask == 0x0000f000 && ddspfRBitMask == 0x00000f00 && ddspfGBitMask == 0x000000f0 && ddspfBBitMask == 0x0000000f;
            var rbga4444 = ddspfABitMask == 0x0000000f && ddspfRBitMask == 0x0000f000 && ddspfGBitMask == 0x000000f0 && ddspfBBitMask == 0x00000f00;

            TextureFormat textureFormat;

            switch (fourCc)
            {
                case true when ddspfFourCc == "DXT1":
                    textureFormat = TextureFormat.DXT1;
                    break;
                case true when ddspfFourCc == "DXT5":
                    textureFormat = TextureFormat.DXT5;
                    break;
                default:
                {
                    switch (rgb)
                    {
                        case true when rgb888 || bgr888:
                            textureFormat = alphaPixels ? TextureFormat.RGBA32 : TextureFormat.RGB24;
                            break;
                        case true when rgb565:
                            textureFormat = TextureFormat.RGB565;
                            break;
                        case true when alphaPixels && argb4444:
                            textureFormat = TextureFormat.ARGB4444;
                            break;
                        case true when alphaPixels && rbga4444:
                            textureFormat = TextureFormat.RGBA4444;
                            break;
                        case false when alpha != luminance:
                            textureFormat = TextureFormat.Alpha8;
                            break;
                        default:
                            return false;
                    }

                    break;
                }
            }

            Texture = new Texture2D(width, height, textureFormat, mipMapCount > 1)
            {
                name = name
            };

            Texture.LoadRawTextureData(data);
            Texture.Apply();

            return true;
        }
    }
}