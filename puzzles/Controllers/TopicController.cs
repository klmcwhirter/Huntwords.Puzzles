#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Services;

namespace puzzles.Controllers
{
    /// <summary>
    /// TopicsController
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/topics")]
    public class TopicsController : Controller
    {
        /// <summary>
        /// TopicsRepository
        /// </summary>
        /// <returns>ITopicsRepository</returns>
        protected ITopicsRepository TopicRepository { get; set; }

        /// <summary>
        /// Construct TopicsController
        /// </summary>
        /// <param name="topicRepository"></param>
        public TopicsController(ITopicsRepository topicRepository)
        {
            TopicRepository = topicRepository;
        }

        // GET api/topic
        /// <summary>
        /// Gets all Topic definitions
        /// </summary>
        /// <returns>List of Topic instances; empty List on exception</returns>
        [HttpGet]
        public IEnumerable<Topic> Get()
        {
            var rc = TopicRepository.GetAll();
            return rc;
        }

        // GET api/topic/5
        /// <summary>
        /// Gets a Topic definition using the TopicId passed in
        /// </summary>
        /// <param name="id">Id of the Topic</param>
        /// <returns>Topic instance</returns>
        [HttpGet("{id}")]
        public Topic Get(int id)
        {
            var rc = TopicRepository.Get(id);
            return rc;
        }

        // POST api/topic
        /// <summary>
        /// Adds a Topic definition
        /// </summary>
        /// <param name="topic">Topic definition</param>
        /// <returns>Topic definition added</returns>
        [HttpPost]
        public IEnumerable<Topic> Post([FromBody]Topic topic)
        {
            TopicRepository.Add(topic);
            TopicRepository.SaveChanges();
            var rc = TopicRepository.GetAll();
            return rc;
        }

        // PUT api/topic/5
        /// <summary>
        /// Updates a Topic definition
        /// </summary>
        /// <param name="topic">Topic definition</param>
        /// <returns>Topic definition added</returns>
        [HttpPut("{id}")]
        public IEnumerable<Topic> Put(int id, [FromBody]Topic topic)
        {
            TopicRepository.Update(id, topic);
            TopicRepository.SaveChanges();
            var rc = TopicRepository.GetAll();
            return rc;
        }

        // DELETE api/topic/5
        /// <summary>
        /// Deletes a Topic definition
        /// </summary>
        /// <param name="id">id of Topic definition to delete</param>
        /// <returns>List of Topic definitions</returns>
        [HttpDelete("{id}")]
        public IEnumerable<Topic> Delete(int id)
        {
            TopicRepository.Delete(id);
            TopicRepository.SaveChanges();
            var rc = TopicRepository.GetAll();
            return rc;
        }
    }
}
