using Application;
using Application.Dto.Profile;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Mapper
{
    public class PersonaProfile:Profile
    {
        public PersonaProfile()
        {
            CreateMap<Persona, ProfileDto>();

            CreateMap<CreateProfile,Persona>();
        }
    }
}
