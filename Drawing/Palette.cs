using System;
using System.Linq;

namespace PixelBox.Drawing
{
    public static class Palette
    {
        public const int TrueBlack = 0;
        public const int TrueWhite = 1;

        public const int SoftBlack = 2;
        public const int SoftWhite = 3;

        public const int DarkBlue = 4;
        public const int DarkPurple = 5;
        public const int DarkGreen = 6;
        public const int Brown = 7;
        public const int DarkGray = 8;
        public const int LightGray = 9;
        public const int White = 10;
        public const int Red = 11;
        public const int Orange = 12;
        public const int Yellow = 13;
        public const int Green = 14;
        public const int Blue = 15;
        public const int Indigo = 16;
        public const int Pink = 17;
        public const int Peach = 18;
        public const int Transparent = 19;

        private static readonly Color[] Colors = new[]
        {
            new Color(0, 0, 0),         // True Black
            new Color(255, 255, 255),   // True White

            new Color(5, 5, 5),         // Soft Black
            new Color(249, 249, 249),   // Soft White

            new Color(29, 43, 83),      // Dark Blue
            new Color(126, 37, 83),     // Dark Purple
            new Color(0, 135, 81),      // Dark Green
            new Color(171, 82, 54),     // Brown
            new Color(95, 87, 79),      // Dark Gray
            new Color(194, 195, 199),   // Light Gray
            new Color(255, 241, 232),   // Soft White (Pico-8)
            new Color(255, 0, 77),      // Red
            new Color(255, 163, 0),     // Orange
            new Color(255, 236, 39),    // Yellow
            new Color(0, 228, 54),      // Green
            new Color(41, 173, 255),    // Blue
            new Color(131, 118, 156),   // Indigo
            new Color(255, 119, 168),   // Pink
            new Color(255, 204, 170),   // Peach
            new Color(0, 0, 0, 0)       // Transparent
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