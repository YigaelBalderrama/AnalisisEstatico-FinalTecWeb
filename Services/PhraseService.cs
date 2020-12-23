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
            var character = await _libraryRepository.GetCharacterAsync(charID); 
            if (character == null)
            {
                throw new NotFoundOperationException($"the character id:{charID}, does not exist");
            }
        }
        private async Task validatePhrase(int phraseID)
        {
            var phrase = await _libraryRepository.GetPhraseAsync(phraseID);
            if (phrase == null)
            {
                throw new NotFoundOperationException($"the Phrase with id:{phraseID}, does not exist");
            }
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

        public async Task<IEnumerable<Phrase>> getPhrases(int charId)
        {
            await validateCharacter(charId);
            return _mapper.Map<IEnumerable<Phrase>>(await _libraryRepository.GetPhrasesAsync(charId));
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

        public async Task<Phrase> CreatePhraseAsync(int characID, Phrase frase)
        {
            await validateCharacter(characID);
            var chrac = _mapper.Map<PhraseEntity>(frase);
            chrac.Character = await _libraryRepository.GetCharacterAsync(characID);
            _libraryRepository.CreatePhrase(chrac);
            var saveResult = await _libraryRepository.SaveChangesAsync();
            if (!saveResult)
            {
                throw new Exception("save error");
            }

            var modelToReturn = _mapper.Map<Phrase>(chrac);
            modelToReturn.CharacterID = characID;
            return modelToReturn;
        }

        public async Task<bool> DeletePhraseAsync(int characID, int PhraseID)
        {
            await GetphraseAsync(characID, PhraseID);
            _libraryRepository.DeletePhrase(PhraseID);
            var saveRestul = await _libraryRepository.SaveChangesAsync();
            if (!saveRestul)
            {
                throw new Exception("Error while saving.");
            }
            return true;
        }

        
    }
}
