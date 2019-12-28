using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ArticleSite.Components
{
    public class FeaturedPostsViewComponent:ViewComponent
    {
        private IArticleRepository _articleRepository;

        public FeaturedPostsViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IViewComponentResult Invoke(int number=2)
        {
            var result = _articleRepository.GetAll().Where(c => c.IsFeatured).Take(number);

            return View(result);
        }
    }
}
