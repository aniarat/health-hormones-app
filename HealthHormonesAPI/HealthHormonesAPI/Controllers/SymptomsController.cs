using HealthHormonesAPI.Exceptions;
using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthHormonesAPI.Controllers;

[Route("api/symptoms")]
[ApiController]
public class SymptomsController : ControllerBase
{
    private readonly ISymptomRepository _symptomRepository;
    private readonly ISymptomChangeService _symptomService;

    public SymptomsController(ISymptomChangeService symptomService, ISymptomRepository symptomRepository)
    {
        _symptomRepository = symptomRepository;
        _symptomService = symptomService;
    }

    // public SymptomsController(ISymptomRepository symptomRepository) =>
    //     this._symptomRepository = symptomRepository;
    
    [HttpGet]
    public async Task<List<Symptom>> Get()
    {
        return await _symptomRepository.GetAllSymptomsAsync();
    }
    [HttpGet("{symptomId}")]
    public async Task<ActionResult<Symptom>> Get(string symptomId)
    {
        var symptom = await _symptomRepository.GetSymptomByIdAsync(symptomId);
        if (symptom is null)
        {
            return NotFound();
        }
        return symptom;
    }
    [HttpPost]
    public async Task<IActionResult> Post(Symptom symptom)
    {
        await _symptomRepository.AddSymptomAsync(symptom);
        return CreatedAtAction(nameof(Get), new { id = symptom.SymptomId }, symptom);
    }
    [HttpPut("{symptomId}")]
    public async Task<IActionResult> Update(string symptomId, [FromBody] Symptom updatedSymptom)
    {
        try
        {
            await _symptomRepository.UpdateSymptomPropertyAsync(symptomId, updatedSymptom);
            return Ok("Symptom updated successfully.");
        }
        catch (SymptomNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the symptom.");
        }
        // var symptom = await _symptomService.UpdateSymptomAsync(symptomId, updatedSymptom);
        // // var symptom = await _symptomRepository.GetSymptomByIdAsync(symptomId);
        // if (symptom is null)
        // {
        //     return NotFound();
        // }
        // // symptom.SymptomId = symptom.SymptomId;
        // // await _symptomRepository.UpdateSymptomAsync(symptomId, symptoms);
        // return Ok();
    }
    [HttpDelete("{symptomId}")]
    public async Task<IActionResult> Delete(string symptomId)
    {
        var symptom = await _symptomRepository.GetSymptomByIdAsync(symptomId);
        if (symptom is null)
        {
            return NotFound();
        }
        await _symptomRepository.DeleteSymptomAsync(symptomId);
        return Ok();
    }
}