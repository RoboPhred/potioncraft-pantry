
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.BezierCurves;

namespace RoboPhredDev.PotionCraft.Pantry
{
    static class PathParser
    {
        public static List<CubicBezierCurve> SvgPathToBezierCurves(string path)
        {
            // TODO: letter command should split as well
            // FIXME: better svg parser.  This requires command letters to not be split.
            var parts = path.Split(' ').Select(x => x.Trim()).Where(x => x.Length > 0).ToList();
            var curves = new List<CubicBezierCurve>();
            var lastEnd = Vector2.zero;
            foreach (var part in parts)
            {
                var curve = PartToCurve(part, lastEnd);
                curves.Add(curve);
                lastEnd = curve.PLast;
            }
            return curves;
        }

        private static CubicBezierCurve PartToCurve(string part, Vector2 start)
        {
            part = part.Trim();
            // TODO: support relative moves
            switch (part[0])
            {
                case 'M':
                case 'L':
                    return AbsoluteLine(part.Substring(1), start);
                case 'C':
                    return AbsoluteCubicCurve(part.Substring(1), start);
                default:
                    throw new Exception($"Unknown path command {part[0]}");
            }
        }

        private static CubicBezierCurve AbsoluteLine(string part, Vector2 start)
        {
            var values = part.Split(',', ' ');
            var end = new Vector2(float.Parse(values[0]), float.Parse(values[1]));
            return new CubicBezierCurve(start, start, end, end);
        }

        private static CubicBezierCurve AbsoluteCubicCurve(string part, Vector2 start)
        {
            Debug.Log("AbsoluteCubicCurve " + part);
            var values = part.Split(',', ' ');
            return new CubicBezierCurve(
                start,
                new Vector2(float.Parse(values[0]), float.Parse(values[1])),
                new Vector2(float.Parse(values[2]), float.Parse(values[3])),
                new Vector2(float.Parse(values[4]), float.Parse(values[5]))
            );
        }
    }
}