using System.Linq;

namespace xfal.Extensions
{
    public static class CharStringExtensions
    {
        public static string Times(this string str, int count) => string.Concat(Enumerable.Repeat(str, count));
        public static string Times(this char c, int count) => c.ToString().Times(count);

        public static string SetChar(this string str, int index, char newChar)
        {
            char[] charArr = str.ToCharArray();
            charArr[index] = newChar;
            return new string(charArr);
        }
        public static bool HasContent(this string str) => !string.IsNullOrWhiteSpace(str);
    }
}