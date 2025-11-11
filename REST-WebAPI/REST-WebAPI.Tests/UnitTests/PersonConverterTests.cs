using FluentAssertions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using REST_WebAPI.Data.Converter.Implementations;
using REST_WebAPI.Data.DTO.V2;
using REST_WebAPI.Models;

namespace REST_WebAPI.Tests.UnitTests {
    public class PersonConverterTests {
        private readonly PersonConverter _converter;

        public PersonConverterTests() {
            _converter = new PersonConverter();
        }

        // PersonDTO to Person conversion tests
        [Fact]
        public void Parse_ShouldConvertPersonDTOToPerson() {

            // Arrange: prepare the data, objects, and dependencies required for the test
            // var dto = new PersonDTO.

            var dto = new PersonDTO {
                Id = 1,
                FirstName = "Mahatman",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male",
                //BirthDay = new DateTime(1869, 10, 2)
            };

            var expectedPerson = new Person {
                Id = 1,
                FirstName = "Mahatman",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male"
            };

            // Act: execute the method or functionality under test
            // var person = _converter.Parse(dto);
            var person = _converter.Parse(dto);

            // Assert: verify that the result matches the expected outcome
            person.Should().NotBeNull();
            person.Id.Should().Be(expectedPerson.Id);
            person.FirstName.Should().Be(expectedPerson.FirstName);
            person.LastName.Should().Be(expectedPerson.LastName);
            person.Address.Should().Be(expectedPerson.Address);
            person.Gender.Should().Be(expectedPerson.Gender);

            person.Should().BeEquivalentTo(expectedPerson);
        }

        [Fact]
        public void Parse_NullPersonDTOShouldReturnNull() {
            PersonDTO dto = null;
            var person = _converter.Parse(dto);
            person.Should().BeNull();
        }


        // Person to PersonDTO conversion tests
        [Fact]
        public void Parse_ShouldConvertPersonToPersonDTO() {

            // Arrange: prepare the data, objects, and dependencies required for the test
            // var dto = new PersonDTO.

            var entity = new Person {
                Id = 1,
                FirstName = "Mahatman",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male"
            };

            var expectedDto = new PersonDTO {
                Id = 1,
                FirstName = "Mahatman",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male"
            };

            // Act: execute the method or functionality under test
            // var person = _converter.Parse(dto);
            var dto = _converter.Parse(entity);

            // Assert: verify that the result matches the expected outcome
            dto.Should().NotBeNull();
            dto.Id.Should().Be(expectedDto.Id);
            dto.FirstName.Should().Be(expectedDto.FirstName);
            dto.LastName.Should().Be(expectedDto.LastName);
            dto.Address.Should().Be(expectedDto.Address);
            dto.Gender.Should().Be(expectedDto.Gender);

            //dto.Should().BeEquivalentTo(expectedDto, options => options.Excluding(dto => dto.BirthDay));
            //dto.BirthDay.Should().NotBeNull();
        }

        [Fact]
        public void Parse_NullPersonShouldReturnNull() {
            Person entity = null;
            var dto = _converter.Parse(entity);
            dto.Should().BeNull();
        }

        [Fact]
        public void ParseList_ShouldConvertPersonDTOListToPersonList() {
            var dtoList = new List<PersonDTO> {
                new PersonDTO {
                    Id = 1,
                    FirstName = "Mahatma",
                    LastName = "Gandhi",
                    Address = "Porbandar - India",
                    Gender = "Male",
                    //BirthDay = new DateTime(1869, 10, 2)
                },
                new PersonDTO {
                    Id = 2,
                    FirstName = "Indira",
                    LastName = "Gandhi",
                    Address = "Allahabad - India",
                    Gender = "Female",
                    //BirthDay = new DateTime(1917, 11, 19)
                },
            };

            var personList = _converter.ParseList(dtoList);

            personList.Should().NotBeNull();
            personList.Should().HaveCount(2);
            personList[0].Should().BeEquivalentTo(
                new Person {
                    Id = 1,
                    FirstName = "Mahatma",
                    LastName = "Gandhi",
                    Address = "Porbandar - India",
                    Gender = "Male"
                }
            );

            personList[1].Should().BeEquivalentTo(
                new Person {
                    Id = 2,
                    FirstName = "Indira",
                    LastName = "Gandhi",
                    Address = "Allahabad - India",
                    Gender = "Female",
                }
            );

            personList[0].FirstName.Should().Be("Mahatma");
            personList[1].FirstName.Should().Be("Indira");
            personList[1].LastName.Should().Be("Gandhi");
        }

        [Fact]
        public void ParseList_NullListPersonDTOShouldReturnNull() {
            List<PersonDTO> dtoList = null;
            var personList = _converter.ParseList(dtoList);
            personList.Should().BeNull();
        }






        [Fact]
        public void ParseList_ShouldConvertPersonListToPersonDTOList() {
            var personList = new List<Person> {
                new Person {
                    Id = 1,
                    FirstName = "Mahatma",
                    LastName = "Gandhi",
                    Address = "Porbandar - India",
                    Gender = "Male",
                },
                new Person {
                    Id = 2,
                    FirstName = "Indira",
                    LastName = "Gandhi",
                    Address = "Allahabad - India",
                    Gender = "Female",
                },
            };

            var dtoList = _converter.ParseList(personList);

            dtoList.Should().NotBeNull();
            dtoList.Should().HaveCount(2);
            dtoList[0].Should().BeEquivalentTo(
                new PersonDTO {
                    Id = 1,
                    FirstName = "Mahatma",
                    LastName = "Gandhi",
                    Address = "Porbandar - India",
                    Gender = "Male",
                    //BirthDay = new DateTime(1869, 10, 2)
                }
                //options => options.Excluding(dto => dto.BirthDay)
            );

            dtoList[1].Should().BeEquivalentTo(
                new PersonDTO {
                    Id = 2,
                    FirstName = "Indira",
                    LastName = "Gandhi",
                    Address = "Allahabad - India",
                    Gender = "Female",
                    //BirthDay = new DateTime(1917, 11, 19)
                }
                //options => options.Excluding(dto => dto.BirthDay)
            );

            dtoList[0].FirstName.Should().Be("Mahatma");
            dtoList[1].FirstName.Should().Be("Indira");
            dtoList[1].LastName.Should().Be("Gandhi");
        }

        [Fact]
        public void ParseList_NullListPersonShouldReturnNull() {
            List<Person> personList = null;
            var dtoList = _converter.ParseList(personList);
            dtoList.Should().BeNull();
        }
    }
}
