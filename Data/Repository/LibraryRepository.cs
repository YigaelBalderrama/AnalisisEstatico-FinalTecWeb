using Microsoft.EntityFrameworkCore;
using SimpsonApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpsonApp.Data.Repository
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly LibraryDbContext _dbContext;
        public LibraryRepository(LibraryDbContext dbContext)
        {
           _dbContext = dbContext;
        }
        public async Task<IEnumerable<CharacterEntity>> GetCharactersAsync(string orderBy, bool showPrase)
        {
            IQueryable<CharacterEntity> query = _dbContext.Characters;
            query = query.AsNoTracking();

            switch (orderBy)
            {
                case "id":
                    query = query.OrderBy(c => c.ID);
                    break;
                case "name":
                    query = query.OrderBy(c => c.Name);
                    break;
                case "age":
                    query = query.OrderBy(c => c.Age);
                    break;
            }
            return await query.ToListAsync();
        }

        public async Task<CharacterEntity> GetCharacterAsync(int charId, bool showPhrases = false)
        {
            IQueryable<CharacterEntity> query = _dbContext.Characters;
            query = query.AsNoTracking();
            if (showPhrases)
            {
                query = query.Include(c => c.Phrases);
            }
            return await query.FirstOrDefaultAsync(c => c.ID == charId);
        }
        public bool UpdateCharacter(CharacterEntity charac)
        {
            var characterToUpdate = _dbContext.Characters.FirstOrDefault(c => c.ID == charac.ID);

            charac.Name = charac.Name ?? characterToUpdate.Name;
            charac.Age= charac.Age ?? characterToUpdate.Age;
            charac.Occupation = charac.Occupation ?? characterToUpdate.Occupation;
            charac.isProta = charac.isProta ?? characterToUpdate.isProta;
            charac.appearingSeason = charac.appearingSeason ?? characterToUpdate.appearingSeason;

            _dbContext.Entry(characterToUpdate).CurrentValues.SetValues(charac);
            return true;
        }
        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                var res = await _dbContext.SaveChangesAsync();
                return res > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        public async Task<IEnumerable<PhraseEntity>> GetPhrasesAsync(int charId=0)
        {
            if (charId == 0)
            {
                IQueryable<PhraseEntity> query = _dbContext.Phrases.Include(p => p.Character);
                query = query.AsNoTracking();
                return await query.ToArrayAsync();
            }
            else
            {
                IQueryable<PhraseEntity> query = _dbContext.Phrases.Where(p => p.Character.ID == charId).Include(p => p.Character);
                query = query.AsNoTracking();
                return await query.ToArrayAsync();
            }
        }

        public async Task<PhraseEntity> GetPhraseAsync(int PharaseId)
        {
            IQueryable<PhraseEntity> query = _dbContext.Phrases;
            query = query.Include(v => v.Character);
            query = query.AsNoTracking();
            var frase = await query.SingleOrDefaultAsync(v => v.ID == PharaseId);
            return frase;
        }

        public async Task<bool> UpdatePhraseAsync(PhraseEntity frase)
        {
            var frasetoupdate = await _dbContext.Phrases.FirstOrDefaultAsync(v => v.ID == frase.ID);
            frase.Content = frase.Content ?? frasetoupdate.Content;
            frase.Season = frase.Season ?? frasetoupdate.Season;
            frase.Popularity= frase.Popularity ?? frasetoupdate.Popularity;

            _dbContext.Entry(frasetoupdate).CurrentValues.SetValues(frase);
            return true;
        }

        public void CreateCharacter(CharacterEntity character)
        {
            _dbContext.Characters.Add(character);
        }

        public async Task<bool> DeleteCharacterAsync(int characId)
        {

            var companyToDelete = await _dbContext.Characters.FirstOrDefaultAsync(c => c.ID == characId);
            _dbContext.Characters.Remove(companyToDelete);
            return true;
        }

        public void CreatePhrase(PhraseEntity frase)
        {
            frase.Likes = 0;
            if (frase.Character!= null)
            {
                _dbContext.Entry(frase.Character).State = EntityState.Unchanged;
            }
            _dbContext.Phrases.Add(frase);
        }

        public bool DeletePhrase(int PhraseID)
        {
            var PhraseToDelete = new PhraseEntity() { ID = PhraseID };
            _dbContext.Entry(PhraseToDelete).State = EntityState.Deleted;
            return true;
        }

        public async Task<bool> addLikesAsync(List<int> listPhrasesId)
        {
            PhraseEntity frase = null, new_frase;
            for (int i=0; i<listPhrasesId.Count; i++)
            {
                frase = await _dbContext.Phrases.FirstOrDefaultAsync(p => listPhrasesId[i] == p.ID);
                new_frase = frase;
                new_frase.Likes++;
                _dbContext.Entry(frase).CurrentValues.SetValues(new_frase);
            }
            return true;
        }
    }
}
