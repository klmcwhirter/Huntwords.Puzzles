#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using hwpuzzles.Core.Models;
using hwpuzzles.Core.Repositories;
using hwpuzzles.Core.Utils;

namespace hwpuzzles.Core.Services
{
    [DataContract]
    public class DbPuzzleWordGenerator
    {
        protected ILogger<DbPuzzleWordGenerator> Logger { get; }
        protected IPuzzlesRepository Repository { get; }

        public DbPuzzleWordGenerator(IPuzzlesRepository repository, ILogger<DbPuzzleWordGenerator> logger)
        {
            Repository = repository;
            Logger = logger;
        }

        protected int CurrentIdx { get; set; } = 0;
        protected ISet<string> Seen { get; set; }

        public string Generate(params object[] options)
        {
            var puzzle = (Puzzle)options[0];

            string rc;
            do
            {
                // If the word list is exhausted, start over
                if (Seen == null || Seen.Count >= puzzle.Words.Length)
                {
                    ResetSeen();
                }

                var idx = puzzle.Words.Length.Random();
                rc = puzzle.PuzzleWords.Skip(idx).FirstOrDefault();
            } while (Seen.Contains(rc));

            Seen.Add(rc);

            return rc;
        }

        protected void ResetSeen()
        {
            Seen = new SortedSet<string>(Comparer<string>.Create(
                        (e1, e2) => e1.ToUpperInvariant().CompareTo(e2.ToUpperInvariant())
                    ));
        }
    }
}
