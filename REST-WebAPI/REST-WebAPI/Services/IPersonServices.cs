using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Services {
    public interface IPersonServices {
        PersonDTO Create(PersonDTO person);
        PersonDTO FindById(long id);
        List<PersonDTO> FindAll();
        PersonDTO Update(PersonDTO person);
        void Delete(long id);

    }
}
