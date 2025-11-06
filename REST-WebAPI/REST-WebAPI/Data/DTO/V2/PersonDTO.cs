using Microsoft.AspNetCore.Authentication;
using REST_WebAPI.JsonSerializers;
using REST_WebAPI.Models;
using System.Text.Json.Serialization;

namespace REST_WebAPI.Data.DTO.V2 {
    public class PersonDTO {

        // [JsonPropertyOrder(1)]
        public long Id { get; set; }

        // [JsonPropertyOrder(2)]
        // [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        // [JsonPropertyOrder(3)]
        // [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        
        // [JsonPropertyOrder(6)]
        public string Address { get; set; }

        // [JsonPropertyOrder(5)]
        // [JsonConverter(typeof(GenderSerializer))]
        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Gender { get; set; }

        // [JsonIgnore]
        // [JsonPropertyOrder(4)]
        // [JsonPropertyName("birth_day")]
        // [JsonConverter(typeof(DateSerializer))]
        public DateTime? BirthDay { get; set; }

        // [JsonPropertyOrder(7)]
        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        // public int Age { get; set; }

        // [JsonIgnore]
        // [JsonPropertyOrder(8)]
        // public bool IsAdult => Age >= 18;

        // [JsonIgnore]
        // [JsonPropertyOrder(9)]
        // public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
