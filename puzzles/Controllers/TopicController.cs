using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Services;

namespace puzzles.Controllers
{
    [Route("api/[controller]")]
    public class TopicsController : Controller
    {
        protected ITopicsRepository TopicRepository { get; set; }

        public TopicsController(ITopicsRepository topicRepository)
        {
            TopicRepository = topicRepository;
        }

        // GET api/topic
        [HttpGet]
        public IEnumerable<Topic> Get()
        {
            var rc = TopicRepository.GetAll();
            return rc;
        }

        // GET api/topic/5
        [HttpGet("{id}")]
        public Topic Get(int id)
        {
            var rc = TopicRepository.Get(id);
            return rc;
        }

        // POST api/topic
        [HttpPost]
        public IEnumerable<Topic> Post([FromBody]Topic topic)
        {
            TopicRepository.Add(topic);
            TopicRepository.SaveChanges();
            var rc = TopicRepository.GetAll();
            return rc;
        }

        // PUT api/topic/5
        [HttpPut("{id}")]
        public IEnumerable<Topic>  Put(int id, [FromBody]Topic topic)
        {
            TopicRepository.Update(id, topic);
            TopicRepository.SaveChanges();
            var rc = TopicRepository.GetAll();
            return rc;
        }

        // DELETE api/topic/5
        [HttpDelete("{id}")]
        public IEnumerable<Topic>  Delete(int id)
        {
            TopicRepository.Delete(id);
            TopicRepository.SaveChanges();
            var rc = TopicRepository.GetAll();
            return rc;
        }
    }
}
