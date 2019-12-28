using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Data.Abstract;
using ArticleSite.Data.Concrete;
using ArticleSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArticleSite.Controllers
{
    public class AdminController : Controller
    {

        private IArticleRepository _articleRepository;
        private ITagsRepository _tagsRepository;
        private ArticleDbContext _context;
        private ArticlesTagsRepository _articlesTagsRepository;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private UserManager<AppUser> _userManager;

        public AdminController(IArticleRepository articleRepository, ITagsRepository tagsRepository, ArticleDbContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager)
        {
            _articleRepository = articleRepository;
            _tagsRepository = tagsRepository;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;

            _articlesTagsRepository = new ArticlesTagsRepository(_context);
        }
        [Authorize(Roles = "Helper, Moderator, Administrator")]

        public IActionResult Index()
        {
            AdminArticleTagModel adminArticleTagModel = new AdminArticleTagModel();

            adminArticleTagModel.Tags = _tagsRepository.GetAll().ToList();
            adminArticleTagModel.Articles = _articleRepository.GetAll().ToList();

            return View(adminArticleTagModel);
        }

        #region Tag Region

        [HttpGet]
        [Authorize(Roles = "Moderator, Administrator")]
        public IActionResult EditTag(string id)
        {
            var tag = _tagsRepository.GetAll().FirstOrDefault(c => c.Id.ToString() == id);

            if (tag != null)
            {
                return Ok(tag);
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Moderator, Administrator")]
        public async Task<IActionResult> EditTag(string editTagName, string editTagId, IFormFile editTagFile)
        {

            if (ModelState.IsValid)
            {
                if (editTagName == null || editTagId == null) return BadRequest();

                if (_tagsRepository.GetAll().Count(c => c.Name == editTagName) <= 0) return BadRequest();

                var tag = _tagsRepository.GetAll().FirstOrDefault(c => c.Id.ToString() == editTagId);

                if (tag != null)
                {
                    if (editTagFile != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\icons", editTagFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await editTagFile.CopyToAsync(stream);

                            tag.IconName = editTagFile.FileName;
                        }
                    }

                    tag.Name = editTagName;
                    _tagsRepository.EditTag(tag);
                    _tagsRepository.SaveAll();

                    return RedirectToAction("Index");
                }

                return BadRequest();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator, Administrator")]
        public async Task<IActionResult> AddTag(Tag tag, IFormFile tagFile)
        {
            if (ModelState.IsValid)
            {
                if (tagFile != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\icons", tagFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await tagFile.CopyToAsync(stream);

                        tag.IconName = tagFile.FileName;
                    }
                }
                else
                {
                    tag.IconName = "placeholder-icon.png";
                }

                var query = _tagsRepository.GetAll().FirstOrDefault(c => c.Name == tag.Name);

                if (query != null)
                {
                    return BadRequest();
                }

                _tagsRepository.AddTag(tag);
                _tagsRepository.SaveAll();

                return Ok(tag);
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Moderator, Administrator")]
        public IActionResult DeleteTag(string id)
        {
            var tag = _tagsRepository.GetAll().FirstOrDefault(c => c.Id.ToString() == id);

            if (tag == null) return BadRequest();

            _tagsRepository.DeleteTag(id);
            _tagsRepository.SaveAll();

            return Ok();
        }

        #endregion

        #region Article Region

        [HttpGet]
        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public IActionResult EditArticle(string id)
        {
            var article = _articleRepository.GetAll().FirstOrDefault(c => c.Id.ToString() == id);

            if (article == null) return BadRequest();

            var articleTags = article.ArticlesTags.ToList();

            var tagIds = articleTags.Select(c => c.TagId).ToList();

            var allTags = _tagsRepository.GetAll().ToList();

            ViewBag.AllTags = allTags;
            ViewBag.CurrentTags = article.ArticlesTags.Select(c => c.Tag).ToList();


            AddArticleModel editAdminModel = new AddArticleModel
            {
                Id = article.Id,
                IsFeatured = article.IsFeatured,
                Name = article.Name,
                TagIds = tagIds,
                Text = article.Text
            };

            return View(editAdminModel);
        }

        [HttpPost]
        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public IActionResult EditArticle(AddArticleModel addArticleModel)
        {
            if (!ModelState.IsValid) return BadRequest(404);

            if (addArticleModel != null)
            {
                var article = _articleRepository.GetAll().FirstOrDefault(c => c.Id == addArticleModel.Id);

                if (article == null)
                {
                    return BadRequest(404);
                }
                var articleTags = new List<ArticlesTags>();

                foreach (var articleModelTagId in addArticleModel.TagIds)
                {
                    articleTags.Add(new ArticlesTags
                    {
                        Tag = _tagsRepository.GetAll().FirstOrDefault(c => c.Id == articleModelTagId),
                        TagId = articleModelTagId,
                        Article = article,
                        ArticleId = article.Id
                    });
                }

                var _article = new Article
                {
                    Id = article.Id,
                    Name = addArticleModel.Name,
                    Text = addArticleModel.Text,
                    AddingDate = article.AddingDate,
                    IsFeatured = addArticleModel.IsFeatured
                };

                _articleRepository.EditArticle(_article);
                var deleteArticlesTagsList = article.ArticlesTags.ToList();

                foreach (var item in deleteArticlesTagsList)
                {
                    _articlesTagsRepository.DeleteArticlesTags(item);
                    _context.SaveChanges();

                }

                foreach (var item in articleTags)
                {
                    _articlesTagsRepository.AddArticlesTags(item);
                    _context.SaveChanges();
                }

                _articleRepository.SaveAll();
            }

            return RedirectToAction("Index");
        }

        public bool ListComparer(List<ArticlesTags> list1, List<ArticlesTags> list2)
        {
            foreach (var l2 in list2)
            {
                if (list1.All(c => c.Tag.Name != l2.Tag.Name))
                {
                    return true;
                }
            }

            return false;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddArticle()
        {
            var tags = _tagsRepository.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Tags = new SelectList(tags, "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddArticle(AddArticleModel articleModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isArticleInDb = _articleRepository.GetAll().Any(c => c.Name == articleModel.Name);

            if (isArticleInDb) return BadRequest();

            var articleTags = new List<ArticlesTags>();
            if (articleModel.TagIds != null)
            {
                foreach (var articleModelTagId in articleModel.TagIds)
                {
                    articleTags.Add(new ArticlesTags
                    {
                        Tag = _tagsRepository.GetAll().FirstOrDefault(c => c.Id == articleModelTagId),
                        TagId = articleModelTagId
                    });
                }
            }
           
            var article = new Article
            {
                AddingDate = DateTime.Now,
                IsFeatured = articleModel.IsFeatured,
                Name = articleModel.Name,
                Text = articleModel.Text,
                ArticlesTags = articleTags
            };

            _articleRepository.AddArticle(article);
            var result = _articleRepository.SaveAll();

            var _article = _articleRepository.GetAll().FirstOrDefault(c => c.Name == article.Name);

            if (result == 1 && articleModel.TagIds != null)
            {
                foreach (var articleTag in articleTags)
                {
                    articleTag.Article = _article;
                    articleTag.ArticleId = _article.Id;

                    _articlesTagsRepository.AddArticlesTags(articleTag);
                    _articlesTagsRepository.SaveAll();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteArticle(string id)
        {
            var article = _articleRepository.GetAll().FirstOrDefault(c => c.Id.ToString() == id);

            if (article == null) return BadRequest();

            _articleRepository.DeleteArticle(id);
            _articleRepository.SaveAll();

            return Ok();

        }

        #endregion

        #region Role Region

        [HttpGet]
        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public IActionResult AddRole()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public async Task<IActionResult> AddRole(RoleModel roleModel)
        {
            if (!ModelState.IsValid) return View(roleModel);

            ViewBag.Roles = _roleManager.Roles.ToList();

            IdentityRole<Guid> role = new IdentityRole<Guid>
            {
                Name = roleModel.RoleName
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("AddRole");
            }

            foreach (var identityError in result.Errors)
            {
                ModelState.AddModelError(string.Empty, identityError.Description);
            }

            return View(roleModel);
        }

        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AddRole");
                }

                return BadRequest(404);
            }

            return BadRequest(404);
        }

        [HttpGet]
        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public async Task<IActionResult> EditRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                bool isInRole = await _userManager.IsInRoleAsync(user, role.Name);

                if (isInRole)
                    members.Add(user);
                else
                    nonMembers.Add(user);
            }

            ViewBag.Members = members;
            ViewBag.NonMembers = nonMembers;

            RoleModel model = new RoleModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public async Task<IActionResult> EditRole(RoleModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = _roleManager.Roles.FirstOrDefault(c => c.Id == roleModel.Id);

                if (role != null)
                {
                    role.Name = roleModel.RoleName;

                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("AddRole");
                    }
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public async Task<IActionResult> EditRoleAddMember(Guid roleId, Guid addToRoleId)
        {
            var isUserExists = await _userManager.FindByIdAsync(addToRoleId.ToString());
            if (isUserExists != null)
            {
                var user = await _userManager.FindByIdAsync(addToRoleId.ToString());
                var role = await _roleManager.FindByIdAsync(roleId.ToString());

                if (user != null && role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }
            return Redirect("editrole/" + roleId);
        }

        [HttpPost]
        [Authorize(Roles = "Helper, Moderator, Administrator")]
        public async Task<IActionResult> EditRoleRemoveMember(Guid roleId, Guid addToRoleId)
        {
            var isUserExists = await _userManager.FindByIdAsync(addToRoleId.ToString());
            if (isUserExists != null)
            {
                var user = await _userManager.FindByIdAsync(addToRoleId.ToString());
                var role = await _roleManager.FindByIdAsync(roleId.ToString());

                if (user != null && role != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return Redirect("editrole/" + roleId);
        }

        #endregion

        public IActionResult UserManagement()
        {
            ViewBag.Users = _userManager.Users;

            return View();
        }
    }
}