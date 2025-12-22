using Azure.Core;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class BlogDbContext:DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Article> _articles { get; set; }
        public virtual DbSet<ArticleQuestion> _articleQuestions { get; set; }

        public virtual DbSet<ArticleSummary> _articleSummaries { get; set; }
      
        public virtual DbSet<User> _users {  get; set; }
        public virtual DbSet<Category> _categories { get; set; }
        public virtual DbSet<Comment> _comments {  get; set; }
     
        public virtual DbSet<Like> _likes { get; set; }

         public virtual DbSet<GlobalChat> _globalChat {  get; set; }

        public virtual DbSet<GlobalMessage> _golbalMessages { get; set; }  
        public virtual DbSet<Persona> _userProfile { get; set; }


        public virtual DbSet<Notification> _notification { get; set; }

        public virtual DbSet<ArticleApproval> _articleApprovals { get; set; }

        public virtual DbSet<ArticleNotification> _articleNotifications { get; set; }
        protected  override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Article

            modelBuilder.Entity<Article>(a =>
            {
                a.ToTable("Articles").HasKey(t => t.ID);
                a.Property(t=>t.ID).ValueGeneratedOnAdd().HasColumnName("ID");
                a.Property(t => t.Title).HasColumnName("Title").HasColumnType("nvarchar(100)");
                a.Property(t => t.Content).HasColumnName("Content").HasColumnType("nvarchar(Max)");
                a.Property(t => t.IsPublished).HasColumnName("IsPublished").HasColumnType("bit");
                a.Property(t => t.CreatedAt).HasColumnName("CreatedDate").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
                a.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
                a.Property(t => t.ImageUrl).HasColumnName("Image").HasColumnType("nvarchar(100)");

                a.HasOne(t => t._category).WithMany(t => t._articles).HasForeignKey(t => t.categoryId).OnDelete(DeleteBehavior.ClientSetNull);
                a.Property(t => t.userID).HasColumnName("userId");
                a.Property(t => t.categoryId).HasColumnName("categoryId");
                a.HasOne(t => t._user).WithMany(t => t._articles).HasForeignKey(t => t.userID).OnDelete(DeleteBehavior.ClientSetNull);

            });

            modelBuilder.Entity<ArticleApproval>
                (
                a=> {
                    a.HasOne(n => n.Article).WithOne(n=>n.articleApproval).HasForeignKey<ArticleApproval>(x=>x._articleId).OnDelete(DeleteBehavior.Cascade);
                    a.HasOne(n => n.User).WithMany(n => n._articleApprovals).HasForeignKey(n => n._adminId).OnDelete(DeleteBehavior.ClientSetNull);


                }
                
                
                );


            modelBuilder.Entity<ArticleNotification>
                (
                a =>
                {
                    a.HasOne(n => n._article).WithMany(n => n._articleNotifications).HasForeignKey(n => n.articleId).OnDelete(DeleteBehavior.Cascade);
                    a.HasOne(n => n._user).WithMany(n => n._articleNotifications).HasForeignKey(n => n.userId).OnDelete(DeleteBehavior.ClientSetNull);
                }
                );

            modelBuilder.Entity<Notification>(entity =>
            {
              
                // 2️⃣ العلاقة مع المستلم
                entity.HasOne(n => n._receiver)
                      .WithMany(u => u.ReceivedNotifications)  // ⬅️ أضف هذا
                      .HasForeignKey(n => n.receiverId)
                      .OnDelete(DeleteBehavior.ClientSetNull);  // ⬅️ غير من Cascade إلى Restrict

                // 3️⃣ العلاقة مع الرسالة
                entity.HasOne(n => n._globalMessage)
                      .WithMany(m => m._notifications)  // ⬅️ أضف هذا
                      .HasForeignKey(n => n.MessageId)
                      .OnDelete(DeleteBehavior.ClientSetNull); 
                // ⬅️ أبق Cascade هنا
                entity.HasIndex(n => n.receiverId);
                entity.HasIndex(n => n.IsRead);
                entity.HasIndex(n => n.MessageId);
            });
            modelBuilder.Entity<GlobalChat>().HasData(new GlobalChat
            { ID = 2, Name = " GolbalChat For system", Description = "المحادثة المفتوحة لجميع المستخدمين" }
                           );

            modelBuilder.Entity<GlobalMessage>(


                ).HasOne(m => m._user).WithMany(t => t._Messages).HasForeignKey(m => m.senderId).OnDelete(DeleteBehavior.NoAction);

          

            modelBuilder.Entity<ArticleSummary>(a =>
            {
                a.ToTable("ArticleSummaries").HasKey(t => t.ID);
                a.Property(t => t.ID).HasColumnName("ID").ValueGeneratedOnAdd();
                a.Property(t => t.Summary).HasColumnName("Summary").HasColumnType("nvarchar(Max)");
                a.Property(t => t.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
                a.HasOne(t => t._article).WithOne(t => t.articleSummary).HasForeignKey<ArticleSummary>(t => t.articleId).
                OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Like>(a =>
            {
                a.ToTable("likes").HasKey(a => a.ID);

                a.Property(x=>x.ID).HasColumnName("ID").ValueGeneratedOnAdd();

                a.Property(t=> t.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
                a.HasOne(t=>t._user).WithOne(t=>t._like).HasForeignKey<Like>(t=>t.userId).OnDelete(DeleteBehavior.Cascade);
                a.HasOne(t=>t._article).WithMany(t=>t._likes).HasForeignKey(t=>t.articleId).OnDelete(DeleteBehavior.Cascade);
            });
          
            modelBuilder.Entity<ArticleQuestion>(a =>
            {
                a.ToTable("ArticleQuestions").HasKey(a => a.ID);
                a.Property(x=>x.ID).HasColumnName("ID").ValueGeneratedOnAdd();
                a.Property(x => x.Ansewr).HasColumnType("nvarchar(Max)").HasColumnName("Answer");
                a.Property(x => x.Question).HasColumnType("nvarchar(Max)").HasColumnName("Question");
                a.Property(x => x.AnsweredAt).HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
                a.Property(a=>a.CreatedAt).HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
                a.HasOne(a=>a._user).WithMany(a=>a._articleQuestions).HasForeignKey(a=>a.userId).OnDelete(DeleteBehavior.ClientSetNull);
                a.HasOne(a=>a._article).WithMany(a=>a._articleQuestions).HasForeignKey(a=>a.articleId).OnDelete(DeleteBehavior.Cascade);

            });






            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments").HasKey(t => t.ID);
                entity.Property(t => t.ID).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(t => t.Content).HasColumnName("content").HasColumnType("nvarchar(100)");
                entity.Property(t => t.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");

                entity.HasOne(t => t._article).WithMany(t => t._comments).HasForeignKey(t => t.articleID).OnDelete(DeleteBehavior.Cascade);

                entity.Property(t => t.articleID).HasColumnName("articleId");
                entity.HasOne(t => t._user).WithMany(t => t._comments).HasForeignKey(t => t.userID).OnDelete(DeleteBehavior.ClientSetNull);
                entity.Property(t => t.userID).HasColumnName("userId");

            });


            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories").HasKey(t => t.ID);
                entity.Property(t => t.ID).ValueGeneratedOnAdd().HasColumnName("ID");
                entity.Property(t => t.CategoryName).HasColumnType("nvarchar(100)").HasColumnName("CategoryName");
                entity.Property(t => t.Description).HasColumnType("nvarchar(100)").HasColumnName("Description");
                

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User").HasKey(t => t.ID);
                entity.Property(r => r.ID).HasColumnName("ID").ValueGeneratedOnAdd();

                entity.Property(t => t.UserName).HasColumnType("nvarchar(100)");
                entity.Property(t => t.Email).HasColumnName("Email").HasColumnType("nvarchar(100)");
                entity.Property(t => t.PasswordHash).HasColumnType("nvarchar(200)");
                entity.Property(t => t.Role).HasColumnType("nvarchar(100)").HasColumnName("Role");

            });


            modelBuilder.Entity<Persona>(e =>
            {
                e.ToTable("UserProfile").HasKey(t => t.ID);
                e.Property(t => t.ID).HasColumnName("Id").ValueGeneratedOnAdd();

                e.Property(t=>t.UserName).HasColumnType("nvarchar(100)");
                e.Property(e=>e.Bio).HasColumnType("nvarchar(100)");
                e.Property(r => r.Location).HasColumnType("nvarchar(100)");

                e.Property(r=>r.PhoneNumber).HasColumnType("nvarchar(100)");

                e.HasOne(r=>r._user).WithOne(e=>e._userProfile).HasForeignKey<Persona>(t=>t.userId).OnDelete(DeleteBehavior.ClientSetNull);
                





            });








        }

    }
}
