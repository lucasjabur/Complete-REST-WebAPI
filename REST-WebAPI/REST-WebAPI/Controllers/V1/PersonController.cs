using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Services;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Hypermedia.Utils;
using Microsoft.AspNetCore.Mvc.Formatters;
using REST_WebAPI.Files.Importers.Factory;

namespace REST_WebAPI.Controllers.V1 {
    [ApiController]
    [Route("api/[controller]/v1")]
    // [EnableCors("LocalPolicy")]
    public class PersonController : ControllerBase {
        private IPersonServices _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonServices personService,
            ILogger<PersonController> logger) {
            _personService = personService;
            _logger = logger;
        }

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        // [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PagedSearchDTO<PersonDTO>))]
        public IActionResult Get([FromQuery] string name, string sortDirection, int pageSize, int page) {
            _logger.LogInformation($"Fetching people with paged search: {name}, {sortDirection}, {pageSize}, {page}");
            return Ok(_personService.FindWithPagedSearch(name, sortDirection, pageSize, page));
        }

        [HttpGet("find-by-name")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
        public IActionResult GetByName([FromQuery] string firstName, [FromQuery] string lastName) {
            _logger.LogInformation("Fetching persons by name: {firstName} {lastName}", firstName, lastName);
            return Ok(_personService.FindByName(firstName, lastName));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        // [EnableCors("LocalPolicy")]
        public IActionResult Get(long id) {
            _logger.LogInformation("Fetching person with ID {id}", id);
            var person = _personService.FindById(id);
            if (person == null) {
                _logger.LogWarning("Person with ID {id} not found", id);
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        // [EnableCors("MultipleOriginPolicy")]
        public IActionResult Post([FromBody] PersonDTO person) {
            _logger.LogInformation("Creating new Person: {firstName}", person.FirstName);

            var createdPerson = _personService.Create(person);
            if (createdPerson == null) {
                _logger.LogError("Failed to create person with name {firstName}", person.FirstName);
                return NotFound();
            }
            return Ok(createdPerson);
        }

        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        public IActionResult Put([FromBody] PersonDTO person) {
            _logger.LogInformation("Updating person with ID {id}", person.Id);

            var createdPerson = _personService.Update(person);
            if (createdPerson == null) {
                _logger.LogError("Failed to update person with ID {id}", person.Id);
                return NotFound();
            }
            _logger.LogDebug("Person updated successfully: {firstName}", createdPerson.FirstName);
            return Ok(createdPerson);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204, Type = typeof(PersonDTO))]
        public IActionResult Delete(int id) {
            _logger.LogInformation("Deleting person with ID {id}", id);
            _personService.Delete(id);
            _logger.LogDebug("Person with ID {id} deleted successfully", id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        public IActionResult Disable(long id) {
            _logger.LogInformation("Disabling person with ID {id}", id);
            var disabledPerson = _personService.Disable(id);
            if (disabledPerson == null) {
                _logger.LogError("Failed to disable person with ID {id}", id);
                return NotFound();
            }
            _logger.LogDebug("Person with ID {id} disabled successfully", id);
            return Ok(disabledPerson);
        }

        [HttpPost("mass-creation")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        public async Task<IActionResult> MassCreation([FromForm] FileUploadDTO input) {
            if (input.File == null || input.File.Length == 0) {
                _logger.LogWarning("No input uploaded for mass creation");
                return BadRequest("File is required");
            }
            _logger.LogInformation("Starting mass creation of people from uploaded input");

            var people = await _personService.MassCreationFileAsync(input.File);

            if (people == null || !people.Any()) {
                _logger.LogError("Mass creation failed or no people were created");
                return NoContent();
            }

            _logger.LogInformation($"Mass creation completed successfully with {people.Count} records.");
            return Ok(people);
        }

        [HttpGet("export-file/{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(415)]
        [ProducesResponseType(200, Type = typeof(FileContentResult))]
        [Produces(MediaTypes.ApplicationCsv, MediaTypes.ApplicationXlsx)]
        public IActionResult ExportFile(string sortDirection, int pageSize, int page, [FromQuery] string name = "") {
            var acceptHeader = Request.Headers["Accept"].ToString();
            if (string.IsNullOrWhiteSpace(acceptHeader)) {
                return BadRequest("Accept header is required");
            }
            
            try {
                var fileResult = _personService.ExportPage(page, pageSize, sortDirection, acceptHeader, name);
                return fileResult;

            } catch (NotSupportedException ex) {
                _logger.LogWarning("Unsupported media type requested: {AcceptHeader}", acceptHeader);
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, ex.Message);
            
            } catch (Exception ex) {
                _logger.LogError(ex, "Unexpected error during file export");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
