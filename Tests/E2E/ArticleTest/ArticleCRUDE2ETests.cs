//using Applicaion.Dto.ArticleDto;
//using Application.Serializer;
//using Domain.Entities;
//using Infrastructure;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
//using System;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http.Json;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Tests.E2E.ArticleTest
//{

//    namespace E2ETests.Features
//    {
//        [TestFixture]
//        public class ArticleCRUDE2ETests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
//        {
//            // 📦 المتغيرات الأساسية - معدلة
//            private CustomWebApplicationFactory<Program> _factory;
//            private HttpClient _client;
//            private BlogDbContext _context;  // ✅ BlogDbContext بدلاً من ApplicationDbContext
//            private int _testCategoryId = 1;
//            private int _createdArticleId;
//            private bool disposedValue;

//            public async Task InitializeAsync()
//            {
//                TestContext.WriteLine("🚀 بداية تهيئة اختبار E2E...");

//                // 1. إنشاء Factory
//                _factory = new CustomWebApplicationFactory<Program>();

//                // 2. إنشاء HttpClient
//                _client = _factory.CreateClient();

//                // 3. الحصول على BlogDbContext من Factory
//                using var scope = _factory.Services.CreateScope();
//                _context = scope.ServiceProvider.GetRequiredService<BlogDbContext>(); // ✅ BlogDbContext

//                // 4. التحقق من وجود البيانات
//                await EnsureTestDataExists();

//                TestContext.WriteLine("✅ التهيئة اكتملت بنجاح!");
//            }

//            private async Task EnsureTestDataExists()
//            {
//                // التحقق من وجود التصنيفات
//                if (!await _context._categories.AnyAsync())  // ✅ _categories
//                {
//                    _context._categories.Add(new Category   // ✅ _categories
//                    {
//                        ID = 1,
//                        CategoryName = "Technology"
//                    });
//                    await _context.SaveChangesAsync();
//                }

//                // نستخدم التصنيف الأول المتوفر
//                var category = await _context._categories.FirstAsync();  // ✅ _categories
//                _testCategoryId = category.ID;
//            }

//            // ==================== TEST 1: CREATE ARTICLE ====================
//            [Test]
//            [Order(1)]
//            public async Task InsertArticle_ValidData_ShouldReturnCreatedArticle()
//            {
//                TestContext.WriteLine("🎬 الاختبار 1: إنشاء مقال جديد...");

//                // 🎯 Arrange: تحضير البيانات
//                var createDto = new CrArticleDto
//                {
//                    Title = "E2E Test Article - إنشاء",
//                    Content = "هذا مقال اختباري يتم إنشاؤه عبر E2E Test",
//                    categoryId = _testCategoryId,
//                    IsPublished = true,
//                    Image = null  // في الاختبار يمكننا تجاهل الصورة
//                };

//                TestContext.WriteLine($"📝 البيانات: {createDto.Title}");

//                // 🚀 Act: إرسال الطلب إلى API
//                // ✅ ملاحظة: الـ Endpoint هو /api/Article/InsertArticle (ليس /api/articles)
//                var response = await _client.PostAsJsonAsync("/api/Article/InsertArticle", createDto);
//                TestContext.WriteLine($"📤 تم إرسال POST إلى /api/Article/InsertArticle");
//                TestContext.WriteLine($"📥 الاستجابة: {response.StatusCode}");

//                // ✅ Assert: التحقق من النتائج
//                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),  // ✅ يرجع 200 OK
//                    $"متوقع OK (200) لكن حصلنا على {response.StatusCode}");

//                // قراءة الاستجابة كـ ApiResponse
//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();
//                Assert.That(apiResponse, Is.Not.Null, "يجب أن لا تكون الاستجابة فارغة");
//                Assert.That(apiResponse.Result, Is.True, "يجب أن تكون العملية ناجحة");
//                Assert.That(apiResponse.Data, Is.Not.Null, "يجب أن تحتوي على بيانات المقال");

//                var createdArticle = apiResponse.Data;
//                Assert.That(createdArticle.ID, Is.GreaterThan(0), "يجب أن يكون للمقال ID");
//                Assert.That(createdArticle.Title, Is.EqualTo(createDto.Title));

//                _createdArticleId = createdArticle.ID;
//                TestContext.WriteLine($"✅ تم إنشاء المقال بنجاح! ID: {_createdArticleId}");

//                // 🔍 التحقق من قاعدة البيانات
//                var dbArticle = await _context._articles  // ✅ _articles
//                    .Include(a => a._category)
//                    .Include(a => a._user)
//                    .FirstOrDefaultAsync(a => a.ID == _createdArticleId);

//                Assert.That(dbArticle, Is.Not.Null, "يجب أن يوجد المقال في قاعدة البيانات");
//                Assert.That(dbArticle.Title, Is.EqualTo(createDto.Title));
//                Assert.That(dbArticle.IsPublished, Is.True);
//            }

//            // ==================== TEST 2: GET ARTICLE BY ID ====================
//            [Test]
//            [Order(2)]
//            public async Task GetArticleByID_ExistingArticle_ShouldReturnArticle()
//            {
//                TestContext.WriteLine($"🎬 الاختبار 2: قراءة المقال (ID: {_createdArticleId})...");

//                // 🚀 Act
//                // ✅ ملاحظة: الـ Endpoint هو /api/Article/GetArticleByID مع query parameter
//                var response = await _client.GetAsync($"/api/Article/GetArticleByID?Id={_createdArticleId}");
//                TestContext.WriteLine($"📤 تم إرسال GET إلى /api/Article/GetArticleByID?Id={_createdArticleId}");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);
//                Assert.That(apiResponse.Data, Is.Not.Null);

//                var article = apiResponse.Data;
//                Assert.That(article.ID, Is.EqualTo(_createdArticleId));
//                Assert.That(article.Title, Contains.Substring("E2E Test Article"));

//                TestContext.WriteLine($"✅ تم قراءة المقال بنجاح!");
//            }

//            // ==================== TEST 3: GET ALL ARTICLES ====================
//            [Test]
//            [Order(3)]
//            public async Task GetAllArticle_ShouldIncludeCreatedArticle()
//            {
//                TestContext.WriteLine("🎬 الاختبار 3: الحصول على جميع المقالات...");

//                // 🚀 Act
//                var response = await _client.GetAsync("/api/Article/GetAllArticle");
//                TestContext.WriteLine("📤 تم إرسال GET إلى /api/Article/GetAllArticle");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);
//                Assert.That(apiResponse.Data, Is.Not.Null);

//                var articles = apiResponse.Data;
//                Assert.That(articles, Has.Count.GreaterThan(0));

//                var ourArticle = articles.FirstOrDefault(a => a.ID == _createdArticleId);
//                Assert.That(ourArticle, Is.Not.Null,
//                    "يجب أن يظهر مقالنا في القائمة العامة");

//                TestContext.WriteLine($"✅ عدد المقالات: {articles.Count}");
//            }

//            // ==================== TEST 4: FILTER BY CATEGORY ====================
//            [Test]
//            [Order(4)]
//            public async Task GetArticleByCategoryName_ShouldReturnArticlesInCategory()
//            {
//                TestContext.WriteLine($"🎬 الاختبار 4: تصفية حسب التصنيف (ID: {_testCategoryId})...");

//                // 🚀 Act
//                var response = await _client.GetAsync($"/api/Article/GetArticleByCategoryName?id={_testCategoryId}");
//                TestContext.WriteLine($"📤 تم إرسال GET إلى /api/Article/GetArticleByCategoryName?id={_testCategoryId}");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);
//                Assert.That(apiResponse.Data, Is.Not.Null);

//                var articles = apiResponse.Data;

//                var ourArticle = articles.FirstOrDefault(a => a.ID == _createdArticleId);
//                Assert.That(ourArticle, Is.Not.Null,
//                    "يجب أن يظهر مقالنا في نتائج التصفية");

//                TestContext.WriteLine($"✅ عدد المقالات في التصنيف: {articles.Count}");
//            }

//            // ==================== TEST 5: UPDATE ARTICLE ====================
//            [Test]
//            [Order(5)]
//            public async Task UpdateArticle_ShouldModifySuccessfully()
//            {
//                TestContext.WriteLine($"🎬 الاختبار 5: تحديث المقال (ID: {_createdArticleId})...");

//                // 🎯 Arrange
//                var updateDto = new UpdateArticleDto
//                {
//                    ID = _createdArticleId,
//                    Title = "E2E Test Article - تم التحديث ✅",
//                    Content = "محتوى محدث لاختبار E2E مع إضافات جديدة...",
//                    categoryId = _testCategoryId,
//                    IsPublished = true
//                };

//                TestContext.WriteLine($"📝 البيانات الجديدة: {updateDto.Title}");

//                // 🚀 Act
//                var response = await _client.PutAsJsonAsync(
//                    "/api/Article/UpdateArticle", updateDto);  // ✅ PUT بدون ID في URL
//                TestContext.WriteLine($"📤 تم إرسال PUT إلى /api/Article/UpdateArticle");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);

//                // 🔍 التحقق من التحديث في قاعدة البيانات
//                var updatedArticle = await _context._articles.FindAsync(_createdArticleId);  // ✅ _articles
//                Assert.That(updatedArticle, Is.Not.Null);
//                Assert.That(updatedArticle.Title, Is.EqualTo(updateDto.Title));
//                Assert.That(updatedArticle.UpdatedAt, Is.GreaterThan(updatedArticle.CreatedAt));

//                TestContext.WriteLine($"✅ تم تحديث المقال بنجاح!");
//            }

//            // ==================== TEST 6: SEARCH ARTICLE ====================
//            [Test]
//            [Order(6)]
//            public async Task SearchArticle_ShouldFindRelevantResults()
//            {
//                TestContext.WriteLine("🎬 الاختبار 6: البحث في المقالات...");

//                // 🚀 Act - البحث بكلمة "اختبار"
//                var response = await _client.GetAsync($"/api/Article/searchArticle?prompt=اختبار");
//                TestContext.WriteLine($"📤 تم إرسال GET إلى /api/Article/searchArticle?prompt=اختبار");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);
//                Assert.That(apiResponse.Data, Is.Not.Null);

//                var articles = apiResponse.Data;

//                var ourArticle = articles.FirstOrDefault(a => a.ID == _createdArticleId);
//                Assert.That(ourArticle, Is.Not.Null,
//                    "يجب أن يظهر مقالنا في نتائج البحث");

//                TestContext.WriteLine($"✅ عدد نتائج البحث: {articles.Count}");
//            }

//            // ==================== TEST 7: DELETE ARTICLE ====================
//            [Test]
//            [Order(7)]
//            public async Task DeleteArticle_ShouldRemoveFromSystem()
//            {
//                TestContext.WriteLine($"🎬 الاختبار 7: حذف المقال (ID: {_createdArticleId})...");

//                // 🚀 Act
//                var response = await _client.DeleteAsync($"/api/Article/DeleteArticle?id={_createdArticleId}");
//                TestContext.WriteLine($"📤 تم إرسال DELETE إلى /api/Article/DeleteArticle?id={_createdArticleId}");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);

//                // 🔍 التحقق من الحذف من قاعدة البيانات
//                var deletedArticle = await _context._articles.FindAsync(_createdArticleId);  // ✅ _articles
//                Assert.That(deletedArticle, Is.Null,
//                    "يجب أن يحذف المقال من قاعدة البيانات");

//                // 🔍 التحقق من أن API يرجع null للمقال المحذوف
//                var getResponse = await _client.GetAsync($"/api/Article/GetArticleByID?Id={_createdArticleId}");
//                getResponse.EnsureSuccessStatusCode();

//                var getApiResponse = await getResponse.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();
//                Assert.That(getApiResponse.Data, Is.Null, "يجب أن يرجع null للمقال المحذوف");

//                TestContext.WriteLine($"✅ تم حذف المقال بنجاح!");
//            }

//            // ==================== TEST 8: GET PENDING ARTICLES ====================
//            [Test]
//            [Order(8)]
//            public async Task GetAllPendingArticles_ShouldReturnUnpublishedArticles()
//            {
//                TestContext.WriteLine("🎬 الاختبار 8: الحصول على المقالات المنتظرة...");

//                // 🚀 Act
//                var response = await _client.GetAsync("/api/Article/GetAllPendingArticles");
//                TestContext.WriteLine("📤 تم إرسال GET إلى /api/Article/GetAllPendingArticles");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);

//                TestContext.WriteLine($"✅ عدد المقالات المنتظرة: {apiResponse.Data?.Count ?? 0}");
//            }

//            // ==================== TEST 9: GET REJECTED ARTICLES ====================
//            [Test]
//            [Order(9)]
//            public async Task GetAllRejectArticles_ShouldReturnRejectedArticles()
//            {
//                TestContext.WriteLine("🎬 الاختبار 9: الحصول على المقالات المرفوضة...");

//                // 🚀 Act
//                var response = await _client.GetAsync("/api/Article/GetAllRejectArticles");
//                TestContext.WriteLine("📤 تم إرسال GET إلى /api/Article/GetAllRejectArticles");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);

//                TestContext.WriteLine($"✅ عدد المقالات المرفوضة: {apiResponse.Data?.Count ?? 0}");
//            }

//            // ==================== TEST 10: GET ALL (INCLUDING UNPUBLISHED) ====================
//            [Test]
//            [Order(10)]
//            public async Task GetAll_ShouldReturnAllArticlesIncludingUnpublished()
//            {
//                TestContext.WriteLine("🎬 الاختبار 10: الحصول على جميع المقالات (بما فيها غير المنشورة)...");

//                // 🚀 Act
//                var response = await _client.GetAsync("/api/Article/GetAll");
//                TestContext.WriteLine("📤 تم إرسال GET إلى /api/Article/GetAll");

//                // ✅ Assert
//                response.EnsureSuccessStatusCode();

//                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();
//                Assert.That(apiResponse, Is.Not.Null);
//                Assert.That(apiResponse.Result, Is.True);
//                Assert.That(apiResponse.Data, Is.Not.Null);

//                var totalCount = await _context._articles.CountAsync();  // ✅ _articles
//                Assert.That(apiResponse.Data.Count, Is.EqualTo(totalCount),
//                    "يجب أن يعيد جميع المقالات بما فيها غير المنشورة");

//                TestContext.WriteLine($"✅ عدد المقالات الكلي: {apiResponse.Data.Count}");
//            }

//            // 🧹 التنظيف بعد الاختبارات
//            public async Task DisposeAsync()
//            {
//                TestContext.WriteLine("🧹 بداية تنظيف بعد الاختبارات...");

//                if (_context != null)
//                {
//                    var testArticles = await _context._articles  // ✅ _articles
//                        .Where(a => a.Title.Contains("E2E Test"))
//                        .ToListAsync();

//                    if (testArticles.Any())
//                    {
//                        _context._articles.RemoveRange(testArticles);  // ✅ _articles
//                        await _context.SaveChangesAsync();
//                        TestContext.WriteLine($"🧹 تم حذف {testArticles.Count} مقال اختباري");
//                    }

//                    await _context.DisposeAsync();
//                }

//                _client?.Dispose();
//                _factory?.Dispose();

//                TestContext.WriteLine("✅ التنظيف اكتمل بنجاح!");
//            }

//            protected virtual void Dispose(bool disposing)
//            {
//                if (!disposedValue)
//                {
//                    if (disposing)
//                    {
//                        // TODO: dispose managed state (managed objects)
//                    }

//                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
//                    // TODO: set large fields to null
//                    disposedValue = true;
//                }
//            }

//            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
//            // ~ArticleCRUDE2ETests()
//            // {
//            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//            //     Dispose(disposing: false);
//            // }

//            void IDisposable.Dispose()
//            {
//                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//                Dispose(disposing: true);
//                GC.SuppressFinalize(this);
//            }
//        }
//    }
//}

using Applicaion.Dto.ArticleDto;
using Application.Serializer;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ArticleE2ETests
{
    [TestFixture]
    [NonParallelizable]
    public class ArticleCRUDE2ETests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private BlogDbContext _dbContext;
        private int _testCategoryId;
        private int _testUserId = 1;
        private int _createdArticleId;
        private bool _disposed;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            Console.WriteLine("🚀 بدء تهيئة اختبارات E2E للمقالات...");

            // 1. إنشاء مصنع التطبيق
            _factory = new CustomWebApplicationFactory();

            // 2. إنشاء عميل HTTP
            _client = _factory.CreateClient();

            // 3. الحصول على DbContext
            using var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

            // 4. تهيئة بيانات الاختبار
            await InitializeTestDataAsync();

            Console.WriteLine("✅ التهيئة اكتملت بنجاح!");
        }

        private async Task InitializeTestDataAsync()
        {
            // تنظيف أي بيانات قديمة
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            // إضافة مستخدم اختباري
            if (!await _dbContext._users.AnyAsync())
            {
                _dbContext._users.Add(new User
                {
                    ID = _testUserId,
                    UserName = "test_author",
                    Email = "author@test.com",
                    PasswordHash = "hashed_password",
                    Role = "Writer"  // مهم: يجب أن يكون Writer
                });
            }

            // إضافة تصنيف اختباري
            if (!await _dbContext._categories.AnyAsync())
            {
                _dbContext._categories.Add(new Category
                {
                    ID = 1,
                    CategoryName = "Technology",
                    Description = "مقالات تقنية"
                });
            }

            await _dbContext.SaveChangesAsync();

            // الحصول على ID التصنيف
            var category = await _dbContext._categories.FirstAsync();
            _testCategoryId = category.ID;

            Console.WriteLine($"📊 بيانات الاختبار: UserId={_testUserId}, CategoryId={_testCategoryId}");
        }

        // ================ الاختبار 1: إنشاء مقال ================
        [Test]
        [Order(1)]
        public async Task CreateArticle_WithValidData_ShouldReturnSuccess()
        {
            Console.WriteLine("🎬 الاختبار 1: إنشاء مقال جديد...");

            // ⚠️ ملاحظة: بما أن CreateArticle يتطلب IFormFile (الصورة)
            // سنستخدم MultipartFormDataContent بدلاً من JSON

            // Arrange
            var content = new MultipartFormDataContent
            {
                { new StringContent("مقال اختبار E2E"), "Title" },
                { new StringContent("محتوى المقال الاختباري لفحص الـ CRUD"), "Content" },
                { new StringContent(_testCategoryId.ToString()), "categoryId" },
                { new StringContent("true"), "IsPublished" }
            };

            // Act
            var response = await _client.PostAsync("/api/Article/InsertArticle", content);
            Console.WriteLine($"📤 POST /api/Article/InsertArticle → {response.StatusCode}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"📄 Response: {responseString}");

            // تحويل الاستجابة إلى ApiResponse<ArticleDto>
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();

            Assert.That(apiResponse, Is.Not.Null);
            Assert.That(apiResponse.Result, Is.True);
            Assert.That(apiResponse.Data, Is.Not.Null);
            Assert.That(apiResponse.Data.ID, Is.GreaterThan(0));

            // حفظ ID المقال للاختبارات التالية
            _createdArticleId = apiResponse.Data.ID;
            Console.WriteLine($"✅ تم إنشاء المقال بنجاح! ID: {_createdArticleId}");
        }

        // ================ الاختبار 2: قراءة مقال ================
        [Test]
        [Order(2)]
        public async Task GetArticleById_ExistingArticle_ShouldReturnArticle()
        {
            Console.WriteLine($"🎬 الاختبار 2: قراءة المقال (ID: {_createdArticleId})...");

            // Arrange & Act
            var response = await _client.GetAsync($"/api/Article/GetArticleByID?Id={_createdArticleId}");
            Console.WriteLine($"📤 GET /api/Article/GetArticleByID → {response.StatusCode}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();

            Assert.That(apiResponse, Is.Not.Null);
            Assert.That(apiResponse.Result, Is.True);
            Assert.That(apiResponse.Data, Is.Not.Null);
            Assert.That(apiResponse.Data.ID, Is.EqualTo(_createdArticleId));
            Assert.That(apiResponse.Data.Title, Is.EqualTo("مقال اختبار E2E"));

            Console.WriteLine($"✅ تم قراءة المقال بنجاح!");
        }

        // ================ الاختبار 3: عرض جميع المقالات ================
        [Test]
        [Order(3)]
        public async Task GetAllArticles_ShouldIncludeCreatedArticle()
        {
            Console.WriteLine("🎬 الاختبار 3: عرض جميع المقالات...");

            // Arrange & Act
            var response = await _client.GetAsync("/api/Article/GetAllArticle");
            Console.WriteLine($"📤 GET /api/Article/GetAllArticle → {response.StatusCode}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();

            Assert.That(apiResponse, Is.Not.Null);
            Assert.That(apiResponse.Result, Is.True);
            Assert.That(apiResponse.Data, Is.Not.Null);
            Assert.That(apiResponse.Data, Has.Count.GreaterThan(0));

            var ourArticle = apiResponse.Data.FirstOrDefault(a => a.ID == _createdArticleId);
            Assert.That(ourArticle, Is.Not.Null, "يجب أن يظهر مقالنا في القائمة");

            Console.WriteLine($"✅ عدد المقالات المنشورة: {apiResponse.Data.Count}");
        }

        // ================ الاختبار 4: تحديث مقال ================
        [Test]
        [Order(4)]
        public async Task UpdateArticle_ShouldModifySuccessfully()
        {
            Console.WriteLine($"🎬 الاختبار 4: تحديث المقال (ID: {_createdArticleId})...");

            // Arrange
            var updateDto = new UpdateArticleDto
            {
                ID = _createdArticleId,
                Title = "مقال اختبار E2E - تم التحديث",
                Content = "محتوى محدث للمقال الاختباري",
                categoryId = _testCategoryId,
                IsPublished = true,
                Image = null  // في الاختبار لا نرسل صورة
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/Article/UpdateArticle", updateDto);
            Console.WriteLine($"📤 PUT /api/Article/UpdateArticle → {response.StatusCode}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();

            Assert.That(apiResponse, Is.Not.Null);
            Assert.That(apiResponse.Result, Is.True);
            Assert.That(apiResponse.Data, Is.Not.Null);
            Assert.That(apiResponse.Data.Title, Is.EqualTo("مقال اختبار E2E - تم التحديث"));

            Console.WriteLine($"✅ تم تحديث المقال بنجاح!");
        }

        // ================ الاختبار 5: حذف مقال ================
        [Test]
        [Order(5)]
        public async Task DeleteArticle_ShouldRemoveFromSystem()
        {
            Console.WriteLine($"🎬 الاختبار 5: حذف المقال (ID: {_createdArticleId})...");

            // Act
            var response = await _client.DeleteAsync($"/api/Article/DeleteArticle?id={_createdArticleId}");
            Console.WriteLine($"📤 DELETE /api/Article/DeleteArticle → {response.StatusCode}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();
            Assert.That(apiResponse, Is.Not.Null);
            Assert.That(apiResponse.Result, Is.True);

            // التحقق من قاعدة البيانات
            var deletedArticle = await _dbContext._articles.FindAsync(_createdArticleId);
            Assert.That(deletedArticle, Is.Null, "يجب أن يحذف المقال من قاعدة البيانات");

            // التحقق من أن API يعيد null للمقال المحذوف
            var getResponse = await _client.GetAsync($"/api/Article/GetArticleByID?Id={_createdArticleId}");
            var getApiResponse = await getResponse.Content.ReadFromJsonAsync<ApiResponse<ArticleDto>>();
            Assert.That(getApiResponse.Data, Is.Null, "يجب أن يرجع null للمقال المحذوف");

            Console.WriteLine($"✅ تم حذف المقال بنجاح!");
        }

        // ================ الاختبار 6: تصفية حسب التصنيف ================
        [Test]
        [Order(6)]
        public async Task FilterByCategory_ShouldReturnArticlesInCategory()
        {
            Console.WriteLine($"🎬 الاختبار 6: تصفية حسب التصنيف (ID: {_testCategoryId})...");

            // أولاً: ننشئ مقال جديد للتأكد من وجود بيانات
            var content = new MultipartFormDataContent
            {
                { new StringContent("مقال للتصفية"), "Title" },
                { new StringContent("محتوى لاختبار التصفية"), "Content" },
                { new StringContent(_testCategoryId.ToString()), "categoryId" },
                { new StringContent("true"), "IsPublished" }
            };

            var createResponse = await _client.PostAsync("/api/Article/InsertArticle", content);
            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // الآن نختبر التصفية
            var response = await _client.GetAsync($"/api/Article/GetArticleByCategoryName?id={_testCategoryId}");
            Console.WriteLine($"📤 GET /api/Article/GetArticleByCategoryName → {response.StatusCode}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();

            Assert.That(apiResponse, Is.Not.Null);
            Assert.That(apiResponse.Result, Is.True);
            Assert.That(apiResponse.Data, Is.Not.Null);
            Assert.That(apiResponse.Data.Count, Is.GreaterThan(0));

            Console.WriteLine($"✅ عدد المقالات في التصنيف: {apiResponse.Data.Count}");
        }

        // ================ الاختبار 7: البحث في المقالات ================
        [Test]
        [Order(7)]
        public async Task SearchArticles_ShouldFindResults()
        {
            Console.WriteLine("🎬 الاختبار 7: البحث في المقالات...");

            // Act
            var response = await _client.GetAsync($"/api/Article/searchArticle?prompt=مقال");
            Console.WriteLine($"📤 GET /api/Article/searchArticle → {response.StatusCode}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ArticleDto>>>();

            Assert.That(apiResponse, Is.Not.Null);
            Assert.That(apiResponse.Result, Is.True);

            if (apiResponse.Data != null)
            {
                Console.WriteLine($"✅ عدد نتائج البحث: {apiResponse.Data.Count}");
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            Console.WriteLine("🧹 بدء التنظيف بعد الاختبارات...");

            try
            {
                // تنظيف قاعدة البيانات
                var testArticles = await _dbContext._articles
                    .Where(a => a.Title.Contains("اختبار") || a.Title.Contains("E2E"))
                    .ToListAsync();

                if (testArticles.Any())
                {
                    _dbContext._articles.RemoveRange(testArticles);
                    await _dbContext.SaveChangesAsync();
                    Console.WriteLine($"🧹 تم حذف {testArticles.Count} مقال اختباري");
                }

                _client?.Dispose();
                _factory?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطأ أثناء التنظيف: {ex.Message}");
            }

            Console.WriteLine("✅ التنظيف اكتمل!");
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}