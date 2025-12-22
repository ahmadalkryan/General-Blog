using Application.Dto.Like;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface ILikeService
    {
        Task<LikeDto> CreateLike(crLike like);
        Task<LikeDto> GetLikeById(int lid);
        Task<int> CountLikes();
        Task<IEnumerable<LikeDto>> GetAllLikesForArticle(int articelID);
        Task<LikeDto> DeleteLike(int likeId);
    }
}
