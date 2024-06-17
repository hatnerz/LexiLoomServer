using LexiLoom.DTO;
using LexiLoom.Models;

namespace LexiLoom.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetLanguages();

        Task<Language> GetLanguageById(int id);

        Task<Language> AddLanguage(NewLaguageModel newLaguageModel);

        Task RemoveLanguage(int languageId);

        Task RemoveLanguageByIso(string iso);
    }
}
