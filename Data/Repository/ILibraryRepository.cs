using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Models;
using SimpsonApp.Data.Entities;

namespace SimpsonApp.Data.Repository
{
    public interface ILibraryRepository
    {
        Task<IEnumerable<CharacterEntity>> GetCharactersAsync(string orderBy, bool showPrase);
        Task<CharacterEntity> GetCharacterAsync(int charId, bool showPhrases = false);
        bool UpdateCharacter(CharacterEntity charac);
        Task<bool> SaveChangesAsync();

    }
}