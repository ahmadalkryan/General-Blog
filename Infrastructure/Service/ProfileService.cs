using Applicaion.IRepository;
using Application.Dto.Profile;
using Application.IService;
using Application.Mapper;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persona = Domain.Entities.Persona;

namespace Infrastructure.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<Persona> _repository;

        private readonly IMapper _mapper;

        public ProfileService(IMapper mapper, IRepository<Persona> repository)
        {
            _mapper = mapper;
            _repository = repository;

        }
        public async Task<ProfileDto> CreateProfile(CreateProfile profile)
        {
            var res = _mapper.Map<Persona>(profile);
            var createdProfile = await _repository.Insertasync(res);

            return _mapper.Map<ProfileDto>(createdProfile);
        }



        public async Task<ProfileDto> GetProfileByUserID(int userId)
        {
            var res = await _repository.GetAllAsync();

            var profile = res.FirstOrDefault(p => p.userId == userId);
            return _mapper.Map<ProfileDto>(profile);
        }

        public async Task<ProfileDto> DeleteProfile(int profileId)
        {
            var profile = await _repository.GetById(profileId);

            if (profile == null)
            {
                return null;
            }
            var deletedProfile = await _repository.RemoveAsync(profile);

            return _mapper.Map<ProfileDto>(deletedProfile);
        }

        public async Task<IEnumerable<ProfileDto>> GetAllProfiles()
        {
            var profiles = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProfileDto>>(profiles);
        }
    }
}
