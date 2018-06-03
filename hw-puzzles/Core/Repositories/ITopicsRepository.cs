#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using hwpuzzles.Core.Models;

namespace hwpuzzles.Core.Repositories
{
    public interface ITopicsRepository
    {
        /// <summary>
        /// Gets all the topics
        /// </summary>
        ICollection<string> GetAll();
        /// <summary>
        /// Adds a topic
        /// </summary>
        /// <param name="topic">topic</param>
        void Add(string topic);
        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="topic">topic</param>
        void Delete(string topic);        
    }
}
