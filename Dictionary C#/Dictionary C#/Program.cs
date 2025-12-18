using System;

namespace Dictionary_C_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            displayArray(FizzBuzz(3, 5, 1, 200).Split("\n"), true, " - ");    
        }

        static string FizzBuzz(int fizzMarker, int buzzMarker, int rangeStart, int rangeEnd)
        {
            string output = "";
            foreach (int i in Enumerable.Range(rangeStart, rangeEnd))
            {
                string line = "";
                if (i % fizzMarker == 0)
                {
                    line += "Fizz";
                }
                if (i % buzzMarker == 0)
                {
                    line += "Buzz";
                }
                if (line.Length < 3)
                {
                    line = "";
                }
                output += line+"\n";
            }
            return output;
        }
        static void displayArray(string[] data, bool showIndex = true, string seperator = " - ")
        {
            string padding = "";
            int requiredPadding = data.Length.ToString().Length;
            while (padding.Length < requiredPadding)
            {
                padding += " ";
            }
            foreach (int j in Enumerable.Range(0, data.Length - 1))
            {
                string line = (showIndex) ? (j + 1).ToString() : "";
                while (line.Length < requiredPadding && showIndex)
                {
                    line += " ";
                }
                line += (data.GetValue(j) != "" && showIndex) ? seperator + data.GetValue(j) : data.GetValue(j);
                Console.WriteLine(line);
            }
        }
    }
}
