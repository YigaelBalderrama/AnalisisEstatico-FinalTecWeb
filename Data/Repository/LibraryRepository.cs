using SimpsonApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

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
    }
}
