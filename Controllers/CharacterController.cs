using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Models;
using SimpsonApp.Exceptions;
using SimpsonApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SimpsonApp.Controllers
{
    
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterservice)
        {
            this._characterService = characterservice;
        }
        [Authorize]
        [HttpGet("phrases")]
        public async Task<ActionResult<IEnumerable<Phrase>>> getPhrases()
        {
            try
            {
                return Ok(await _characterService.getPhrases());
            }
            catch (NotFoundOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something happend: {ex.Message}");
            }

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> getCharacters(bool showPrase=false,string orderBy="ID")
        {
            try
            {
                return Ok(await _characterService.getCharacters(orderBy, showPrase));
            }
            catch (BadRequestOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something go Wrong {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("{charId:int}", Name = "GetCharacter")]
        public async Task<ActionResult<Character>> GetCharacter(int charId, bool showPhrases = false)
        {
            try
            {
                return await _characterService.GetCharacterAsync(charId, showPhrases);
            }
            catch (NotFoundOperationException ex)
            {
                return NotFound(ex.Message); ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something happend: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPut("{charId:int}")]
       public async Task<IActionResult> UpdateCharacter(int charId,[FromBody]Character c)
        {
            var validator = new ModelsValidator();
            if (!validator.validateModelFields(ModelState, true).Any())
            {
                try
                {
                    return Ok(await _characterService.UpdateCharacter(charId, c));
                }
                catch (NotFoundOperationException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Something happend: {ex.Message}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Character>> CreateCharacterAsync([FromBody] Character charac)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newCharacter = await _characterService.CreateCharacterAsync(charac);
                return CreatedAtRoute("GetCharacter", new { charId = newCharacter.ID }, newCharacter);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something happend: {ex.Message}");
            }
        }
        [Authorize]
        [HttpDelete("{charID:int}")]
        public async Task<ActionResult<DeleteModel>> DeleteCharacterAsync(int charID)
        {
            try
            {
                return Ok(await _characterService.DeleteCharacterAsync(charID));
            }
            catch (NotFoundOperationException ex)
            {
                return NotFound(ex.Message); ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something happend: {ex.Message}");
            }
        }
    }
}
