//using Domain.Entities;
//using Infrastructure;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using System;

//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;

//namespace Tests.E2E.ArticleTest
//{

//    public class CustomWebApplicationFactory<TProgram> :WebApplicationFactory<TProgram> where TProgram : class
//    {

//        protected override void ConfigureWebHost(IWebHostBuilder builder)
//        {
//            builder.ConfigureServices(services =>
//            {
//                // 1️⃣ إزالة BlogDbContext الحالي (إذا كان موجوداً)
//                var dbContextDescriptor = services.SingleOrDefault(
//                    d => d.ServiceType == typeof(DbContextOptions<BlogDbContext>)); // ✅ BlogDbContext

//                if (dbContextDescriptor != null)
//                    services.Remove(dbContextDescriptor);

//                // 2️⃣ إضافة BlogDbContext مع In-Memory Database
//                services.AddDbContext<BlogDbContext>(options =>  // ✅ BlogDbContext
//                {
//                    options.UseInMemoryDatabase($"E2ETestDb_{Guid.NewGuid()}");
//                    options.EnableSensitiveDataLogging();
//                    options.EnableDetailedErrors();
//                });


//                // في CustomWebApplicationFactory
//                services.AddAuthentication("Test")
//                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
//                    "Test", options => { });
//                // 3️⃣ Build Service Provider
//                var sp = services.BuildServiceProvider();

//                // 4️⃣ إنشاء scope وتهيئة database
//                using (var scope = sp.CreateScope())
//                {
//                    var scopedServices = scope.ServiceProvider;
//                    var db = scopedServices.GetRequiredService<BlogDbContext>(); // ✅ BlogDbContext

//                    db.Database.EnsureCreated();

//                    // 5️⃣ تهيئة البيانات الأساسية
//                    SeedTestData(db);
//                }
//            });
//        }

//        private void SeedTestData(BlogDbContext context)  // ✅ BlogDbContext
//        {
//            // فقط إذا كانت البيانات غير موجودة
//            if (!context._users.Any())  // ✅ استخدم _users بدلاً من Users
//            {
//                // إنشاء مستخدمين اختبارين
//                context._users.AddRange(  // ✅ _users
//                    new User
//                    {
//                        ID = 1,
//                        UserName = "testauthor",
//                        Email = "author@test.com",
//                        PasswordHash = "hashed_password",
//                        Role = "Author"
//                    },
//                    new User
//                    {
//                        ID = 2,
//                        UserName = "testuser",
//                        Email = "user@test.com",
//                        PasswordHash = "hashed_password",
//                        Role = "User"
//                    }
//                );
//            }

//            if (!context._categories.Any())  // ✅ _categories
//            {
//                context._categories.AddRange(  // ✅ _categories
//                    new Category { ID = 1, CategoryName = "Technology" },
//                    new Category { ID = 2, CategoryName = "Science" }
//                );
//            }

//            context.SaveChanges();
//        }




//    }


//// إنشاء TestAuthHandler
//public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//{
//    public TestAuthHandler(
//        IOptionsMonitor<AuthenticationSchemeOptions> options,
//        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
//        : base(options, logger, encoder, clock) { }

//    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
//    {
//        var claims = new[] {
//            new Claim(ClaimTypes.NameIdentifier, "1"),
//            new Claim(ClaimTypes.Name, "test_user"),
//            new Claim(ClaimTypes.Role, "User")
//        };
//        var identity = new ClaimsIdentity(claims, "Test");
//        var principal = new ClaimsPrincipal(identity);
//        var ticket = new AuthenticationTicket(principal, "Test");

//        return Task.FromResult(AuthenticateResult.Success(ticket));
//    }
//}



//}
using Castle.Core.Logging;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ArticleE2ETests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. إزالة DbContext الحالي
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BlogDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // 2. إضافة InMemory Database
                services.AddDbContext<BlogDbContext>(options =>
                {
                    options.UseInMemoryDatabase("E2E_Test_DB");
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                });

                // 3. إضافة مصادقة للاختبارات
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "Test", options => { });

                // 4. Build Service Provider
                var sp = services.BuildServiceProvider();

                // 5. تهيئة قاعدة البيانات
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            });
        }
    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, (Microsoft.Extensions.Logging.ILoggerFactory)logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // إنشاء claims لمستخدم اختباري
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "test_author"),
                new Claim(ClaimTypes.Role, "Writer")  // ⚠️ مهم: يجب أن يكون Writer لإنشاء مقالات
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}