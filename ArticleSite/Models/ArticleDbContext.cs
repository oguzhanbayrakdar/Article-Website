using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArticleSite.Models
{
    public class ArticleDbContext:IdentityDbContext<AppUser,IdentityRole<Guid>,Guid>
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ArticlesTags>().HasKey(mb => new {mb.ArticleId, mb.TagId});
            //modelBuilder.Entity<Article>()
            //    .HasOne(a => a.ArticleViewsCount)
            //    .WithMany(b => b.Articles)
            //    .HasForeignKey<ArticleViewsCount>(b => b.ArticleRef);
            // https://www.learnentityframeworkcore.com/configuration/one-to-one-relationship-configuration

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticlesTags> ArticlesTags { get; set; }
        public DbSet<ArticleViewsCount> ArticleViewsCounts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
