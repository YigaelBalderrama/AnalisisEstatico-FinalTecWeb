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
        private LibraryDbContext _dbContext;
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
            var companyToUpdate = _dbContext.Characters.FirstOrDefault(c => c.ID == charac.ID);
            _dbContext.Entry(companyToUpdate).CurrentValues.SetValues(charac);
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

                throw ex;
            }

        }

        public async Task<IEnumerable<PhraseEntity>> GetPhrasesAsync(int charID)
        {
            IQueryable<PhraseEntity> query = _dbContext.Phrases;
            query = query.Where(v => v.Character.ID == charID);
            query = query.Include(v => v.Character);
            query = query.AsNoTracking();

            return await query.ToArrayAsync(); 
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
            frasetoupdate.Content = frase.Content ?? frasetoupdate.Content;
            frasetoupdate.Popularity= frase.Popularity ?? frasetoupdate.Popularity;
            return true;
        }
    }
}
