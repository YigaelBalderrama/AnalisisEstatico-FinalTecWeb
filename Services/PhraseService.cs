using Microsoft.AspNetCore.Mvc;
using SimpsonApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Data.Entities;
using AutoMapper;
using SimpsonApp.Data.Repository;
using SimpsonApp.Exceptions; 

namespace SimpsonApp.Services
{
    public class PhraseService : IPhraseService
    {
        private IMapper _mapper;
        private ILibraryRepository _libraryRepository;
        public PhraseService(IMapper mapper, ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _libraryRepository = libraryRepository;
        }

        private async Task validateCharacter(int charID)
        {
            var company = await _libraryRepository.GetCharacterAsync(charID); 
            if (company == null)
            {
                throw new NotFoundOperationException($"the company id:{charID}, does not exist");
            }
        }
        private async Task validatePhrase(int phraseID)
        {
            var videogame = await _libraryRepository.GetPhraseAsync(phraseID);
            if (videogame == null)
            {
                throw new NotFoundOperationException($"the Phrase with id:{phraseID}, does not exist");
            }
        }
        public async Task<IEnumerable<Phrase>> getPhrases(int charID)
        {
            await validateCharacter(charID);
            var character = await _libraryRepository.GetPhrasesAsync(charID);
            return _mapper.Map<IEnumerable<Phrase>>(character);
        }

        public async  Task<Phrase> GetphraseAsync(int charID, int PharaseId)
        {
            await validateCharacter(charID);
            await validatePhrase(PharaseId);
            var frase = await _libraryRepository.GetPhraseAsync(PharaseId);
            if (frase.Character.ID != charID)
            {
                throw new NotFoundOperationException($"the Phrase with id:{PharaseId} does not exists for Character id:{charID}");
            }
            return _mapper.Map<Phrase>(frase);
        }

        public async Task<bool> UpdatePhraseAsync(int characID, int phraseID, Phrase Frase)
        {
            await GetphraseAsync(characID,phraseID);
            Frase.ID = phraseID;
            await _libraryRepository.UpdatePhraseAsync(_mapper.Map<PhraseEntity>(Frase));
            var saveRestul = await _libraryRepository.SaveChangesAsync();
            if (!saveRestul)
            {
                throw new Exception("Error while saving.");
            }
            return true;
        }
    }
}
