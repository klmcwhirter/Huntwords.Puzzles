using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Services;

namespace puzzles.Controllers
{
    [Route("api/[controller]")]
    public class PuzzlesController : Controller
    {
        protected IPuzzlesRepository PuzzleRepository { get; set; }
        public ILogger<PuzzlesController> Logger { get; }

        public PuzzlesController(IPuzzlesRepository puzzleRepository, ILogger<PuzzlesController> logger)
        {
            PuzzleRepository = puzzleRepository;
            Logger = logger;
        }

        // GET api/puzzle
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
        [HttpGet("{id}")]
        public Puzzle Get(int id)
        {
            var rc = PuzzleRepository.Get(id);
            return rc;
        }


        // POST api/puzzle
        [HttpPost]
        public Puzzle Post([FromBody]Puzzle puzzle)
        {
            var rc = PuzzleRepository.Add(puzzle);
            PuzzleRepository.SaveChanges();
            return rc;
        }

        // PUT api/puzzle
        [HttpPut]
        public Puzzle Put([FromBody]Puzzle puzzle)
        {
            var rc = PuzzleRepository.Update(puzzle.Id, puzzle);
            PuzzleRepository.SaveChanges();
            return rc;
        }

        // PUT api/puzzles/{id}/addWord
        [HttpPut("{id}/addWord")]
        public Puzzle AddWord(int id, [FromBody]PuzzleWord word)
        {
            var rc = PuzzleRepository.AddWord(id, word.Word);
            PuzzleRepository.SaveChanges();
            return rc;
        }

        // DELETE api/puzzles/{id}/deleteWord/{wordId}
        [HttpDelete("{id}/deleteWord/{wordId}")]
        public Puzzle DeleteWord(int id, int wordId)
        {
            var rc = PuzzleRepository.DeleteWord(id, wordId);
            PuzzleRepository.SaveChanges();
            return rc;
        }

        // DELETE api/puzzle/5
        [HttpDelete("{id}")]
        public IEnumerable<Puzzle> Delete(int id)
        {
            PuzzleRepository.Delete(id);
            PuzzleRepository.SaveChanges();
            return PuzzleRepository.GetAll();
        }
    }
}
