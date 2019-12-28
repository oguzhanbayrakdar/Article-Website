using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Models;

namespace ArticleSite.Data.Abstract
{
    public interface IArticleRepository
    {
        IQueryable<Article> GetAll();

        void DeleteArticle(string articleId);
        void AddArticle(Article article);
        void EditArticle(Article article);
        int SaveAll();
        Task SaveAllAsync();
    }
}
