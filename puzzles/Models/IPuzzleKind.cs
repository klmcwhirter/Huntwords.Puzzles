
namespace puzzles.Models
{
    public interface IPuzzleKind : IGenerator<PuzzleWord>
    {
        int? PuzzleId { get; }
        string Key { get; }
        string Name { get; }
        string Description { get; }

        bool IsIdValid(int? id);

        PuzzleKindFeatures Features { get; }
    }
}
