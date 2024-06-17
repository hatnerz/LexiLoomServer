using LexiLoom.Database;
using LexiLoom.DTO;
using LexiLoom.Exceptions;
using LexiLoom.Interfaces;
using LexiLoom.Models;
using Microsoft.EntityFrameworkCore;

namespace LexiLoom.Services
{
    public class LanguageService : ServiceBase, ILanguageService
    {
        public LanguageService(LexiLoomDbContext context) : base(context)
        { }

        public async Task<Language> AddLanguage(NewLaguageModel newLaguageModel)
        {
            var isLanguageWithIsoExists = await _context.Languages.AnyAsync(e => e.IsoCode == newLaguageModel.IsoCode.ToLower());
            if(isLanguageWithIsoExists)
            {
                throw new AlreadyExistsException("Language", "iso code");
            }

            Language newLanguage = new Language()
            {
                IsoCode = newLaguageModel.IsoCode.ToLower(),
                Name = newLaguageModel.Name
            };

            await _context.Languages.AddAsync(newLanguage);
            await _context.SaveChangesAsync();
        
            return newLanguage;
        }

        public async Task<Language> GetLanguageById(int id)
        {
            Language? foundLanguage = await _context.Languages.FindAsync(id);
            if (foundLanguage == null)
            {
                throw new NotFoundException("Language", id);
            }

            return foundLanguage;
        }

        public async Task<IEnumerable<Language>> GetLanguages()
        {
            var foundLanguages = await _context.Languages.ToListAsync();
            return foundLanguages;
        }

        public async Task RemoveLanguage(int languageId)
        {
            Language? foundLanguage = await _context.Languages.FindAsync(languageId);
            if (foundLanguage == null)
            {
                throw new NotFoundException("Language", languageId);
            }

            _context.Languages.Remove(foundLanguage);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLanguageByIso(string iso)
        {
            Language? foundLanguage = await _context.Languages.FirstOrDefaultAsync(e => e.IsoCode == iso);
            if (foundLanguage == null)
            {
                throw new NotFoundException("Language", "iso code", iso);
            }

            _context.Languages.Remove(foundLanguage);
            await _context.SaveChangesAsync();
        }
    }
}
