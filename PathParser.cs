
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.BezierCurves;

namespace RoboPhredDev.PotionCraft.Pantry
{
    static class PathParser
    {
        public static List<CubicBezierCurve> SvgPathToBezierCurves(string path)
        {
            var curves = new List<CubicBezierCurve>();
            var lastEnd = Vector2.zero;
            CubicBezierCurve curve;
            while ((curve = PartToCurve(ref path, lastEnd)) != null)
            {
                // Filter out any M 0,0 that might have gotten in from path editors.
                if (curve.PFirst == curve.PLast && curve.PFirst == curve.P1 && curve.PFirst == curve.P2)
                {
                    continue;
                }
                lastEnd = curve.PLast;
                curves.Add(curve);
            }
            return curves;
        }

        private static string GetToken(ref string svgPath)
        {
            var token = "";
            int i = 0;
            bool? isAlphanumeric = null;
            for (; i < svgPath.Length; i++)
            {
                var c = svgPath[i];
                if (c == ' ' || c == ',' || c == '\n' || c == '\r')
                {
                    if (token.Length > 0)
                    {
                        break;
                    }
                    continue;
                }

                if (!isAlphanumeric.HasValue)
                {
                    isAlphanumeric = char.IsLetter(c);
                }
                else if (char.IsLetter(c) != isAlphanumeric.Value)
                {
                    break;
                }

                token += c;
            }

            svgPath = svgPath.Substring(i);

            if (token.Length == 0)
            {
                return null;
            }

            return token;
        }

        private static float GetFloatTokenOrFail(ref string svgPath)
        {
            var token = GetToken(ref svgPath);
            if (!float.TryParse(token, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result))
            {
                throw new Exception($"Failed to parse float token: \"{token}\"");
            }
            return result;
        }

        private static CubicBezierCurve PartToCurve(ref string svgPath, Vector2 start)
        {
            var token = GetToken(ref svgPath);
            if (token == null)
            {
                return null;
            }
            // TODO: support relative moves
            switch (token)
            {
                case "M":
                case "L":
                    return AbsoluteLine(ref svgPath, start);
                case "H": return AbsoluteHorizontal(ref svgPath, start);
                case "V": return AbsoluteVertical(ref svgPath, start);
                case "C":
                    return AbsoluteCubicCurve(ref svgPath, start);
                default:
                    throw new Exception($"Unknown path command {token}");
            }
        }

        private static CubicBezierCurve AbsoluteLine(ref string svgPath, Vector2 start)
        {
            var end = new Vector2(GetFloatTokenOrFail(ref svgPath), GetFloatTokenOrFail(ref svgPath));
            return new CubicBezierCurve(start, start, end, end);
        }

        private static CubicBezierCurve AbsoluteHorizontal(ref string svgPath, Vector2 start)
        {
            var end = new Vector2(GetFloatTokenOrFail(ref svgPath), start.y);
            return new CubicBezierCurve(start, start, end, end);
        }

        private static CubicBezierCurve AbsoluteVertical(ref string svgPath, Vector2 start)
        {
            var end = new Vector2(start.x, GetFloatTokenOrFail(ref svgPath));
            return new CubicBezierCurve(start, start, end, end);
        }

        private static CubicBezierCurve AbsoluteCubicCurve(ref string svgPath, Vector2 start)
        {
            return new CubicBezierCurve(
                start,
                new Vector2(GetFloatTokenOrFail(ref svgPath), GetFloatTokenOrFail(ref svgPath)),
                new Vector2(GetFloatTokenOrFail(ref svgPath), GetFloatTokenOrFail(ref svgPath)),
                new Vector2(GetFloatTokenOrFail(ref svgPath), GetFloatTokenOrFail(ref svgPath))
            );
        }
    }
}