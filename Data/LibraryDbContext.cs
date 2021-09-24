using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpsonApp.Data.Entities;
namespace SimpsonApp.Data
{
    public  class LibraryDbContext:IdentityDbContext
    {
        public DbSet<CharacterEntity> Characters { get; set; }
        public DbSet<PhraseEntity> Phrases { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CharacterEntity>().ToTable("Characters");
            builder.Entity<CharacterEntity>().Property(c => c.ID).ValueGeneratedOnAdd();
            builder.Entity<CharacterEntity>().HasMany(c => c.Phrases).WithOne(v => v.Character);

            builder.Entity<PhraseEntity>().ToTable("Phrases");
            builder.Entity<PhraseEntity>().Property(v => v.ID).ValueGeneratedOnAdd();
            builder.Entity<PhraseEntity>().HasOne(v => v.Character).WithMany(c => c.Phrases);
        }


    }
}