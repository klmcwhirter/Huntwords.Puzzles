namespace puzzles.Services
{
    public class PuzzleGeneratorOptions
    {
        public int MaxWidth { get; set; }

        public int MaxHeight {get; set; }

        public double WordDensity { get; set; }

        public string[] Kinds { get; set; }
    }
}
