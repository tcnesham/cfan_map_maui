using System.Linq;

namespace CFAN.SchoolMap.Helpers
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool HasBigLetters(this string str)
        {
            if (str == null) return false;
            return str.Any(char.IsUpper);
        }

        public static char FirstOrGivenValue(this string source, char given)
        {
            if ((source?.Length ?? 0) == 0) return given;
            return source[0];
        }
    }
}
