namespace Application.Extensions.ServiceCreator
{
    public static class StringFormatter
    {

        public static string AddOptional(this string line)
        {
            if (line.Length > 0 && !line.Contains('?'))
            {
                var wordNameLastIndex = line.IndexOf(':');
                var newString = line[..wordNameLastIndex] + '?' + line[wordNameLastIndex..];

                return newString;
            }

            return line;

        }

        public static string ExtendTypes(this string line)
        {
            if (line.Length > 0)
            {
                if (line.Contains(": boolean;"))
                {
                    line = line.Replace(": boolean;", ":boolean | null;");
                }

                if (line.Contains(": Date") || line.Contains(": Date;"))
                {
                    line = line.Replace(": Date", ": Date | string");
                }

                if (line.Contains(": number;"))
                {
                    line = line.Replace(": number;", ": number | null;");
                }
            }

            return line;
        }

        public static string ChangeFirstLetterToLower(this string line)
        {
            var value = line.Substring(0, 4) + char.ToLower(line[4]) + line.Substring(5);
            return value;
        }
    }
}
