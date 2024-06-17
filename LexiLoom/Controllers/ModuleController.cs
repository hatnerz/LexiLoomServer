using LexiLoom.DTO;
using LexiLoom.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LexiLoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            this._moduleService = moduleService;
        }

        [HttpPost("empty")]
        public async Task<IActionResult> CreateEmptyModule([FromBody] NewEmptyModuleModel model)
        {
            var result = await _moduleService.CreateEmptyModule(model);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModuleWithWords([FromBody] NewModuleModel model)
        {
            var result = await _moduleService.CreateModuleWithWords(model);
            return Ok(result);
        }

        [HttpPost("{moduleId}/word/{wordId}")]
        public async Task<IActionResult> AddWordToModule([FromRoute] int moduleId, [FromRoute] int wordId)
        {
            var result = await _moduleService.AddWordToModule(wordId, moduleId);
            return Ok(result);
        }

        [HttpDelete("{moduleId}/word/{wordId}")]
        public async Task<IActionResult> RemoveWordFromModule([FromRoute] int moduleId, [FromRoute] int wordId)
        {
            await _moduleService.RemoveWordFromModule(wordId, moduleId);
            return Ok();
        }

        [HttpDelete("{moduleId}")]
        public async Task<IActionResult> RemoveModule([FromRoute] int moduleId)
        {
            await _moduleService.RemoveModule(moduleId);
            return Ok();
        }

        [HttpGet("all/user/{userId}")]
        public async Task<IActionResult> GetUserModules([FromRoute] int userId)
        {
            var result = await _moduleService.GetUserModules(userId);
            return Ok(result);
        }

        [HttpGet("details/{moduleId}")]
        public async Task<IActionResult> GetModuleDetails([FromRoute] int moduleId)
        {
            var result = await _moduleService.GetModuleWithDetails(moduleId);
            return Ok(result);
        }

    }
}
