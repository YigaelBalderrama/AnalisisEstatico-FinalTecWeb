using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpsonApp.Models;

namespace SimpsonApp.Services
{
    public interface IPhraseService
    {
        Task<Phrase> GetphraseAsync(int charID, int PharaseId);
        Task<bool> UpdatePhraseAsync(int characID, int phraseID, Phrase Frase);
        Task<Phrase> CreatePhraseAsync(int characID, Phrase frase);
        Task<bool> DeletePhraseAsync(int characID, int PhraseID);
        Task<bool> addLikes(int characID, List<int> listPhrasesId);
        Task<IEnumerable<Phrase>> getPhrases(int charId);
    }
}
