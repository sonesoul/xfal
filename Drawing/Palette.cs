using System;
using System.Linq;

namespace PixelBox.Drawing
{
    public static class Palette
    {
        public const int Black = 0;
        public const int White = 1;
        public const int DarkBlue = 2;
        public const int DarkPurple = 3;
        public const int DarkGreen = 4;
        public const int Brown = 5;
        public const int DarkGray = 6;
        public const int LightGray = 7;
        public const int SoftWhite = 8;
        public const int Red = 9;
        public const int Orange = 10;
        public const int Yellow = 11;
        public const int Green = 12;
        public const int Blue = 13;
        public const int Indigo = 14;
        public const int Pink = 15;
        public const int Peach = 16;
        public const int Transparent = 17;

        private static readonly Color[] Colors = new[]
        {
            new Color(0, 0, 0),         // 0 Black
            new Color(255, 255, 255),   // 1 White
            new Color(29, 43, 83),      // 2 Dark Blue
            new Color(126, 37, 83),     // 3 Dark Purple
            new Color(0, 135, 81),      // 4 Dark Green
            new Color(171, 82, 54),     // 5 Brown
            new Color(95, 87, 79),      // 6 Dark Gray
            new Color(194, 195, 199),   // 7 Light Gray
            new Color(255, 241, 232),   // 8 White
            new Color(255, 0, 77),      // 9 Red
            new Color(255, 163, 0),     // 10 Orange
            new Color(255, 236, 39),    // 11 Yellow
            new Color(0, 228, 54),      // 12 Green
            new Color(41, 173, 255),    // 13 Blue
            new Color(131, 118, 156),   // 14 Indigo
            new Color(255, 119, 168),   // 15 Pink
            new Color(255, 204, 170),   // 16 Peach
            new Color(0, 0, 0, 0)       // 17 Transparent
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