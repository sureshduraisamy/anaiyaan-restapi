using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientLibrary;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace REST_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonCRUDController : ControllerBase
    {
        private readonly PatientManager manager = new PatientManager();

        // GET: api/<JsonCRUDController>
        [HttpGet]
       
        public IEnumerable<PatientDetails> Get()
        {
            var patients = manager.GetAllPatients();
            return patients;
        }

        // GET api/<JsonCRUDController>/5
        [HttpGet("Select/{patientId}")]
        public IActionResult Get(long patientId)
        {
            try
            {
                var patients = manager.GetAllPatients();
                return Ok(patients.FirstOrDefault(p => p.Id == patientId));
            }catch(Exception ex)
            {
                return StatusCode(500, $"Get Patient Error: {ex.Message}");
            }
        }

        // POST api/<JsonCRUDController>
        [HttpPost]
        public ActionResult<PatientDetails> Post([FromBody] PatientDetails patient)
        {
            try
            {
                var allPatients = manager.GetAllPatients();
                if (allPatients.Any(p => p.Mobile == patient.Mobile || p.Email == patient.Email))
                {
                    // Return HTTP 409 Conflict with message
                    return BadRequest("A patient with the same mobile or email already exists.");
                }

                var newPatient = new PatientDetails
                {
                    Name = patient.Name,
                    Age = patient.Age,
                    Email = patient.Email,
                    Location = patient.Location,
                    Mobile = patient.Mobile
                    // Id will be assigned inside AddPatient
                };

                manager.AddPatient(newPatient);

                // Return HTTP 201 Created with new patient's data and location header
                //return CreatedAtAction(nameof(GetById), new { id = newPatient.Id }, newPatient);
                return Ok("Patient  added Successfully");
            }
            catch (Exception ex)
            {
                // Return HTTP 500 Internal Server Error with exception message
                return StatusCode(500, $"Patient not added: {ex.Message}");
            }
        }


        // PUT api/<controller>/5
        [HttpPut("Update/{patientId}")]
        public IActionResult Put(long patientId, [FromBody] PatientDetails patient)
        {
            try
            {
                bool isUpdated = manager.UpdatePatient(patientId, existingPatient =>
                {
                    existingPatient.Name = patient.Name;
                    existingPatient.Age = patient.Age;
                    existingPatient.Location = patient.Location;
                });

                if (isUpdated)
                    return Ok("Patient Details Updated successfully");           // 200 OK
                else
                    return NotFound("The Patient Id is Not Matched");     // 404 Not Found if patient not found
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while updating the patient. {ex.Message}"); // 500 Internal Server Error
            }
        }

        // DELETE api/<JsonCRUDController>/5
        [HttpDelete("Remove/{patientId}")]
        public IActionResult Delete(long patientId)
        {
            try
            {
                bool isRemoved = manager.RemovePatient(patientId);

                if (isRemoved)
                    return Ok("Patient Details Deleted successfully");           // 200 OK
                else
                    return NotFound("The Patient Id is Not Matched");
            }

            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while updating the patient. {ex.Message}"); // 500 Internal Server Error
            }
        }
    }
}
