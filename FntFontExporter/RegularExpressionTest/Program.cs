using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularExpressionTest
{
    class Program
    {
        private static readonly string regexPattern = @"^\w+(\s|$)|\w+\=(\d+(\s|$)|\d+\,\d+(\s|$)|""[^""]*""|\d+\,\d+\,\d+\,\d+)";
        private static readonly string content = "info font=\"Roboto Condensed\" size=64 bold=0 italic=0 charset=\"\" unicode=0 stretchH=100 smooth=1 aa=1 padding=2,2,2,2 spacing=2,2";

        static void Main(string[] args)
        {
            Regex regex = new Regex(regexPattern);
            MatchCollection matches = regex.Matches(content);
            foreach (Match match in matches)
            {
                if (!match.Success)
                    continue;

                Console.WriteLine(match.Value);

            }

            Console.ReadLine();
        }
    }
}
