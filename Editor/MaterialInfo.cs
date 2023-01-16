using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace ForgeLightToolkit.Editor
{
    [XmlRoot("MaterialInfo")]
    public class MaterialInfo
    {
        private MaterialInfo()
        {
        }

        public static MaterialInfo Instance => _instance.Value;

        private static readonly Lazy<MaterialInfo> _instance = new(() =>
        {
            var serializer = new XmlSerializer(typeof(MaterialInfo));

            var materialInfoData = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/ForgeLight/materials_3.xml");

            if (materialInfoData is null)
            {
                Debug.LogError("Failed to find Assets/ForgeLight/materials_3.xml.");
                return null;
            }

            using var fileStream = new MemoryStream(materialInfoData.bytes);

            return serializer.Deserialize(fileStream) as MaterialInfo;
        });

        [XmlElement("InputLayout")]
        public List<InputLayout> InputLayouts { get; set; } = null!;

        public class InputLayout
        {
            private string _name = null!;

            [XmlAttribute]
            public string Name
            {
                get => _name;
                set
                {
                    _name = value;
                    NameHash = JenkinsHelper.JenkinsOneAtATimeHash(_name);
                }
            }

            [XmlIgnore]
            public uint NameHash { get; set; }

            [XmlElement("Entry")]
            public List<Entry> Entries { get; set; } = null!;

            public class Entry
            {
                [XmlAttribute]
                public int Stream { get; set; }

                [XmlAttribute]
                public int Offset { get; set; }

                [XmlAttribute]
                public EntryType Type { get; set; }

                public enum EntryType
                {
                    [XmlEnum("Float1")] Float1,
                    [XmlEnum("Float2")] Float2,
                    [XmlEnum("Float3")] Float3,
                    [XmlEnum("Float4")] Float4,
                    [XmlEnum("D3dcolor")] D3DColor,
                    [XmlEnum("Ubyte4")] UByte4,
                    [XmlEnum("Short2")] Short2,
                    [XmlEnum("Short4")] Short4,
                    [XmlEnum("Ubyte4n")] UByte4N,
                    [XmlEnum("Short2n")] Short2N,
                    [XmlEnum("Short4n")] Short4N,
                    [XmlEnum("Ushort2n")] UShort2N,
                    [XmlEnum("Ushort4n")] UShort4N,
                    [XmlEnum("Udec3")] UDec3,
                    [XmlEnum("Dec3n")] Dec3N,
                    [XmlEnum("Float16_2")] Float162,
                    [XmlEnum("Float16_4")] Float164
                }

                [XmlAttribute]
                public EntryUsage Usage { get; set; }

                public enum EntryUsage
                {
                    [XmlEnum("Position")] Position,
                    [XmlEnum("BlendWeight")] BlendWeight,
                    [XmlEnum("BlendIndices")] BlendIndices,
                    [XmlEnum("Normal")] Normal,
                    [XmlEnum("Psize")] PSize,
                    [XmlEnum("Texcoord")] TexCoord,
                    [XmlEnum("Tangent")] Tangent,
                    [XmlEnum("Binormal")] Binormal,
                    [XmlEnum("PositionT")] PositionT,
                    [XmlEnum("Color")] Color,
                    [XmlEnum("Fog")] Fog,
                    [XmlEnum("Depth")] Depth
                }

                public int UsageIndex { get; set; }
            }
        }

        [XmlElement("ParameterGroup")]
        public List<ParameterGroup> ParameterGroups { get; set; } = null!;

        public class ParameterGroup
        {
            private string _name = null!;

            [XmlAttribute]
            public string Name
            {
                get => _name;
                set
                {
                    _name = value;
                    NameHash = JenkinsHelper.JenkinsOneAtATimeHash(_name);
                }
            }

            [XmlIgnore]
            public uint NameHash { get; set; }

            [XmlElement("Parameter")]
            public List<Parameter> Parameters { get; set; } = null!;

            public class Parameter
            {
                private string _name = null!;

                [XmlAttribute]
                public string Name
                {
                    get => _name;
                    set
                    {
                        _name = value;
                        NameHash = JenkinsHelper.JenkinsOneAtATimeHash(_name);
                    }
                }

                [XmlIgnore]
                public uint NameHash { get; set; }

                private string _variable = null!;

                [XmlAttribute]
                public string Variable
                {
                    get => _variable;
                    set
                    {
                        _variable = value;
                        VariableHash = JenkinsHelper.JenkinsOneAtATimeHash(_variable);
                    }
                }

                [XmlIgnore]
                public uint VariableHash { get; set; }

                [XmlAttribute]
                public ParameterType Type { get; set; }

                public enum ParameterType
                {
                    [XmlEnum("Int")] Int,
                    [XmlEnum("Float")] Float,
                    [XmlEnum("Float4")] Float4,
                    [XmlEnum("Float4x4")] Float4x4,
                    [XmlEnum("Texture")] Texture,

                    // NOTE: Little hack to support the other size floats.
                    [XmlEnum("Float1")] Float1 = Float4,
                    [XmlEnum("Float2")] Float2 = Float4,
                    [XmlEnum("Float3")] Float3 = Float4,
                }

                [XmlAttribute]
                public string DefaultValue { get; set; } = null!;

                [XmlAttribute]
                public string DefaultX { get; set; } = null!;

                [XmlAttribute]
                public string DefaultY { get; set; } = null!;

                [XmlAttribute]
                public string DefaultZ { get; set; } = null!;
            }
        }

        [XmlElement("MaterialDefinition")]
        public List<MaterialDefinition> MaterialDefinitions { get; set; } = null!;

        public class MaterialDefinition
        {
            private string _name = null!;

            [XmlAttribute]
            public string Name
            {
                get => _name;
                set
                {
                    _name = value;
                    NameHash = JenkinsHelper.JenkinsOneAtATimeHash(_name);
                }
            }

            [XmlIgnore]
            public uint NameHash { get; set; }

            [XmlAttribute]
            public string Type { get; set; } = null!;

            [XmlElement("DrawStyle")]
            public List<DrawStyle> DrawStyles { get; set; } = null!;

            public class DrawStyle
            {
                [XmlAttribute]
                public string Name { get; set; } = null!;

                private string _effect = null!;

                [XmlAttribute]
                public string Effect
                {
                    get => _effect;
                    set
                    {
                        _effect = value;
                        EffectHash = JenkinsHelper.JenkinsOneAtATimeHash(_effect);
                    }
                }

                [XmlIgnore]
                public uint EffectHash { get; set; }

                private string _inputLayout = null!;

                [XmlAttribute]
                public string InputLayout
                {
                    get => _inputLayout;
                    set
                    {
                        _inputLayout = value;
                        InputLayoutHash = JenkinsHelper.JenkinsOneAtATimeHash(_inputLayout);
                    }
                }

                [XmlIgnore]
                public uint InputLayoutHash { get; set; }
            }

            [XmlElement("Property")]
            public List<Property> Properties { get; set; } = null!;

            public class Property
            {
                private string _name = null!;

                [XmlAttribute]
                public string Name
                {
                    get => _name;
                    set
                    {
                        _name = value;
                        NameHash = JenkinsHelper.JenkinsOneAtATimeHash(_name);
                    }
                }

                [XmlIgnore]
                public uint NameHash { get; set; }
            }
        }
    }
}