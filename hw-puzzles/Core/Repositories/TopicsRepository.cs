#pragma warning disable CS1572, CS1573, CS1591
using hwpuzzles.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace hwpuzzles.Core.Repositories
{
    /// <summary>
    /// Repository for topics
    /// </summary>
    public class TopicsRepository : ITopicsRepository
    {
        public void Add(string topic)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string topic)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<string> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
