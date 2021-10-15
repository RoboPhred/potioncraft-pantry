
using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RoboPhredDev.PotionCraft.Pantry.Yaml
{
    class MaybeStringArray : List<string> { }

    class MaybeStringArrayTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(MaybeStringArray).IsAssignableFrom(type);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var value = new MaybeStringArray();
            if (parser.TryConsume<Scalar>(out var scalar))
            {
                value.Add(scalar.Value);
            }
            else
            {
                var values = (List<string>)Deserializer.DeserializeFromParser(Deserializer.CurrentFilePath, typeof(List<string>), parser);
                value.AddRange(values);
            }

            return value;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}