using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Models;
using SimpsonApp.Exceptions;
using SimpsonApp.Services;
using Microsoft.AspNetCore.Http;

namespace SimpsonApp.Controllers
{
    [Route("api/[controller]")]
    public class CharacterController:ControllerBase
    {
        private ICharacterService _characterService;
        public CharacterController(ICharacterService characterservice)
        {
            this._characterService = characterservice; 
        }
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
        [HttpGet("{charId:int}", Name = "GetCharacter")]
        public async Task<ActionResult<Character>> GetCompanyAsync(int charId, bool showPhrases = false)
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
        [HttpPut("{charId:int}")]
       public async Task<IActionResult> UpdateCharacter(int charId,[FromBody]Character c)
        {
            try
            {
                if (!ModelState.IsValid)
                { 
                    return BadRequest();   
                }
                return Ok(await _characterService.UpdateCharacter(charId , c));
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



    }
}
