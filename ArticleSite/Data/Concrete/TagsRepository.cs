using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using ArticleSite.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleSite.Data.Concrete
{
    public class TagsRepository:ITagsRepository
    {
        private ArticleDbContext _context;

        public TagsRepository(ArticleDbContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> GetAll()
        {
            return _context.Tags
                .Include(c=>c.ArticlesTags)
                    .ThenInclude(t=>t.Article);
        }

        public void DeleteTag(string id)
        {
            var tag = _context.Tags.FirstOrDefault(t=>t.Id.ToString()==id);
            if (tag!=null)
            {
                _context.Tags.Remove(tag);
            }
        }

        public void EditTag(Tag tag)
        {
            _context.Entry(tag).State = EntityState.Modified;
        }

        public void AddTag(Tag tag)
        {
            if (tag != null)
            {
                _context.Tags.Add(tag);
            }
        }

        public void SaveAll()
        {
            _context.SaveChanges();
        }
    }
}
