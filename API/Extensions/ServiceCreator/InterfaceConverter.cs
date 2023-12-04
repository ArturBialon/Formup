using System.Collections.Generic;
using System.IO;

namespace Application.Extensions.ServiceCreator
{
    public static class InterfaceConverter
    {
        public static void ReadFile(string path)
        {
            var lines = File.ReadLines(path);
            LoopLines(lines, path);
        }
        private static void LoopLines(IEnumerable<string> lines, string path)
        {
            var newLines = new List<string>();
            var metInterface = false;

            foreach (var line in lines)
            {
                CreateNewLines(newLines, line, ref metInterface);
            }
        }
        private static void CreateNewLines(List<string> newLines, string line, ref bool metInterface)
        {
            string newLine;

            if (Validators.IsViewModelInterfaceMet(line))
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

            if (Validators.IsViewMdelInterfaceEnded(line))
            {
                metInterface = false;
                newLine = line;
                newLines.Add(newLine);

                return;
            }

            newLine = line.ChangeSecondLetterToLower();
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
