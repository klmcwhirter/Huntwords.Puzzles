#pragma warning disable CS1572, CS1573, CS1591
using System;
using System.Collections.Generic;
using hwpuzzles.Core.Models;

namespace hwpuzzles.Core.Services
{
    public static class WordGeneratorsNamesProvider
    {
        public static readonly string Cached = "Cached";
        public static readonly string Random = "Random";
        public static readonly string Word = "Word";

        public static string[] Names { get; }

        static WordGeneratorsNamesProvider()
        {
            Names = new[]
            {
                Cached,
                Random,
                Word
            };
        }
    }
}