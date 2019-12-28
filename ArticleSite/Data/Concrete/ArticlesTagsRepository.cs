using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleSite.Data.Concrete
{
    public class ArticlesTagsRepository
    {
        private ArticleDbContext _context;

        public ArticlesTagsRepository(ArticleDbContext context)
        {
            _context = context;
        }

        public void AddArticlesTags(ArticlesTags articlesTags)
        {
            _context.Entry<ArticlesTags>(articlesTags).State = EntityState.Added;

        }

        public void DeleteArticlesTags(ArticlesTags articlesTags)
        {
            _context.Entry<ArticlesTags>(articlesTags).State = EntityState.Deleted;
        }

        public void SaveAll()
        {
            _context.SaveChanges();
        }

        public void SaveAllAsync()
        {
            _context.SaveChangesAsync();
        }

    }

}
