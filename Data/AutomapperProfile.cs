using AutoMapper;
using SimpsonApp.Data.Entities;
using SimpsonApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpsonApp.Data
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            this.CreateMap<CharacterEntity, Character>()
                .ReverseMap();


            this.CreateMap<Phrase, PhraseEntity>()
                .ForMember(des => des.Character, opt => opt.MapFrom(scr => new CharacterEntity { ID = scr.CharacterID }))
                .ReverseMap()
                .ForMember(dest => dest.CharacterID, opt => opt.MapFrom(scr => scr.Character.ID));

        }
    }
}
