#pragma warning disable CS1572, CS1573, CS1591
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using hwpuzzles.Core.Models;
using hwpuzzles.Core.Utils;
using hwpuzzles.Core.Repositories;

namespace hwpuzzles.Core.Services
{
    public class PuzzleBoardGenerator : IGenerator<PuzzleBoard>
    {
        static IDictionary<WordSlope, PuzzlePoint> DirectionOffsets = new Dictionary<WordSlope, PuzzlePoint>()
        {
            { WordSlope.NW, new PuzzlePoint(-1, -1) },
            { WordSlope.N , new PuzzlePoint( 0, -1) },
            { WordSlope.NE, new PuzzlePoint( 1, -1) },
            { WordSlope.E , new PuzzlePoint( 1,  0) },
            { WordSlope.SE, new PuzzlePoint( 1,  1) },
            { WordSlope.S , new PuzzlePoint( 0,  1) },
            { WordSlope.SW, new PuzzlePoint(-1,  1) },
            { WordSlope.W , new PuzzlePoint(-1,  0) },
        }.ToImmutableDictionary();

        static WordSlope[] Directions => DirectionOffsets.Keys.ToArray();
        int MaxTries { get; set; }
        int MinDiagonals { get; set; }
        IPuzzlesRepository PuzzlesRepository { get; }
        ICharacterGenerator CharacterGenerator { get; }
        IGenerator<IList<string>> PuzzleWordGenerator { get; }
        PuzzleBoardGeneratorOptions Options { get; set; }
        ILogger<PuzzleBoardGenerator> Logger { get; }

        public PuzzleBoardGenerator(
            IPuzzlesRepository puzzlesRepository,
            ICharacterGenerator characterGenerator,
            IGenerator<IList<string>> puzzleWordGenerator,
            IOptions<PuzzleBoardGeneratorOptions> options,
            ILogger<PuzzleBoardGenerator> logger)
        {
            PuzzlesRepository = puzzlesRepository;
            CharacterGenerator = characterGenerator;
            PuzzleWordGenerator = puzzleWordGenerator;
            Options = options.Value;
            Logger = logger;
        }

        public PuzzleBoard Generate(params object[] options)
        {
            var name = (string)options[0];
            var verbose = (bool)options[1];

            var puzzle = PuzzlesRepository.Get(name);

            // Some algorithms try to do fancy things like sort the word list in reverse length order
            // This generates placements that rarely include diagonal directions. Just say no!

            var rc = new PuzzleBoard
            {
                Width = Options.MaxWidth,
                Height = Options.MaxHeight,
                Puzzle = puzzle
            };

            Logger.LogInformation($"puzzle={puzzle}");
            Logger.LogInformation($"PuzzleWordGenerator={PuzzleWordGenerator}");

            var words = PuzzleWordGenerator.Generate(puzzle, Options);

            // No sense in trying more cells than exist in the grid
            // factor is used to account for potential duplication in the random number generator
            MaxTries = Directions.Length * rc.Width * rc.Height * Options.RandomFactor;

            // Round down
            MinDiagonals = (int)Math.Ceiling(words.Count * Options.DiagonalRatio);

            var retry = 1;
            for (; retry <= Options.Retries; retry++)
            {
                rc.Letters = new string[rc.Height, rc.Width];
                rc.Solutions = new WordSolution[words.Count];

                for (var w = 0; w < words.Count; w++)
                {
                    var word = words[w];
                    rc.Solutions[w] = PlaceWord(rc, word, verbose);
                }

                if (!ValidSolution(retry, rc, verbose))
                {
                    continue;
                }

                if (verbose) Logger.LogInformation($"Found a solution for puzzle={rc.Puzzle.Name} after {retry} tries");
                break; // phew found a solution
            }

            FillWithRandomLetters(rc);

            return rc;
        }

        private WordSolution PlaceWord(PuzzleBoard board, string word, bool verbose)
        {
            if (verbose) Logger.LogDebug($"Placing word={word}");

            var rc = new WordSolution
            {
                Word = word,
                Placed = false,
                WordSlope = WordSlope.S,
                Origin = new PuzzlePoint(),
                Points = new List<PuzzlePoint>()
            };

            var tries = 0;
            for (; tries < MaxTries; tries++)
            {
                var x = board.Width.Random();
                var y = board.Height.Random();

                // Is the first cell a probable match
                if (board.Letters[y, x] == null || board.Letters[y, x] == word[0].ToString())
                {
                    // Iterate over directions in random order
                    var dirIdxs = Enumerable.Range(0, Directions.Length).ToArray();
                    dirIdxs.Shuffle();

                    foreach (var idx in dirIdxs)
                    {
                        // get the selected direction deltas
                        var wordSlope = Directions[idx];
                        var dirOffsets = DirectionOffsets[wordSlope];

                        // Make sure the cells for word are empty, contain matching char
                        // and are not contained inside another solution
                        if (WordWillFit(rc, board, dirOffsets, x, y))
                        {
                            WordCopyToBoard(word, board, dirOffsets, x, y);

                            if (verbose) Logger.LogDebug($"{word} was placed @ ({x},{y}) via {wordSlope} for puzzle={board.Puzzle.Name} in {tries} tries");

                            rc.Placed = true;
                            rc.WordSlope = wordSlope;
                            rc.Origin = new PuzzlePoint(x, y);

                            return rc;
                        }
                    }
                }
            }

            if (verbose) Logger.LogDebug($"{word} was not placed in {tries} tries");

            return rc;
        }

        private void FillWithRandomLetters(PuzzleBoard board)
        {
            for (var r = 0; r < board.Height; r++)
            {
                for (int c = 0; c < board.Width; c++)
                {
                    if (board.Letters[r, c] == null)
                    {
                        board.Letters[r, c] = CharacterGenerator.Generate();
                    }
                }
            }
        }

        /// Validate the quality of the solution
        private bool ValidSolution(int retry, PuzzleBoard board, bool verbose)
        {
            var solutions = board.Solutions;

            // If any words were not placed - try again
            if (solutions.Any(s => !s.Placed))
            {
                if (verbose) Logger.LogError($">>>> At least 1 word was not placed for puzzle={board.Puzzle.Name} - retrying for try {retry + 1}");
                return false;
            }

            // If there are no diagonally placed words - try again
            var numDiags = solutions.CountDirection(new[] { WordSlope.NW, WordSlope.NW, WordSlope.SW, WordSlope.SE });
            if (numDiags < MinDiagonals)
            {
                if (verbose) Logger.LogError($">>>> There were not enough diagonal placements for puzzle={board.Puzzle.Name} - wanting at least {MinDiagonals} but got {numDiags} - retrying for try {retry + 1}");
                return false;
            }

            // If there are no vertically placed words - try again
            var numVerts = solutions.CountDirection(new[] { WordSlope.N, WordSlope.S });
            if (numVerts < 1)
            {
                if (verbose) Logger.LogError($">>>> There were no vertical placements for puzzle={board.Puzzle.Name} - retrying for try {retry + 1}");
                return false;
            }

            // If there are no horizontally placed words - try again
            var numHoriz = solutions.CountDirection(new[] { WordSlope.W, WordSlope.E });
            if (numHoriz < 1)
            {
                if (verbose) Logger.LogError($">>>> There were no horizontal placements for puzzle={board.Puzzle.Name} - retrying for try {retry + 1}");
                return false;
            }

            return true;
        }

        private void WordCopyToBoard(string word, PuzzleBoard board, PuzzlePoint directionOffsets, int x, int y)
        {
            for (int position = 0; position < word.Length; position++)
            {
                var thisChar = word[position].ToString();
                var currX = x + (position * directionOffsets.X);
                var currY = y + (position * directionOffsets.Y);

                board.Letters[currY, currX] = thisChar;
            }
        }

        private bool WordWillFit(WordSolution solution, PuzzleBoard board, PuzzlePoint directionOffsets, int x, int y)
        {
            var rc = true;

            // We may be finding another placement ...
            solution.Points.Clear();

            for (int position = 0; position < solution.Word.Length; position++)
            {
                var thisChar = solution.Word[position].ToString();
                var currX = x + (position * directionOffsets.X);
                var currY = y + (position * directionOffsets.Y);

                if (currX < 0 || currX > (board.Width - 1) || currY < 0 || currY > (board.Height - 1))
                {
                    rc = false;
                    break; // outside the bounds of the grid
                }
                // First chance - target cell is empty; Second chance - target cell contains the letter
                if (board.Letters[currY, currX] != null && board.Letters[currY, currX] != thisChar)
                {
                    rc = false;
                    break; // won't fit
                }

                // Looks like it will fit so far - add Point ...
                solution.Points.Add(new PuzzlePoint(currX, currY));
            }

            // Verify there is no overlay inside another word placement
            if (rc)
            {
                rc = !board.Solutions.Any(
                    s =>
                        s != null &&
                        solution.Word.Length <= s.Word.Length &&
                        solution.Points.Intersect(s.Points).SequenceEqual(solution.Points)
                    );
            }

            return rc;
        }
    }

    public static class WordSolutionArrayExtensions
    {
        public static int CountDirection(this WordSolution[] solutions, WordSlope[] slopes)
        {
            var rc = solutions.Select(s => s.WordSlope)
                        .Distinct()
                        .Intersect(slopes)
                        .Count();
            return rc;
        }
    }
}
