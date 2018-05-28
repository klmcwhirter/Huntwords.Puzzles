#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Options;
using puzzles.Models;
using System.Linq;

namespace puzzles.Services
{
    public class PuzzleWordGenerator : IGenerator<IList<PuzzleWord>>
    {
        protected IIndex<string, IPuzzleKind> WordGenerators { get; }
        public PuzzleWordGenerator(
            IIndex<string, IPuzzleKind> wordGenerators)
        {
            WordGenerators = wordGenerators;
        }

        public IList<PuzzleWord> Generate(params object[] options)
        {
            var puzzle = (Puzzle)options[0];
            var boardOptions = (PuzzleBoardGeneratorOptions)options[1];

            var rc = new List<PuzzleWord>();
            var displayedWords = new List<string>();
            var letterCount = (int)(boardOptions.MaxWidth * boardOptions.MaxHeight * boardOptions.WordDensity);
            var wordGenerator = SelectWordGenerator(puzzle);

            do
            {
                var puzzleWord = wordGenerator.Generate(puzzle);
                // Note this is destructive, but it has the positive benefit of changing all placed words to upper case
                puzzleWord.Word = puzzleWord.Word.ToUpperInvariant();
                rc.Add(puzzleWord);
                if ((letterCount - puzzleWord.Word.Length) >= 0)
                {
                    displayedWords.Add(puzzleWord.Word);
                }
                letterCount -= puzzleWord.Word.Length;
            } while (letterCount > 0);

            // Fix up Id's for in-memory generated lists - relies on Id being provided as a negative #
            // Note the .ToArray() call is needed to force the Select query to be executed.
            if (rc.Count > 0 && rc.First().Id < 0)
            {
                rc.Select((pw, idx) => { if (pw.Id < 0) pw.Id = idx; return pw; }).ToArray();
            }

            return rc;
        }

        protected IPuzzleKind SelectWordGenerator(Puzzle puzzle)
        {
            IPuzzleKind rc;

            var kind = puzzle.Kind?.Key;
            if(kind == null)
            {
                // If the kind is null then it is not an in-memory kind
                kind = DbPuzzleWordGenerator.StaticKey;
            }

            if (!WordGenerators.TryGetValue(kind, out rc))
            {
                rc = WordGenerators[WordWordGenerator.StaticKey];
            }
            return rc;
        }
    }
}
