using LexiLoom.DTO;
using LexiLoom.Interfaces;
using LexiLoom.Services;
using Microsoft.AspNetCore.Mvc;

namespace LexiLoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            this._languageService = languageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLanguage([FromBody] NewLaguageModel model)
        {
            var result = await _languageService.AddLanguage(model);
            return Ok(result);
        }

        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetLanguageById([FromRoute] int languageId)
        {
            var result = await _languageService.GetLanguageById(languageId);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLanguages()
        {
            var result = await _languageService.GetLanguages();
            return Ok(result);
        }

        [HttpDelete("id/{languageId}")]
        public async Task<IActionResult> RemoveLanguage([FromRoute] int languageId)
        {
            await _languageService.RemoveLanguage(languageId);
            return Ok();
        }

        [HttpDelete("iso/{isoCode}")]
        public async Task<IActionResult> RemoveLanguageByIso([FromRoute] string isoCode)
        {
            await _languageService.RemoveLanguageByIso(isoCode);
            return Ok();
        }
    }
}
