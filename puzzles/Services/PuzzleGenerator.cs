using System.Collections.Generic;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Options;
using puzzles.Models;
using System.Linq;

namespace puzzles.Services
{
    public class PuzzleGenerator : IGenerator<Puzzle>
    {
        public IPuzzleKind[] Kinds => Options.Kinds.Select(k => SelectWordGenerator(k, 1)).ToArray();

        protected IGenerator<PuzzleBoard> BoardGenerator { get; }

        protected IIndex<string, IPuzzleKind> WordGenerators { get; }
        protected PuzzleGeneratorOptions Options { get; }

        public PuzzleGenerator(IIndex<string, IPuzzleKind> wordGenerators,
                               IGenerator<PuzzleBoard> boardGenerator,
                               IOptions<PuzzleGeneratorOptions> options)
        {
            WordGenerators = wordGenerators;
            BoardGenerator = boardGenerator;
            Options = options.Value;
        }

        public Puzzle Generate(params object[] options)
        {
            var mode = (string)options[0];
            var id = (int?)options[1];
            var rc = GeneratePuzzleBoard(Options.MaxWidth, Options.MaxHeight, Options.WordDensity, mode, id);
            return rc;
        }

        protected Puzzle GeneratePuzzleBoard(int maxWidth, int maxHeight, double wordDensity, string mode, int? id)
        {
            var words = new List<PuzzleWord>();
            var displayedWords = new List<string>();
            var letterCount = (int)(maxWidth * maxHeight * wordDensity);
            var wordGenerator = SelectWordGenerator(mode, id);

            do
            {
                var puzzleWord = wordGenerator.Generate(id);
                puzzleWord.Word = puzzleWord.Word.ToUpperInvariant();
                words.Add(puzzleWord);
                if ((letterCount - puzzleWord.Word.Length) >= 0)
                {
                    displayedWords.Add(puzzleWord.Word);
                }
                letterCount -= puzzleWord.Word.Length;
            } while (letterCount > 0);

            // Fix up Id's for in-memory generated lists - relies on Id being provided as a negative #
            // Note the .ToArray() call is needed to force the Select query to be executed.
            if (words.Count > 0 && words.First().Id < 0)
            {
                words.Select((pw, idx) => { if (pw.Id < 0) pw.Id = idx; return pw; }).ToArray();
            }
            var rc = new Puzzle()
            {
                Id = wordGenerator.PuzzleId ?? 0,
                Name = wordGenerator.Name,
                Description = wordGenerator.Description,
                PuzzleWords = words.ToArray(),
                Kind = wordGenerator,
                Board = BoardGenerator.Generate(maxWidth, maxHeight, displayedWords.ToArray())
            };

            return rc;
        }

        protected IPuzzleKind SelectWordGenerator(string mode, int? id)
        {
            IPuzzleKind rc;
            if (!WordGenerators.TryGetValue(mode, out rc))
            {
                var localrc = WordGenerators[WordWordGenerator.StaticKey];
                if(localrc.IsIdValid(id))
                {
                    rc = localrc;
                }
            }
            return rc;
        }
    }
}
