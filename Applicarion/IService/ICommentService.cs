using Applicarion.Dto.ArticleDto;
using Applicarion.Dto.CommentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.IService
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllComments();
        Task<CommentDto> GetCommentByID(int id);

        Task<CommentDto> CreateComment(CreateCommentDto createCommentDto);
        Task<CommentDto> UpdateComment(UpdateCommentDto updateCommentDto );
        Task<CommentDto> DeleteComment(int id);
    }
}
