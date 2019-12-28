using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using ArticleSite.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleSite.Data.Concrete
{
    public class ArticleRepository : IArticleRepository
    {
        private ArticleDbContext _context;

        public ArticleRepository(ArticleDbContext context)
        {
            _context = context;
        }

        public IQueryable<Article> GetAll()
        {

            var articles = _context.Articles
                .Include(c => c.ArticlesTags)
                    .ThenInclude(t => t.Tag).AsNoTracking();

            return articles;

        }

        public void DeleteArticle(string articleId)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id.ToString() == articleId);

            if (article != null)
            {
                _context.Entry(article).State = EntityState.Deleted;
            }
        }

        public void AddArticle(Article article)
        {
            if (article != null)
            {
                article.AddingDate = DateTime.Now;

                _context.Entry(article).State = EntityState.Added;
            }
        }


        public void EditArticle(Article article)
        {
            if (article != null)
            {
                article.LatestUpdateDate = DateTime.Now;
                var local = _context.Set<Article>()
                    .Local
                    .FirstOrDefault(entry => entry.Id.Equals(article.Id));

                // check if local is not null 
                if (local != null) // I'm using a extension method
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(article).State = EntityState.Modified;
            }
        }

        public int SaveAll()
        {
            return _context.SaveChanges();
        }

        public async Task SaveAllAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
