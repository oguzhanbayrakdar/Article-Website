using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Models;

namespace ArticleSite.Data.Abstract
{
    public interface ITagsRepository
    {
        IQueryable<Tag> GetAll();

        void DeleteTag(string id);
        void EditTag(Tag tag);
        void AddTag(Tag tag);

        void SaveAll();
    }
}
