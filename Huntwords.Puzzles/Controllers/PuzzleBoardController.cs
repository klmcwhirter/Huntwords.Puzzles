#pragma warning disable CS1572, CS1573, CS1591
using System.Threading.Tasks;
using Huntwords.Common.Models;
using Huntwords.Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Huntwords.Puzzles.Controllers
{
    /// <summary>
    /// PuzzleBoardController
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/puzzleboard")]
    public class PuzzleBoardController : Controller
    {
        protected IPuzzleBoardRepository Repository { get; }
        public PuzzleBoardController(IPuzzleBoardRepository repository)
        {
            this.Repository = repository;
        }


        // GET api/puzzleboard/Fruit
        /// <summary>
        /// Gets a generated PuzzleBoard using the PuzzleId passed in
        /// </summary>
        /// <param name="name">Name of the Puzzle to generate</param>
        /// <returns>PuzzleBoard instance</returns>
        [HttpGet("{name}")]
        public PuzzleBoard Get(string name) => Repository.Pop(name);
    }
}
