﻿using LexiLoom.DTO;
using LexiLoom.Models;

namespace LexiLoom.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<Module>> GetUserModules(int userId);

        Task<Module> GetModuleWithDetails(int moduleId);

        Task<Module> CreateEmptyModule(NewEmptyModuleModel newModuleModel);

        Task<Module> CreateModuleWithWords(NewModuleModel newModuleModel);

        Task<WordInModule> AddWordToModule(int wordId, int moduleId);

        Task RemoveWordFromModule(int wordId, int moduleId);

        Task RemoveModule(int moduleId);

        Task<ModuleGameModel> CreateModuleTestGame(int moduleId, int wordsCount, int answerOptionsCount, string baseLanguageIso);

        Task<int> GetModuleWordsCount(int moduleId);

        Task<IEnumerable<Word>> GetWordsNotAddedInModule(int moduleId);
    }
}
