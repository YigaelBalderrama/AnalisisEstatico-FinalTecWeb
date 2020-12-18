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
        Task<IEnumerable<Phrase>> getPhrases(int charID);
        Task<Phrase> GetphraseAsync(int charID, int PharaseId);

        Task<bool> UpdatePhraseAsync(int characID, int phraseID, Phrase Frase);
    }
}
