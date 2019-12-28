using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using ArticleSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArticleSite.Components
{
    public class TagsViewComponent : ViewComponent
    {
        private ITagsRepository _tagsRepository;

        public TagsViewComponent(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<Tag> tags = _tagsRepository.GetAll()
                .OrderByDescending(c => c.Id);

            //TODO: Makale sayısına göre büyükten küçüğe sırala

            return View(tags);
        }
    }
}
