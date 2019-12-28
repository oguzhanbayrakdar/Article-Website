using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using ArticleSite.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleSite.Data.Concrete
{
    public class CommentRepository: ICommentRepository
    {
        private ArticleDbContext _context;

        public CommentRepository(ArticleDbContext context)
        {
            _context = context;
        }

        public IQueryable<Comment>  GetComments(Article article)
        {
            var comments = _context.Comments
                .Include(i => i.User)
                .Include(t => t.Article)
                .Include(c=>c.ReplyTo)
                .Where(c => c.Article.Id == article.Id);

            return comments;
        }

        public Comment GetComment(string id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id.ToString() == id);
            if (comment !=null)
            {
                return comment;
            }
            return new Comment();
        }

        public void DeleteComment(string id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id.ToString() == id);

            if (comment != null)
            {
                _context.Entry(comment).State = EntityState.Deleted;
            }
        }

        public void AddComment(Comment comment)
        {
            comment.CommentTime = DateTime.Now;
            _context.Entry(comment).State = EntityState.Added;
        }

        public int SaveAll()
        {
            return _context.SaveChanges();
        }

    }
}
