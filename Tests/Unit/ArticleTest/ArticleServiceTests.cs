using Applicaion.Dto.ArticleDto;
using Applicaion.Dto.UserDto;
using Applicaion.IRepository;
using Application.IService;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Service;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit.ArticleTest
{
    [TestFixture]
    public class ArticleServiceTests 
    {
        private Mock<IRepository<Article>> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IUserService> _mockUserService;
        private Mock<INotiService> _mockNotiService;
        private ArticleService _articleService;


        private Article _testArticle;
        private UserDto _testUser;
        private ArticleDto _testArticleDto;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository<Article>>();
            _mockMapper = new Mock<IMapper>();
            _mockNotiService = new Mock<INotiService>();
            _mockUserService = new Mock<IUserService>();

            _articleService = new ArticleService(
                 _mockUserService.Object,

                _mockRepository.Object
                ,
                _mockMapper.Object
                
                 ,_mockNotiService.Object
                
                );
            _testUser = new UserDto
            {
                Id=1,
                UserName="testuser",
                Email= "test@example.com"

            };

            _testArticle = new Article
            {
                ID=1,
                Title="Test Article",
                Content="Test Content",
                userID=1,
                categoryId=1,
                IsPublished=true,
                CreatedAt = DateTime.Now,
                UpdatedAt=DateTime.Now,
                ImageUrl= "test.jpg"
            };

            _testArticleDto= new ArticleDto
            {
                ID = 1,
                Title = "Test Article",
                Content = "Test Content",
                userID = 1,
                categoryId = 1,
                IsPublished = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ImageUrl = "test.jpg"
            };


        }


        //create article Test
        [Test]
        public async Task CreateArticle_ValidData_ShouldReturnArticleDto()
        {
            //Arrange
            var crArticleDto = new CrArticleDto
            {
                Title = "New Article",
                Content = "Article Content",
                categoryId = 1,
                IsPublished = false,
                Image = null,
                


            };
            var createArticleDto = new CreateArticleDto
            {
                userID=1,
                Title="New Article",
                Content="Content",
                categoryId=1,
                IsPublished = true,
                Image = null,
            };
            var article = new Article { ID = 1 };

            var articleDto= new ArticleDto { ID = 1 };

            _mockUserService.Setup(x => x.GetCurrentUserAsync())
               .ReturnsAsync(_testUser);
            _mockMapper.Setup(x => x.Map<Article>(It.IsAny<CreateArticleDto>()))
                .Returns(article);
            _mockRepository.Setup(x => x.Insertasync(article))
                .ReturnsAsync(article);
            _mockMapper.Setup(x => x.Map<ArticleDto>(article))
                .Returns(articleDto);

            var result = await _articleService.CreateArticle(crArticleDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(1));

            _mockUserService.Verify(x => x.GetCurrentUserAsync(), Times.Once);
            _mockRepository.Verify(x => x.Insertasync(It.IsAny<Article>()), Times.Once);
            _mockNotiService.Verify(x => x.NotifyNewGlobalMessageAsync(result), Times.Once);










        }


        [Test]
        public void CreateArticle_NullDto_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _articleService.CreateArticle(null));
        }

        //[Test]
        //[TestCase("")]
        //[TestCase("   ")]
        //[TestCase(null)]
        //public void CreateArticle_InvalidTitle_ThrowsArgumentException(string invalidTitle)
        //{
        //    // Arrange
        //    var dto = new CrArticleDto
        //    {
        //        Title = invalidTitle,
        //        Content = "Valid Content",
        //        CategoryId = 1
        //    };

        //    _mockUserService.Setup(x => x.GetCurrentUserAsync())
        //        .ReturnsAsync(_testUser);

        //    // Act & Assert
        //    Assert.ThrowsAsync<ArgumentException>(() =>
        //        _articleService.CreateArticle(dto));
        //}

        [Test]
        public async Task GetArticleById_ExistingArticle_ReturnArticleDto()
        {


            // Arrange
            var articleId = 1;
            _mockRepository.Setup(x => x.GetById(articleId))
                .ReturnsAsync(_testArticle);
            _mockMapper.Setup(x => x.Map<ArticleDto>(_testArticle))
                .Returns(_testArticleDto);

            // Act
            var result = await _articleService.GetArticleByID(articleId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(articleId));
            _mockRepository.Verify(x => x.GetById(articleId), Times.Once);


        }

        //[Test]
        //public void GetArticleByID_NonExistingArticle_ThrowsKeyNotFoundException()
        //{
        //    // Arrange
        //    var nonExistingId = 999;
        //    _mockRepository.Setup(x => x.GetById(nonExistingId))
        //        .ReturnsAsync((Article)null);

        //    // Act & Assert
        //    Assert.ThrowsAsync<KeyNotFoundException>(() =>
        //        _articleService.GetArticleByID(nonExistingId));
        //}

        [Test]
        public async Task GetAllArticles_ReturnsOnlyPublishedArticles()
        {
            // Arrange
            var publishedArticles = new List<Article>
            {
                new Article { ID = 1, Title = "Published 1", IsPublished = true },
                new Article { ID = 2, Title = "Published 2", IsPublished = true }
            };

            var articleDtos = publishedArticles.Select(a =>
                new ArticleDto { ID = a.ID, Title = a.Title }).ToList();

            _mockRepository.Setup(x => x.GetAllAsync(
                    It.IsAny<Expression<Func<Article, bool>>>(),
                    It.IsAny<Expression<Func<Article, object>>>(),
                    It.IsAny<Expression<Func<Article, object>>>()))
                .ReturnsAsync(publishedArticles);
            _mockMapper.Setup(x => x.Map<IEnumerable<ArticleDto>>(publishedArticles))
                .Returns(articleDtos);

            // Act
            var result = await _articleService.GetAllArticles();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(a => a.Title.Contains("Published")), Is.True);
        }



        [Test]
        public async Task FilterByCategory_ValidCategory_ReturnsFilteredArticles()
        {
            // Arrange
            var categoryId = 1;
            var filteredArticles = new List<Article>
            {
                new Article { ID = 1, Title = "Article 1", categoryId = 1, IsPublished = true },
                new Article {   ID = 2, Title = "Article 2", categoryId = 1, IsPublished = true }
            };

            var articleDtos = filteredArticles.Select(a =>
                new ArticleDto { ID = a.ID, Title = a.Title, categoryId = a.categoryId }).ToList();

            _mockRepository.Setup(x => x.GetAllAsync(
                    It.IsAny<Expression<Func<Article, bool>>>(),
                    It.IsAny<Expression<Func<Article, object>>>(),
                    It.IsAny<Expression<Func<Article, object>>>()))
                .ReturnsAsync(filteredArticles);
            _mockMapper.Setup(x => x.Map<IEnumerable<ArticleDto>>(filteredArticles))
                .Returns(articleDtos);

            // Act
            var result = await _articleService.FilterByCategory(categoryId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(a => a.categoryId == categoryId), Is.True);
        }

        //update article test

        [Test]
        public async Task UpdateArticle_ValidData_ReturnsUpdatedArticle()
        {
            // Arrange
            var updateDto = new UpdateArticleDto
            {
                ID = 1,
                Title = "Updated Title",
                Content = "Updated Content",
                categoryId = 2,
                Image = null,
                IsPublished = true
            };

            var updatedArticle = new Article
            {
                ID = 1,
                Title = "Updated Title",
                Content = "Updated Content",
                categoryId = 2,
                userID = 1,
                IsPublished = true
            };

            var articleDto = new ArticleDto { ID = 1, Title = "Updated Title" };

            _mockUserService.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(_testUser);
            _mockMapper.Setup(x => x.Map<Article>(updateDto))
                .Returns(updatedArticle);
            _mockRepository.Setup(x => x.UpdateAsync(updatedArticle))
                .ReturnsAsync(updatedArticle);
            _mockMapper.Setup(x => x.Map<ArticleDto>(updatedArticle))
                .Returns(articleDto);

            // Act
            var result = await _articleService.UpdateArticle(updateDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Updated Title"));
            _mockRepository.Verify(x => x.UpdateAsync(updatedArticle), Times.Once);
        }



        //Delete 

        [Test]
        public async Task DeleteArticle_ExistingArticle_ReturnsDeletedArticle()
        {
            // Arrange
            var articleId = 1;
            var articleToDelete = new Article { ID = articleId, Title = "To Delete" };
            var articleDto = new ArticleDto { ID = articleId, Title = "To Delete" };

            _mockRepository.Setup(x => x.GetById(articleId))
                .ReturnsAsync(articleToDelete);
            _mockMapper.Setup(x => x.Map<ArticleDto>(articleToDelete))
                .Returns(articleDto);

            // Act
            var result = await _articleService.DeleteArticle(articleId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(articleId));
            _mockRepository.Verify(x => x.RemoveAsync(articleToDelete), Times.Once);
        }






    }

}
