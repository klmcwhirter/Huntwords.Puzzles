using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Services;

namespace puzzles.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        protected ITagsRepository TagsRepository { get; set; }

        public TagsController(ITagsRepository tagRepository)
        {
            TagsRepository = tagRepository;
        }

        // GET api/tag
        [HttpGet]
        public IEnumerable<Tag> Get()
        {
            var rc = TagsRepository.GetAll();
            return rc;
        }

        // GET api/tag/5
        [HttpGet("{id}")]
        public Tag Get(int id)
        {
            var rc = TagsRepository.Get(id);
            return rc;
        }

        // POST api/tag/
        [HttpPost]
        public void Post([FromBody]Tag tag)
        {
            TagsRepository.Add(tag);
            TagsRepository.SaveChanges();
        }

        // PUT api/tag/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Tag tag)
        {
            TagsRepository.Update(id, tag);
            TagsRepository.SaveChanges();
        }

        // DELETE api/tag/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            TagsRepository.Delete(id);
            TagsRepository.SaveChanges();
        }
    }
}
