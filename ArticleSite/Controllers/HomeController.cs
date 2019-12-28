using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using ArticleSite.Data.Concrete;
using ArticleSite.Models;
using ArticleSite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using Syncfusion.EJ2.Linq;

namespace ArticleSite.Controllers
{
    public class HomeController : Controller
    {
        private IArticleRepository _articleRepository;
        private ITagsRepository _tagsRepository;
        private ArticleDbContext _context;
        private ArticleViewsCountRepository _articleViewsCountRepository;
        private ICommentRepository _commentRepository;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public HomeController(IArticleRepository articleRepository, ITagsRepository tagsRepository,  ArticleDbContext context, ICommentRepository commentRepository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _articleRepository = articleRepository;
            _tagsRepository = tagsRepository;
            _context = context;
            _commentRepository = commentRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _articleViewsCountRepository = new ArticleViewsCountRepository(_context);
        }

        public IActionResult Index()
        {
            var featuredArticles = _articleRepository.GetAll().Where(c => c.IsFeatured).ToList();
            var articles = _articleRepository.GetAll().Take(7).Where(c => c.IsFeatured==false).ToList();

            var sumOfArticles = featuredArticles.Concat(articles).ToList();

            return View(sumOfArticles);
        }

        public IActionResult AllTags()
        {
            var result = _tagsRepository.GetAll().ToList();

            return View(result);
        }

        public IActionResult Article(string id)
        {
            var article = _articleRepository.GetAll().FirstOrDefault(c => c.Id.ToString() == id);

            if (article != null)
            {
                var comments = _commentRepository.GetComments(article)
                    .OrderBy(c => c.CommentTime).ToList();

                var mainComments = comments.Where(c => c.IsMain).ToList();

                ViewBag.Replies = comments.Where(c => c.IsMain == false).ToList();

                var articleComments = new ArticleCommentViewModel
                {
                    Article = article,
                    Comments = mainComments
                };

                _articleViewsCountRepository.AddArticleViewCount(article);
                return View(articleComments);
            }

            return BadRequest(404);
        }

        public IActionResult CategoryArticle(string id)
        {

            var articles = _articleRepository.GetAll();

            List<Article> result = new List<Article>();

            foreach (var article in articles)
            {
                foreach (var articleTag in article.ArticlesTags)
                {
                    if (articleTag.TagId.ToString()==id)
                    {
                        result.Add(article);           
                    }
                }
            }

            if (result.Any())
            {
                ViewData["TagName"] = _tagsRepository.GetAll().FirstOrDefault(c => c.Id.ToString() == id)?.Name;
                return View(result);
            }

            return BadRequest(404);
        }
        
        public IActionResult SearchResults(string query)
        {
            if (query!=null)
            {
                var result = _articleRepository.GetAll().Where(c => c.Name.Contains(query)).ToList();
                return View(result);

            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddComment(Comment comment, string articleId, string replyToInput)
        {

            if (!ModelState.IsValid) return BadRequest();

            var article = _articleRepository.GetAll().FirstOrDefault(c => c.Id.ToString() == articleId);
            var replyTo = _commentRepository.GetComment(replyToInput);
            var user = await GetCurrentUserAsync();

            comment.IsMain = comment.MainCommentId == null;

            if (article != null && !string.IsNullOrWhiteSpace(comment.CommentText))
            {
                comment.Article = article;
                comment.ReplyTo = replyTo;
                comment.User = user;
                if (string.IsNullOrEmpty(replyTo.CommentText))
                {
                    comment.ReplyTo = null;
                }
                
                _commentRepository.AddComment(comment);
                _commentRepository.SaveAll();
            }

            return Redirect("/Home/Article/"+articleId);
        }
    }
}