using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using ArticleSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArticleSite.Components
{
    public class RecentPostsViewComponent:ViewComponent
    {
        private IArticleRepository articleRepository;
        
        public RecentPostsViewComponent(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        public IViewComponentResult Invoke()
        {
            var articles = articleRepository.GetAll().OrderByDescending(a => a.AddingDate).Take(6).ToList();

            return View(articles);
        }
    }
}
