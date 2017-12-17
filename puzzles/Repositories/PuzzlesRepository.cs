using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using puzzles.Models;
using puzzles.Services;

namespace puzzles.Repositories
{
    /// <summary>
    /// Storage agnostic repository for puzzles
    /// </summary>
    public class PuzzlesRepository : IPuzzlesRepository
    {
        /// <summary>
        /// Accessor to get at the configured kinds of puzzles
        /// </summary>
        /// <returns></returns>
        protected IIndex<string, IPuzzleKind> PuzzleKinds { get; }
        /// <summary>
        /// DbRepositoryRepository to use
        /// </summary>
        /// <returns>DbPuzzlesRepository</returns>
        protected DbPuzzlesRepository Repository { get; }

        /// <summary>
        /// Construct a PuzzlesRepository
        /// </summary>
        /// <param name="puzzleKinds"></param>
        /// <param name="dbPuzzleRepository"></param>
        public PuzzlesRepository(
            IIndex<string, IPuzzleKind> puzzleKinds,
            DbPuzzlesRepository dbPuzzleRepository)
        {
            PuzzleKinds = puzzleKinds;
            Repository = dbPuzzleRepository;
        }

        /// <summary>
        /// Add a puzzle
        /// </summary>
        /// <param name="entity">Puzzle</param>
        /// <returns>Puzzle</returns>
        public Puzzle Add(Puzzle entity) => Repository.Add(entity);

        /// <summary>
        /// Add a word to a puzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        /// <param name="word"></param>
        /// <returns>Puzzle</returns>
        public Puzzle AddWord(int id, string word) => Repository.AddWord(id, word);

        /// <summary>
        /// Delete a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        public void Delete(int id) => Repository.Delete(id);

        /// <summary>
        /// Delete a word from a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        /// <param name="wordId"></param>
        /// <returns>Puzzle</returns>
        public Puzzle DeleteWord(int id, int wordId) => Repository.DeleteWord(id, wordId);

        /// <summary>
        /// Get a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        /// <returns>Puzzle</returns>
        public Puzzle Get(int id) => GetAll().Where(p => p.Id == id).FirstOrDefault();

        /// <summary>
        /// Get all puzzles
        /// </summary>
        /// <remarks>
        /// Joins in-memory puzzle definitions with db puzzle definitions and returns the combined list
        /// </remarks>
        /// <returns>IQueryable&lt;Puzzle&gt;</returns>
        public IQueryable<Puzzle> GetAll()
        {
            var inMemoryPuzzleKinds = new[] { PuzzleKinds[RandomWordGenerator.StaticKey], PuzzleKinds[WordWordGenerator.StaticKey] };
            var rc = new List<Puzzle>(inMemoryPuzzleKinds.Where(k => !k.Features.HasSavedPuzzles).Select(k => k.Puzzle));
            var dbQ = Repository.GetAll();
            rc.AddRange(dbQ);
            return rc.AsQueryable();
        }

        /// <summary>
        ///  Delegates to Repository to save any changes
        /// </summary>
        public void SaveChanges() => Repository.SaveChanges();

        /// <summary>
        /// Update a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        /// <param name="entity">Puzzle</param>
        /// <returns>Puzzle</returns>
        public Puzzle Update(int id, Puzzle entity)
        {
            UpdateWords(entity);
            return Repository.Update(id, entity);
        }

        /// <summary>
        /// Delegate to Repository.UpdateWords(entity)
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateWords(Puzzle entity) => Repository.UpdateWords(entity);
    }
}