using LexiLoom.DTO;
using LexiLoom.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LexiLoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IWordService _wordService;

        public WordController(IWordService wordService)
        {
            this._wordService = wordService;
        }

        [HttpPost("add-to-module/{moduleId}")]
        public async Task<IActionResult> AddNewWordWithTranslationsWithModule([FromBody] AddWordModel newWord, [FromRoute] int moduleId)
        {
            var result = await _wordService.AddNewWordWithTranslationsWithModule(newWord, moduleId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewWordWithTranslationsWithoutModule([FromBody] AddWordModel newWord)
        {
            var result = await _wordService.AddNewWordWithTranslationsWithoutModule(newWord);
            return Ok(result);
        }

        [HttpPost("translation/{wordId}")]
        public async Task<IActionResult> AddTranslationToWord([FromBody] NewTranslationModel model, [FromRoute] int wordId)
        {
            var result = await _wordService.AddTranslationToWord(wordId, model.LanguageIso, model.TranslationText);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserWords([FromRoute] int userId)
        {
            var result = await _wordService.GetUserWordsWithTranslations(userId);
            return Ok(result);
        }

        [HttpGet("id/{wordId}")]
        public async Task<IActionResult> GetWordWithDetails([FromRoute] int wordId)
        {
            var result = await _wordService.GetWordWithTranslations(wordId);
            return Ok(result);
        }

        [HttpDelete("id/{wordId}")]
        public async Task<IActionResult> RemoveWord([FromRoute] int wordId)
        {
            await _wordService.RemoveWord(wordId);
            return Ok();
        }

    }
}
