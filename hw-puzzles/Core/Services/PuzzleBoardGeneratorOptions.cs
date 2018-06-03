#pragma warning disable CS1572, CS1573, CS1591
namespace hwpuzzles.Core.Services
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
