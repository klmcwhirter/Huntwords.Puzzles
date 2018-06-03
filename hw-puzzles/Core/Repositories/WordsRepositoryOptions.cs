#pragma warning disable CS1572, CS1573, CS1591
namespace hwpuzzles.Core.Repositories
{
    public class WordsRepositoryOptions
    {
        public string WordFilePath { get; set; }
        public int MinWordLength { get; set; }
        public int MaxWordLength { get; set; }
    }
}
