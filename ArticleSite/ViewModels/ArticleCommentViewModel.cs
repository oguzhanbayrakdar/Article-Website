using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Models;

namespace ArticleSite.ViewModels
{
    public class ArticleCommentViewModel
    {
        public Article Article { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
