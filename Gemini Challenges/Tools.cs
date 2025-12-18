using System.Linq.Expressions;
using System.Xml;

namespace Tools
{
    /* Import Modules:
           using CT = Tools.ConsoleTools;
           using LCSV = Tools.LoadCSVFromFile; 
     */
    static public class ConsoleTools
    {
        /*
         * Usage:
         *      Single Line:
         *          CT.Print(string/int/bool);  
         *          outputs data to the Console
         *      Array/unIndexed:
         *          CT.Print(String[]/int[]/Object[]/List<Object[]>, null, "Title of Array", Indexing Spacer Maximum)
         *      Array/Indexed:
         *          CT.Print(String[]/int[]/Object[]/List<Object[]>, int[] of indexes for Data, "Title of Array", Indexing Spacer Maximum)
         *          CT.Print((int[] Indices, String[]/int[]/Object[]/List<Object[]>), "Title of Array", Indexing Spacer Maximum)
         *          The bottom method is prefered as it takes a tuple which allows for indexed data to be exported directly from a function
         *          The top is simply there to help with routing
         * 
         * 
         * 
         * 
         */
        public static void Print(string input) { Console.WriteLine(input); }
        public static void Print(int input) { Console.WriteLine(input); }
        public static void Print(bool input) { Console.WriteLine(input); }
        public static void Print(double input) { Console.WriteLine(input); }
        public static void Print(string[] input, int[]? index = null, string Name = "Array: ", int SpacerMax = 2)
        {
            if (input == null) { Console.WriteLine("Input Empty"); }
            else
            {
                String LineSpacer = "";
                String EndSpacer = "-------";
                foreach (int i in Enumerable.Range(0, SpacerMax)) { LineSpacer += " "; }
                foreach (int i in Enumerable.Range(0, Name.Length)) { EndSpacer += "-"; }

                Console.WriteLine("------ " + Name + " ------");
                foreach (int i in Enumerable.Range(0, input.Length))
                {
                    try { Console.WriteLine(((index == null) ? i : index[i]).ToString() + LineSpacer[((index == null) ? i : index[i]).ToString().Length..(SpacerMax)] + "| " + input[i]); }
                    catch (ArgumentOutOfRangeException ex) { Console.WriteLine("SpacerMax too small to show full data, try increasing it."); }
                    
                }
                Console.WriteLine(EndSpacer + "-------");
            }
        }
        public static void Print(int[] input, int[]? index = null, string Name = "Array: ", int SpacerMax = 2)
        {
            ConsoleTools.Print(input.Select(i => i.ToString()).ToArray(), index, Name, SpacerMax);
        }
        public static void Print(double[] input, int[]? index = null, string Name = "Array: ", int SpacerMax = 2)
        {
            ConsoleTools.Print(input.Select(i => i.ToString()).ToArray(), index, Name, SpacerMax);
        }
        public static void Print(Object[] input, int[]? index = null, string Name = "Array: ", int SpacerMax = 2)
        {
            ConsoleTools.Print(input.Select(i => i.ToString()).ToArray(), index, Name, SpacerMax);
        }
        public static void Print(List<Object[]> input, int[]? index = null, string Name = "Array: ", int SpacerMax = 2)
        {
            ConsoleTools.Print(input.Select(i => ConsoleTools.ToString(i)).ToArray(), index, Name, SpacerMax);
        }
        public static void Print((int[] Index, Object[] Data) input, string Name = "Array: ", int SpacerMax = 2)
        {
            ConsoleTools.Print(input.Data, input.Index, Name, SpacerMax);
        }
        public static void Print((int[] Index, int[] Data) input, string Name = "Array: ", int SpacerMax = 2)
        {
            ConsoleTools.Print(input.Data, input.Index, Name, SpacerMax);
        }
        public static void Print((int[] Index, String[] Data) input, string Name = "Array: ", int SpacerMax = 2)
        {
            ConsoleTools.Print(input.Data, input.Index, Name, SpacerMax);
        }
        public static void Print((int[] Index, List<Object[]> Data) input, string Name = "Array: ", int SpacerMax = 2)
        {
            ConsoleTools.Print(input.Data.Select(i => ConsoleTools.ToString(i)).ToArray(), input.Index, Name, SpacerMax);
        }





        public static String ToString(Object[] input, String Seperator = ", ")
        {
            String output = "";
            foreach (Object line in input)
            {
                output += line.ToString() + Seperator;
            }
            return output[0..^Seperator.Length];
        }
        public static String ToString(int[] input, String Seperator = ", ")
        {
            String output = "";
            foreach (Object line in input)
            {
                output += line.ToString() + Seperator;
            }
            return output[0..^Seperator.Length];
        }
        public static int[] ToInt(String[] input) { return input.Select(i => Convert.ToInt32(i)).ToArray(); }
        public static int[] ToInt(Object[] input) { return input.Select(i => Convert.ToInt32(i)).ToArray(); }

        public static void EmptyLine(int n = 1) { for (int i = 0; i < n; i++) { Console.WriteLine(""); } }
    }
    public class LoadCSVFromFile
    {
        /*    Example Usage:
                var Data = new LCSV("D:\\source\\Gemini Challenges\\flights.log");
                CT.Print(Data.Indices);
                CT.Print(Data.GetLine(3));
                CT.Print(CT.ToString(Data.GetLine(3)));
                CT.Print(Data.GetIndex(Data.Indices[0]));
        */

        public String[] Indices;
        public Dictionary<String, Object[]> Data = new Dictionary<string, Object[]>();
        public LoadCSVFromFile(String fname, String IndexSeperator=", ", String DataSeperator=", ")
        {
            String[] FileData = File.ReadLines(fname).ToArray();

            //If Indices are defined, use those names, otherwise make them numeric
            if (FileData[0][0] == '#') { Indices = FileData[0][1..].Split(IndexSeperator); FileData = FileData[1..FileData.Length]; }
            else { Indices = Enumerable.Range(0, FileData[0].Split(DataSeperator).Length).Select(i => $"{i:D}").ToArray(); }

            //Generate dictionary and fill it with empty arrays
            foreach (string index in Indices) { Data.Add(index, new Object[FileData.Length]); }
            for (int i = 0; i < Indices.Length; i++)
            {
                Data.TryGetValue(Indices[i], out Object[] DataAtIndex);
                for (int j = 0; j < FileData.Length; j++)
                {
                    DataAtIndex[j] = FileData[j].Split(DataSeperator)[i];
                }
            }
        }
        public Object[] GetLine(int line)
        {
            Object[] output = new Object[Indices.Length];
            for (int i = 0; i < Indices.Length; i++)
            {
                Data.TryGetValue(Indices[i], out Object[] DataAtIndex);
                output[i] = DataAtIndex[line];
            }
            return output;
        }
        public List<Object[]> GetLine(int[] line)
        {
            List<Object[]> output = new List<Object[]>();
            for (int i = 0; i < line.Length; i++) 
            {
                output.Add(GetLine(line[i]));
            }
            return output;
        }
        public List<Object[]> GetAllLines() { return this.GetLine(Enumerable.Range(0, this.LineCount()).ToArray()); }
        public Object[] GetData(String IndexName)
        {
            Data.TryGetValue(IndexName, out Object[] Output);
            return Output;
        }
        public Object[] GetData(int Index)
        {
            Data.TryGetValue(this.Indices[Index], out Object[] Output);
            return Output;
        }
        public int LineCount() { return this.GetData(0).Length; }
    }
    public static class Sort
    {
        public static (int[], int[]) Bubble(int[] input)
        {
            (List<int>, List<int>) output = (new List<int>(), new List<int>());
            var data = input.Select((value, index) => new { OriginalIndex = index, Value = value });
            var Sorted = data.OrderBy(item => item.Value).ToList();
            
            foreach (var item in Sorted) { output.Item1.Add(item.OriginalIndex); output.Item2.Add(item.Value); }
            return (output.Item1.ToArray(), output.Item2.ToArray());
            
        }
    }

}
