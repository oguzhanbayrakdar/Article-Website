using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Models;

namespace ArticleSite.Data.Abstract
{
    public interface ICommentRepository
    {
        IQueryable<Comment> GetComments(Article article);

        Comment GetComment(string id);
        void DeleteComment(string id);
        void AddComment(Comment comment);

        int SaveAll();
    }
}
