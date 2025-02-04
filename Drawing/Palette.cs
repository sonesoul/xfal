using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelBox.Drawing
{
    public static class Palette
    {
        public static readonly int Black = 0;
        public static readonly int DarkBlue = 1;
        public static readonly int DarkPurple = 2;
        public static readonly int DarkGreen = 3;
        public static readonly int Brown = 4;
        public static readonly int DarkGray = 5;
        public static readonly int LightGray = 6;
        public static readonly int White = 7;
        public static readonly int Red = 8;
        public static readonly int Orange = 9;
        public static readonly int Yellow = 10;
        public static readonly int Green = 11;
        public static readonly int Blue = 12;
        public static readonly int Indigo = 13;
        public static readonly int Pink = 14;
        public static readonly int Peach = 15;
        public static readonly int Transparent = 16;

        private static readonly Color[] Colors = new[]
        {
            new Color(0, 0, 0),         // 0 Black
            new Color(29, 43, 83),      // 1 Dark Blue
            new Color(126, 37, 83),     // 2 Dark Purple
            new Color(0, 135, 81),      // 3 Dark Green
            new Color(171, 82, 54),     // 4 Brown
            new Color(95, 87, 79),      // 5 Dark Gray
            new Color(194, 195, 199),   // 6 Light Gray
            new Color(255, 241, 232),   // 7 White
            new Color(255, 0, 77),      // 8 Red
            new Color(255, 163, 0),     // 9 Orange
            new Color(255, 236, 39),    // 10 Yellow
            new Color(0, 228, 54),      // 11 Green
            new Color(41, 173, 255),    // 12 Blue
            new Color(131, 118, 156),   // 13 Indigo
            new Color(255, 119, 168),   // 14 Pink
            new Color(255, 204, 170),   // 15 Peach
            new Color(0, 0, 0, 0)       // 16 Transparent
        };
        
        public static Color GetColor(int index) => Colors[index];
        public static bool Contains(Color color) => Colors.Contains(color);

        public static int GetNearestIndex(Color color)
        {
            var colors = Colors;

            double minDistance = double.MaxValue;
            int closestColorIndex = 0;
            
            for (int i = 0; i < colors.Length; i++)
            {
                Color c = colors[i];

                double distance = Math.Sqrt(
                    Math.Pow(c.R - color.R, 2) +
                    Math.Pow(c.G - color.G, 2) +
                    Math.Pow(c.B - color.B, 2)
                );

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestColorIndex = i;
                }
            }

            return closestColorIndex;
        }
    }
}