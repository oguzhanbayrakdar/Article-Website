using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleSite.Data.Concrete
{
    public class ArticleViewsCountRepository
    {
        private ArticleDbContext _context;

        public ArticleViewsCountRepository(ArticleDbContext context)
        {
            _context = context;
        }

        public void AddArticleViewCount(Article article)
        {
            var avc = _context.ArticleViewsCounts
                .FirstOrDefault(c => c.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && 
                                     c.Article.Id == article.Id);
            var avcViewCount = 0;

            ArticleViewsCount articleViewsCount = new ArticleViewsCount
            {
                Article = article,
                Date = DateTime.Now
            };

            if (avc != null)
            {
                avcViewCount = avc.Count + 1;

                articleViewsCount.Id = avc.Id;
                articleViewsCount.Count = avcViewCount;

                _context.Update(articleViewsCount);
            }
            else
            {
                articleViewsCount.Count = 1;
                
                _context.Entry(articleViewsCount).State = EntityState.Added;
            }

            _context.SaveChanges();
        }

        public void GetViewCountPerArticle()
        {
            //var articleViewCounts = _context.ArticleViewsCounts
            //    .Include(c=>c.Article)
            //    .GroupBy(g=>g.Count)
            //    .ToList();
            //var list = new List<ArticleViewsCount>();
            //var count = 0;


        }

    }
}
