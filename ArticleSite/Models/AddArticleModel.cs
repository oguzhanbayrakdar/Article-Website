using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArticleSite.Models
{
    public class AddArticleModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<Guid> TagIds { get; set; }
        public bool IsFeatured { get; set; }

    }
}
