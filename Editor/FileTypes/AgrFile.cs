#nullable enable

using System.IO;
using System.Xml.Serialization;

using UnityEngine;

namespace ForgeLightToolkit.Editor.FileTypes
{
    public class AgrFile : ScriptableObject
    {
        public ActorSet? ActorSet;

        public bool Load(string filePath)
        {
            name = Path.GetFileNameWithoutExtension(filePath);

            using var fileStream = File.OpenRead(filePath);

            var serializer = new XmlSerializer(typeof(ActorSet));

            var actorSet = serializer.Deserialize(fileStream) as ActorSet;

            if(actorSet is null)
                return false;

            ActorSet = actorSet;

            return true;
        }
    }
}