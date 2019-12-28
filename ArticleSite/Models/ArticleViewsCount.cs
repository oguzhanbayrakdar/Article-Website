using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArticleSite.Models
{
    public class ArticleViewsCount
    {
        public int Id { get; set; }

        public int Count { get; set; }
        public DateTime Date { get; set; }

        public Guid ArticleRef { get; set; }
        public Article Article { get; set; }

    }
}
