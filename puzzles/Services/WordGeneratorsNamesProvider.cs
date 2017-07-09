using System;
using System.Collections.Generic;
using puzzles.Models;

namespace puzzles.Services
{
    public static class WordGeneratorsNamesProvider
    {
        public static string[] Names { get; }

        static WordGeneratorsNamesProvider()
        {
            Names = new[]
            {
                DbPuzzleWordGenerator.StaticKey,
                RandomWordGenerator.StaticKey,
                WordWordGenerator.StaticKey
            };
        }
    }
}