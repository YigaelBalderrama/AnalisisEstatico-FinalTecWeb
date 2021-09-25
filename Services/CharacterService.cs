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
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;
        private readonly HashSet<string> allowedOrderByParameters = new HashSet<string>()
        {
            "id","name","age"
        };
        public CharacterService(IMapper mapper, ILibraryRepository libraryRepository)
        {
            this._mapper = mapper;
            this._libraryRepository = libraryRepository;
        }
        public async Task<IEnumerable<Phrase>> getPhrases()
        {
            var character = await _libraryRepository.GetPhrasesAsync();
            return _mapper.Map<IEnumerable<Phrase>>(character);
        }
        public async Task<IEnumerable<Character>> getCharacters(string orderBy, bool showPrase = false)
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
                throw new DatabaseException("Database Error");
            }
            return await GetCharacterAsync(charId);
        }

        public async Task<Character> CreateCharacterAsync(Character charac)
        {
            var characEntity = _mapper.Map<CharacterEntity>(charac);
            _libraryRepository.CreateCharacter(characEntity);
            var result = await _libraryRepository.SaveChangesAsync();

            if (result)
            {
                return _mapper.Map<Character>(characEntity);
            }

            throw new DatabaseException("Database Error");
        }

        public async Task<DeleteModel> DeleteCharacterAsync(int characID)
        {
            await GetCharacterAsync(characID);

            var DeleteResult = await _libraryRepository.DeleteCharacterAsync(characID);

            var saveResult = await _libraryRepository.SaveChangesAsync();

            if (!DeleteResult)
            {
                throw new DatabaseException("Database Error");
            }
            if (saveResult)
            {
                return new DeleteModel()
                {
                    IsSuccess = saveResult,
                    Message = "The character was removed."
                };
            }
            else
            {
                return new DeleteModel()
                {
                    IsSuccess = saveResult,
                    Message = "The character was not removed, Check Database State"
                };
            }
            
        }
    }
}
