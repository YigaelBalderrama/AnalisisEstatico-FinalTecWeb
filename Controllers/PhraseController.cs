using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonApp.Services;
using SimpsonApp.Models;
using SimpsonApp.Data.Repository;
using SimpsonApp.Exceptions;
using Microsoft.AspNetCore.Http;

namespace SimpsonApp.Controllers
{
    [Route("api/character/{characterID:int}/[controller]")]
    public class PhraseController : ControllerBase
    {
        private IPhraseService _phraseService;
        public PhraseController(IPhraseService phraseService)
        {
            _phraseService = phraseService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phrase>>>getPhrases(int charID)
        {
            try
            {
                return Ok(await _phraseService.getPhrases(charID));
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

        [HttpGet("{charID:int}", Name = "GetPharse")]
        public async Task<ActionResult<Phrase>> GetPhraseAsync(int charID, int PharaseId)
        {
            try
            {
                return Ok(await _phraseService.GetphraseAsync(charID, PharaseId));
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

        [HttpPut("{phareID:int}")]
        public async Task<ActionResult<Phrase>> UpdatePhraseAsync(int characID, int phraseID, [FromBody] Phrase Frase)
        {
            try
            {
                return Ok(await _phraseService.UpdatePhraseAsync(characID, phraseID, Frase));
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
