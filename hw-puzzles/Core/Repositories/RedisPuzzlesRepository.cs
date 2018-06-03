using System.Collections.Generic;
using hwpuzzles.Core.Models;
using ServiceStack.Redis.Generic;

namespace hwpuzzles.Core.Repositories
{
    public class RedisPuzzlesRepository : IPuzzlesRepository
    {
        protected IRedisTypedClient<Puzzle> RedisTypedClient { get; }

        public RedisPuzzlesRepository(IRedisTypedClient<Puzzle> redisTypedClient)
        {
            RedisTypedClient = redisTypedClient;
        }

        public Puzzle Add(Puzzle puzzle)
        {
            var rc = RedisTypedClient.Store(puzzle);
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
            RedisTypedClient.DeleteById(name);
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
            var rc = RedisTypedClient.GetById(name);
            return rc;
        }

        public ICollection<Puzzle> GetAll()
        {
            var rc = RedisTypedClient.GetAll();
            return rc;
        }

        public Puzzle Update(string name, Puzzle puzzle)
        {
            var rc = RedisTypedClient.Store(puzzle);
            return rc;
        }
    }
}