namespace PixelBox.Extensions
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Directly inverts the value.
        /// </summary>
        /// <returns>
        /// New value of the boolean.
        /// </returns>
        public static bool Invert(this ref bool value) => value = !value;

        /// <param name="value"></param>
        /// <returns>
        /// 1 if the value is true, 0 if the value is false.
        /// </returns>
        public static int ToInt(this bool value) => value ? 1 : 0;
    }
}