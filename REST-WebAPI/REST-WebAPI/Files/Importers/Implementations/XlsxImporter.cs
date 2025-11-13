using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2013.Word;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Files.Importers.Contract;

namespace REST_WebAPI.Files.Importers.Implementations {
    public class XlsxImporter : IFileImporter {
        public Task<List<PersonDTO>> ImportFileAsync(Stream fileStream) {
            var people = new List<PersonDTO>();
            using var workBook = new XLWorkbook(fileStream);
            var workSheet = workBook.Worksheets.First();

            var rows = workSheet.RowsUsed().Skip(1);

            foreach (var row in rows) {
                if (!row.Cell(1).IsEmpty()) {

                    people.Add(new PersonDTO {
                        FirstName = row.Cell(1).GetString(),
                        LastName = row.Cell(2).GetString(),
                        Address = row.Cell(3).GetString(),
                        Gender = row.Cell(4).GetString(),
                        Enabled = true
                    });
                }
            }
            return Task.FromResult(people);
        }
    }
}