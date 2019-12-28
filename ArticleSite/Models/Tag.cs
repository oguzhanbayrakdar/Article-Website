using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ArticleSite.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string IconName { get; set; }

        public virtual IList<ArticlesTags> ArticlesTags { get; set; }
    }
}
