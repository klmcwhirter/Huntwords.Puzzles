#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Huntwords.Common.Repositories;

namespace Huntwords.Puzzles.Controllers
{
    /// <summary>
    /// TopicsController
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/topics")]
    public class TopicsController : Controller
    {
        /// <summary>
        /// TopicRepository
        /// </summary>
        /// <returns>ITopicsRepository</returns>
        protected ITopicsRepository TopicsRepository { get; set; }

        /// <summary>
        /// Construct a TopicsController
        /// </summary>
        /// <param name="TopicRepository">ITopicsRepository</param>
        public TopicsController(ITopicsRepository TopicRepository)
        {
            TopicsRepository = TopicRepository;
        }

        // GET api/Topic
        /// <summary>
        /// Gets all Topic definitions
        /// </summary>
        /// <returns>List of Topic instances; empty List on exception</returns>
        [HttpGet]
        public ICollection<string> Get()
        {
            var rc = TopicsRepository.GetAll();
            return rc;
        }

        // POST api/Topic/
        /// <summary>
        /// Adds a Topic definition
        /// </summary>
        /// <param name="topic">Topic</param>
        /// <returns>Topic definition added</returns>
        [HttpPost]
        public ICollection<string> Post([FromBody]string topic)
        {
            TopicsRepository.Add(topic);
            var rc = TopicsRepository.GetAll();
            return rc;
        }

        // DELETE api/Topic/5
        /// <summary>
        /// Deletes a Topic definition
        /// </summary>
        /// <param name="topic">Topic to delete</param>
        /// <returns>List of Topics</returns>
        [HttpDelete("{topic}")]
        public ICollection<string> Delete(string topic)
        {
            TopicsRepository.Delete(topic);
            var rc = TopicsRepository.GetAll();
            return rc;
        }
    }
}
