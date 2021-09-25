﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpsonApp.Models;
namespace SimpsonApp.Services
{
    public interface ICharacterService
    {
        Task<IEnumerable<Character>> getCharacters(string orderBy, bool showPrase = false);
        Task<Character> UpdateCharacter(int charId, Character c);
        Task<Character> GetCharacterAsync(int charID, bool showPhrase = false);
        Task<Character> CreateCharacterAsync(Character charac);
        Task<IEnumerable<Phrase>> getPhrases();

        Task<DeleteModel> DeleteCharacterAsync(int characID);

    }
}
