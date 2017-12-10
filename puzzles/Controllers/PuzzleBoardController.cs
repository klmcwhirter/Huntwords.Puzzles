using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Services;

namespace puzzles.Controllers
{
    /// <summary>
    /// PuzzleBoardController
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/puzzleboard")]
    public class PuzzleBoardController : Controller
    {
        /// <summary>
        /// PuzzleBoardCache
        /// </summary>
        /// <returns>PuzzleBoardCache</returns>
        protected PuzzleBoardCache PuzzleBoardCache { get; set; }

        /// <summary>
        /// Construct a PuzzleBoardController
        /// </summary>
        /// <param name="puzzleBoardCache">PuzzleBoardCache</param>
        public PuzzleBoardController(PuzzleBoardCache puzzleBoardCache)
        {
            PuzzleBoardCache = puzzleBoardCache;
        }

        // GET api/puzzleboard/3
        /// <summary>
        /// Gets a generated PuzzleBoard using the PuzzleId passed in
        /// </summary>
        /// <param name="id">Id of the Puzzle to generate</param>
        /// <returns>PuzzleBoard instance</returns>
        [HttpGet("{id}")]
        public PuzzleBoard Get(int id) => PuzzleBoardCache.Dequeue(id);
    }
}
