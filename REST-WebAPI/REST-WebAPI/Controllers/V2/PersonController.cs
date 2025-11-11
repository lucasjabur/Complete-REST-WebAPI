using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Data.DTO.V2;
using REST_WebAPI.Services.Implementations;

namespace REST_WebAPI.Controllers.V2 {

    [ApiController]
    [Route("api/[controller]/v2")]
    public class PersonController : ControllerBase {

        private PersonServicesImplV2 _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(PersonServicesImplV2 personService, ILogger<PersonController> logger) {
            _personService = personService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        public IActionResult Create([FromBody] PersonDTO person) {

            _logger.LogInformation($"Creating new Person: '{person.FirstName}'.");
            var createdPerson = _personService.Create(person);

            if (createdPerson == null) {
                _logger.LogWarning($"Failed to create Person with name: '{person.FirstName}'!");
                return NotFound();
            }

            // createdPerson.Gender = null;
            // createdPerson.Age = 0;
            // createdPerson.Age = 32;
            return Ok(createdPerson);
        }
    }
}
