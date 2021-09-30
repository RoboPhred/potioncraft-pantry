
using System;
using System.Collections.Generic;
using Utils.BezierCurves;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    class PantryIngredientPath : List<CubicBezierCurve>
    {
        public static PantryIngredientPath FromSvgPath(string path)
        {
            var curves = PathParser.SvgPathToBezierCurves(path);
            var pathItem = new PantryIngredientPath();
            pathItem.AddRange(curves);
            return pathItem;
        }
    }

    class PantryIngredientPathTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(PantryIngredientPath);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var str = parser.Consume<Scalar>().Value;
            return PantryIngredientPath.FromSvgPath(str);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotSupportedException();
        }
    }
}