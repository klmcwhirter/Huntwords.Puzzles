using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace puzzles.Repositories
{
    public class WordsRepository : IWordsRepository
    {
        protected List<string> Words { get; set; }
        public int WordCount => Words.Count;

        protected WordsRepositoryOptions Options { get; set; }
        protected ILogger<WordsRepository> Logger { get; }

        public WordsRepository(
            IOptions<WordsRepositoryOptions> options,
            ILogger<WordsRepository> logger)
        {
            Options = options.Value;
            Logger = logger;

            Initialize();
        }

        internal void Initialize()
        {
            try
            {
                Words = File.ReadAllLines(Options.WordFilePath)
                            // Filter words so they are within the configured length window
                            .Where(l => l.Length >= Options.MinWordLength && l.Length <= Options.MaxWordLength)
                            // Filter out words with puntuation
                            .Where(s => Regex.IsMatch(s, "^[A-Za-z0-9]+$"))
                            .ToList();
                Logger.LogInformation($"Found {WordCount} words.");
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, "Error: ");
                Words = new List<string> { "word" };
            }
        }

        public string Get(int id) => Words[id];

        public IQueryable<string> GetAll() => Words.AsQueryable();

        public void Add(string word) => throw new NotImplementedException();

        public void Update(int id, string word) => throw new NotImplementedException();

        public void Delete(int id) => throw new NotImplementedException();

        public void SaveChanges() => throw new NotImplementedException();
    }
}
