using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Huntwords.Common.Models;

namespace Huntwords.Puzzles.Controllers
{
    /// <summary>
    /// PuzzlesController
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/puzzles")]
    public class PuzzlesController : Controller
    {
        /// <summary>
        /// PuzzlesRepository
        /// </summary>
        /// <returns>IPuzzlesRepository</returns>
        protected IPuzzlesRepository PuzzleRepository { get; set; }
        /// <summary>
        /// Logger
        /// </summary>
        /// <returns>ILogger&lt;PuzzlesController&gt;</returns>
        protected ILogger<PuzzlesController> Logger { get; }

        /// <summary>
        /// Construct a PuzzlesController
        /// </summary>
        /// <param name="puzzleRepository">IPuzzlesRepository</param>
        /// <param name="logger">ILogger</param>
        public PuzzlesController(IPuzzlesRepository puzzleRepository, ILogger<PuzzlesController> logger)
        {
            PuzzleRepository = puzzleRepository;
            Logger = logger;
        }

        // GET api/puzzle
        /// <summary>
        /// Gets all Puzzle definitions
        /// </summary>
        /// <returns>List of Puzzle instances; empty List on exception</returns>
        [HttpGet]
        public IEnumerable<Puzzle> Get()
        {
            IEnumerable<Puzzle> rc = new List<Puzzle>();
            try
            {
                var localrc = PuzzleRepository.GetAll();
                rc = localrc.ToList();
            }
            catch (Exception e)
            {
                Logger.LogError(e, string.Empty);

                // In case of exception return empty List
            }
            return rc;
        }

        // GET api/puzzle/5
        /// <summary>
        /// Gets a Puzzle definition using the PuzzleId passed in
        /// </summary>
        /// <param name="name">Name of the Puzzle</param>
        /// <returns>Puzzle instance</returns>
        [HttpGet("{name}")]
        public Puzzle Get(string name)
        {
            var rc = PuzzleRepository.Get(name);
            return rc;
        }


        // POST api/puzzle
        /// <summary>
        /// Adds a Puzzle definition
        /// </summary>
        /// <param name="puzzle">Puzzle definition</param>
        /// <returns>Puzzle definition added</returns>
        [HttpPost]
        public Puzzle Post([FromBody]Puzzle puzzle)
        {
            var rc = PuzzleRepository.Add(puzzle);
            return rc;
        }

        // PUT api/puzzle
        /// <summary>
        /// Updates a Puzzle definition
        /// </summary>
        /// <param name="puzzle">Puzzle definition</param>
        /// <returns>Puzzle definition added</returns>
        [HttpPut]
        public Puzzle Put([FromBody]Puzzle puzzle)
        {
            var rc = PuzzleRepository.Update(puzzle.Name, puzzle);
            return rc;
        }

        // PUT api/puzzles/{id}/words
        /// <summary>
        /// Adds a Puzzle word
        /// </summary>
        /// <param name="name">name of Puzzle definition into which to add word</param>
        /// <param name="word">Puzzle word</param>
        /// <returns>Puzzle modified</returns>
        [HttpPut("{id}/words")]
        public Puzzle AddWord(string name, [FromBody]string word)
        {
            var rc = PuzzleRepository.AddWord(name, word);
            return rc;
        }

        // DELETE api/puzzles/{id}/words/{wordId}
        /// <summary>
        /// Deletes a Puzzle word
        /// </summary>
        /// <param name="name">name of Puzzle definition from which to delete word</param>
        /// <param name="word">word to delete</param>
        /// <returns>Puzzle modified</returns>
        [HttpDelete("{name}/words/{word}")]
        public Puzzle DeleteWord(string name, string word)
        {
            var rc = PuzzleRepository.DeleteWord(name, word);
            return rc;
        }

        // DELETE api/puzzle/5
        /// <summary>
        /// Deletes a Puzzle definition
        /// </summary>
        /// <param name="name">name of Puzzle definition to delete</param>
        /// <returns>List of Puzzle definitions</returns>
        [HttpDelete("{name}")]
        public IEnumerable<Puzzle> Delete(string name)
        {
            PuzzleRepository.Delete(name);
            return PuzzleRepository.GetAll();
        }
    }
}
