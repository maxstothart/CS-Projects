using System.IO.Pipes;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using CT = Tools.ConsoleTools;
using LCSV = Tools.LoadCSVFromFile;
using SORT = Tools.Sort;

namespace Gemini_Challenges
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (false) //basic functions
            {
                int[] x = { 3, 4, 5, 6, 6, 7, 8, 9, 9 };
                var cc = new CurrencyConverter();

                CT.Print(Functions.ReverseString("Test"));
                CT.Print(Functions.IsPalindrome("Test"));
                CT.Print(Functions.IsPalindrome("Hannah"));
                CT.Print(Functions.NumVowels("Banana"));
                Console.WriteLine(Functions.ArrayMinMax(x));
                CT.EmptyLine(2);
                CT.Print(Functions.FindDuplicates(x), null, "Duplicates In Array");
                CT.Print(Functions.GenerateFibonacci(5), null, "Fibonacci Generator");
                CT.EmptyLine(2);
                CT.Print(cc.Convert("25.23 NZD", "USD").ToString("F2"));
            }
            if (false) //Flight path data loading tests
            {
                var Data = new LCSV("D:\\source\\Gemini Challenges\\TestData\\flights.log", ", ", ",");
                CT.Print(Data.Indices);
                CT.Print(Data.GetLine(3));
                CT.Print(CT.ToString(Data.GetLine(3)));
                CT.Print(Data.GetData(Data.Indices[0]));
            }
            if (false) //Finished Flight Path Validation Code
            {
                var FPV = new FlightPathValidator(new LCSV("D:\\source\\Gemini Challenges\\TestData\\flights.log", ", ", ","));

                CT.Print(FPV.GetDuplicates(), "Duplicates: ");
                CT.Print(FPV.CheckGateConflicts(), "Gate Conflicts: ");
            }
            if (true)
            {
                var GLB = new GridLoadBalancer(new LCSV("D:\\source\\Gemini Challenges\\TestData\\GridLoadBalancer\\Substations3.dat"));
                GLB.UpdateDemand(new LCSV("D:\\source\\Gemini Challenges\\TestData\\GridLoadBalancer\\Demand3.log"));

                CT.Print("Initial Data:");
                CT.Print(GLB.GetNodes().Select(i => i.Item1 + ", " + i.Item2 + ", " + i.Item3.ToString()).ToArray(), null, "Nodes: ");
                CT.Print(GLB.GetDraw().Select(i => i.Item1.ToString()+", "+i.Item2.ToString()).ToArray(), null, "Current Power Draw (KW): ");
                CT.Print(GLB.GetHNode().Select(i => i.Item1 + ", " + i.Item2).ToArray(), null, "Hungriest Node per Station: ");
                CT.Print(GLB.CheckOverload().Select(i => i.Item1.ToString() + ", " + i.Item2.ToString() + ", " + i.Item3.ToString() + "KW").ToArray(), null, "Overloads: ");
                CT.EmptyLine(3);
                CT.Print("Before and After Balancing:");
                CT.Print(GLB.GetDrawPercentage().Select(i => i.Item1.ToString() + ", " + i.Item2.ToString()+"%").ToArray(), null, "Current Power Draw (%): ");

                GLB.SmartRebalance(95);

                CT.Print(GLB.GetDrawPercentage().Select(i => i.Item1.ToString() + ", " + i.Item2.ToString() + "%").ToArray(), null, "Current Power Draw (%): ");
                CT.EmptyLine(3);
                CT.Print("Final Data:");
                CT.Print(GLB.GetNodes().Select(i => i.Item1 + ", " + i.Item2 + ", " + i.Item3.ToString()).ToArray(), null, "Nodes: ");
                CT.Print(GLB.GetDraw().Select(i => i.Item1.ToString() + ", " + i.Item2.ToString()).ToArray(), null, "Current Power Draw (KW): ");
                CT.Print(GLB.GetHNode().Select(i => i.Item1 + ", " + i.Item2).ToArray(), null, "Hungriest Node per Station: ");
                CT.Print(GLB.CheckOverload().Select(i => i.Item1.ToString() + ", " + i.Item2.ToString() + ", " + i.Item3.ToString() + "KW").ToArray(), null, "Overloads: ");

            }


        }
        

    }
    static class Functions
    {
        public static string ReverseString(string input) => new string(input.Reverse().ToArray());
        public static bool IsPalindrome(string input) => input.ToLower() == ReverseString(input.ToLower());

        public static int NumVowels(string input)
        {
            int output = 0;
            input = input.ToUpper();
            for (int i = 0; i < input.Length; i++)
            {
                if ("AEIOU".Contains(input[i])) { output++; }
            }
            return output;
        }
        public static (int, int) ArrayMinMax(int[] Data)
        {
            (int min, int max) = (Data[0], Data[0]);
            for(int i = 0; i < Data.Length; i++)
            {
                if (Data[i] > max) { max = Data[i]; }
                if (Data[i] < min) { min = Data[i]; }
            }
            return (min, max);
        }
        public static int[] FindDuplicates(int[] Data)
        {
            HashSet<int> x = new HashSet<int>();
            List<int> output = new List<int>();
            for (int i = 0; i < Data.Length; i++)
            {
                if (!x.Add(Data[i]))
                {
                    output.Add(Data[i]);
                }
            }
            return output.ToArray();
        }
        public static Tuple<int[], Object[]> FindDuplicates(Object[] Data)
        {
            Dictionary<Object, List<int>> Store = new Dictionary<object, List<int>>();
            (List<int>, List<Object>) Output = (new List<int>(), new List<object>());

            foreach (int i in Enumerable.Range(0, Data.Length))
            {
                Store.TryAdd(Data[i], new List<int>());
                Store[Data[i]].Add(i);
            }
            foreach (var entry in Store)
            {
                if (entry.Value.Count >= 2) { foreach (int x in entry.Value) { Output.Item1.Add(x); Output.Item2.Add(entry.Key); } }
            }
            return new Tuple<int[], Object[]>(Output.Item1.ToArray(), Output.Item2.ToArray());
        }

        public static int[] GenerateFibonacci(int Max)
        {
            int[] output = new int[Max];
            output[0] = 1;
            output[1] = 1;
            for (int i = 1; i < Max-1; i++)
            {
                output[i + 1] = output[i - 1] + output[i];
            }
            return output;
        }
    }
    public class CurrencyConverter
    {
        int universalUnit;
        Dictionary<string, double> RatiosToUU = new Dictionary<string, double>();
        public CurrencyConverter()
        {
            RatiosToUU.Add("USD", 1);
            RatiosToUU.Add("NZD", 0.58d);
            RatiosToUU.Add("AUD", 0.66d);
        }
        public double Convert(String input, String OutputCurrency = "USD")
        {
            //23.54 NZD
            RatiosToUU.TryGetValue(input.Substring(input.Length-3,3), out double InRatio);
            RatiosToUU.TryGetValue(OutputCurrency, out double OutRatio);
            return (double)Math.Round((double.Parse(input[0..^3]) / InRatio) * OutRatio, 2);
        }



    }
    public class FlightPathValidator
    {
        public LCSV Data;

        public FlightPathValidator(LCSV data)
        {
            Data = data;
        }

        public (int[], List<Object[]>) GetDuplicates()
        {
            int[] DupCount = new int[Data.LineCount()];
            List<int> Output = new List<int>();
            foreach (int i in Enumerable.Range(0, Data.Indices.Length))
            {
                Object[] x = Data.GetData(i);
                Tuple<int[], Object[]> y = Functions.FindDuplicates(x);
                foreach (int j in y.Item1)
                {
                    DupCount[j]++;
                }
            }
            foreach (int i in Enumerable.Range(0, DupCount.Length)) { if (DupCount[i] == Data.Indices.Length) { Output.Add(i); } }
            ;
            return (Output.ToArray(), Data.GetLine(Output.ToArray()));

        }
        public (int[], List<Object[]>) CheckGateConflicts()
        {
            int[] ConflictCount = new int[Data.LineCount()];
            List<int> Output = new List<int>();
            (int[], int[]) gateTimes = SORT.Bubble(CT.ToInt(Data.GetData("TakeoffTime")));
            foreach (int i in Enumerable.Range(1, gateTimes.Item2.Length-1))
            {
                if (gateTimes.Item2[i] - gateTimes.Item2[i-1] <= 15)
                {
                    Output.Add(gateTimes.Item1[i]);
                    Output.Add(gateTimes.Item1[i-1]);
                }
            }
            //CT.Print(Data.GetLine(Output.ToArray()));

            return (Output.ToArray(), Data.GetLine(Output.ToArray()));
        }
    }
    
    public class GridLoadBalancer
    {
        public class Substation(String id, double maxcap)
        {
            //Nodes[NodeID} = NodeDraw;
            //Nodes.Remove(NodeID);

            public String ID = id;
            public Dictionary<String, double> Nodes = new Dictionary<string, double>();
            public double MaxCapacity = maxcap;
            public double GetDraw() 
            { 
                if (Nodes.Count < 1) { return 0; }
                double i = 0;  
                foreach (double draw in Nodes.Values) 
                {
                    i += draw;
                }
                return i;
            }
            public double GetDrawPercentage()
            {
                return (double)Math.Round((this.GetDraw() / this.MaxCapacity) * 100, 2);
            }
            public (String, double) GetHungriestNode()
            {
                if (Nodes.Count < 1) { return (null, 0); }
                var x = Nodes.OrderBy(i => i.Value);
                return (x.Select(i => i.Key).ToArray()[^1], x.Select(i => i.Value).ToArray()[^1]);
            }
            
        }
        Dictionary<String, Substation> Substations = new Dictionary<String, Substation>();
        public GridLoadBalancer(LCSV substations)
        {
            List<Object[]> Data = substations.GetAllLines();
            foreach (Object[] line in Data)
            {
                Substations.Add(line[0].ToString(), new Substation(line[0].ToString(), double.Parse(line[1].ToString())));
            }
        }
        public void UpdateDemand(LCSV demand)
        {
            foreach (Substation ss in Substations.Values) { ss.Nodes = new Dictionary<String, double>(); }
            foreach (Object[] line in demand.GetAllLines())
            {
                //CT.Print(line);
                if (Substations.TryGetValue(line[1].ToString(), out Substation ss)) 
                { 
                    ss.Nodes[line[0].ToString()] = double.Parse(line[2].ToString()); 
                }
                else 
                {
                    CT.Print("Remapping \"" + line[0].ToString() + "\" from \"" + line[1] + "\" to \"" + this.SmartMap((line[0].ToString(), double.Parse(line[2].ToString())), 100)+"\"");
                    //Console.WriteLine(line[1].ToString() + line[0]);
                    
                }
            }
            this.SmartRebalance(100);
            CT.EmptyLine(2);
        }
        public List<(String, String, double)> GetNodes()
        {
            List<(String, String, double)> Out = new List<(string, string, double)>();
            foreach (Substation ss in Substations.Values) { foreach (var node in ss.Nodes) { Out.Add((ss.ID, node.Key, node.Value)); } }
            return Out;
        }
        public List<(String, double)> GetDraw()
        {
            List<(String, double)> Out = new List<(string, double)>();
            foreach (Substation ss in Substations.Values) { Out.Add((ss.ID, ss.GetDraw())); }
            return Out;
        }
        public List<(String, double)> GetDrawPercentage()
        {
            List<(String, double)> Out = new List<(string, double)>();
            foreach (Substation ss in Substations.Values) { Out.Add((ss.ID, ss.GetDrawPercentage())); }
            return Out;
        }
        public List<(String, String)> GetHNode()
        {
            List<(String, String)> Out = new List<(string, String)>();
            foreach (Substation ss in Substations.Values) { Out.Add((ss.ID, ss.GetHungriestNode().Item1+", "+ss.GetHungriestNode().Item2.ToString())); }
            return Out;
        }
        public List<(String, double, double)> CheckOverload()
        {
            List<(String, double, double)> Output = new List<(String, double, double)>();
            foreach (Substation ss in Substations.Values)
            {
                double i = ss.GetDraw();
                if (i > ss.MaxCapacity)
                {
                    Output.Add((ss.ID, ss.MaxCapacity, i-ss.MaxCapacity));
                }
            }
            return Output;
        }
        public void SmartRebalance(int SwitchThreshold = 90)
        {
            foreach (Substation ss in Substations.Values)
            {
                //this.SmartMap(ss, SwitchThreshold);
                (String, double) HungriestNode = ss.GetHungriestNode();
                if (HungriestNode.Item1 == null) { break; }
                foreach (Substation nss in Substations.Values)
                {
                    if ((((HungriestNode.Item2 + nss.GetDraw())/nss.MaxCapacity)*100) < ((ss.GetDrawPercentage() > 100) ? 100: 100-(100-SwitchThreshold)/2))
                    {
                        nss.Nodes[HungriestNode.Item1] = ss.Nodes[HungriestNode.Item1];
                        ss.Nodes.Remove(HungriestNode.Item1);
                        break;
                    }
                }
                
            }
        }
        public String SmartMap((String ID, double Draw) Node, int SwitchThreshold = 90)
        {
            foreach (Substation ss in Substations.Values)
            {
                if (((ss.GetDraw()+Node.Draw)/ss.MaxCapacity)*100 < SwitchThreshold)
                {
                    ss.Nodes.Add(Node.ID, Node.Draw);
                    return ss.ID;
                }
            }
            return null;
        }
    }


}
