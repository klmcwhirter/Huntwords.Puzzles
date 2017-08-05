namespace puzzles.Services
{
    public class PuzzleBoardGeneratorOptions
    {
        public int CacheSize { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public int Retries { get; set; }
        public double DiagonalRatio { get; set; }
        public int RandomFactor { get; set; }
        public double WordDensity { get; set; }
    }
}
