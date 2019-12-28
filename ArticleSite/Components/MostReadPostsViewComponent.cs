using System.Linq;
using ArticleSite.Data.Abstract;
using ArticleSite.Data.Concrete;
using ArticleSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArticleSite.Components
{
    public class MostReadPostsViewComponent:ViewComponent
    {
        private IArticleRepository _articleRepository;
        private ArticleViewsCountRepository _articleViewsCountRepository;
        private ArticleDbContext _context;
        public MostReadPostsViewComponent(IArticleRepository articleRepository, ArticleDbContext context)
        {
            _articleRepository = articleRepository;
            _context = context;
            _articleViewsCountRepository = new ArticleViewsCountRepository(_context);
        }

        public IViewComponentResult Invoke(int number=4)
        {
            
            var result = _articleRepository.GetAll().OrderBy(a => a.AddingDate).Take(number);
            //TODO: Tık sayılarını alabilirsen burayı değiştirirsin.
            return View(result);
        }
    }
}
