using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using ArticleSite.Data.Concrete;
using ArticleSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArticleSite.Components
{
    public class TopMenuTags: ViewComponent
    {
        private ITagsRepository _tagsRepository;
        public TopMenuTags(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        public IViewComponentResult Invoke(int count)
        {
            var tags = _tagsRepository.GetAll().Take(count).ToList();
            if (tags.Any())
            {
                return View(tags);
            }
            //TODO: buraya bir şeyler yap null dönmemeli.
            return null;
        }

    }
}
