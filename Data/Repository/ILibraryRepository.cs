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
       Task <IEnumerable<CharacterEntity>>GetCharacterAsync(string orderBy, bool showPrase);

    }
}
