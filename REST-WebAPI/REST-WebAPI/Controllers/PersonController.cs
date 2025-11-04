using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Models;
using REST_WebAPI.Services;

namespace REST_WebAPI.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase {

        private IPersonServices _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonServices personService, ILogger<PersonController> logger) {
            _personService = personService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get() {

            _logger.LogInformation("Fetching all people.");

            return Ok(_personService.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id) {

            _logger.LogInformation($"Fetching Person with Id = '{id}'.");

            var person = _personService.FindById(id);
            if (person == null) {
                _logger.LogWarning($"Person with Id = '{id}' was not found!");
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Person person) {

            _logger.LogInformation($"Creating new Person: '{person.FirstName}'.");
            var createdPerson = _personService.Create(person);
            
            if (createdPerson == null) {
                _logger.LogWarning($"Failed to create Person with name: '{person.FirstName}'!");
                return NotFound();
            }

            return Ok(createdPerson);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Person person) {

            _logger.LogInformation($"Updating Person with Id = '{person.Id}'.");
            var createdPerson = _personService.Update(person);
            
            if (createdPerson == null) {
                _logger.LogWarning($"Failed to update Person with Id: '{person.Id}'.");
                return NotFound();
            }

            _logger.LogDebug($"Person updated successfully: '{person.FirstName}'.");
            return Ok(createdPerson);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {

            _logger.LogInformation($"Deleting Person with Id = '{id}'.");
            _personService.Delete(id);
            _logger.LogDebug($"Person with Id = '{id}' was deleted successfully.");
            return NoContent();
        }
    }
}
