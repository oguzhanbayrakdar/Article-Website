using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;

namespace ArticleSite.Models
{
    public class AdminArticleTagModel
    {
        public List<Article> Articles { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
