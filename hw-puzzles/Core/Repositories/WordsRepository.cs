#pragma warning disable CS1572, CS1573, CS1591
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;

namespace hwpuzzles.Core.Repositories
{
    public class WordsRepository : IWordsRepository
    {
        const string WordsList = "urn:words";

        public int WordCount { get; set; }

        public IRedisClient RedisClient { get; }
        protected ILogger<WordsRepository> Logger { get; }

        public WordsRepository(
            IRedisClient redisClient,
            ILogger<WordsRepository> logger)
        {
            RedisClient = redisClient;
            Logger = logger;

            Initialize();
        }

        internal void Initialize()
        {
            try
            {
                WordCount = RedisClient.Lists[WordsList].Count;
                Logger.LogInformation($"Found {WordCount} words.");
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, "Error: ");
            }
        }

        public string Get(int id) => RedisClient.Lists[WordsList].GetRange(id, id).FirstOrDefault();
    }
}
