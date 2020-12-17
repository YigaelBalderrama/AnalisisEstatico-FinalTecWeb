
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpsonApp.Data.Entities;
namespace SimpsonApp.Data.Repository
{
    public  class LibraryDbContext:IdentityDbContext
    {
        public DbSet<CharacterEntity> Characters { get; set; }
        public DbSet<PhraseEntity> PhraseS { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CharacterEntity>().ToTable("Characters");
            modelBuilder.Entity<CharacterEntity>().Property(c => c.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<CharacterEntity>().HasMany(c => c.Phrases).WithOne(v => v.Character);

            modelBuilder.Entity<PhraseEntity>().ToTable("Phrases");
            modelBuilder.Entity<PhraseEntity>().Property(v => v.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<PhraseEntity>().HasOne(v => v.Character).WithMany(c => c.Phrases);
        }


    }
}