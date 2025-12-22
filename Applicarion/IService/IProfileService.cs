using Application.Dto.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IProfileService
    {
        Task<ProfileDto> GetProfileByUserID(int userId);

        Task<ProfileDto>CreateProfile(CreateProfile profile);

        Task<IEnumerable<ProfileDto>> GetAllProfiles();
        Task<ProfileDto> DeleteProfile(int profileId);
    }
}
