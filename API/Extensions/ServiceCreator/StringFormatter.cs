namespace Application.Extensions.ServiceCreator
{
    public static class StringFormatter
    {

        public static string AddOptional(this string line)
        {
            if (line.Length > 0 && !line.Contains('?'))
            {
                var wordNameLastIndex = line.IndexOf(':');
                var newString = line.Substring(0, wordNameLastIndex) + '?' + line.Substring(wordNameLastIndex);

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
                    line = line.Replace(": boolean;", ":boolean | null | undefiened;");
                }

                if (line.Contains(": Date") || line.Contains(": Date;"))
                {
                    line = line.Replace(": Date", ": Date | string");
                }

                if (line.Contains(": number;"))
                {
                    line = line.Replace(": number;", ": number | null | undefiened;");
                }
            }

            return line;
        }

        public static string ChangeSecondLetterToLower(this string line)
        {
            return line.Substring(0, 4) + char.ToLower(line[4]) + line.Substring(5);
        }
    }
}
