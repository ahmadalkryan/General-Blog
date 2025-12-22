using Applicaion.Dto.ArticleDto;
using Applicaion.IRepository;
using Applicaion.IService;
using Application.IService;
using AutoMapper;
using Domain.Entities;


namespace Infrastructure.Service
{
    public class ArticleService :IArticleService
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly INotiService _notiService;
       
        public ArticleService(IUserService userService, IRepository<Article> repository, IMapper mapper, INotiService notiService )
        {
            _articleRepository = repository;
            _mapper = mapper;
           

            _userService = userService;
            _notiService = notiService;
        }
        public async Task<ArticleDto> CreateArticle(CrArticleDto createArticleDto)
        {
            if (createArticleDto == null)
                throw new ArgumentNullException(nameof(createArticleDto));


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
           var result =  _mapper.Map<ArticleDto>(art);

            await _notiService.NotifyNewGlobalMessageAsync(result);
            return result;

        }

        public async Task<ArticleDto> DeleteArticle(int id)
        {
            var art = await _articleRepository.GetById(id);

            await _articleRepository.RemoveAsync(art);

            return _mapper.Map<ArticleDto>(art);
        }
        //approval
        public async Task<IEnumerable<ArticleDto>> GetAllArticles()
        {
           // var articles = (await _articleRepository.GetAllAsync()).Where(x => x.IsPublished == true).ToList();
           var articles= await _articleRepository.GetAllAsync(x=>x.IsPublished==true,x=>x._category,x=>x._user);

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

            //var art = await GetAllArticles();
            //var filtered = art.Where(a =>a.categoryId == id).ToList();
            var articles = await _articleRepository.GetAllAsync(x => x.IsPublished == true && x.categoryId == id, x => x._category, x => x._user);


            return _mapper.Map<IEnumerable<ArticleDto>>(articles);

           
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

       public async Task<IEnumerable<ArticleDto>>GetApprovalArticles()
        {
            //var articles = await _articleRepository.GetAllAsync();

            //var result = articles.Where(x=>x.IsPublished == true).ToList();
            var articles =await _articleRepository.GetAllAsync(x=>x.IsPublished==true,x=>x._user ,x=>x._category);
            return _mapper.Map<IEnumerable<ArticleDto>>(articles);
        }

        public  async Task<bool> ApproveArticle(int articleId)
        {
            var art = await _articleRepository.GetById(articleId);
            art.IsPublished = true;
            art.UpdatedAt=DateTime.Now;
            await _articleRepository.UpdateAsync(art);
            return true;

        }

        public Task<IEnumerable<ArticleDto>> GetAllApprovalsArticles()
        {
            throw new NotImplementedException();

        }

        public async Task<IEnumerable<ArticleDto>> GetRejectArticle()
        {
            //var allArticles = await _articleRepository.GetAllAsync();

            //var rejectedArticles = allArticles
            //    .Where(x => x.IsPublished == false &&
            //               x.articleApproval != null &&
            //               x.articleApproval.Status == ApprovalStatus.Rejected)
            //    .ToList();
            var articles =await _articleRepository.GetAllAsync(x=>x.IsPublished==false&&x.articleApproval.Status==ApprovalStatus.Rejected,x=>x._user ,x=>x._category);

            return _mapper.Map<IEnumerable<ArticleDto>>(articles);
            //var result = (await _articleRepository.GetAllAsync()).
            //    Where(x => x.IsPublished == false &&x.articleApproval.Status==ApprovalStatus.Rejected).ToList();
            //return _mapper.Map<IEnumerable< ArticleDto>>(result);
        }

       public async Task<IEnumerable<ArticleDto>> GetAll()
        { 
            var result = await _articleRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ArticleDto>>(result);
        }

       public async  Task<IEnumerable<ArticleDto>> GetPendingArticle()
        {
            //var result = await _articleRepository.GetAllAsync();

            //var pendingArticles = result
            //     .Where(x => x.IsPublished == false &&
            //                 (x.articleApproval == null))
            //    .ToList();

            var articles = await _articleRepository.GetAllAsync(x=>x.IsPublished==false&&x.articleApproval==null,x=>x.articleApproval,x=>x._user ,x=>x._category);
            return _mapper.Map<IEnumerable<ArticleDto>>(articles);
        }
    }
}
