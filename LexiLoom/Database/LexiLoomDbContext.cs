using LexiLoom.Models;
using Microsoft.EntityFrameworkCore;

namespace LexiLoom.Database
{
    public class LexiLoomDbContext : DbContext
    {

        public LexiLoomDbContext()
        { }

        public LexiLoomDbContext(DbContextOptions<LexiLoomDbContext> options)
            : base(options)
        { }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<Translation> Translations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Word> Words { get; set; }

        public DbSet<WordInModule> WordsInModules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Words)
                .WithOne(w => w.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Modules)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Word>()
                .HasMany(w => w.Translations)
                .WithOne(t => t.Word)
                .HasForeignKey(t => t.WordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
