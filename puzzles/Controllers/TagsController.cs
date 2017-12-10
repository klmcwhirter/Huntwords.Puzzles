using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Services;

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
        public IEnumerable<Tag> Get()
        {
            var rc = TagsRepository.GetAll();
            return rc;
        }

        // GET api/tag/5
        /// <summary>
        /// Gets a Tag definition using the TagId passed in
        /// </summary>
        /// <param name="id">Id of the Tag</param>
        /// <returns>Tag instance</returns>
        [HttpGet("{id}")]
        public Tag Get(int id)
        {
            var rc = TagsRepository.Get(id);
            return rc;
        }

        // POST api/tag/
        /// <summary>
        /// Adds a Tag definition
        /// </summary>
        /// <param name="tag">Tag definition</param>
        /// <returns>Tag definition added</returns>
        [HttpPost]
        public Tag[] Post([FromBody]Tag tag)
        {
            TagsRepository.Add(tag);
            TagsRepository.SaveChanges();
            var rc = TagsRepository.GetAll();
            return rc.ToArray();
        }

        // PUT api/tag/5
        /// <summary>
        /// Updates a Tag definition
        /// </summary>
        /// <param name="tag">Tag definition</param>
        /// <returns>Tag definition added</returns>
        [HttpPut("{id}")]
        public IEnumerable<Tag> Put(int id, [FromBody]Tag tag)
        {
            TagsRepository.Update(id, tag);
            TagsRepository.SaveChanges();
            var rc = TagsRepository.GetAll();
            return rc.ToArray();
        }

        // DELETE api/tag/5
        /// <summary>
        /// Deletes a Tag definition
        /// </summary>
        /// <param name="id">id of Tag definition to delete</param>
        /// <returns>List of Tag definitions</returns>
        [HttpDelete("{id}")]
        public IEnumerable<Tag> Delete(int id)
        {
            TagsRepository.Delete(id);
            TagsRepository.SaveChanges();
            var rc = TagsRepository.GetAll();
            return rc.ToArray();
        }
    }
}
