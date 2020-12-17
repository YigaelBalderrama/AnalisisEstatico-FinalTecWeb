using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Models;
namespace SimpsonApp.Services
{
    public interface ICharacterService
    {
        Task<IEnumerable<Character>> getCharacters(string orderBy, bool showPrase);
    }
}
