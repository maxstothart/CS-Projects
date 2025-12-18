using System;
using System.IO;
using System.Linq;

namespace Note_Associations
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Music m = new Music();
            //Console.WriteLine(m.Convert("C4", '#', true));

            int[] CMajor = new int[] { 3, 5, 7, 8, 10, 12, 14 };
            int[] Cminor = new int[] { 15, 17, 18, 20, 22, 23, 25 };

            //int[] CMajor2 = m.ConvertList(new String[] { "C1", "D1", "E1", "F1", "G1", "A2", "B2" });
            //int[] Cminor2 = m.ConvertList(new String[] { "C1", "D1", "Eb1", "F1", "G1", "Ab2", "Bb2" });

            //Console.WriteLine(String.Join(", ", CMajor));
            //Console.WriteLine(String.Join(", ", m.ConvertList(Cminor2, true, false)));
            //Console.WriteLine(String.Join(", ", m.GenerateScale("Gb", "Major", 16)));

            m.WriteScalesToFile("D:\\source\\Note Associations\\scales.txt");
        }
        
        struct Temperament()
        {
            public String?[] Naturals { get; } = { "A", null, "B", "C", null, "D", null, "E", "F", null, "G", null };
            public String?[] Flats { get; } = { null, "Bb", "Cb", null, "Db", null, "Eb", "Fb", null, "Gb", null, "Ab" };
            public String?[] Sharps { get; } = { null, "A#", null, "B#", "C#", null, "D#", null, "E#", "F#", null, "G#" };
            public Char[] Order { get; } = "ABCDEFG".ToCharArray();
            //public char?[] Accidentals { get; } = { null, '#', 'b' };
        }
        class Music
        {
            private Temperament temp = new Temperament();
            private int tlen;
            public string?[] notes;

            public Music()
            {
                tlen = temp.Naturals.Length;
                notes = temp.Naturals.Concat(temp.Sharps).Concat(temp.Flats).ToArray();
            }
            public static string FormatNumber(int number, bool ShowRoot)
            {
                if (ShowRoot && number % 8 == 0)
                {
                    return "root";
                }
                string strnum = number.ToString();
                switch (strnum[strnum.Length - 1])
                {
                    case '1':
                        return strnum + "st";
                    case '2':
                        return strnum + "nd";
                    case '3':
                        return strnum + "rd";
                    default:
                        return strnum + "th";
                }
            }

            public (String, int) SplitOffsets(String Note, int defaultOffset = 3)
            {
                int offset = 0;
                bool changedOffset = false;
                //Unreachable default value
                foreach (int i in Enumerable.Range(0, Note.Length))
                {
                    if (char.IsDigit(Note[i]))
                    {
                        changedOffset = true;
                        offset += (int)(char.GetNumericValue(Note[i]) * Math.Pow(10, Note.Length - (i + 1)));
                    }
                }
                if (Note.Contains('-'))
                {
                    offset *= -1;
                }
                return (Note.Substring(0, Note.Length - ((changedOffset) ? offset.ToString().Length : 0)), ((changedOffset) ? offset : defaultOffset));
            }
            public (int, int) SplitOffsets(int Note)
            {
                return (Note % tlen, (Note - (Note % tlen)) / tlen);
            }

            public int ConvertNote(String note, bool Index = false)
            {
                int offset;
                (note, offset) = SplitOffsets(note);
                return (Array.IndexOf(notes, note) % tlen) + offset * tlen;
            }
            public String ConvertNote(int note, char? accidental = null, char? PrevNote = 'h', bool Index = false)
            //convert string back to string with specified accidental
            //e.g. Eb - D#
            {
                (note, int offset) = SplitOffsets(note);
                int j = 0;
                //Console.WriteLine(temp.Order[(Array.IndexOf(temp.Order, PrevNote) + 1) % temp.Order.Length]);
                while (notes[note] == null || notes[note][notes[note].Length - 1] != accidental && accidental != null && j <= 2 || notes[note][0] == PrevNote && j <= 5 || notes[note][0] != temp.Order[(Array.IndexOf(temp.Order, PrevNote) + 1) % temp.Order.Length] && j <= 2)
                {
                    j++;
                    note = (note + tlen) % notes.Length;
                }

                return notes[note] + ((Index) ? offset.ToString() : null);
            }
            public int[] ConvertList(String[] note)
            {
                int[] Out = new int[note.Length];
                foreach (int i in Enumerable.Range(0, note.Length))
                {
                    //Console.WriteLine(note[i]);
                    Out[i] = this.ConvertNote(note[i]);
                }

                return Out;


            }
            public String[] ConvertList(int[] note, bool indexed = true, char? root = null, bool formatting = true)
            {
                String[] OUT = new string[note.Length];
                char? PrevNote = ((root == null) ? 'h' : temp.Order[(Array.IndexOf(temp.Order, root) + (temp.Order.Length - 1)) % temp.Order.Length]);
                foreach (int i in Enumerable.Range(0, note.Length))
                {
                    //Console.WriteLine(PrevNote);
                    OUT[i] = ConvertNote(note[i], null, PrevNote, indexed);
                    PrevNote = OUT[i][0];
                }
                return OUT;
            }
            public int[] getScaleSpacing(String Mode)
            {
                switch (Mode)
                {
                    case "Major":
                        return new int[] { 2, 2, 1, 2, 2, 2, 1 };
                    case "minor":
                        return new int[] { 2, 1, 2, 2, 1, 2, 2 };
                    case "Phrygian":
                        return new int[] { 1, 2, 2, 2, 1, 2, 2 };
                }
                return null;
            }
            public String[] GenerateScale(String root, String Mode, int range = 8)
            {
                int[] spacing = getScaleSpacing(Mode);
                int[] Out = new int[range];
                Out[0] = ConvertNote(root, false);
                foreach (int i in Enumerable.Range(1, range - 1))
                {
                    Out[i] = Out[i - 1] + spacing[(i - 1) % spacing.Length];
                    //Console.WriteLine(Out[i]);
                }
                return ConvertList(Out, true, root[0]);
            }
            public void WriteScalesToFile(string filename)
            {
                string[] scaleStore = new String[notes.Length * 4];
                int i = 0;
                foreach (String note in notes)
                {

                    if (note != null)
                    {
                        foreach (String mode in new String[] { "Major", "minor", "Phrygian" })
                        {
                            i++;
                            scaleStore[i] = (String.Join(", ", new String[] { mode }.Concat(GenerateScale(note, mode, 16))));
                        }
                    }
                }
                File.WriteAllLines(filename, scaleStore[0..(i + 1)]);

            }
        }
    }
}
 