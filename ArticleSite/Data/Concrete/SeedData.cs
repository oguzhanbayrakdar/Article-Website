using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArticleSite.Data.Concrete
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ArticleDbContext>();
                
                //var context = app.ApplicationServices.GetRequiredService<ArticleDbContext>();

                //context.Database.Migrate();

                if (!context.Articles.Any())
                {
                    var tags = new[]
                    {
                    new Tag {Name = "Javascript"},
                    new Tag {Name = "C#"},
                    new Tag {Name = ".Net Core"},
                    new Tag {Name = "ReactJS"},
                    new Tag {Name = "Elastic Search"},
                    new Tag {Name = "Blazor"}
                };
                    context.Tags.AddRange(tags);

                    var articles = new[]
                    {
                    new Article {AddingDate = DateTime.Now.AddDays(-5), Name = "Javascript Article Title", Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget leo at velit imperdiet varius. In eu ipsum vitae velit congue iaculis vitae at risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae vehicula enim. Sed quis ante quis eros maximus dignissim a eu mi. Proin varius arcu metus."},
                    new Article {AddingDate = DateTime.Now.AddDays(-2), Name = "C# Article Title", Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget leo at velit imperdiet varius. In eu ipsum vitae velit congue iaculis vitae at risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae vehicula enim. Sed quis ante quis eros maximus dignissim a eu mi. Proin varius arcu metus."},
                    new Article {AddingDate = DateTime.Now.AddDays(-15), Name = ".Net Core Article Title", Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget leo at velit imperdiet varius. In eu ipsum vitae velit congue iaculis vitae at risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae vehicula enim. Sed quis ante quis eros maximus dignissim a eu mi. Proin varius arcu metus."},
                    new Article {AddingDate = DateTime.Now.AddDays(-225), Name = "ReactJS Article Title", Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget leo at velit imperdiet varius. In eu ipsum vitae velit congue iaculis vitae at risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae vehicula enim. Sed quis ante quis eros maximus dignissim a eu mi. Proin varius arcu metus."},
                    new Article {AddingDate = DateTime.Now.AddDays(-45), Name = "Elastic Search Article Title", Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget leo at velit imperdiet varius. In eu ipsum vitae velit congue iaculis vitae at risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae vehicula enim. Sed quis ante quis eros maximus dignissim a eu mi. Proin varius arcu metus."},
                    new Article {AddingDate = DateTime.Now.AddDays(-35), Name = "Blazor Article Title", Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget leo at velit imperdiet varius. In eu ipsum vitae velit congue iaculis vitae at risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae vehicula enim. Sed quis ante quis eros maximus dignissim a eu mi. Proin varius arcu metus."},
                    new Article {AddingDate = DateTime.Now.AddDays(-50), Name = "F# and .Net Core Article Title", Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget leo at velit imperdiet varius. In eu ipsum vitae velit congue iaculis vitae at risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae vehicula enim. Sed quis ante quis eros maximus dignissim a eu mi. Proin varius arcu metus."},
                    new Article {AddingDate = DateTime.Now.AddDays(-33), Name = "C# and .Net Core Article Title", Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget leo at velit imperdiet varius. In eu ipsum vitae velit congue iaculis vitae at risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae vehicula enim. Sed quis ante quis eros maximus dignissim a eu mi. Proin varius arcu metus."},
                };
                    context.Articles.AddRange(articles);

                    var articlesTags = new[]
                    {
                    new ArticlesTags { Article = articles[0], Tag = tags[0]},
                    new ArticlesTags { Article = articles[0], Tag = tags[1]},
                    new ArticlesTags { Article = articles[0], Tag = tags[2]},
                    new ArticlesTags { Article = articles[1], Tag = tags[2]},
                    new ArticlesTags { Article = articles[1], Tag = tags[3]},
                    new ArticlesTags { Article = articles[1], Tag = tags[1]},
                    new ArticlesTags { Article = articles[2], Tag = tags[5]},
                    new ArticlesTags { Article = articles[2], Tag = tags[4]},
                    new ArticlesTags { Article = articles[3], Tag = tags[4]},
                    new ArticlesTags { Article = articles[3], Tag = tags[3]},
                    new ArticlesTags { Article = articles[4], Tag = tags[2]},
                    new ArticlesTags { Article = articles[5], Tag = tags[1]},
                    new ArticlesTags { Article = articles[5], Tag = tags[0]},
                    new ArticlesTags { Article = articles[6], Tag = tags[2]},
                    new ArticlesTags { Article = articles[7], Tag = tags[4]},
                    new ArticlesTags { Article = articles[7], Tag = tags[1]},
                };

                    context.ArticlesTags.AddRange(articlesTags);

                    context.SaveChanges();
                }
            }
        }
    }
}
