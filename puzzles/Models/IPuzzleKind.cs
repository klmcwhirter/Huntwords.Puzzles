#pragma warning disable CS1572, CS1573, CS1591

namespace puzzles.Models
{
    public interface IPuzzleKind : IGenerator<PuzzleWord>, IPuzzleProvider
    {
        string Key { get; }
        string Name { get; }
        string Description { get; }

        PuzzleKindFeatures Features { get; }
    }
}
