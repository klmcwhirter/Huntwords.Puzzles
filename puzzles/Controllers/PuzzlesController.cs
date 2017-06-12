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
        protected IGenerator<Puzzle> PuzzleGenerator { get; set; }
        public ILogger<PuzzlesController> Logger { get; }

        public PuzzlesController(IPuzzlesRepository puzzleRepository, IGenerator<Puzzle> puzzleGenerator, ILogger<PuzzlesController> logger)
        {
            PuzzleRepository = puzzleRepository;
            PuzzleGenerator = puzzleGenerator;
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
        public void Post([FromBody]Puzzle puzzle)
        {
            PuzzleRepository.Add(puzzle);
            PuzzleRepository.SaveChanges();
        }

        // PUT api/puzzle
        [HttpPut]
        public void Put([FromBody]Puzzle puzzle)
        {
            PuzzleRepository.Update(puzzle.Id, puzzle);
            PuzzleRepository.SaveChanges();
        }

        // DELETE api/puzzle/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            PuzzleRepository.Delete(id);
            PuzzleRepository.SaveChanges();
        }


        // GET api/puzzle/kind/:kind
        [HttpGet("kind/{kind}/{id?}")]
        public Puzzle GetKind(string kind, int? id)
        {
            var rc = PuzzleGenerator.Generate(kind, id);
            return rc;
        }

        // GET api/puzzle/kinds
        [HttpGet("kinds")]
        public IPuzzleKind[] GetKinds()
        {
            var rc = (PuzzleGenerator as PuzzleGenerator)?.Kinds;
            return rc;
        }
    }
}
