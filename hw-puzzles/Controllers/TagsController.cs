#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using hwpuzzles.Core.Repositories;

namespace puzzles.Controllers
{
    /// <summary>
    /// TagsController
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/tags")]
    public class TagsController : Controller
    {
        /// <summary>
        /// TagRepository
        /// </summary>
        /// <returns>ITagsRepository</returns>
        protected ITagsRepository TagsRepository { get; set; }

        /// <summary>
        /// Construct a TagsController
        /// </summary>
        /// <param name="tagRepository">ITagsRepository</param>
        public TagsController(ITagsRepository tagRepository)
        {
            TagsRepository = tagRepository;
        }

        // GET api/tag
        /// <summary>
        /// Gets all Tag definitions
        /// </summary>
        /// <returns>List of Tag instances; empty List on exception</returns>
        [HttpGet]
        public ICollection<string> Get()
        {
            var rc = TagsRepository.GetAll();
            return rc;
        }

        // POST api/tag/
        /// <summary>
        /// Adds a Tag definition
        /// </summary>
        /// <param name="tag">Tag definition</param>
        /// <returns>Tag definition added</returns>
        [HttpPost]
        public ICollection<string> Post([FromBody]string tag)
        {
            TagsRepository.Add(tag);
            var rc = TagsRepository.GetAll();
            return rc;
        }

        // DELETE api/tag/5
        /// <summary>
        /// Deletes a Tag definition
        /// </summary>
        /// <param name="tag">tag to delete</param>
        /// <returns>List of tags</returns>
        [HttpDelete("{tag}")]
        public ICollection<string> Delete(string tag)
        {
            TagsRepository.Delete(tag);
            var rc = TagsRepository.GetAll();
            return rc;
        }
    }
}
