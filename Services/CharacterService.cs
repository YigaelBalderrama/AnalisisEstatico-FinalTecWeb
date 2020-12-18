﻿using AutoMapper;
using SimpsonApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Exceptions;
using SimpsonApp.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using SimpsonApp.Data.Entities;

namespace SimpsonApp.Services
{
    public class CharacterService : ICharacterService
    {
        private ILibraryRepository _libraryRepository;
        private IMapper _mapper;
        private HashSet<string> allowedOrderByParameters = new HashSet<string>()
        {
            "ID","Name","Age"
        };
        public CharacterService(IMapper mapper, ILibraryRepository libraryRepository)
        {
            this._mapper = mapper;
            this._libraryRepository = libraryRepository;
        }
        public async Task<IEnumerable<Character>> getCharacters(string orderBy, bool showPrase)
        {
            if (!allowedOrderByParameters.Contains(orderBy.ToLower()))
            {
                throw new BadRequestOperationException($"the field: {orderBy} is not supported, please use one of these {string.Join(",", allowedOrderByParameters)}");
            }

            var entityList = await _libraryRepository.GetCharactersAsync(orderBy, showPrase);
            var modelList = _mapper.Map<IEnumerable<Character>>(entityList);
            return modelList;
        }
        public async Task<Character> GetCharacterAsync(int charID, bool showPhrase = false)
        {
            var charac = await _libraryRepository.GetCharacterAsync(charID, showPhrase);
            if (charac== null)
            {
                throw new NotFoundOperationException($"The character with id:{charID} does not exists");
            }

            return _mapper.Map<Character>(charac);
        }
        public async Task<Character> UpdateCharacter(int charId, Character c)
        {
            var characterEntity = _mapper.Map<CharacterEntity>(c);
            await GetCharacterAsync(charId);
            characterEntity.ID = charId;
            _libraryRepository.UpdateCharacter(characterEntity);

            var saveResult = await _libraryRepository.SaveChangesAsync();

            if (!saveResult)
            {
                throw new Exception("Database Error");
            }
            return c;
        }
    }
}
