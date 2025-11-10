using Applicarion.Dto.ArticleDto;
using Applicarion.IRepository;
using Applicarion.IService;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ArticleService(IUserService userService, IArticleRepository repository, IMapper mapper)
        {
            _articleRepository = repository;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<ArticleDto> CreateArticle(CrArticleDto createArticleDto)
        {

            var user = await _userService.GetCurrentUserAsync();

            var createart = new CreateArticleDto
            {
                userID = user.Id,
                categoryId = createArticleDto.categoryId,
                Title = createArticleDto.Title,
                Content = createArticleDto.Content,
                IsPublished = createArticleDto.IsPublished,
                Image = createArticleDto.Image,


            };
            var article = _mapper.Map<Article>(createart);
            var art = await _articleRepository.Insertasync(article);
            return _mapper.Map<ArticleDto>(art);
        }

        public async Task<ArticleDto> DeleteArticle(int id)
        {
            var art = await _articleRepository.GetById(id);

            await _articleRepository.RemoveAsync(art);

            return _mapper.Map<ArticleDto>(art);
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticles()
        {
            var articles = await _articleRepository.GetAllAsync();


            //var articleDtos = articles.Select(article => new ArticleDto
            //{
            //    ID = article.ID,
            //    Title = article.Title,
            //    Content = article.Content,
            //    ImageUrl = article.ImageUrl,
            //    CreatedAt = article.CreatedAt,
            //    UpdatedAt = article.UpdatedAt,
            //    IsPublished = article.IsPublished,
            //    categoryId = article.categoryId,
            //    userID = article.userID,
            //    CategoryName = article._category?.CategoryName ?? "Uncategorized",
            //    AuthorName = article._user?.UserName ?? "Unknown Author"
            //}).ToList();

            //return articleDtos;
            return _mapper.Map<IEnumerable<ArticleDto>>(articles);
        }
        public async Task<ArticleDto> GetArticleByID(int id)
        {
            var art = await _articleRepository.GetById(id);

            return _mapper.Map<ArticleDto>(art);
        }

        public async Task<ArticleDto> UpdateArticle(UpdateArticleDto updateArticleDto)
        {
            var article = _mapper.Map<Article>(updateArticleDto);
            var user = await _userService.GetCurrentUserAsync();
            article.userID = user.Id;

            var art = await _articleRepository.UpdateAsync(article);
            return _mapper.Map<ArticleDto>(art);
        }

        public async Task<IEnumerable<ArticleDto>> FilterByCategory(int id)
        {

            var art = await GetAllArticles();
            var filtered = art.Where(a =>a.categoryId == id).ToList();

            return _mapper.Map<IEnumerable<ArticleDto>>(filtered);

           // var art = await _articleRepository.FindAsync(x => x._category.CategoryName == category, x => x._category);

            //return _mapper.Map<IEnumerable<ArticleDto>>(art);
        }

        private double calculateSimilarityScore(Article article , string serachTerm)
        {
            var title = article.Title.ToLower();
            var content = article.Content.ToLower();
            serachTerm = serachTerm.ToLower();
            var Term = serachTerm.Split(' ', '.','?');
            int titleMatches = title.Split(' ', '.', ',', '!', '?').Count(word => word == serachTerm);
            int contentMatches = content.Split(' ', '.', ',', '!', '?').Count(word => word == serachTerm);
            int totalWords = title.Split(' ', '.', ',', '!', '?').Length + content.Split(' ', '.', ',', '!', '?').Length;
            int totalMatches = titleMatches + contentMatches;
            return (double) totalWords/ totalMatches;
        }

        public  async Task<IEnumerable<ArticleDto>> SearchArticle(string prompt)
        {
           var searchTerm = prompt.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(term => term.Length > 2).Distinct().ToArray();
            var articles = await _articleRepository.GetAllAsync();

            var filteredArticles = articles
                .Where(article => CalculateMatchPersentage(article, searchTerm.ToArray()) >= 10)
                .ToList();




            //var searched = articles
            //    .Select(a => new { Article = a, SimilarityScore = calculateSimilarityScore(a, prompt) })
            //    .Where(x => x.SimilarityScore > 0)
            //    .OrderByDescending(x => x.SimilarityScore)
            //    .Select(x => x.Article)
            //    .ToList();
            var searched = articles.Where(a => a.Title.ToLower().Contains(prompt.ToLower()) || a.Content.ToLower().Contains(prompt.ToLower())).ToList();
            return _mapper.Map<IEnumerable<ArticleDto>>(searched);

        }

        public int CountWordMatches(Article article, string [] searchTerm)
        {
            var content = $"{article.Title} {article.Content} {article._category.CategoryName}".ToLower();

            return searchTerm.Count(word => content.Contains(word));

        }
        public double CalculateMatchPersentage(Article article , string[] searchTerm)
        {
            var  count = CountWordMatches(article, searchTerm);
            return (double)count / searchTerm.Length * 100;
        }
    }
}
