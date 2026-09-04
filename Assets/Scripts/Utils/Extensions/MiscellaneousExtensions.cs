using System;

namespace Core.Utils.Extensions
{
    public static class MiscellaneousExtensions
    {
        public static string Simplify(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray()).Replace("/", "_").Replace("+", "-").TrimEnd('=');
        }
    }
}