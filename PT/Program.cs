using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var PTInstance = new PT();
            PTInstance.loadFile("D:\\source\\PT\\elements.txt");
            var GameInstance = new Game(1, 20, PT.MODE.NAME, PT.MODE.SYMBOL, PTInstance);

            GameInstance.Play(10);
        }

    }
    public class PT
    {
        public List<string> Names { get; private set; } = new List<string>();
        public List<string> Symbols { get; private set; } = new List<string>();
        public enum MODE
        {
            NAME,
            SYMBOL,
            NUMBER
        }

        public void loadFile(string path)
        {
            string[] elements = File.ReadAllText(path).Split("\n");
            this.Names.Add("");
            this.Symbols.Add("");
            foreach (int i in Enumerable.Range(0, elements.Length))
            {
                this.Names.Add(elements[i].Split(",")[0]);
                //Console.WriteLine(element.Split(",")[0]);

                this.Symbols.Add(elements[i].Split(",")[1]);
                //Console.WriteLine(element.Split(",")[1]);
            }
        }
        public string getData(MODE mode, int index)
        {
            switch (mode)
            {
                case MODE.NAME:
                    return this.Names[index].ToString();
                case MODE.SYMBOL:
                    return this.Symbols[index].ToString();
                case MODE.NUMBER:
                    return index.ToString();
            }
            return "";
        }
    }
    public class Game
    {
        private int randMin;
        private int randMax;
        private PT.MODE given;
        private PT.MODE query;
        public PT PTInstance;
        

        public Game(int min, int max, PT.MODE given, PT.MODE query, PT instance)
        {
            randMin = min;
            randMax = max;
            given = given;
            query = query;
            PTInstance = instance;
        }

        public void Play(int rounds)
        {
            var r = new Random();
            int prevIndex = 0;
            int currIndex = 0;
            for (int i = 0; i < rounds; i++)
            {
                while (currIndex == prevIndex)
                {
                    currIndex = r.Next(randMin, randMax);
                }
                prevIndex = currIndex;

                this.frame(currIndex, given, query);
            }
        }
        public bool frame(int index, PT.MODE given, PT.MODE query)
        {
            string ans = PTInstance.getData(given, index);
            string que = PTInstance.getData(query, index);

            Console.WriteLine(que);
            string input = Console.ReadLine();

            Console.WriteLine((input.ToLower() == ans.ToLower()) ? "Correct" : "Incorrect" + " - (" + ans + ")");
            return (input == ans);

        }
    }
}
