namespace Music
{
    public class Temperment
    {
        public string[] TTET = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        public string[] TTETFlat = { "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab" };

        public enum Note
        {
            A = 0,
            ASharp = 1,
            Bb = 1,
            B = 2,
            BSharp = 3,
            Cb = 2,
            C = 3,
            CSharp = 4,
            Db = 4,
            D = 5,
            DSharp = 6,
            Eb = 6,
            E = 7,
            ESharp = 8,
            Fb = 7,
            F = 8,
            FSharp = 9,
            Gb = 9,
            G = 10,
            GSharp = 11,
            Ab = 11
        }

        public string getNote(Note note)
        {
            return TTET[(int)note];
        }

    }
}
