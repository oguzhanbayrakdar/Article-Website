using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace ArticleSite.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime AddingDate { get; set; }
        public DateTime LatestUpdateDate { get; set; }
        public bool IsFeatured { get; set; }

        public virtual IList<ArticlesTags> ArticlesTags { get; set; }
        public virtual IList<ArticleViewsCount> ArticleViewsCounts { get; set; }

    }
}
