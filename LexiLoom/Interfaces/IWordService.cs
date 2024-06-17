using LexiLoom.DTO;
using LexiLoom.Models;

namespace LexiLoom.Interfaces
{
    public interface IWordService
    {
        Task<Word> GetWordWithTranslations(int wordId);

        Task<IEnumerable<Word>> GetUserWordsWithTranslations(int userId);

        Task<Word> AddNewWordWithTranslationsWithoutModule(AddWordModel newWord);

        Task<Word> AddNewWordWithTranslationsWithModule(AddWordModel newWord, int moduleId);

        Task RemoveWord(int wordId);

        Task<Translation> AddTranslationToWord(int wordId, string languageIso, string translation);
    }
}
