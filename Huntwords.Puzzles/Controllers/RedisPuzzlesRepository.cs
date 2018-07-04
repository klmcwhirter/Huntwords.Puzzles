#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using Huntwords.Common.Models;
using Huntwords.Common.Repositories;
using ServiceStack.Redis;

namespace Huntwords.Puzzles.Controllers
{
    public class RedisPuzzlesRepository : IPuzzlesRepository
    {
        protected string PuzzlePrefix = "urn:puzzle:";

        public RedisPuzzlesRepository(IRedisClient redisClient)
        {
            RedisClient = redisClient;
        }

        public IRedisClient RedisClient { get; }

        public Puzzle Add(Puzzle puzzle)
        {
            var key = PuzzlePrefix + puzzle.Name;
            RedisClient.Set(key, puzzle);
            var rc = Get(puzzle.Name);
            return rc;
        }

        public Puzzle AddWord(string name, string word)
        {
            var puzzle = Get(name);
            puzzle.PuzzleWords.Add(word);
            var rc = Update(puzzle.Name, puzzle);
            return rc;
        }

        public void Delete(string name)
        {
            var key = PuzzlePrefix + name;
            RedisClient.Remove(key);
        }

        public Puzzle DeleteWord(string name, string word)
        {
            var puzzle = Get(name);
            puzzle.PuzzleWords.Remove(word);
            var rc = Update(name, puzzle);
            return rc;
        }

        public Puzzle Get(string name)
        {
            var key = PuzzlePrefix + name;
            var rc = RedisClient.Get<Puzzle>(key);
            return rc;
        }

        public ICollection<Puzzle> GetAll()
        {
            var keys = RedisClient.SearchKeys(PuzzlePrefix + "*");
            var rc = RedisClient.GetAll<Puzzle>(keys);
            return rc.Values;
        }

        public Puzzle Update(string name, Puzzle puzzle)
        {
            var rc = Add(puzzle);
            return rc;
        }
    }
}