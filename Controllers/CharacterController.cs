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

    }
}
