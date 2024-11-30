using System.Collections.Generic;
using System.IO;

namespace Application.Extensions.ServiceCreator
{
    public static class InterfaceConverter
    {
        public static void ReadFile(string path)
        {
            List<string> lines;
            using (var reader = new StreamReader(path))
            {
                lines = new List<string>();
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            lines = LoopLines(lines);
            SaveFile(lines, path);
        }

        private static List<string> LoopLines(IEnumerable<string> lines)
        {
            var newLines = new List<string>();
            var metInterface = false;

            foreach (var line in lines)
            {
                CreateNewLines(newLines, line, ref metInterface);
            }

            return newLines;
        }

        private static void CreateNewLines(List<string> newLines, string line, ref bool metInterface)
        {
            string newLine;

            if (line.Contains("export interface") && line.Contains("DTO"))
            {
                metInterface = true;
                newLine = line;
                newLines.Add(newLine);

                return;
            }

            if (!metInterface)
            {
                newLine = line;
                newLines.Add(newLine);

                return;
            }

            if (line.Contains('}'))
            {
                metInterface = false;
                newLine = line;
                newLines.Add(newLine);

                return;
            }

            newLine = line.ChangeFirstLetterToLower();
            newLine = newLine.ExtendTypes();
            newLine = newLine.AddOptional();

            newLines.Add(newLine);
        }

        public static void SaveFile(IEnumerable<string> lines, string path)
        {
            File.WriteAllLines(path, lines);
        }
    }
}
