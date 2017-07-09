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
            throw new NotImplementedException();
        }

        public Puzzle AddWord(int id, string word)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Puzzle DeleteWord(int id, int wordId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Puzzle Update(int id, Puzzle entity)
        {
            throw new NotImplementedException();
        }
    }
}