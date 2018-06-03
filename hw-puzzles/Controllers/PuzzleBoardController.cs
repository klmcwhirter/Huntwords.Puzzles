using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hwpuzzles.Core.Models;

namespace hwpuzzles.Controllers
{
    /// <summary>
    /// PuzzleBoardController
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/puzzleboard")]
    public class PuzzleBoardController : Controller
    {
        // GET api/puzzleboard/3
        /// <summary>
        /// Gets a generated PuzzleBoard using the PuzzleId passed in
        /// </summary>
        /// <param name="id">Id of the Puzzle to generate</param>
        /// <returns>PuzzleBoard instance</returns>
        [HttpGet("{id}")]
        public PuzzleBoard Get(int id) => null;
    }
}
