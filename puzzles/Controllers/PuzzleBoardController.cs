using Microsoft.AspNetCore.Mvc;
using puzzles.Models;
using puzzles.Repositories;

namespace puzzles.Controllers
{
    [Route("api/[controller]")]
    public class PuzzleBoardController : Controller
    {
        protected IGenerator<PuzzleBoard> PuzzleBoardGenerator { get; set; }
        protected IPuzzlesRepository PuzzleRepository { get; set; }

        public PuzzleBoardController(
            IGenerator<PuzzleBoard> puzzleBoardGenerator,
            IPuzzlesRepository puzzleRepository
        )
        {
            PuzzleBoardGenerator = puzzleBoardGenerator;
            PuzzleRepository = puzzleRepository;
        }

        // GET api/puzzleboard/3
        [HttpGet("{id}")]
        public PuzzleBoard Get(int id)
        {
            var puzzle = PuzzleRepository.Get(id);
            var rc = PuzzleBoardGenerator.Generate(puzzle);
            return rc;
        }
    }
}
