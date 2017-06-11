using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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

        public PuzzlesController(IPuzzlesRepository puzzleRepository, IGenerator<Puzzle> puzzleGenerator)
        {
            PuzzleRepository = puzzleRepository;
            PuzzleGenerator = puzzleGenerator;
        }

        // GET api/puzzle
        [HttpGet]
        public IEnumerable<Puzzle> Get()
        {
            var rc = PuzzleRepository.GetAll();
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
