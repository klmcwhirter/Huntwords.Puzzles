using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Services;

namespace puzzles.Controllers
{
    [Route("api/[controller]")]
    public class PuzzleBoardController : Controller
    {
        protected PuzzleBoardCache PuzzleBoardCache { get; set; }

        public PuzzleBoardController(PuzzleBoardCache puzzleBoardCache)
        {
            PuzzleBoardCache = puzzleBoardCache;
        }

        // GET api/puzzleboard/3
        [HttpGet("{id}")]
        public PuzzleBoard Get(int id) => PuzzleBoardCache.Dequeue(id);
    }
}
