#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Options;
using hwpuzzles.Core.Models;
using System.Linq;

namespace hwpuzzles.Core.Services
{
    public class PuzzleWordGenerator : IGenerator<IList<string>>
    {
        protected IIndex<string, string> WordGenerators { get; }
        public PuzzleWordGenerator(
            IIndex<string, string> wordGenerators)
        {
            WordGenerators = wordGenerators;
        }

        public IList<string> Generate(params object[] options)
        {
            var puzzle = (Puzzle)options[0];
            var boardOptions = (PuzzleBoardGeneratorOptions)options[1];

            var rc = new List<string>();
            var displayedWords = new List<string>();
            var letterCount = (int)(boardOptions.MaxWidth * boardOptions.MaxHeight * boardOptions.WordDensity);
            IGenerator<string> wordGenerator = null; // TODO: SelectWordGenerator(puzzle);

            do
            {
                var puzzleWord = wordGenerator.Generate(puzzle);
                // Note this is destructive, but it has the positive benefit of changing all placed words to upper case
                puzzleWord = puzzleWord.ToUpperInvariant();
                rc.Add(puzzleWord);
                if ((letterCount - puzzleWord.Length) >= 0)
                {
                    displayedWords.Add(puzzleWord);
                }
                letterCount -= puzzleWord.Length;
            } while (letterCount > 0);

#if DONT_DO_THIS
            // Fix up Id's for in-memory generated lists - relies on Id being provided as a negative #
            // Note the .ToArray() call is needed to force the Select query to be executed.
            if (rc.Count > 0 && rc.First().Id < 0)
            {
                rc.Select((pw, idx) => { if (pw.Id < 0) pw.Id = idx; return pw; }).ToArray();
            }
#endif
            return rc;
        }
    }
}
