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
        public async Task<IEnumerable<CharacterEntity>> GetCharacterAsync(string orderBy, bool showPrase)
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
    }
}
