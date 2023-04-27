using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace ForgeLightToolkit
{
    [Serializable]
    [XmlRoot("ActorSet")]
    public class ActorSet
    {
        [Serializable]
        public class _ObjectTerrainDataId
        {
            [XmlAttribute]
            public int ObjectTerrainDataIdOverride;
        }

        [XmlElement("ObjectTerrainDataId")]
        public _ObjectTerrainDataId ObjectTerrainDataId;

        [Serializable]
        public class Actor
        {
            [XmlAttribute("name")]
            public string Name;

            [XmlAttribute("position")]
            public string Position;

            [XmlAttribute("orientation")]
            public string Orientation;

            [Serializable]
            public class TextureAlias
            {
                [XmlAttribute("name")]
                public string Name;
            }

            [XmlElement("textureAlias")]
            public List<TextureAlias> TextureAliases;

            [XmlAttribute("tintAlias")]
            public string TintAlias;

            [XmlAttribute("tintId")]
            public int TintId;

            [XmlAttribute("compositeEffectId")]
            public int CompositeEffectId;

            [XmlAttribute("actorUsage")]
            public int ActorUsage;

            [XmlAttribute]
            public int EquipmentSlots;

            [XmlAttribute]
            public int EquipTypeSelection;

            [XmlAttribute("equipType")]
            public int EquipType;

            [XmlAttribute]
            public string EquippedSlotName;
        }

        [XmlElement("Actor")]
        public List<Actor> Actors;

        [Serializable]
        public class CustomizationTint
        {
            [XmlAttribute]
            public string TagName;

            [XmlAttribute]
            public int TintId;
        }

        [XmlElement("CustomizationTint")]
        public List<CustomizationTint> CustomizationTints;
    }
}