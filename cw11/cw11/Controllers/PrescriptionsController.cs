using System.Transactions;
using cw10.Data;
using cw10.DTOs;
using cw10.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cw10.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService _service;

    public PrescriptionsController(IDbService dbService)
    {
        _service = dbService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription(NewPrescriptionDto newPrescriptionDto)
    {
        var idPatient = newPrescriptionDto.Patient.IdPatient;

        if (!await _service.DoesPatientExist(newPrescriptionDto.Patient.IdPatient))
        {
            idPatient = await _service.AddNewPatient(newPrescriptionDto.Patient);
        }

        if (newPrescriptionDto.Medicaments.Count > 10)
            return BadRequest("There can be only 10 medicaments on one prescription!");

        foreach (var medicament in newPrescriptionDto.Medicaments)
        {
            if (!await _service.DoesMedicationExist(medicament.IdMedicament))
                return BadRequest($"Medicament with id = {medicament.IdMedicament} does not exist!");
        }

        if (!(newPrescriptionDto.DueDate >= newPrescriptionDto.Date))
            return BadRequest("Due date must be later than the date of prescription!");

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var idPrescription = await _service.AddNewPrescription(newPrescriptionDto.Date, newPrescriptionDto.DueDate,
                idPatient, newPrescriptionDto.IdDoctor);

            foreach (var medicament in newPrescriptionDto.Medicaments)
            {
                await _service.AddNewPrescriptionMedicament(medicament.IdMedicament, idPrescription, medicament.Dose,
                    medicament.Details);
            }

            scope.Complete();
        }

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescription(int id)
    {
        if (!await _service.DoesPatientExist(id))
            return NotFound("Patient with id = " + id + " does not exist!");

        return Ok(await _service.GetPatientInfo(id));
    }
}