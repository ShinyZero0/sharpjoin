using System;
using System.IO; // Mono compatibility
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace sharpjoin
{ // Mono compatibility
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: sharpjoin --join <file> or sharpjoin --split <file>");
                return;
            }
            switch (args[0])
            {
                case "--join":
                    Join(args[1]);
                    break;
                case "--split":
                    Split(args[1]);
                    break;

                default:
                    Console.WriteLine("Usage: sharpjoin --join <file> or sharpjoin --split <file>");
                    break;
            }
        }

        public static void Join(string source)
        {
            string[] data = File.ReadAllLines(source);
            int i = 0;
            var paras = new List<string>();
            while (i < data.Length)
            {
                string startLine = data[i];
                if (!string.IsNullOrWhiteSpace(startLine))
                {
                    int localI = i;
                    var paraLines = new List<string>();
                    string localLine = data[localI];
                    while (!string.IsNullOrWhiteSpace(localLine) && localI < data.Length)
                    {
                        localLine = data[localI];
                        paraLines.Add(localLine);
                        localI++;
                    }
                    i = localI;
                    paras.Add(Regex.Replace((String.Join(" ", paraLines)), " +$", ""));
                }
                else
                {
                    i++;
                }
            }
            paras.ForEach(p => Console.WriteLine(p));
        }

        public static void Split(string source)
        {
            string[] data = File.ReadAllLines(source);
            var output = new List<string>();
            foreach (string line in data)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string sentence = Regex.Replace(
                        line,
                        @"([\.\?!])\s+((--\s+)|(—\s+)?[A-ZА-Я])",
                        match =>
                        {
                        var m = match.Groups;
                        return string.Format("{0}\n{1}", m[1].ToString(), m[2].ToString());
                        }
                        );
                string result = string.Format("{0}\n", sentence);
                output.Add(result);
            }
            output.ForEach(p => Console.WriteLine(p));
        }
    }
}
