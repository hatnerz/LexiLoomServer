using LexiLoom.Database;
using LexiLoom.DTO;
using LexiLoom.Exceptions;
using LexiLoom.Interfaces;
using LexiLoom.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LexiLoom.Services
{
    public class WordService : ServiceBase, IWordService
    {
        public WordService(LexiLoomDbContext context) : base(context) { }

        public async Task<Word> AddNewWordWithTranslationsWithModule(AddWordModel newWord, int moduleId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Word wordToAdd = new Word()
                    {
                        Name = newWord.Name,
                        UserId = newWord.UserId,
                        AddingTime = DateTime.UtcNow
                    };

                    _context.Words.Add(wordToAdd);
                    await _context.SaveChangesAsync();

                    foreach(var translation in newWord.Translations)
                    {
                        Language? language = await _context.Languages.FirstOrDefaultAsync(e => e.IsoCode == translation.LanguageIso);
                        
                        if (language == null)
                        {
                            throw new NotFoundException("Language", "iso code", translation.LanguageIso);
                        }

                        Translation newTranslation = new Translation()
                        {
                            WordId = wordToAdd.Id,
                            TranslationText = translation.TranslationText,
                            LanguageId = language.Id
                        };

                        _context.Translations.Add(newTranslation);
                    }

                    await _context.SaveChangesAsync();
                    var isModuleExists = await _context.Modules.AnyAsync(e => e.Id == moduleId);

                    if (!isModuleExists)
                    {
                        throw new NotFoundException("Module", moduleId);
                    }

                    WordInModule wordInModule = new WordInModule()
                    {
                        WordId = wordToAdd.Id,
                        ModuleId = moduleId,
                        AddingTime = DateTime.UtcNow
                    };

                    _context.WordsInModules.Add(wordInModule);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return wordToAdd;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                
            }
        }

        public async Task<Word> AddNewWordWithTranslationsWithoutModule(AddWordModel newWord)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Word wordToAdd = new Word()
                    {
                        Name = newWord.Name,
                        UserId = newWord.UserId,
                        AddingTime = DateTime.UtcNow
                    };

                    _context.Words.Add(wordToAdd);
                    await _context.SaveChangesAsync();

                    foreach (var translation in newWord.Translations)
                    {
                        Language? language = await _context.Languages.FirstOrDefaultAsync(e => e.IsoCode == translation.LanguageIso);

                        if (language == null)
                        {
                            throw new NotFoundException("Language", "iso code", translation.LanguageIso);
                        }

                        Translation newTranslation = new Translation()
                        {
                            WordId = wordToAdd.Id,
                            TranslationText = translation.TranslationText,
                            LanguageId = language.Id
                        };

                        _context.Translations.Add(newTranslation);
 
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return wordToAdd;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
        }

        public async Task<Translation> AddTranslationToWord(int wordId, string languageIso, string translation)
        {
            Word? foundWord = await _context.Words.FirstOrDefaultAsync(e => e.Id == wordId);
            if(foundWord == null)
            {
                throw new NotFoundException("Word", wordId);
            }

            Language? foundLanguage = await _context.Languages.FirstOrDefaultAsync(e => e.IsoCode == languageIso);
            if(foundLanguage == null)
            {
                throw new NotFoundException("Language", "iso", languageIso);
            }

            Translation newTranslation = new Translation()
            {
                WordId = wordId,
                LanguageId = foundLanguage.Id,
                TranslationText = translation,
            };

            _context.Translations.Add(newTranslation);
            await _context.SaveChangesAsync();

            return newTranslation;
        }

        public async Task<IEnumerable<Word>> GetUserWordsWithTranslations(int userId)
        {
            var foundWords = await _context.Words
                .Include(e => e.Translations)!
                .ThenInclude(e => e.Language)
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return foundWords;
        }

        public async Task<Word> GetWordWithTranslations(int wordId)
        {
            var foundWord = await _context.Words
                .Include(e => e.Translations)!
                .ThenInclude(e => e.Language)
                .FirstOrDefaultAsync(e => e.Id == wordId);

            if(foundWord == null)
            {
                throw new NotFoundException("Word", wordId);
            }

            return foundWord;
        }

        public async Task RemoveWord(int wordId)
        {
            Word? foundWord = await _context.Words.Include(e => e.WordInModules).Include(e => e.Translations).FirstOrDefaultAsync(e => e.Id == wordId);
            if (foundWord == null)
            {
                throw new NotFoundException("Word", wordId);
            }

            _context.WordsInModules.RemoveRange(foundWord.WordInModules!);
            _context.Translations.RemoveRange(foundWord.Translations!);
            _context.Words.Remove(foundWord);

            await _context.SaveChangesAsync();
        }
    }
}
