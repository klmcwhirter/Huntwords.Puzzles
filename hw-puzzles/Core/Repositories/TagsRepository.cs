#pragma warning disable CS1572, CS1573, CS1591
using hwpuzzles.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace hwpuzzles.Core.Repositories
{
    /// <summary>
    /// Repository for tags
    /// </summary>
    public class TagsRepository : ITagsRepository
    {
        public void Add(string tag)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string tag)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<string> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
