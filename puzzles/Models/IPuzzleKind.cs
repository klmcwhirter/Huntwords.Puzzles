
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
