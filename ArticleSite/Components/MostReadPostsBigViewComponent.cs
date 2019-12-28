using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ArticleSite.Components
{
    public class MostReadPostsBigViewComponent:ViewComponent
    {
        private IArticleRepository _articleRepository;

        public MostReadPostsBigViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IViewComponentResult Invoke(int number = 4)
        {
            var result = _articleRepository.GetAll().OrderBy(a => a.AddingDate).Take(number);

            return View(result);
        }
    }
}
