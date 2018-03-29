namespace BotFramework.Helpers
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsCpf(this string str)
        {
            if (IsEmpty(str))
                return false;

            var cleanString = str.Trim('-').Trim('.').Trim(' ');
            if (cleanString.Length.Equals(11) && long.TryParse(cleanString, out long result))
                return true;

            return false;
        }
    }
}