using System;

namespace PixelBox.Extensions
{
    public static class NumericExtensions
    {
        public static float Clamp01(this float value) => Clamp(value, 0, 1);
        public static double Clamp01(this double value) => Clamp(value, 0, 1);
        public static int Clamp01(this int value) => Clamp(value, 0, 1);

        public static T ClampMin<T>(this T value, T min) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;

            return value;
        }
        public static T ClampMax<T>(this T value, T max) where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
                return max;

            return value;
        }

        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return value.ClampMin(min).ClampMax(max);
        }
  
        public static int Abs(this int value) => Math.Abs(value);
        public static float Abs(this float value) => Math.Abs(value);
        
        public static float Deg2Rad(this float degrees) => degrees * (float)Math.PI / 180;
        public static float Rad2Deg(this float radians) => radians * 180f / (float)Math.PI;

        public static float Floored(this float value) => (float)Math.Floor(value);
        public static float Ceiled(this float value) => (float)Math.Ceiling(value);
        public static float Rounded(this float value, int digits = 0) => (float)Math.Round(value, digits);

        public static string ToSizeString(this long sizeInBytes)
        {
            long sizeBytes = sizeInBytes;
            double sizeKb = sizeBytes / 1024.0;
            double sizeMb = sizeKb / 1024.0;
            double sizeGb = sizeMb / 1024.0;

            string finalSize = $"{sizeBytes} B";

            if (sizeGb >= 1)
            {
                finalSize = $"{sizeGb:F2} GB";
            }
            else if (sizeMb >= 1)
            {
                finalSize = $"{sizeMb:F2} MB";
            }
            else if (sizeKb >= 1)
            {
                finalSize = $"{sizeKb:F2} KB";
            }
            
            return finalSize;
        }
    }
}