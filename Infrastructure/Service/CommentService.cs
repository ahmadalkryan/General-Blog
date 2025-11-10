using Applicarion.Dto.CommentDto;
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
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CommentService(IMapper mapper ,IRepository<Comment> repository ,IUserService userService)
        {
            _mapper = mapper;
            _repository = repository;
            _userService = userService;
            
        }

        public async Task<CommentDto> CreateComment(CreateCommentDto createCommentDto)
        {
            var comment = _mapper.Map<Comment>(createCommentDto);
            var user = await _userService.GetCurrentUserAsync();

            comment.userID = user.Id;
            await _repository.Insertasync(comment);

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> DeleteComment(int id)
        {
             var comm = await _repository.GetById(id);

            if (comm != null)
            {
                await _repository.RemoveAsync(comm);
            }
            return _mapper.Map<CommentDto>(comm);
        }

        public async Task<IEnumerable<CommentDto>> GetAllComments()=>
       _mapper.Map<IEnumerable<CommentDto>>(await _repository.GetAllAsync());
        

        public async Task<CommentDto> GetCommentByID(int id)
        {
            var comm = await _repository.GetById(id);
            return _mapper.Map<CommentDto>(comm);
        }

        public async Task<CommentDto> UpdateComment(UpdateCommentDto updateCommentDto)
        {
            var comment = _mapper.Map<Comment>(updateCommentDto);
            var user = await _userService.GetCurrentUserAsync();

            comment.userID = user.Id;
            await _repository.UpdateAsync(comment);

            return _mapper.Map<CommentDto>(comment);
        }
    }
}
