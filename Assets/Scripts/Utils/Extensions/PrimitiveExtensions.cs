namespace Utils.Extensions
{
    public static class PrimitiveExtensions
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}