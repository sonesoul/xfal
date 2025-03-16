using System;
using System.Linq;

namespace PixelBox.Drawing
{
    public static class Palette
    {
        public static Color Black => Colors[0];
        public static Color DarkBlue => Colors[1];
        public static Color DarkPurple => Colors[2];
        public static Color DarkGreen => Colors[3];
        public static Color Brown => Colors[4];
        public static Color DarkGray => Colors[5];
        public static Color LightGray => Colors[6];
        public static Color White => Colors[7];
        public static Color Red => Colors[8];
        public static Color Orange => Colors[9];
        public static Color Yellow => Colors[10];
        public static Color Green => Colors[11];
        public static Color Blue => Colors[12];
        public static Color Indigo => Colors[13];
        public static Color Pink => Colors[14];
        public static Color Peach => Colors[15];

        private static readonly Color[] Colors = new[]
        {
            new Color(0, 0, 0),         // Black
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