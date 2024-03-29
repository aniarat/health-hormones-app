using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthHormonesAPI.Controllers;

[Route("api/symptoms")]
[ApiController]
public class SymptomsController : ControllerBase
{
    private readonly ISymptomService symptomService;

    public SymptomsController(ISymptomService symptomService) =>
        this.symptomService = symptomService;
    
    [HttpGet]
    public async Task<List<Symptom>> Get()
    {
        return await symptomService.GetAllSymptomsAsync();
    }
    [HttpGet("{symptomId}")]
    public async Task<ActionResult<Symptom>> Get(string symptomId)
    {
        var symptom = await symptomService.GetSymptomByIdAsync(symptomId);
        if (symptom is null)
        {
            return NotFound();
        }
        return symptom;
    }
    [HttpPost]
    public async Task<IActionResult> Post(Symptom symptom)
    {
        await symptomService.AddSymptomAsync(symptom);
        return CreatedAtAction(nameof(Get), new { id = symptom.Id }, symptom);
    }
    [HttpPut("{symptomId}")]
    public async Task<IActionResult> Update(string symptomId, Symptom symptoms)
    {
        var symptom = await symptomService.GetSymptomByIdAsync(symptomId);
        if (symptom is null)
        {
            return NotFound();
        }
        symptoms.Id = symptom.Id;
        await symptomService.UpdateSymptomAsync(symptomId, symptoms);
        return Ok();
    }
    [HttpDelete("{symptomId}")]
    public async Task<IActionResult> Delete(string symptomId)
    {
        var symptom = await symptomService.GetSymptomByIdAsync(symptomId);
        if (symptom is null)
        {
            return NotFound();
        }
        await symptomService.DeleteSymptomAsync(symptomId);
        return Ok();
    }
}