using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Services;

namespace REST_WebAPI.Controllers.V1 {

    [ApiController]
    [Route("api/[controller]/v1")]
    // [EnableCors("LocalPolicy")]
    public class PersonController : ControllerBase {

        private IPersonServices _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonServices personService, ILogger<PersonController> logger) {
            _personService = personService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
        public IActionResult Get() {

            _logger.LogInformation("Fetching all people.");

            return Ok(_personService.FindAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        // [EnableCors("LocalPolicy")]
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
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        // [EnableCors("MultipleOriginPolicy")]
        public IActionResult Create([FromBody] PersonDTO person) {

            _logger.LogInformation($"Creating new Person: '{person.FirstName}'.");
            var createdPerson = _personService.Create(person);
            
            if (createdPerson == null) {
                _logger.LogWarning($"Failed to create Person with name: '{person.FirstName}'!");
                return NotFound();
            }

            Response.Headers.Add("X-API-Deprecated", "true");
            Response.Headers.Add("X-API-Deprecation-Date", "2026-12-31");

            return Ok(createdPerson);
        }

        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        public IActionResult Update([FromBody] PersonDTO person) {

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
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204, Type = typeof(PersonDTO))]
        public IActionResult Delete(int id) {

            _logger.LogInformation($"Deleting Person with Id = '{id}'.");
            _personService.Delete(id);
            _logger.LogDebug($"Person with Id = '{id}' was deleted successfully.");
            return NoContent();
        }
    }
}
