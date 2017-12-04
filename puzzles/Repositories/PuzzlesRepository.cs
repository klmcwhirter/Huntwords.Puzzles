using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using puzzles.Models;
using puzzles.Services;

namespace puzzles.Repositories
{
    public class PuzzlesRepository : IPuzzlesRepository
    {
        protected IIndex<string, IPuzzleKind> PuzzleKinds { get; }
        protected DbPuzzlesRepository Repository { get; }

        public PuzzlesRepository(
            IIndex<string, IPuzzleKind> puzzleKinds,
            DbPuzzlesRepository dbPuzzleRepository)
        {
            PuzzleKinds = puzzleKinds;
            Repository = dbPuzzleRepository;
        }


        public Puzzle Add(Puzzle entity)
        {
            return Repository.Add(entity);
        }

        public Puzzle AddWord(int id, string word)
        {
            return Repository.AddWord(id, word);
        }

        public void Delete(int id)
        {
            Repository.Delete(id);
        }

        public Puzzle DeleteWord(int id, int wordId)
        {
            return Repository.DeleteWord(id, wordId);
        }

        public Puzzle Get(int id)
        {
            return GetAll().Where(p => p.Id == id).FirstOrDefault();
        }

        public IQueryable<Puzzle> GetAll()
        {
            var inMemoryPuzzleKinds = new[] { PuzzleKinds[RandomWordGenerator.StaticKey], PuzzleKinds[WordWordGenerator.StaticKey] };
            var rc = new List<Puzzle>(inMemoryPuzzleKinds.Where(k => !k.Features.HasSavedPuzzles).Select(k => k.Puzzle));
            var dbQ = Repository.GetAll();
            rc.AddRange(dbQ);
            return rc.AsQueryable();
        }

        public void SaveChanges()
        {
            Repository.SaveChanges();
        }

        public Puzzle Update(int id, Puzzle entity)
        {
            return Repository.Update(id, entity);
        }
    }
}