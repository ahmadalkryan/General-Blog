using Applicaion.IRepository;
using Application.Dto.Like;
using Application.IService;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class LikeService : ILikeService
    {
        private readonly IRepository<Like> _repository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public LikeService(IRepository<Like> repository ,IMapper mapper ,IUserService userService)
        {
            _mapper = mapper;   
            _userService=userService;
            _repository = repository;
        }
        public async Task<int> CountLikes(int articleId)
        {
            var count = await GetAllLikesForArticle(articleId);
            return count?.Count()?? 0;
            
        }

        public async Task<LikeDto> CreateLike(CreateLike like)
        {
           var crlike = _mapper.Map<Like>(like);
            
            var like1 = await _repository.Insertasync(crlike);

            return _mapper.Map<LikeDto>(crlike);
        }

       
       

      public async  Task<IEnumerable<LikeDto>> GetAllLikesForArticle(int articleID)
        {

            var res = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<LikeDto>>(res.Where(x => x.articleId == articleID));

        }

        Task<int> ILikeService.CountLikes()
        {
            throw new NotImplementedException();
        }

      public  async Task<LikeDto> CreateLike(crLike like)
        {
            var user = await _userService.GetCurrentUserAsync();
            var create = new CreateLike
            {
                articleId = like.articleId,
                userId = user.Id,
                
            };
            var crlike = _mapper.Map<Like>(create);

            var like1 = await _repository.Insertasync(crlike);

            return _mapper.Map<LikeDto>(crlike);
        }

        public async Task<LikeDto> DeleteLike(int likeId)
        {
            var like = await _repository.GetById(likeId);

            await _repository.RemoveAsync(like);
            return _mapper.Map<LikeDto>(like);


        }

       public async Task<LikeDto> GetLikeById(int lid)
        {

            var like = _repository.GetById(lid);
            return _mapper.Map<LikeDto>(like);
        }
    }
}
