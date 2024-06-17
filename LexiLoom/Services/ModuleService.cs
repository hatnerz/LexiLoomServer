using LexiLoom.Database;
using LexiLoom.DTO;
using LexiLoom.Exceptions;
using LexiLoom.Interfaces;
using LexiLoom.Models;
using Microsoft.EntityFrameworkCore;

namespace LexiLoom.Services
{
    public class ModuleService : ServiceBase, IModuleService
    {
        private readonly IWordService _wordService;

        public ModuleService(LexiLoomDbContext context, IWordService wordService) : base(context)
        {
            this._wordService = wordService;
        }

        public async Task<Module> CreateEmptyModule(NewEmptyModuleModel newModuleModel)
        {
            var isUserExists = await _context.Users.AnyAsync(e => e.Id == newModuleModel.UserId);

            if(!isUserExists)
            {
                throw new NotFoundException("Provided user");
            }

            Module newModule = new Module()
            {
                Name = newModuleModel.Name,
                Descriptions = newModuleModel.Description,
                UserId = newModuleModel.UserId
            };

            await _context.Modules.AddAsync(newModule);
            await _context.SaveChangesAsync();

            return newModule;
        }

        public async Task<Module> CreateModuleWithWords(NewModuleModel newModuleModel)
        {
            NewEmptyModuleModel newEmptyModule = new NewEmptyModuleModel()
            {
                UserId = newModuleModel.UserId,
                Name = newModuleModel.Name,
                Description = newModuleModel.Description
            };

            var createdModule = await CreateEmptyModule(newEmptyModule);

            foreach (var word in newModuleModel.Words)
            {
                await _wordService.AddNewWordWithTranslationsWithModule(word, createdModule.Id);
            }

            return createdModule;
        }

        public async Task<WordInModule> AddWordToModule(int wordId, int moduleId)
        {
            var foundWord = await _context.Words.FindAsync(wordId);
            var foundModule = await _context.Modules.FindAsync(moduleId);
        
            if (foundModule == null)
            {
                throw new NotFoundException("Module", moduleId);
            }

            if (foundWord == null)
            {
                throw new NotFoundException("Word", wordId);
            }

            WordInModule wordInModule = new WordInModule()
            {
                WordId = wordId,
                ModuleId = moduleId,
                AddingTime = DateTime.UtcNow
            };

            await _context.WordsInModules.AddAsync(wordInModule);
            await _context.SaveChangesAsync();

            return wordInModule;
        }

        public async Task RemoveWordFromModule(int wordId, int moduleId)
        {
            var foundWord = await _context.Words.FindAsync(wordId);
            var foundModule = await _context.Modules.FindAsync(moduleId);

            if (foundModule == null)
            {
                throw new NotFoundException("Module", moduleId);
            }

            if (foundWord == null)
            {
                throw new NotFoundException("Word", wordId);
            }

            var wordInModule = await _context.WordsInModules.FirstOrDefaultAsync(e => e.WordId == wordId && e.ModuleId == moduleId);

            if (wordInModule == null)
            {
                throw new NotFoundException("Word in module");
            }

            _context.WordsInModules.Remove(wordInModule);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveModule(int moduleId)
        {
            var foundModule = await _context.Modules.Include(e => e.Words).FirstOrDefaultAsync(e => e.Id == moduleId);
            if (foundModule == null)
            {
                throw new NotFoundException("Module", moduleId);
            }

            _context.WordsInModules.RemoveRange(foundModule.Words!);
            _context.Modules.Remove(foundModule);
            await _context.SaveChangesAsync();
        }

        public Task<ModuleGameModel> CreateModuleTestGame(int moduleId, int wordsCount, int answerOptionsCount, string baseLanguageIso)
        {
            var wordsInModule = _context.WordsInModules.Include(e => e.Word).ThenInclude(e => e.Translations).Where(e => e.ModuleId == moduleId);
            return null;
        }

        public async Task<IEnumerable<Module>> GetUserModules(int userId)
        {
            var isUserFound = await _context.Users.AnyAsync(e => e.Id == userId);

            if(!isUserFound)
            {
                throw new NotFoundException("Provided user");
            }

            var foundModules = await _context.Modules.ToListAsync();

            return foundModules;
        }

        public async Task<Module> GetModuleWithDetails(int moduleId)
        {
            var foundModule = await _context.Modules
                .Include(e => e.Words)!
                .ThenInclude(e => e.Word)
                .ThenInclude(e => e.Translations)!
                .ThenInclude(e => e.Language)
                .FirstOrDefaultAsync(e => e.Id == moduleId);
            
            if(foundModule == null)
            {
                throw new NotFoundException("Module", moduleId);
            }

            return foundModule;
        }

        public async Task<int> GetModuleWordsCount(int moduleId)
        {
            var foundModule = await _context.Modules.Include(e => e.Words).FirstOrDefaultAsync(e => e.Id == moduleId);
            
            if (foundModule == null)
            {
                throw new NotFoundException("Module", moduleId);
            }

            return foundModule.Words!.Count();
        }

        public async Task<IEnumerable<Word>> GetWordsNotAddedInModule(int moduleId)
        {
            var foundModule = await _context.Modules.Include(e => e.Words).FirstOrDefaultAsync(e => e.Id == moduleId);

            if (foundModule == null)
            {
                throw new NotFoundException("Module", moduleId);
            }

            var usersWords = await _context.Words.Where(e => e.UserId == foundModule.UserId).ToListAsync();

            var notAddedWords = usersWords.Where(e => !(foundModule.Words!.Select(w => w.WordId).Contains(e.Id))).ToList();

            return notAddedWords;
        }
    }
}
