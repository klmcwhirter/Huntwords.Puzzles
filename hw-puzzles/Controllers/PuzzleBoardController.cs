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
        public IGenerator<PuzzleBoard> PbGenerator { get; }
        
        public PuzzleBoardController(IGenerator<PuzzleBoard> pbGenerator)
        {
            PbGenerator = pbGenerator;
        }


        // GET api/puzzleboard/3
        /// <summary>
        /// Gets a generated PuzzleBoard using the PuzzleId passed in
        /// </summary>
        /// <param name="id">Id of the Puzzle to generate</param>
        /// <returns>PuzzleBoard instance</returns>
        [HttpGet("{id}")]
        public PuzzleBoard Get(int id) => PbGenerator.Generate(id, false);
    }
}
