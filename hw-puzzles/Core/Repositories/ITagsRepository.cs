#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using hwpuzzles.Core.Models;

namespace hwpuzzles.Core.Repositories
{
    public interface ITagsRepository
    {
        /// <summary>
        /// Gets all the tags
        /// </summary>
        ICollection<string> GetAll();
        /// <summary>
        /// Adds a tag
        /// </summary>
        /// <param name="tag">tag</param>
        void Add(string tag);
        /// <summary>
        /// Deletes a tag
        /// </summary>
        /// <param name="tag">tag</param>
        void Delete(string tag);        
    }
}
