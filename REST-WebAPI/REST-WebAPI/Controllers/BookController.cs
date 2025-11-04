using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Models;
using REST_WebAPI.Services;

namespace REST_WebAPI.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase {

        private IBookServices _bookServices;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookServices bookServices, ILogger<BookController> logger) {
            _bookServices = bookServices;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get() {

            _logger.LogInformation("Fetching all books.");

            return Ok(_bookServices.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id) {

            _logger.LogInformation($"Fetching Book with Id = '{id}'.");
            var book = _bookServices.FindById(id);

            if (book == null) {
                _logger.LogWarning($"Book with Id = '{id}' was not found!");
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book book) {
            _logger.LogInformation($"Creating new Book '{book.Title}'.");
            var createdBook = _bookServices.Create(book);

            if (createdBook == null) {
                _logger.LogWarning($"Failed to create Book with name: '{book.Title}'!");
                return NotFound();
            }

            return Ok(createdBook);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Book book) {
            _logger.LogInformation($"Updating Book '{book.Title}'.");
            var updatedBook = _bookServices.Update(book);

            if (updatedBook == null) {
                _logger.LogWarning($"Failed to update Book with name: '{book.Title}'!");
                return NotFound();
            }

            _logger.LogDebug($"Book updated successfully: '{book.Title}'.");
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            _logger.LogInformation($"Deleting Book with Id = '{id}'!");
            _bookServices.Delete(id);
            _logger.LogDebug($"Book with Id = '{id}' was deleted successfully.");
            return NoContent();
        }
    }
}
