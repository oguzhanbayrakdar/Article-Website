using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ArticleSite.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public AppUser User { get; set; }
        public DateTime CommentTime { get; set; }
        public Article Article { get; set; }
        public Comment ReplyTo { get; set; }
        public string CommentText { get; set; }

        public bool IsMain { get; set; }
        public string MainCommentId { get; set; }

    }
}
