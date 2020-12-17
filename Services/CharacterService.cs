using AutoMapper;
using SimpsonApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Exceptions;
using SimpsonApp.Data.Repository;

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

            var entityList = await _libraryRepository.GetCharacterAsync(orderBy, showPrase);
            var modelList = _mapper.Map<IEnumerable<Character>>(entityList);
            return modelList;
        }
    }
}
